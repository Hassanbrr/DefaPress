// Configurations/ArticleConfiguration.cs
using DefaPress.Domain;
 
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DefaPress.Infrastructure.Persistence.Configurations
{
    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> b)
        {
            b.HasKey(a => a.ArticleId);

            b.Property(a => a.Title).IsRequired().HasMaxLength(250);
            b.Property(a => a.Slug).IsRequired().HasMaxLength(250);
            b.Property(a => a.Summary).HasMaxLength(1000);
            b.Property(a => a.Content).IsRequired();
            b.Property(a => a.ImageUrl).HasMaxLength(1000);
            b.Property(a => a.Source).HasMaxLength(500);

            // Indexes
            b.HasIndex(a => a.Slug).IsUnique();
            b.HasIndex(a => a.CreatedAt);

            // Relationships
            b.HasOne(a => a.ArticleCategory)
             .WithMany(c => c.Articles)
             .HasForeignKey(a => a.ArticleCategoryId)
             .OnDelete(DeleteBehavior.Restrict); // جلوگیری از حذف دسته‌بندی همراه با مقالات

            b.HasOne(a => a.Author)
             .WithMany(u => u.Articles)
             .HasForeignKey(a => a.AuthorId)
             .OnDelete(DeleteBehavior.SetNull); // اگر نویسنده حذف شد، مقاله حفظ و authorId = null

            // Article <-> Tag many-to-many (skip navigation)
            b.HasMany(a => a.Tags)
             .WithMany(t => t.Articles)
             .UsingEntity<Dictionary<string, object>>(
                "ArticleTag",
                right => right.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.Cascade),
                left => left.HasOne<Article>().WithMany().HasForeignKey("ArticleId").OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.HasKey("ArticleId", "TagId");
                    join.HasIndex("TagId");
                    join.ToTable("ArticleTags");
                });

            // MediaFiles navigation
            b.HasMany(a => a.MediaFiles)
             .WithOne(m => m.Article)
             .HasForeignKey(m => m.ArticleId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
