using DefaPress.Application.DTOs;
using DefaPress.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DefaPress.Presentation.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleCategoryAdminController : Controller
    {
        private readonly IArticleCategoryService _articleCategoryService;

        public ArticleCategoryAdminController(IArticleCategoryService articleCategoryService)
        {
            _articleCategoryService = articleCategoryService;
        }
        [HttpGet]

        public async Task<IActionResult> Index()
        {
            var categories = await _articleCategoryService.GetAllCategoriesAsync();

            return View(categories);
        }

        // DataTable Ajax Source


        [HttpGet]
        public async Task<IActionResult> Upsert(int? id, CancellationToken ct)
        {
            var categories = await _articleCategoryService.GetAllCategoriesAsync();
            ViewBag.ParentCategories = categories
                .Where(c => id == null || c.CategoryId != id.Value) // حذف دسته جاری
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                }).ToList();
            if (id == null)
            {
                // حالت Create
                return PartialView("_Upsert", new ArticleCategoryCreateDto());
            }

            // حالت Edit
            var category = await _articleCategoryService.GetCategoryByIdAsync(id.Value, ct);
            if (category == null) return NotFound();

            var dto = new ArticleCategoryCreateDto
            {
                Name = category.Name,
                Slug = category.Slug,
                Description = category.Description,
                ParentCategoryId = category.ParentCategoryId,
                DisplayOrder = category.DisplayOrder
            };

            ViewBag.CategoryId = id.Value;
            return PartialView("_Upsert", dto);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(int? id, ArticleCategoryCreateDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return PartialView("_Upsert", dto);

            if (id == null || id == 0)
            {
                // Create
                await _articleCategoryService.AddCategoryAsync(dto, ct);
            }
            else
            {
                // Update
                await _articleCategoryService.UpdateCategoryAsync(id.Value, dto, ct);
            }

            return Json(new { success = true });
        }

    }
}
