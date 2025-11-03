using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Application.DTOs
{
    // DTOs/Media
    public class MediaFileDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string FileType { get; set; }
        public DateTime UploadedAt { get; set; }
        public int? ArticleId { get; set; }
    }

    public class MediaFileCreateDto
    {
        public IFormFile File { get; set; }
        public int? ArticleId { get; set; }
    }

    public class MediaFileUpdateDto
    {
        public int? ArticleId { get; set; }
    }

}
