// DefaPress.Infrastructure/Context/ApplicationDbContext.cs
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DefaPress.Domain;
using Microsoft.AspNetCore.Identity;

namespace DefaPress.Infrastructure.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSets
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleCategory> ArticleCategories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<MediaFile> MediaFiles { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<NewsletterSubscriber> NewsletterSubscribers { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ----------------------------
            // ArticleCategory
            // ----------------------------
            builder.Entity<ArticleCategory>(b =>
            {
                b.HasKey(c => c.CategoryId);

                b.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                b.Property(c => c.Slug)
                    .HasMaxLength(250);

                b.Property(c => c.Description)
                    .HasMaxLength(2000);

                b.HasIndex(c => c.Slug);

                // Self-referencing parent-child
                b.HasOne(c => c.ParentCategory)
                    .WithMany(p => p.SubCategories)
                    .HasForeignKey(c => c.ParentCategoryId)
                    .OnDelete(DeleteBehavior.Restrict); // جلوگیری از حذف والد هنگام وجود فرزند
            });

            // ----------------------------
            // Tag
            // ----------------------------
            builder.Entity<Tag>(b =>
            {
                b.HasKey(t => t.TagId);
                b.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                b.HasIndex(t => t.Name).IsUnique();
            });

            // ----------------------------
            // Article
            // ----------------------------
            builder.Entity<Article>(b =>
            {
                b.HasKey(a => a.ArticleId);

                b.Property(a => a.Title)
                    .IsRequired()
                    .HasMaxLength(300);

                b.Property(a => a.Slug)  
                    .HasMaxLength(250);

                b.Property(a => a.Summary)
                    .HasMaxLength(1000);

                b.Property(a => a.Content)
                    .IsRequired();

                b.Property(a => a.ImageUrl)
                    .HasMaxLength(1000);

                b.Property(a => a.Source)
                    .HasMaxLength(500);

                b.HasIndex(a => a.Slug).IsUnique();
                b.HasIndex(a => a.CreatedAt);

                // Article -> Category (FK = ArticleCategoryId)
                b.HasOne(a => a.ArticleCategory)
                    .WithMany(c => c.Articles)
                    .HasForeignKey(a => a.ArticleCategoryId)
                    .OnDelete(DeleteBehavior.Restrict); // جلوگیری از حذف دسته وقتی مقاله وجود دارد

                // Article -> Author (ApplicationUser)  (AuthorId nullable)
                b.HasOne(a => a.Author)
                    .WithMany(u => u.Articles)
                    .HasForeignKey(a => a.AuthorId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Article -> MediaFiles
                b.HasMany(a => a.MediaFiles)
                    .WithOne(m => m.Article)
                    .HasForeignKey(m => m.ArticleId)
                    .OnDelete(DeleteBehavior.Cascade);

                // many-to-many Article <-> Tag (skip-navigation, explicit join table name ArticleTags)
                b.HasMany(a => a.Tags)
                 .WithMany(t => t.Articles)
                 .UsingEntity<Dictionary<string, object>>(
                    "ArticleTags",
                    right => right.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.Cascade),
                    left => left.HasOne<Article>().WithMany().HasForeignKey("ArticleId").OnDelete(DeleteBehavior.Cascade),
                    join =>
                    {
                        join.HasKey("ArticleId", "TagId");
                        join.ToTable("ArticleTags");
                        join.HasIndex("TagId");
                    });
            });

            // ----------------------------
            // Comment
            // ----------------------------
            builder.Entity<Comment>(b =>
            {
                b.HasKey(c => c.Id);

                b.Property(c => c.Content)
                    .IsRequired()
                    .HasMaxLength(2000);

                b.Property(c => c.CreatedAt);
                b.Property(c => c.UpdatedAt);

                b.HasOne(c => c.Article)
                    .WithMany(a => a.Comments)
                    .HasForeignKey(c => c.ArticleId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(c => c.User)
                    .WithMany(u => u.Comments)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Restrict); // جلوگیری از حذف کاربر در صورت وجود کامنت

                // Nested comments (parent-child)
                b.HasOne(c => c.ParentComment)
                    .WithMany(p => p.Replies)
                    .HasForeignKey(c => c.ParentCommentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ----------------------------
            // MediaFile
            // ----------------------------
            builder.Entity<MediaFile>(b =>
            {
                b.HasKey(m => m.Id);
                b.Property(m => m.FileName).IsRequired().HasMaxLength(500);
                b.Property(m => m.FileUrl).IsRequired().HasMaxLength(1000);
                b.Property(m => m.FileType).HasMaxLength(100);
                b.HasIndex(m => m.UploadedAt);
                // ArticleId is optional (nullable)
                b.HasOne(m => m.Article)
                    .WithMany(a => a.MediaFiles)
                    .HasForeignKey(m => m.ArticleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ----------------------------
            // Setting
            // ----------------------------
            builder.Entity<Setting>(b =>
            {
                b.HasKey(s => s.Id);
                b.Property(s => s.Category).HasMaxLength(100);
                b.Property(s => s.Key).IsRequired().HasMaxLength(200);
                b.Property(s => s.Value).HasMaxLength(4000);
                b.HasIndex(s => new { s.Category, s.Key }).IsUnique();
            });

            // ----------------------------
            // NewsletterSubscriber
            // ----------------------------
            builder.Entity<NewsletterSubscriber>(b =>
            {
                b.HasKey(n => n.Id);
                b.Property(n => n.Email).IsRequired().HasMaxLength(320);
                b.HasIndex(n => n.Email).IsUnique();
            });

            // ----------------------------
            // ContactMessage
            // ----------------------------
            builder.Entity<ContactMessage>(b =>
            {
                b.HasKey(c => c.Id);
                b.Property(c => c.FullName).HasMaxLength(200);
                b.Property(c => c.Email).HasMaxLength(320);
                b.Property(c => c.Phone).HasMaxLength(50);
                b.Property(c => c.Message).IsRequired().HasMaxLength(5000);
            });

            // ----------------------------
            // AuditLog
            // ----------------------------
            builder.Entity<AuditLog>(b =>
            {
                b.HasKey(a => a.Id);
                b.Property(a => a.Action).IsRequired().HasMaxLength(100);
                b.Property(a => a.EntityName).HasMaxLength(200);
                b.Property(a => a.EntityId).HasMaxLength(100);
                b.Property(a => a.IPAddress).HasMaxLength(45);
                b.HasIndex(a => a.CreatedAt);

                b.HasOne(a => a.User)
                    .WithMany() // اگر خواستی navigation از User به AuditLogs اضافه کنی، اینجا تغییر بده
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ----------------------------
            // ApplicationUser (additional config)
            // ----------------------------
            builder.Entity<ApplicationUser>(b =>
            {
                b.Property(u => u.FullName).HasMaxLength(200);
                b.Property(u => u.ProfileImageUrl).HasMaxLength(1000);
                b.HasIndex(u => u.CreatedAt);
                // Comments and Articles navigation defined on other entities
            });
        }
    }
}
