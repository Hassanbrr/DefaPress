using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Application.DTOs
{
    // DTOs/Comment
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsApproved { get; set; }

        public int ArticleId { get; set; }
        public string UserId { get; set; }
        public string? UserName { get; set; }

        public int? ParentCommentId { get; set; }
        public List<CommentDto> Replies { get; set; } = new();
    }

    public class CommentCreateDto
    {
        public int ArticleId { get; set; }
        public string Content { get; set; }
        public int? ParentCommentId { get; set; }
    }

}
