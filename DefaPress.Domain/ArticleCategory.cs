using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Domain

{
    public class ArticleCategory
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; } = 0;

        // Parent-Child Relationship
        public int? ParentCategoryId { get; set; }
        public ArticleCategory? ParentCategory { get; set; }
        public ICollection<ArticleCategory> SubCategories { get; set; }

        // Navigation
        public ICollection<Article> Articles { get; set; }
    }

}
