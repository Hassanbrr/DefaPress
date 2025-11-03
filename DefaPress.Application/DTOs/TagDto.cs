using System.Collections.Generic;

namespace DefaPress.Application.DTOs
{
    public class TagDto
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        public int ArticleCount { get; set; } // تعداد مقالات مرتبط
    }

    public class TagCreateDto
    {
        public string Name { get; set; }
    }

    public class TagUpdateDto
    {
        public int TagId { get; set; }
        public string Name { get; set; }
    }
}