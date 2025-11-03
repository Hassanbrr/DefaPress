using AutoMapper;
using DefaPress.Application.DTOs;
using DefaPress.Application.Interfaces;
using DefaPress.Domain;
using DefaPress.Infrastructure.Modules.Base.Interfaces;
using Helps;

namespace DefaPress.Application.Services;

public class ArticleService : IArticleService
{
    private readonly IUnitOffWork _unitOffWork;
    private readonly IMapper _mapper;

    public ArticleService(IMapper mapper, IUnitOffWork unitOffWork)
    {
        _mapper = mapper;
        _unitOffWork = unitOffWork;
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
        var article = await _unitOffWork.ArticleRepository.GetByIdAsync(id, cancellationToken);
        if (article == null) return null;

        // Increment view count
        article.ViewsCount++;
        _unitOffWork.ArticleRepository.Update(article, cancellationToken);
        await _unitOffWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ArticleDetailDto>(article);
    }

    public async Task<int> CreateArticleAsync(ArticleCreateDto dto, CancellationToken cancellationToken = default)
    {
        var article = _mapper.Map<Article>(dto);

        // Generate slug if not provided - استفاده از SlugHelper
        if (string.IsNullOrEmpty(article.Slug))
        {
            article.Slug = SlugHelper.SlugifyPersian(article.Title);
        }

        // Set published date if publishing
        if (dto.IsPublished && !dto.PublishedAt.HasValue)
        {
            article.PublishedAt = DateTime.UtcNow;
        }

        await _unitOffWork.ArticleRepository.AddAsync(article, cancellationToken);
        await _unitOffWork.SaveChangesAsync(cancellationToken);

        return article.ArticleId;
    }

    public async Task<bool> UpdateArticleAsync(int id, ArticleUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var article = await _unitOffWork.ArticleRepository.GetByIdAsync(id, cancellationToken);
        if (article == null) return false;

        _mapper.Map(dto, article);

        // Generate slug if not provided - استفاده از SlugHelper
        if (string.IsNullOrEmpty(article.Slug))
        {
            article.Slug = SlugHelper.SlugifyPersian(article.Title);
        }

        // Update published date if status changed to published
        if (dto.IsPublished && !article.PublishedAt.HasValue)
        {
            article.PublishedAt = DateTime.UtcNow;
        }

        _unitOffWork.ArticleRepository.Update(article, cancellationToken);
        await _unitOffWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteArticleAsync(int id, CancellationToken cancellationToken = default)
    {
        var article = await _unitOffWork.ArticleRepository.GetByIdAsync(id, cancellationToken);
        if (article == null) return false;

        _unitOffWork.ArticleRepository.Remove(article, cancellationToken);
        await _unitOffWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> PublishArticleAsync(int id, CancellationToken cancellationToken = default)
    {
        var article = await _unitOffWork.ArticleRepository.GetByIdAsync(id, cancellationToken);
        if (article == null) return false;

        article.IsPublished = true;
        article.PublishedAt ??= DateTime.UtcNow;

        _unitOffWork.ArticleRepository.Update(article, cancellationToken);
        await _unitOffWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> UnpublishArticleAsync(int id, CancellationToken cancellationToken = default)
    {
        var article = await _unitOffWork.ArticleRepository.GetByIdAsync(id, cancellationToken);
        if (article == null) return false;

        article.IsPublished = false;

        _unitOffWork.ArticleRepository.Update(article, cancellationToken);
        await _unitOffWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    // متدهای جدید برای داشبورد
    public async Task<int> GetTotalArticlesCountAsync(CancellationToken cancellationToken = default)
    {
        var articles = await _unitOffWork.ArticleRepository.GetAllAsync();
        return articles.Count();
    }

    public async Task<int> GetPublishedArticlesCountAsync(CancellationToken cancellationToken = default)
    {
        var articles = await _unitOffWork.ArticleRepository.GetAllAsync();
        return articles.Count(a => a.IsPublished);
    }

    public async Task<IEnumerable<ArticleListDto>> GetRecentArticlesAsync(int count, CancellationToken cancellationToken = default)
    {
        var allArticles = await _unitOffWork.ArticleRepository.GetAllAsync(
            includeProperties: "ArticleCategory,Author");

        var recentArticles = allArticles
            .OrderByDescending(a => a.CreatedAt)
            .Take(count)
            .ToList();

        return _mapper.Map<IEnumerable<ArticleListDto>>(recentArticles);
    }

    public async Task<IEnumerable<ArticleChartDataDto>> GetArticlesChartDataAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        // دریافت تمام مقالات و فیلتر کردن در حافظه
        var allArticles = await _unitOffWork.ArticleRepository.GetAllAsync();

        var filteredArticles = allArticles
            .Where(a => a.CreatedAt >= startDate && a.CreatedAt <= endDate)
            .ToList();

        var chartData = filteredArticles
            .GroupBy(a => a.CreatedAt.Date)
            .Select(g => new ArticleChartDataDto
            {
                Date = g.Key.ToString("MM/dd"),
                Count = g.Count()
            })
            .OrderBy(x => x.Date)
            .ToList();

        // پر کردن روزهای گم‌شده
        var fullChartData = FillMissingDays(chartData, startDate, endDate);

        return fullChartData;
    }

    private List<ArticleChartDataDto> FillMissingDays(List<ArticleChartDataDto> existingData, DateTime startDate, DateTime endDate)
    {
        var result = new List<ArticleChartDataDto>();
        var existingDataDict = existingData.ToDictionary(x => x.Date, x => x.Count);

        for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
        {
            var dateString = date.ToString("MM/dd");
            var count = existingDataDict.ContainsKey(dateString) ? existingDataDict[dateString] : 0;

            result.Add(new ArticleChartDataDto
            {
                Date = dateString,
                Count = count
            });
        }

        return result;
    }
}