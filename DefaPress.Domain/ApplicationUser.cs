using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DefaPress.Domain
{

    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Article> Articles { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }

}