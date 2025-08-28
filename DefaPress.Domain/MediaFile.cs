using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Domain
{
    public class MediaFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string FileType { get; set; } // image, video, audio, pdf
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // FK
        public int? ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
