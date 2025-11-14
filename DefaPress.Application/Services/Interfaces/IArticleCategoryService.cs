using DefaPress.Application.DTOs;

namespace DefaPress.Application.Services.Interfaces
{
    public interface IArticleCategoryService
    {
        Task<IEnumerable<ArticleCategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<ArticleCategoryDto>> GetCategoryTreeAsync(int maxDepth = 5, CancellationToken cancellationToken = default);
        Task<ArticleCategoryDto?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> AddCategoryAsync(ArticleCategoryCreateDto dto, CancellationToken cancellationToken = default);
        Task<bool> UpdateCategoryAsync(int id, ArticleCategoryCreateDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteCategoryAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> MoveCategoryAsync(int id, int? newParentId, CancellationToken cancellationToken = default);
        Task UpdateDisplayOrdersAsync(IEnumerable<(int CategoryId, int DisplayOrder)> orders, CancellationToken cancellationToken = default);
 
    }
}