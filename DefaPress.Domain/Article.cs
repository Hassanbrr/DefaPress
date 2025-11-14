 
 

namespace DefaPress.Domain
{
    public class Article
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string? Slug { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public bool IsPublished { get; set; }
        public int ViewsCount { get; set; } = 0;
        public string? Source { get; set; }
        public bool IsBreakingNews { get; set; } = false;
        public bool IsFeatured { get; set; } = false;

        // FK
        public int ArticleCategoryId { get; set; }
        public ArticleCategory ArticleCategory { get; set; }

        public string? AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        // Navigation
        public ICollection<Comment> Comments { get; set; }

        public ICollection<Tag> Tags { get; set; }
        
        public ICollection<MediaFile> MediaFiles { get; set; }
    }
}
