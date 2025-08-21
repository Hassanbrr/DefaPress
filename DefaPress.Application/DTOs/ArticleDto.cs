namespace DefaPress.Application.DTOs
{
    public class ArticleDto
    {
        public int ArticleId { get; set; }               // شناسه مقاله
        public string Title { get; set; }                // عنوان
        public string Slug { get; set; }                 // اسلاگ (URL Friendly)
        public string Summary { get; set; }              // خلاصه
        public string Content { get; set; }              // محتوای کامل
        public string? ImageUrl { get; set; }            // عکس
        public bool IsPublished { get; set; }            // وضعیت انتشار
        public DateTime? PublishedAt { get; set; }       // زمان انتشار

        // 🔹 دسته‌بندی
        public int ArticleCategoryId { get; set; }       // FK به دسته‌بندی
        public string CategoryName { get; set; }         // نام دسته‌بندی (نمایشی)

        // 🔹 نویسنده
        public string? AuthorId { get; set; }            // شناسه نویسنده
        public string AuthorName { get; set; }           // نام نویسنده (نمایشی)

        // 🔹 ناوبری
        public List<string> Tags { get; set; }           // لیست تگ‌ها
        public int CommentsCount { get; set; }           // تعداد کامنت‌ها
    }
}