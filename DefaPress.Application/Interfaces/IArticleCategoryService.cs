using DefaPress.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Application.Interfaces
{
    public interface IArticleCategoryService
    {
        Task<IEnumerable<ArticleCategoryDto>> GetAllCategoriesAsync();
        Task<ArticleCategoryDto> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(ArticleCategoryDto categoryDto);
        Task UpdateCategoryAsync(ArticleCategoryDto categoryDto);
        Task DeleteCategoryAsync(int id);
    }
}
