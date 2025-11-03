using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Application.DTOs
{
    public class NewsletterSubscriberDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime SubscribedAt { get; set; }
        public bool IsActive { get; set; }
    }
    public class NewsletterSubscriberCreateDto
    {
        public string Email { get; set; }
    }

    public class NewsletterSubscriberUpdateDto
    {
        public bool IsActive { get; set; }
    }
}
