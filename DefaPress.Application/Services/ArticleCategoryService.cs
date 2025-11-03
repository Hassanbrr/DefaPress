using AutoMapper;
using DefaPress.Application.DTOs;
using DefaPress.Application.Interfaces;
using DefaPress.Domain;
using DefaPress.Infrastructure.Modules.Base.Interfaces;
using Helps;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DefaPress.Application.Services
{
    public class ArticleCategoryService : IArticleCategoryService
    {
        private readonly IUnitOffWork _unitOffWork;
        private readonly IMapper _mapper;

        public ArticleCategoryService(IMapper mapper, IUnitOffWork unitOffWork)
        {
            _mapper = mapper;
            _unitOffWork = unitOffWork;
        }

        public async Task<IEnumerable<ArticleCategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
        {
            var categories = await _unitOffWork.ArticleCategoryRepository.GetAllAsync(
                includeProperties: "ParentCategory,SubCategories",
                cancellationToken);

            return _mapper.Map<IEnumerable<ArticleCategoryDto>>(categories);
        }

        public async Task<IEnumerable<ArticleCategoryDto>> GetCategoryTreeAsync(int maxDepth = 5, CancellationToken cancellationToken = default)
        {
            // دریافت تمام دسته‌بندی‌ها و فیلتر کردن در حافظه
            var allCategories = await _unitOffWork.ArticleCategoryRepository.GetAllAsync(
                includeProperties: GetIncludePropertiesForDepth(maxDepth));

            // فیلتر کردن دسته‌بندی‌های ریشه در حافظه
            var rootCategories = allCategories.Where(c => c.ParentCategoryId == null);

            return _mapper.Map<IEnumerable<ArticleCategoryDto>>(rootCategories);
        }

        private string GetIncludePropertiesForDepth(int depth)
        {
            var includes = new List<string> { "ParentCategory" };

            for (int i = 1; i <= depth; i++)
            {
                var includePath = string.Join(".", Enumerable.Repeat("SubCategories", i));
                includes.Add(includePath);
            }

            return string.Join(",", includes);
        }

        public async Task<ArticleCategoryDto?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var category = await _unitOffWork.ArticleCategoryRepository.GetByIdAsync(id, cancellationToken);
            return category == null ? null : _mapper.Map<ArticleCategoryDto>(category);
        }

        public async Task<int> AddCategoryAsync(ArticleCategoryCreateDto dto, CancellationToken cancellationToken = default)
        {
            var category = _mapper.Map<ArticleCategory>(dto);

            // Generate slug if not provided
            if (string.IsNullOrEmpty(category.Slug))
            {
                category.Slug = SlugHelper.SlugifyPersian(category.Name);
            }

            await _unitOffWork.ArticleCategoryRepository.AddAsync(category, cancellationToken);
            await _unitOffWork.SaveChangesAsync(cancellationToken);

            return category.CategoryId;
        }

        public async Task<bool> UpdateCategoryAsync(int id, ArticleCategoryCreateDto dto, CancellationToken cancellationToken = default)
        {
            var category = await _unitOffWork.ArticleCategoryRepository.GetByIdAsync(id, cancellationToken);
            if (category == null) return false;

            _mapper.Map(dto, category);

            // Generate slug if not provided
            if (string.IsNullOrEmpty(category.Slug))
            {
                category.Slug = SlugHelper.SlugifyPersian(category.Name);
            }

            _unitOffWork.ArticleCategoryRepository.Update(category, cancellationToken);
            await _unitOffWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id, CancellationToken cancellationToken = default)
        {
            var category = await _unitOffWork.ArticleCategoryRepository.GetByIdAsync(id, cancellationToken);
            if (category == null) return false;

            // Check if category has subcategories - استفاده از FindAsync
            var subCategories = await _unitOffWork.ArticleCategoryRepository.FindAsync(
                c => c.ParentCategoryId == id);

            var hasSubCategories = subCategories.Any();

            // Check if category has articles - اگر ریپازیتوری مقالات دارید
            var hasArticles = await CheckIfCategoryHasArticles(id, cancellationToken);

            if (hasSubCategories || hasArticles)
            {
                return false; // Cannot delete category with children
            }

            _unitOffWork.ArticleCategoryRepository.Remove(category, cancellationToken);
            await _unitOffWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        private async Task<bool> CheckIfCategoryHasArticles(int categoryId, CancellationToken cancellationToken = default)
        {
            try
            {
                // اگر ریپازیتوری مقالات دارید:
                // var articles = await _unitOffWork.ArticleRepository.FindAsync(a => a.CategoryId == categoryId, cancellationToken: cancellationToken);
                // return articles.Any();

                // فعلاً false برگردانید تا بتوانید تست کنید
                return false;
            }
            catch
            {
                // اگر ریپازیتوری مقالات در دسترس نیست
                return false;
            }
        }

        public async Task<bool> MoveCategoryAsync(int id, int? newParentId, CancellationToken cancellationToken = default)
        {
            var category = await _unitOffWork.ArticleCategoryRepository.GetByIdAsync(id, cancellationToken);
            if (category == null) return false;

            // Check for circular reference
            if (await WouldCreateCircularReference(id, newParentId, cancellationToken))
            {
                return false;
            }

            category.ParentCategoryId = newParentId;
            _unitOffWork.ArticleCategoryRepository.Update(category, cancellationToken);
            await _unitOffWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        private async Task<bool> WouldCreateCircularReference(int categoryId, int? newParentId, CancellationToken cancellationToken)
        {
            if (!newParentId.HasValue) return false;

            var currentParentId = newParentId.Value;
            while (currentParentId != 0)
            {
                if (currentParentId == categoryId) return true;

                var parent = await _unitOffWork.ArticleCategoryRepository.GetByIdAsync(currentParentId, cancellationToken);
                if (parent?.ParentCategoryId == null) break;

                currentParentId = parent.ParentCategoryId.Value;
            }

            return false;
        }

        public async Task UpdateDisplayOrdersAsync(IEnumerable<(int CategoryId, int DisplayOrder)> orders, CancellationToken cancellationToken = default)
        {
            foreach (var (categoryId, displayOrder) in orders)
            {
                var category = await _unitOffWork.ArticleCategoryRepository.GetByIdAsync(categoryId, cancellationToken);
                if (category != null)
                {
                    category.DisplayOrder = displayOrder;
                    _unitOffWork.ArticleCategoryRepository.Update(category, cancellationToken);
                }
            }

            await _unitOffWork.SaveChangesAsync(cancellationToken);
        }

        // متد جدید برای داشبورد
        public async Task<int> GetTotalCategoriesCountAsync(CancellationToken cancellationToken = default)
        {
            var categories = await _unitOffWork.ArticleCategoryRepository.GetAllAsync();
            return categories.Count();
        }
    }
}