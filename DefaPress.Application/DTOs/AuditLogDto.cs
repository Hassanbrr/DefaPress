using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Application.DTOs
{
    public class AuditLogDto
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string EntityName { get; set; }
        public string? EntityId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? IPAddress { get; set; }
        public string? UserName { get; set; } // optional convenience
    }
}
