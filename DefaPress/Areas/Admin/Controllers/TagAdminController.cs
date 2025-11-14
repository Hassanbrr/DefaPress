using DefaPress.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DefaPress.Presentation.Web.Areas.Admin.Controllers
{
    // در کنترلر TagAdminController
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin,Editor")]
    public class TagAdminController : Controller
    {
        private readonly ITagService _tagService;

        public TagAdminController(ITagService tagService)
        {
            _tagService = tagService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            var tags = await _tagService.GetAllTagsAsync(cancellationToken);
            return View(tags);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Json(new { success = false, message = "نام تگ نمی‌تواند خالی باشد." });

            var tag = await _tagService.CreateTagAsync(name, cancellationToken);
            return Json(new { success = true, message = "تگ با موفقیت ایجاد شد.", tag = new { id = tag.TagId, text = tag.Name } });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var result = await _tagService.DeleteTagAsync(id, cancellationToken);
            if (result)
                return Json(new { success = true, message = "تگ با موفقیت حذف شد." });

            return Json(new { success = false, message = "خطا در حذف تگ." });
        }
    }
}
