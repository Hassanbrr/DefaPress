using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Application.ArticleCategories.Dtos
{
    public class ArticleCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }

        public int? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }

        public List<ArticleCategoryDto> SubCategories { get; set; }
    }
}
