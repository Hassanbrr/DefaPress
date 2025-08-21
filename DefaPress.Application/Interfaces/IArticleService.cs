 
using DefaPress.Application.DTOs;

namespace DefaPress.Application.Interfaces
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticleDto>> GetAllArticlesAsync();
        Task<ArticleDto> GetArticleByIdAsync(int id);
        Task AddArticleAsync(ArticleDto articleDto);
        Task UpdateArticleAsync(ArticleDto articleDto);
        Task DeleteArticleAsync(int id);

        Task PublishArticleAsync(int id);
        Task UnpublishArticleAsync(int id);

    }
}
