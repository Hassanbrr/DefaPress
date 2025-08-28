// Configurations/ArticleCategoryConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefaPress.Domain;

namespace DefaPress.Infrastructure.Persistence.Configurations
{
    public class ArticleCategoryConfiguration : IEntityTypeConfiguration<ArticleCategory>
    {
        public void Configure(EntityTypeBuilder<ArticleCategory> b)
        {
            b.HasKey(c => c.CategoryId);
            b.Property(c => c.Name).IsRequired().HasMaxLength(200);
            b.Property(c => c.Slug).HasMaxLength(250);
            b.HasIndex(c => c.Slug).IsUnique(false);
            // Self reference
            b.HasOne(c => c.ParentCategory)
                .WithMany(p => p.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}