using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DefaPress.Application.Interfaces;
using DefaPress.Application.DTOs;

namespace DefaPress.Presentation.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin,Editor,Author")]
    public class ArticleAdminController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IArticleCategoryService _categoryService;

        public ArticleAdminController(IArticleService articleService, IArticleCategoryService categoryService)
        {
            _articleService = articleService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetAllArticlesAsync();
            return View(articles);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = categories;
            return PartialView("_CreateModal");
        }

        [HttpPost]
     
        public async Task<IActionResult> Create(ArticleCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                // Set current user as author
                dto.AuthorId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                var articleId = await _articleService.CreateArticleAsync(dto);
                if (articleId > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return Json(new { success = false, message = "خطا در ایجاد مقاله" });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var article = await _articleService.GetArticleByIdAsync(id);
            if (article == null) return NotFound();

            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = categories;

            // مپ کردن کامل خصوصیات از ArticleDetailDto به ArticleUpdateDto
            var updateDto = new ArticleUpdateDto
            {
                ArticleId = article.ArticleId,
                Title = article.Title,
                Content = article.Content,
                Summary = article.Summary,
                Slug = article.Slug,
                ImageUrl = article.ImageUrl,
                Source = article.Source,
                IsPublished = article.IsPublished,
                IsBreakingNews = article.IsBreakingNews,
                IsFeatured = article.IsFeatured,
                PublishedAt = article.PublishedAt,
                ArticleCategoryId = article.ArticleCategoryId,
                // اگر تگ‌ها و فایل‌های رسانه‌ای نیاز دارید، آنها را هم اضافه کنید
            };

            return PartialView("_EditModal", updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ArticleUpdateDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _articleService.UpdateArticleAsync(dto.ArticleId, dto);
                if (result)
                {
                    return Json(new { success = true, message = "مقاله با موفقیت ویرایش شد" });
                }
            }
            return Json(new { success = false, message = "خطا در ویرایش مقاله" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _articleService.DeleteArticleAsync(id);
            if (result)
            {
                return Json(new { success = true, message = "مقاله با موفقیت حذف شد" });
            }
            return Json(new { success = false, message = "خطا در حذف مقاله" });
        }

        [HttpPost]
        public async Task<IActionResult> Publish(int id)
        {
            var result = await _articleService.PublishArticleAsync(id);
            if (result)
            {
                return Json(new { success = true, message = "مقاله با موفقیت منتشر شد" });
            }
            return Json(new { success = false, message = "خطا در انتشار مقاله" });
        }

        [HttpPost]
        public async Task<IActionResult> Unpublish(int id)
        {
            var result = await _articleService.UnpublishArticleAsync(id);
            if (result)
            {
                return Json(new { success = true, message = "مقاله از حالت انتشار خارج شد" });
            }
            return Json(new { success = false, message = "خطا در تغییر وضعیت مقاله" });
        }
    }
}