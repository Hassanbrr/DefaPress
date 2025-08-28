using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Domain
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string Action { get; set; } // Create, Update, Delete, Login
        public string EntityName { get; set; }
        public string? EntityId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? IPAddress { get; set; }
    }
}
