using AutoMapper;
using DefaPress.Application.DTOs;
using DefaPress.Application.Services.Interfaces;
using DefaPress.Application.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DefaPress.Presentation.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin,Editor")]
    public class ArticleAdminController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IArticleCategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService; 
        public ArticleAdminController(
            IArticleService articleService,
            IArticleCategoryService categoryService,
            ITagService tagService,
            IMapper mapper,
            IImageService imageService)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _tagService = tagService;
            _mapper = mapper;
            _imageService = imageService; 
        }

        // GET: ArticleAdmin
        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            var articles = await _articleService.GetAllArticlesAsync(cancellationToken);
            return View(articles);
        }

        // GET: ArticleAdmin/Create
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            await PopulateViewBag(cancellationToken);
            return PartialView("_Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticleCreateDto dto, CancellationToken cancellationToken = default)
        {
            // دیباگ: چک کردن مقادیر ورودی
            Console.WriteLine($"Title: {dto.Title}");
            Console.WriteLine($"TagIds Count: {dto.TagIds?.Count}");
            if (dto.TagIds != null)
            {
                Console.WriteLine($"TagIds: {string.Join(", ", dto.TagIds)}");
            }

            var validator = new ArticleCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(dto, cancellationToken);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                await PopulateViewBag(cancellationToken);
                return PartialView("_Create", dto);
            }
          
            dto.AuthorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var articleId = await _articleService.CreateArticleAsync(dto, cancellationToken);
            
            if (articleId > 0)
            {
                return Json(new { success = true, message = "مقاله با موفقیت ایجاد شد." });
            }

            ModelState.AddModelError(string.Empty, "خطا در ایجاد مقاله.");
            await PopulateViewBag(cancellationToken);
            return PartialView("_Create", dto);
        }

        // GET: ArticleAdmin/Edit/5
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken = default)
        {
            var article = await _articleService.GetArticleByIdAsync(id, cancellationToken);
            if (article == null)
            {
                return NotFound();
            }

            var updateDto = _mapper.Map<ArticleUpdateDto>(article);

            // گرفتن تگ‌های فعلی مقاله
            var currentTagIds = await _articleService.GetArticleTagIdsAsync(id, cancellationToken);
            updateDto.TagIds = currentTagIds;

            await PopulateViewBag(cancellationToken);
            return PartialView("_Edit", updateDto);
        }

        // POST: ArticleAdmin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ArticleUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var validator = new ArticleUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(dto, cancellationToken);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                await PopulateViewBag(cancellationToken);
                return PartialView("_Edit", dto);
            }
            dto.AuthorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _articleService.UpdateArticleAsync(dto.ArticleId, dto, cancellationToken);

            if (result)
            {
                return Json(new { success = true, message = "مقاله با موفقیت به‌روزرسانی شد." });
            }

            ModelState.AddModelError(string.Empty, "خطا در به‌روزرسانی مقاله.");
            await PopulateViewBag(cancellationToken);
            return PartialView("_Edit", dto);
        }

        // POST: ArticleAdmin/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var result = await _articleService.DeleteArticleAsync(id, cancellationToken);
            if (result)
            {
                return Json(new { success = true, message = "مقاله با موفقیت حذف شد." });
            }
            return Json(new { success = false, message = "خطا در حذف مقاله." });
        }

        // POST: ArticleAdmin/Publish/5
        [HttpPost]
        public async Task<IActionResult> Publish(int id, CancellationToken cancellationToken = default)
        {
            var result = await _articleService.PublishArticleAsync(id, cancellationToken);
            if (result)
            {
                return Json(new { success = true, message = "مقاله با موفقیت منتشر شد." });
            }
            return Json(new { success = false, message = "خطا در انتشار مقاله." });
        }

        // POST: ArticleAdmin/Unpublish/5
        [HttpPost]
        public async Task<IActionResult> Unpublish(int id, CancellationToken cancellationToken = default)
        {
            var result = await _articleService.UnpublishArticleAsync(id, cancellationToken);
            if (result)
            {
                return Json(new { success = true, message = "مقاله با موفقیت از حالت انتشار خارج شد." });
            }
            return Json(new { success = false, message = "خطا در عدم انتشار مقاله." });
        }

        // GET: ArticleAdmin/GetTags
        [HttpGet]
        public async Task<IActionResult> GetTags(string search, CancellationToken cancellationToken = default)
        {
            var tags = await _tagService.SearchTagsAsync(search, cancellationToken);
            return Json(tags.Select(t => new { id = t.TagId, text = t.Name }));
        }

        [HttpGet]
        public async Task<IActionResult> GetArticleTags(int articleId, CancellationToken cancellationToken = default)
        {
            var tagIds = await _articleService.GetArticleTagIdsAsync(articleId, cancellationToken);
            return Json(new { tagIds });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateArticleTags(int articleId, [FromBody] List<int> tagIds, CancellationToken cancellationToken = default)
        {
            var result = await _articleService.UpdateArticleTagsAsync(articleId, tagIds, cancellationToken);
            if (result)
            {
                return Json(new { success = true, message = "تگ‌های مقاله با موفقیت به‌روزرسانی شد." });
            }
            return Json(new { success = false, message = "خطا در به‌روزرسانی تگ‌های مقاله." });
        }

        // آپلود تصویر برای CKEditor
        [HttpPost]
        public async Task<IActionResult> UploadImageAdmin(IFormFile upload)
        {
            try
            {
                if (upload == null || upload.Length == 0)
                {
                    return BadRequest(new { error = new { message = "فایلی انتخاب نشده است" } });
                }

                // آپلود تصویر
                var uploadPath = "uploads/images/articles/ckeditor";
                var result = await _imageService.UploadImageAsync(upload, uploadPath);

                if (!result.Success)
                {
                    return BadRequest(new { error = new { message = result.ErrorMessage } });
                }

                // تولید URL کامل برای تصویر بهینه‌شده
                var imageUrl = $"{Request.Scheme}://{Request.Host}/{result.OptimizedPath}";

                return Ok(new { url = imageUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = new { message = $"خطا در آپلود تصویر: {ex.Message}" } });
            }
        }

        // آپلود تصویر شاخص مقاله
        [HttpPost]
        public async Task<IActionResult> UploadFeaturedImage(IFormFile imageFile)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                {
                    return Json(new { success = false, message = "فایلی انتخاب نشده است" });
                }

                // آپلود تصویر شاخص
                var uploadPath = "uploads/images/articles/featured";
                var result = await _imageService.UploadImageAsync(imageFile, uploadPath);

                if (!result.Success)
                {
                    return Json(new { success = false, message = result.ErrorMessage });
                }

                var imageUrl = $"/{result.OptimizedPath}";

                return Json(new
                {
                    success = true,
                    imageUrl = imageUrl,
                    fileName = result.FileName,
                    optimizedSize = result.OptimizedSize,
                    compressionRatio = result.CompressionRatio
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"خطا در آپلود تصویر: {ex.Message}" });
            }
        }

        private async Task PopulateViewBag(CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(cancellationToken);
            ViewBag.Categories = categories.Select(c => new { CategoryId = c.CategoryId, Name = c.Name }).ToList();

            var tags = await _tagService.GetAllTagsAsync(cancellationToken);
            ViewBag.Tags = tags.Select(t => new { TagId = t.TagId, Name = t.Name }).ToList();
        }
    }
}