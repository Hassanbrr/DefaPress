using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Application.Comments.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public string UserName { get; set; }
        public int ArticleId { get; set; }
        public string ArticleTitle { get; set; }
    }
}
