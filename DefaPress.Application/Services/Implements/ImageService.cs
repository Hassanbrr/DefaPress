using DefaPress.Application.DTOs.Results;
using DefaPress.Application.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp; // ImageSharp
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DefaPress.Application.Services.Implements
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ImageService> _logger;

        public string[] AllowedExtensions => new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
        public long MaxFileSize => 10 * 1024 * 1024; // 10MB

        public ImageService(IWebHostEnvironment environment, ILogger<ImageService> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public async Task<ImageUploadResult> UploadImageAsync(IFormFile file, string uploadPath, bool optimizeForWeb = true)
        {
            var result = new ImageUploadResult();

            try
            {
                // اعتبارسنجی فایل
                if (!ValidateFile(file, result))
                    return result;

                // ایجاد پوشه اگر وجود ندارد
                var fullUploadPath = Path.Combine(_environment.WebRootPath, uploadPath);
                Directory.CreateDirectory(fullUploadPath);

                // تولید نام فایل منحصربه‌فرد
                var fileName = GenerateUniqueFileName(file.FileName);
                var filePath = Path.Combine(fullUploadPath, fileName);
                var relativePath = Path.Combine(uploadPath, fileName).Replace("\\", "/");

                // ذخیره فایل اصلی
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                result.FilePath = relativePath;
                result.FileName = fileName;
                result.OriginalSize = file.Length;

                // بهینه‌سازی تصویر برای وب
                if (optimizeForWeb)
                {
                    var optimizationResult = await OptimizeImageForWebAsync(filePath);
                    if (optimizationResult.Success)
                    {
                        result.OptimizedPath = optimizationResult.OptimizedPath;
                        result.OptimizedSize = optimizationResult.OptimizedSize;
                    }
                    else
                    {
                        // اگر بهینه‌سازی失敗 شد، از فایل اصلی استفاده کن
                        result.OptimizedPath = relativePath;
                        result.OptimizedSize = result.OriginalSize;
                        _logger.LogWarning($"Image optimization failed: {optimizationResult.ErrorMessage}");
                    }
                }
                else
                {
                    result.OptimizedPath = relativePath;
                    result.OptimizedSize = result.OriginalSize;
                }

                result.Success = true;
                _logger.LogInformation($"Image uploaded successfully: {fileName}, Compression: {result.CompressionRatio:F2}%");
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = $"خطا در آپلود تصویر: {ex.Message}";
                _logger.LogError(ex, "Error uploading image");
            }

            return result;
        }

        public async Task<ImageOptimizationResult> OptimizeImageAsync(string imagePath, ImageOptimizationOptions options)
        {
            var result = new ImageOptimizationResult();

            try
            {
                var fullImagePath = Path.Combine(_environment.WebRootPath, imagePath);

                if (!File.Exists(fullImagePath))
                {
                    result.ErrorMessage = "فایل تصویر وجود ندارد";
                    return result;
                }

                var fileInfo = new FileInfo(fullImagePath);
                result.OriginalSize = fileInfo.Length;

                // تولید مسیر برای فایل بهینه‌شده
                var optimizedPath = GenerateOptimizedImagePath(imagePath);
                var fullOptimizedPath = Path.Combine(_environment.WebRootPath, optimizedPath);

                // ایجاد پوشه مقصد اگر وجود ندارد
                Directory.CreateDirectory(Path.GetDirectoryName(fullOptimizedPath));

                // پردازش و بهینه‌سازی تصویر با استفاده از ImageSharp
                using (var image = await SixLabors.ImageSharp.Image.LoadAsync(fullImagePath))
                {
                    // تغییر سایز اگر لازم باشد
                    if (image.Width > options.MaxWidth || image.Height > options.MaxHeight)
                    {
                        var resizeOptions = new ResizeOptions
                        {
                            Size = new SixLabors.ImageSharp.Size(options.MaxWidth, options.MaxHeight),
                            Mode = options.MaintainAspectRatio ? ResizeMode.Max : ResizeMode.Stretch
                        };

                        image.Mutate(x => x.Resize(resizeOptions));
                    }

                    // ذخیره با فرمت بهینه
                    var extension = Path.GetExtension(fullOptimizedPath).ToLower();
                    await SaveOptimizedImage(image, fullOptimizedPath, extension, options);

                    result.OptimizedSize = new FileInfo(fullOptimizedPath).Length;
                    result.CompressionRatio = (1 - (double)result.OptimizedSize / result.OriginalSize) * 100;
                    result.OptimizedPath = optimizedPath;
                    result.Success = true;
                }

                _logger.LogInformation($"Image optimized: {imagePath} -> {result.CompressionRatio:F2}% compression");
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = $"خطا در بهینه‌سازی تصویر: {ex.Message}";
                _logger.LogError(ex, "Error optimizing image");
            }

            return result;
        }

        public async Task<bool> DeleteImageAsync(string imagePath)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath))
                    return true;

                var fullPath = Path.Combine(_environment.WebRootPath, imagePath);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);

                    // حذف نسخه بهینه‌شده اگر وجود دارد
                    var optimizedPath = GenerateOptimizedImagePath(imagePath);
                    var fullOptimizedPath = Path.Combine(_environment.WebRootPath, optimizedPath);

                    if (File.Exists(fullOptimizedPath))
                    {
                        File.Delete(fullOptimizedPath);
                    }

                    _logger.LogInformation($"Image deleted: {imagePath}");
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image");
                return false;
            }
        }

        public string GenerateOptimizedImagePath(string originalPath)
        {
            var directory = Path.GetDirectoryName(originalPath);
            var fileName = Path.GetFileNameWithoutExtension(originalPath);
            var extension = Path.GetExtension(originalPath);

            // برای تصاویر بهینه‌شده از پسوند webp استفاده می‌کنیم
            var optimizedExtension = ".webp";

            return Path.Combine(directory ?? "", "optimized", $"{fileName}{optimizedExtension}").Replace("\\", "/");
        }

        public bool IsImageFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var extension = Path.GetExtension(file.FileName).ToLower();
            return Array.Exists(AllowedExtensions, ext => ext == extension);
        }

        #region Private Methods

        private bool ValidateFile(IFormFile file, ImageUploadResult result)
        {
            if (file == null || file.Length == 0)
            {
                result.ErrorMessage = "فایل انتخاب نشده است";
                return false;
            }

            if (file.Length > MaxFileSize)
            {
                result.ErrorMessage = $"حجم فایل نباید بیشتر از {MaxFileSize / 1024 / 1024} مگابایت باشد";
                return false;
            }

            if (!IsImageFile(file))
            {
                result.ErrorMessage = "فرمت فایل مجاز نیست. فرمت‌های مجاز: " + string.Join(", ", AllowedExtensions);
                return false;
            }

            return true;
        }

        private string GenerateUniqueFileName(string originalFileName)
        {
            var extension = Path.GetExtension(originalFileName);
            var fileName = Path.GetFileNameWithoutExtension(originalFileName);
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var random = Guid.NewGuid().ToString("N").Substring(0, 8);

            return $"{fileName}_{timestamp}_{random}{extension}";
        }

        private async Task<ImageOptimizationResult> OptimizeImageForWebAsync(string imagePath)
        {
            var options = new ImageOptimizationOptions
            {
                Quality = 85,
                MaxWidth = 1920,
                MaxHeight = 1080,
                MaintainAspectRatio = true,
                EnableWebP = true,
                EnableProgressiveJpeg = true
            };

            var relativePath = GetRelativePath(imagePath);
            return await OptimizeImageAsync(relativePath, options);
        }

        private async Task SaveOptimizedImage(SixLabors.ImageSharp.Image image, string outputPath, string extension, ImageOptimizationOptions options)
        {
            switch (extension)
            {
                case ".webp" when options.EnableWebP:
                    await image.SaveAsWebpAsync(outputPath, new WebpEncoder
                    {
                        Quality = options.Quality,
                        Method = WebpEncodingMethod.Default
                    });
                    break;

                case ".jpg":
                case ".jpeg":
                    var jpegEncoder = new JpegEncoder
                    {
                        Quality = options.Quality
                    };

                    // تنظیم progressive jpeg اگر پشتیبانی شود
                    if (options.EnableProgressiveJpeg)
                    {
                        // در نسخه‌های جدید ImageSharp، این ویژگی ممکن است متفاوت باشد
                        // این تنظیم برای سازگاری با نسخه‌های مختلف
                        jpegEncoder = new JpegEncoder
                        {
                            Quality = options.Quality
                            // Progressive property در برخی نسخه‌ها وجود ندارد
                        };
                    }

                    await image.SaveAsJpegAsync(outputPath, jpegEncoder);
                    break;

                case ".png":
                    await image.SaveAsPngAsync(outputPath, new PngEncoder
                    {
                        CompressionLevel = PngCompressionLevel.BestCompression
                    });
                    break;

                default:
                    // برای سایر فرمت‌ها از JPEG استفاده کن
                    await image.SaveAsJpegAsync(outputPath, new JpegEncoder
                    {
                        Quality = options.Quality
                    });
                    break;
            }
        }

        private string GetRelativePath(string fullPath)
        {
            var webRootPath = _environment.WebRootPath;
            return fullPath.Replace(webRootPath, "").TrimStart(Path.DirectorySeparatorChar);
        }

        #endregion
    }
}