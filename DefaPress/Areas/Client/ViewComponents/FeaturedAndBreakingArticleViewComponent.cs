using DefaPress.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DefaPress.Presentation.Web.Areas.Client.ViewComponents
{
    public class FeaturedAndBreakingArticleViewComponent : ViewComponent
    {
        private readonly IArticleService _articleService;
        public FeaturedAndBreakingArticleViewComponent(IArticleService articleService)
        {
            _articleService = articleService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
          var articles =  await  _articleService.GetAllFeaturedAndBreakingArticles();
            return View(articles);
        }
    }
}
