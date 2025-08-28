using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Application.DTOs
{
    public class ApplicationUserDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class ApplicationUserUpdateDto
    {
        public string FullName { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
