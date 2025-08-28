using Microsoft.AspNetCore.Http;

namespace DefaPress.Application.DTOs
{
    // DTOs/Article
    public class ArticleListDto
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string? Summary { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public bool IsPublished { get; set; }
        public int ViewsCount { get; set; }
        public bool IsBreakingNews { get; set; }
        public bool IsFeatured { get; set; }

        public int ArticleCategoryId { get; set; }
        public string? CategoryName { get; set; }

        public string? AuthorId { get; set; }
        public string? AuthorName { get; set; }

        public List<string> TagNames { get; set; } = new();
        public string? MainMediaUrl { get; set; } // optional convenience
    }

    public class ArticleDetailDto : ArticleListDto
    {
        public string Content { get; set; }
        public List<string> MediaUrls { get; set; } = new();
        public string? Source { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class ArticleCreateDto
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string? Summary { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }

        public DateTime? PublishedAt { get; set; }
        public bool IsPublished { get; set; } = false;
        public bool IsBreakingNews { get; set; } = false;
        public bool IsFeatured { get; set; } = false;
        public string? Source { get; set; }

        public int ArticleCategoryId { get; set; }
        public string? AuthorId { get; set; }

        // tags by id (many-to-many handled in service or resolver)
        public List<int> TagIds { get; set; } = new();

        // media upload (controller typically handles IFormFile)
        public List<IFormFile>? MediaFiles { get; set; }
    }

    public class ArticleUpdateDto : ArticleCreateDto
    {
        public int ArticleId { get; set; }
    }
}