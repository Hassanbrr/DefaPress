using DefaPress.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DefaPress.Presentation.Web.Areas.Client.ViewComponents
{
    public class ArticleCategoriesViewComponent : ViewComponent
    {
        private readonly IArticleCategoryService _articleCategoryService;

        public ArticleCategoriesViewComponent(IArticleCategoryService articleCategoryService)
        {
            _articleCategoryService = articleCategoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _articleCategoryService.GetAllCategoriesAsync();
            return View(categories);
        }
    }
}
