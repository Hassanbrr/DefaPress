using DefaPress.Application.DTOs;

namespace DefaPress.Application.Services.Interfaces
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticleListDto>> GetAllArticlesAsync(CancellationToken cancellationToken = default);
        Task<ArticleDetailDto?> GetArticleByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<ArticleListDto>> GetAllFeaturedAndBreakingArticles( CancellationToken cancellationToken = default);

        Task<List<int>> GetArticleTagIdsAsync(int articleId, CancellationToken cancellationToken = default);

        Task<int> CreateArticleAsync(ArticleCreateDto dto, CancellationToken cancellationToken = default);
        Task<bool> UpdateArticleAsync(int id, ArticleUpdateDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteArticleAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> PublishArticleAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> UnpublishArticleAsync(int id, CancellationToken cancellationToken = default);


        Task<bool> UpdateArticleTagsAsync(int articleId, List<int> tagIds,
            CancellationToken cancellationToken = default);

    }
}