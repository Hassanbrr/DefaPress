using Azure;
using DefaPress.Domain;
using DefaPress.Domain.YourProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DefaPress.Repository.Context;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Article> Articles { get; set; }
    public DbSet<ArticleCategory> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Setting> Settings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Category - Parent/Child (Self Reference)
        builder.Entity<ArticleCategory>()
            .HasMany(c => c.SubCategories)
            .WithOne(c => c.ParentCategory)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Category - Article (One-to-Many)
        builder.Entity<ArticleCategory>()
            .HasMany(c => c.Articles)
            .WithOne(a => a.ArticleCategory)
            .HasForeignKey(a => a.ArticleCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Article - User (Author) (One-to-Many)
        builder.Entity<Article>()
            .HasOne(a => a.Author)
            .WithMany(u => u.Articles)
            .HasForeignKey(a => a.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Article - Comment (One-to-Many)
        builder.Entity<Article>()
            .HasMany(a => a.Comments)
            .WithOne(c => c.Article)
            .HasForeignKey(c => c.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Comment - User (One-to-Many)
        builder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Article - Tag (Many-to-Many without explicit join table)
        builder.Entity<Article>()
            .HasMany(a => a.Tags)
            .WithMany(t => t.Articles)
            .UsingEntity(j => j.ToTable("ArticleTags"));

        // Setting - Simple Key
        builder.Entity<Setting>()
            .HasKey(s => s.Id);
    }
}
