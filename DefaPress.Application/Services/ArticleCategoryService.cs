using DefaPress.Application.Interfaces; 
using DefaPress.Application.DTOs;

namespace DefaPress.Application.Services
{
    public class ArticleCategoryService : IArticleCategoryService
    {
        public Task<IEnumerable<ArticleCategoryDto>> GetAllCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ArticleCategoryDto> GetCategoryByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task AddCategoryAsync(ArticleCategoryDto categoryDto)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCategoryAsync(ArticleCategoryDto categoryDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCategoryAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
