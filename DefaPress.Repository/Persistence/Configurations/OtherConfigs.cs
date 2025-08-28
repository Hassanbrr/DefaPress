// Configurations/OtherConfigs.cs (Settings, NewsletterSubscriber, ContactMessage, AuditLog)
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefaPress.Domain;

namespace DefaPress.Infrastructure.Persistence.Configurations
{
    public class SettingConfiguration : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> b)
        {
            b.HasKey(s => s.Id);
            b.Property(s => s.Category).HasMaxLength(100);
            b.Property(s => s.Key).IsRequired().HasMaxLength(200);
            b.Property(s => s.Value).HasMaxLength(4000);
            b.HasIndex(s => new { s.Category, s.Key }).IsUnique();
        }
    }

    public class NewsletterSubscriberConfiguration : IEntityTypeConfiguration<NewsletterSubscriber>
    {
        public void Configure(EntityTypeBuilder<NewsletterSubscriber> b)
        {
            b.HasKey(n => n.Id);
            b.Property(n => n.Email).IsRequired().HasMaxLength(320);
            b.HasIndex(n => n.Email).IsUnique();
        }
    }

    public class ContactMessageConfiguration : IEntityTypeConfiguration<ContactMessage>
    {
        public void Configure(EntityTypeBuilder<ContactMessage> b)
        {
            b.HasKey(c => c.Id);
            b.Property(c => c.FullName).HasMaxLength(200);
            b.Property(c => c.Email).HasMaxLength(320);
            b.Property(c => c.Phone).HasMaxLength(50);
            b.Property(c => c.Message).IsRequired().HasMaxLength(5000);
        }
    }

    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> b)
        {
            b.HasKey(a => a.Id);
            b.Property(a => a.Action).IsRequired().HasMaxLength(100);
            b.Property(a => a.EntityName).HasMaxLength(200);
            b.Property(a => a.EntityId).HasMaxLength(100);
            b.Property(a => a.IPAddress).HasMaxLength(45);
            b.HasIndex(a => a.CreatedAt);

            b.HasOne(a => a.User)
             .WithMany() // اگر نمی‌خوایم navigation از User به AuditLogs، می‌ذاریم بدون WithMany(u=>u.AuditLogs)
             .HasForeignKey(a => a.UserId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
