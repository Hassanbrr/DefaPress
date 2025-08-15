using DefaPress.Domain.YourProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Domain
{
    public class Article
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime PublishedAt { get; set; }
        public bool IsPublished { get; set; }

        // FK
        public int ArticleCategoryId { get; set; }
        public ArticleCategory ArticleCategory { get; set; }

        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        // Navigation
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}
