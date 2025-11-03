using DefaPress.Application.DTOs;

namespace DefaPress.Application.Interfaces
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticleListDto>> GetAllArticlesAsync(CancellationToken cancellationToken = default);
        Task<ArticleDetailDto?> GetArticleByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CreateArticleAsync(ArticleCreateDto dto, CancellationToken cancellationToken = default);
        Task<bool> UpdateArticleAsync(int id, ArticleUpdateDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteArticleAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> PublishArticleAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> UnpublishArticleAsync(int id, CancellationToken cancellationToken = default);

        // متدهای جدید برای داشبورد
        Task<int> GetTotalArticlesCountAsync(CancellationToken cancellationToken = default);
        Task<int> GetPublishedArticlesCountAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<ArticleListDto>> GetRecentArticlesAsync(int count, CancellationToken cancellationToken = default);
        Task<IEnumerable<ArticleChartDataDto>> GetArticlesChartDataAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    }
}