// Configurations/MediaFileConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefaPress.Domain;

namespace DefaPress.Infrastructure.Persistence.Configurations
{
    public class MediaFileConfiguration : IEntityTypeConfiguration<MediaFile>
    {
        public void Configure(EntityTypeBuilder<MediaFile> b)
        {
            b.HasKey(m => m.Id);
            b.Property(m => m.FileName).IsRequired().HasMaxLength(500);
            b.Property(m => m.FileUrl).IsRequired().HasMaxLength(1000);
            b.Property(m => m.FileType).HasMaxLength(100);
            b.HasIndex(m => m.UploadedAt);
        }
    }
}