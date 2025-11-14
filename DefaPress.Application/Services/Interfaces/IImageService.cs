using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefaPress.Application.DTOs.Results;

namespace DefaPress.Application.Services.Interfaces
{
    public interface IImageService
    {
        Task<ImageUploadResult> UploadImageAsync(IFormFile file, string uploadPath, bool optimizeForWeb = true);
        Task<ImageOptimizationResult> OptimizeImageAsync(string imagePath, ImageOptimizationOptions options);
        Task<bool> DeleteImageAsync(string imagePath);
        string GenerateOptimizedImagePath(string originalPath);
        bool IsImageFile(IFormFile file);
        string[] AllowedExtensions { get; }
        long MaxFileSize { get; }
    }
}
