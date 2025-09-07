using System;
using System.Collections.Generic;

namespace DefaPress.Application.DTOs
{
    public class ArticleCategoryDto
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public int? ParentCategoryId { get; set; }

        // جدید: برای نمایش نام دستهٔ والد در خروجی
        public string? ParentCategoryName { get; set; }

        // جدید: لیست زیردسته‌ها (بازگشتی، از ArticleCategoryDto استفاده می‌شود)
        public List<ArticleCategoryDto> SubCategories { get; set; } = new();
    }

    public class ArticleCategoryCreateDto
    {
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public int? ParentCategoryId { get; set; }
        public int DisplayOrder { get; set; } = 0;
    }
     
}