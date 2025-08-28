using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DefaPress.Application.DTOs;
using DefaPress.Application.Interfaces;
using DefaPress.Domain;
using DefaPress.Infrastructure.Modules.Base.Interfaces; // for IUnitOffWork or adjust namespace
// adjust namespaces above to match پروژهٔ شما

namespace DefaPress.Application.Services
{
    public class ArticleCategoryService : IArticleCategoryService
    {
        private readonly IUnitOffWork _uow;
        private readonly IMapper _mapper;

        public ArticleCategoryService(IUnitOffWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArticleCategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
        {
            // includeProperties could be used if repo supports string includes; we'll request all and map
            var entities = (await _uow.ArticleCategoryRepository.GetAllAsync(includeProperties: "ParentCategory", ct: cancellationToken))
                .OrderBy(c => c.DisplayOrder)
                .ToList();

            return _mapper.Map<List<ArticleCategoryDto>>(entities);
        }

        public async Task<IEnumerable<ArticleCategoryDto>> GetCategoryTreeAsync(int maxDepth = 5, CancellationToken cancellationToken = default)
        {
            // Load all categories (no tracking ideally done by repo)
            var all = (await _uow.ArticleCategoryRepository.GetAllAsync(includeProperties: null, ct: cancellationToken))
                .OrderBy(c => c.DisplayOrder)
                .ToList();

            // Map each entity to DTO (shallow)
            var lookup = all.ToDictionary(c => c.CategoryId, c => _mapper.Map<ArticleCategoryDto>(c));

            // Ensure SubCategories lists are empty initially
            foreach (var dto in lookup.Values)
                dto.SubCategories = new List<ArticleCategoryDto>();

            var roots = new List<ArticleCategoryDto>();

            foreach (var e in all)
            {
                var dto = lookup[e.CategoryId];
                if (e.ParentCategoryId.HasValue && lookup.TryGetValue(e.ParentCategoryId.Value, out var parentDto))
                {
                    dto.ParentCategoryName = parentDto.Name;
                    parentDto.SubCategories.Add(dto);
                }
                else
                {
                    roots.Add(dto);
                }
            }

            // Trim depth if requested
            if (maxDepth > 0)
            {
                void Trim(List<ArticleCategoryDto> nodes, int currentDepth)
                {
                    if (nodes == null || !nodes.Any()) return;
                    if (currentDepth >= maxDepth)
                    {
                        foreach (var n in nodes) n.SubCategories = new List<ArticleCategoryDto>();
                        return;
                    }

                    foreach (var n in nodes) Trim(n.SubCategories, currentDepth + 1);
                }

                Trim(roots, 1);
            }

            return roots;
        }

        public async Task<ArticleCategoryDto?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _uow.ArticleCategoryRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null) return null;

            // load parent name if exist (repo.GetByIdAsync does not include ParentCategory nav)
            if (entity.ParentCategoryId.HasValue)
            {
                var parent = await _uow.ArticleCategoryRepository.GetByIdAsync(entity.ParentCategoryId.Value, cancellationToken);
                entity.ParentCategory = parent; // optional: for mapper if it relies on ParentCategory
            }

            var dto = _mapper.Map<ArticleCategoryDto>(entity);
            dto.ParentCategoryName = entity.ParentCategory?.Name;
            return dto;
        }

        public async Task<int> AddCategoryAsync(ArticleCategoryCreateDto dto, CancellationToken cancellationToken = default)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var entity = _mapper.Map<ArticleCategory>(dto);

            // Validate parent if provided
            if (dto.ParentCategoryId.HasValue)
            {
                var parent = await _uow.ArticleCategoryRepository.GetByIdAsync(dto.ParentCategoryId.Value, cancellationToken);
                if (parent == null)
                    throw new KeyNotFoundException("Parent category not found.");

                entity.ParentCategoryId = parent.CategoryId;
            }

            // Optionally set DisplayOrder if not provided: put at end
            if (entity.DisplayOrder == 0)
            {
                var all = (await _uow.ArticleCategoryRepository.GetAllAsync(includeProperties: null, ct: cancellationToken)).ToList();
                entity.DisplayOrder = all.Any() ? all.Max(x => x.DisplayOrder) + 1 : 1;
            }

            await _uow.ArticleCategoryRepository.AddAsync(entity, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return entity.CategoryId;
        }

        public async Task<bool> UpdateCategoryAsync(int id, ArticleCategoryCreateDto dto, CancellationToken cancellationToken = default)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var entity = await _uow.ArticleCategoryRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null) return false;

            // Update simple fields
            entity.Name = dto.Name;
            entity.Slug = dto.Slug;
            entity.Description = dto.Description;
            entity.DisplayOrder = dto.DisplayOrder;

            // Parent change: ensure not self and not creating a cycle
            if (dto.ParentCategoryId == id)
                throw new InvalidOperationException("Category cannot be its own parent.");

            if (dto.ParentCategoryId.HasValue)
            {
                var newParent = await _uow.ArticleCategoryRepository.GetByIdAsync(dto.ParentCategoryId.Value, cancellationToken);
                if (newParent == null) throw new KeyNotFoundException("Parent category not found.");

                // walk up parents to detect cycle
                var p = newParent;
                while (p != null)
                {
                    if (p.CategoryId == entity.CategoryId)
                        throw new InvalidOperationException("Changing parent would create a cycle.");
                    p = p.ParentCategoryId.HasValue ? await _uow.ArticleCategoryRepository.GetByIdAsync(p.ParentCategoryId.Value, cancellationToken) : null;
                }

                entity.ParentCategoryId = newParent.CategoryId;
            }
            else
            {
                entity.ParentCategoryId = null;
            }

            _uow.ArticleCategoryRepository.Update(entity, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id, CancellationToken cancellationToken = default)
        {
            // We want to check children and articles before deleting.
            // Since repository's GetByIdAsync doesn't include nav props, load all with includes and then find id.
            var allWithChildren = (await _uow.ArticleCategoryRepository.GetAllAsync(includeProperties: "SubCategories,Articles", ct: cancellationToken)).ToList();
            var entity = allWithChildren.FirstOrDefault(c => c.CategoryId == id);
            if (entity == null) return false;

            if ((entity.SubCategories != null && entity.SubCategories.Any()) ||
                (entity.Articles != null && entity.Articles.Any()))
            {
                // Policy: don't delete categories that have subcategories or attached articles.
                return false;
            }

            _uow.ArticleCategoryRepository.Remove(entity, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> MoveCategoryAsync(int id, int? newParentId, CancellationToken cancellationToken = default)
        {
            var entity = await _uow.ArticleCategoryRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null) return false;

            if (newParentId == id) throw new InvalidOperationException("Category cannot be parent of itself.");

            if (newParentId.HasValue)
            {
                var newParent = await _uow.ArticleCategoryRepository.GetByIdAsync(newParentId.Value, cancellationToken);
                if (newParent == null) throw new KeyNotFoundException("New parent not found.");

                // check cycle
                var p = newParent;
                while (p != null)
                {
                    if (p.CategoryId == entity.CategoryId)
                        throw new InvalidOperationException("Moving category would create a cycle.");
                    p = p.ParentCategoryId.HasValue ? await _uow.ArticleCategoryRepository.GetByIdAsync(p.ParentCategoryId.Value, cancellationToken) : null;
                }

                entity.ParentCategoryId = newParent.CategoryId;
            }
            else
            {
                entity.ParentCategoryId = null;
            }

            _uow.ArticleCategoryRepository.Update(entity, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task UpdateDisplayOrdersAsync(IEnumerable<(int CategoryId, int DisplayOrder)> orders, CancellationToken cancellationToken = default)
        {
            if (orders == null) return;

            var ids = orders.Select(o => o.CategoryId).ToList();
            var entities = (await _uow.ArticleCategoryRepository.GetAllAsync(includeProperties: null, ct: cancellationToken))
                .Where(c => ids.Contains(c.CategoryId)).ToList();

            var map = orders.ToDictionary(o => o.CategoryId, o => o.DisplayOrder);
            foreach (var e in entities)
            {
                if (map.TryGetValue(e.CategoryId, out var order))
                    e.DisplayOrder = order;
            }

            // repo.Update not strictly required for tracked entities, but call for clarity
            foreach (var e in entities) _uow.ArticleCategoryRepository.Update(e, cancellationToken);

            await _uow.SaveChangesAsync(cancellationToken);
        }
    }
}
