using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Domain
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsApproved { get; set; }

        // FK
        public int ArticleId { get; set; }
        public Article Article { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
