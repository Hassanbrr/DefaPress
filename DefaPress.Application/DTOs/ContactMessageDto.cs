using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Application.DTOs
{

    public class ContactMessageDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }
   
        public class ContactMessageCreateDto
        {
            public string FullName { get; set; }
            public string Email { get; set; }
            public string? Phone { get; set; }
            public string Message { get; set; }
        }

        public class ContactMessageUpdateDto
        {
            public bool IsRead { get; set; }
        }
 
}
