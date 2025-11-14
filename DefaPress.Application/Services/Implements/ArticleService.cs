using AutoMapper;
using DefaPress.Application.DTOs;
using DefaPress.Application.Services.Interfaces;
using DefaPress.Domain;
using DefaPress.Infrastructure.Modules.Base.Interfaces;
using Helps; 
using Microsoft.AspNetCore.Identity;

namespace DefaPress.Application.Services.Implements;

public class ArticleService : IArticleService
{
    private readonly IUnitOffWork _unitOffWork;
    private readonly IMapper _mapper;
    private readonly IImageService _imageService;
    private readonly UserManager<ApplicationUser> _userManager ;


    public ArticleService(IMapper mapper, IUnitOffWork unitOffWork, IImageService imageService, UserManager<ApplicationUser> userManager)
    {
        _mapper = mapper;
        _unitOffWork = unitOffWork;
        _imageService = imageService;
        _userManager = userManager;
    }

    public async Task<IEnumerable<ArticleListDto>> GetAllArticlesAsync(CancellationToken cancellationToken = default)
    {
        var articles = await _unitOffWork.ArticleRepository.GetAllAsync(
            includeProperties: "ArticleCategory,Author,Tags,MediaFiles",
            cancellationToken);

        return _mapper.Map<IEnumerable<ArticleListDto>>(articles);
    }

    public async Task<ArticleDetailDto?> GetArticleByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        // استفاده از include برای لود کردن تگ‌ها
        var article = await _unitOffWork.ArticleRepository.GetByIdAsync(id, "Tags,ArticleCategory,Author", cancellationToken);
        if (article == null) return null;

        // Increment view count
        article.ViewsCount++;
        _unitOffWork.ArticleRepository.Update(article, cancellationToken);
        await _unitOffWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ArticleDetailDto>(article);
    }

    public async Task<IEnumerable<ArticleListDto>> GetAllFeaturedAndBreakingArticles(CancellationToken cancellationToken = default)
    {
        var articles = await _unitOffWork.ArticleRepository.FindAsync(a => a.IsBreakingNews || a.IsFeatured, "ArticleCategory,Tags,Author");
        return _mapper.Map<IEnumerable<ArticleListDto>>(articles);
    }

    public async Task<int> CreateArticleAsync(ArticleCreateDto dto, CancellationToken cancellationToken = default)
    {
        var article = _mapper.Map<Article>(dto);

        // آپلود تصویر شاخص اگر وجود دارد
        if (dto.ImageFile != null && dto.ImageFile.Length > 0)
        {
            var uploadResult = await _imageService.UploadImageAsync(dto.ImageFile, "uploads/images/articles/featured");
            if (uploadResult.Success)
            {
                article.ImageUrl = $"/{uploadResult.OptimizedPath}";
            }
        }
        if (string.IsNullOrEmpty(article.AuthorId))
        {
            var user = await _userManager.FindByIdAsync(article.AuthorId);

            article.Author = user;
        }
        // Generate slug if not provided
        if (string.IsNullOrEmpty(article.Slug))
        {
            article.Slug = SlugHelper.SlugifyPersian(article.Title);
        }

        // Set published date if publishing
        if (dto.IsPublished && !dto.PublishedAt.HasValue)
        {
            article.PublishedAt = DateTime.UtcNow;
        }

        // اضافه کردن تگ‌ها - با چک کردن null
        if (dto.TagIds != null && dto.TagIds.Any())
        {
            // مطمئن شویم که Tags collection مقداردهی شده است
            article.Tags = article.Tags ?? new List<Tag>();

            foreach (var tagId in dto.TagIds)
            {
                var tag = await _unitOffWork.TagRepository.GetByIdAsync(tagId, null, cancellationToken);
                if (tag != null)
                {
                    article.Tags.Add(tag);
                }
            }
        }

        await _unitOffWork.ArticleRepository.AddAsync(article, cancellationToken);
        await _unitOffWork.SaveChangesAsync(cancellationToken);

        return article.ArticleId;
    }

    public async Task<bool> UpdateArticleAsync(int id, ArticleUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var article = await _unitOffWork.ArticleRepository.GetByIdAsync(id, "Tags", cancellationToken);
        if (article == null) return false;

        // آپلود تصویر جدید اگر وجود دارد
        if (dto.ImageFile != null && dto.ImageFile.Length > 0)
        {
            // حذف تصویر قبلی اگر وجود دارد
            if (!string.IsNullOrEmpty(article.ImageUrl))
            {
                await _imageService.DeleteImageAsync(article.ImageUrl.TrimStart('/'));
            }

            // آپلود تصویر جدید
            var uploadResult = await _imageService.UploadImageAsync(dto.ImageFile, "uploads/images/articles/featured");
            if (uploadResult.Success)
            {
                article.ImageUrl = $"/{uploadResult.OptimizedPath}";
            }
        }
        else if (!string.IsNullOrEmpty(dto.ImageUrl))
        {
            // اگر URL مستقیم ارائه شده
            article.ImageUrl = dto.ImageUrl;
        }
        if (string.IsNullOrEmpty(article.AuthorId))
        {
            var user = await _userManager.FindByIdAsync(article.AuthorId);

            article.Author = user;
        }
        _mapper.Map(dto, article);

        // Generate slug if not provided
        if (string.IsNullOrEmpty(article.Slug))
        {
            article.Slug = SlugHelper.SlugifyPersian(article.Title);
        }

        // Update published date if status changed to published
        if (dto.IsPublished && !article.PublishedAt.HasValue)
        {
            article.PublishedAt = DateTime.UtcNow;
        }

        // به‌روزرسانی تگ‌ها
        await UpdateArticleTagsAsync(id, dto.TagIds, cancellationToken);

        _unitOffWork.ArticleRepository.Update(article, cancellationToken);
        await _unitOffWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteArticleAsync(int id, CancellationToken cancellationToken = default)
    {
        var article = await _unitOffWork.ArticleRepository.GetByIdAsync(id, null, cancellationToken);
        if (article == null) return false;

        // حذف تصویر مرتبط اگر وجود دارد
        if (!string.IsNullOrEmpty(article.ImageUrl))
        {
            await _imageService.DeleteImageAsync(article.ImageUrl.TrimStart('/'));
        }

        _unitOffWork.ArticleRepository.Remove(article, cancellationToken);
        await _unitOffWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> PublishArticleAsync(int id, CancellationToken cancellationToken = default)
    {
        var article = await _unitOffWork.ArticleRepository.GetByIdAsync(id, null, cancellationToken);
        if (article == null) return false;

        article.IsPublished = true;
        article.PublishedAt ??= DateTime.UtcNow;

        _unitOffWork.ArticleRepository.Update(article, cancellationToken);
        await _unitOffWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> UnpublishArticleAsync(int id, CancellationToken cancellationToken = default)
    {
        var article = await _unitOffWork.ArticleRepository.GetByIdAsync(id, null, cancellationToken);
        if (article == null) return false;

        article.IsPublished = false;

        _unitOffWork.ArticleRepository.Update(article, cancellationToken);
        await _unitOffWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> UpdateArticleTagsAsync(int articleId, List<int> tagIds, CancellationToken cancellationToken = default)
    {
        var article = await _unitOffWork.ArticleRepository.GetByIdAsync(articleId, "Tags", cancellationToken);
        if (article == null) return false;

        // مطمئن شویم که Tags collection مقداردهی شده است
        article.Tags = article.Tags ?? new List<Tag>();

        // حذف تگ‌های موجود
        article.Tags.Clear();

        // اضافه کردن تگ‌های جدید
        if (tagIds != null && tagIds.Any())
        {
            foreach (var tagId in tagIds)
            {
                var tag = await _unitOffWork.TagRepository.GetByIdAsync(tagId, null, cancellationToken);
                if (tag != null)
                {
                    article.Tags.Add(tag);
                }
            }
        }

        _unitOffWork.ArticleRepository.Update(article, cancellationToken);
        await _unitOffWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<List<int>> GetArticleTagIdsAsync(int articleId, CancellationToken cancellationToken = default)
    {
        var article = await _unitOffWork.ArticleRepository.GetByIdAsync(articleId, "Tags", cancellationToken);
        if (article == null) return new List<int>();

        return article.Tags?.Select(t => t.TagId).ToList() ?? new List<int>();
    }
}