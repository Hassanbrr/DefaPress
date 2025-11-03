using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DefaPress.Application.Interfaces;
using DefaPress.Application.DTOs;

namespace DefaPress.Presentation.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class DashboardController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IArticleCategoryService _categoryService;

        public DashboardController(IArticleService articleService, IArticleCategoryService categoryService)
        {
            _articleService = articleService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetAllArticlesAsync();
            var categories = await _categoryService.GetAllCategoriesAsync();

            var model = new DashboardViewModel
            {
                TotalArticles = articles.Count(),
                PublishedArticles = articles.Count(a => a.IsPublished),
                TotalCategories = categories.Count(),
                RecentArticles = articles.OrderByDescending(a => a.CreatedAt).Take(5)
            };

            return View(model);
        }

        public async Task<JsonResult> GetArticlesChartData()
        {
            var articles = await _articleService.GetAllArticlesAsync();
            var last30Days = articles.Where(a => a.CreatedAt >= DateTime.UtcNow.AddDays(-30));

            var chartData = last30Days
                .GroupBy(a => a.CreatedAt.Date)
                .Select(g => new { Date = g.Key.ToString("MM/dd"), Count = g.Count() })
                .OrderBy(x => x.Date);

            return Json(chartData);
        }
    }

    public class DashboardViewModel
    {
        public int TotalArticles { get; set; }
        public int PublishedArticles { get; set; }
        public int TotalCategories { get; set; }
        public IEnumerable<ArticleListDto> RecentArticles { get; set; }
    }
}