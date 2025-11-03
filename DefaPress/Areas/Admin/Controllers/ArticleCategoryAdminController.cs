using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DefaPress.Application.Interfaces;
using DefaPress.Application.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace DefaPress.Presentation.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin,Editor")]
    public class ArticleCategoryAdminController : Controller
    {
        private readonly IArticleCategoryService _categoryService;

        public ArticleCategoryAdminController(IArticleCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadCategoryTree()
        {
            var categories = await _categoryService.GetCategoryTreeAsync();
            return PartialView("_CategoryTree", categories);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryStats()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            var stats = new
            {
                Total = categories.Count(),
                RootCategories = categories.Count(c => c.ParentCategoryId == null),
                SubCategories = categories.Count(c => c.ParentCategoryId != null),
                MaxDepth = CalculateMaxDepth(categories)
            };
            return Json(stats);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetCategoryTreeAsync();
            ViewBag.ParentCategories = categories;
            return PartialView("_CreateModal");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticleCategoryCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                var categoryId = await _categoryService.AddCategoryAsync(dto);
                if (categoryId > 0)
                {
                    return Json(new { success = true, message = "دسته‌بندی با موفقیت ایجاد شد" });
                }
            }
            return Json(new { success = false, message = "خطا در ایجاد دسته‌بندی" });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();

            var categories = await _categoryService.GetCategoryTreeAsync();
            ViewBag.ParentCategories = categories;

            var updateDto = new ArticleCategoryCreateDto
            {
                Name = category.Name,
                Slug = category.Slug,
                Description = category.Description,
                ParentCategoryId = category.ParentCategoryId,
                DisplayOrder = category.DisplayOrder
            };

            ViewBag.CategoryId = id;
            return PartialView("_EditModal", updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ArticleCategoryCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryService.UpdateCategoryAsync(id, dto);
                if (result)
                {
                    return Json(new { success = true, message = "دسته‌بندی با موفقیت ویرایش شد" });
                }
            }
            return Json(new { success = false, message = "خطا در ویرایش دسته‌بندی" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (result)
            {
                return Json(new { success = true, message = "دسته‌بندی با موفقیت حذف شد" });
            }
            return Json(new { success = false, message = "خطا در حذف دسته‌بندی. ممکن است دارای زیردسته یا مقالات باشد." });
        }

        [HttpPost]
        public async Task<IActionResult> MoveCategory(int id, int? newParentId)
        {
            var result = await _categoryService.MoveCategoryAsync(id, newParentId);
            if (result)
            {
                return Json(new { success = true, message = "دسته‌بندی با موفقیت جابجا شد" });
            }
            return Json(new { success = false, message = "خطا در جابجایی دسته‌بندی" });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDisplayOrders([FromBody] List<(int CategoryId, int DisplayOrder)> orders)
        {
            await _categoryService.UpdateDisplayOrdersAsync(orders);
            return Json(new { success = true, message = "ترتیب نمایش با موفقیت به‌روزرسانی شد" });
        }

        private int CalculateMaxDepth(IEnumerable<ArticleCategoryDto> categories)
        {
            int maxDepth = 0;
            foreach (var category in categories.Where(c => c.ParentCategoryId == null))
            {
                maxDepth = Math.Max(maxDepth, CalculateDepth(category, categories));
            }
            return maxDepth;
        }

        private int CalculateDepth(ArticleCategoryDto category, IEnumerable<ArticleCategoryDto> allCategories, int currentDepth = 1)
        {
            int maxDepth = currentDepth;
            var children = allCategories.Where(c => c.ParentCategoryId == category.CategoryId);
            foreach (var child in children)
            {
                maxDepth = Math.Max(maxDepth, CalculateDepth(child, allCategories, currentDepth + 1));
            }
            return maxDepth;
        }
    }
}