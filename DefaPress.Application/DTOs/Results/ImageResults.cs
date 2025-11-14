using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Application.DTOs.Results
{
    public class ImageUploadResult
    {
        public bool Success { get; set; }
        public string FilePath { get; set; }
        public string OptimizedPath { get; set; }
        public string FileName { get; set; }
        public long OriginalSize { get; set; }
        public long OptimizedSize { get; set; }
        public string ErrorMessage { get; set; }
        public double CompressionRatio => OriginalSize > 0 ? (1 - (double)OptimizedSize / OriginalSize) * 100 : 0;
    }

    public class ImageOptimizationResult
    {
        public bool Success { get; set; }
        public string OptimizedPath { get; set; }
        public long OriginalSize { get; set; }
        public long OptimizedSize { get; set; }
        public double CompressionRatio { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ImageOptimizationOptions
    {
        public int Quality { get; set; } = 80;
        public int MaxWidth { get; set; } = 1920;
        public int MaxHeight { get; set; } = 1080;
        public bool MaintainAspectRatio { get; set; } = true;
        public bool EnableWebP { get; set; } = true;
        public bool EnableProgressiveJpeg { get; set; } = true;
    }
}
