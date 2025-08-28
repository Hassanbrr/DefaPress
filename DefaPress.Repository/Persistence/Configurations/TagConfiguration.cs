// Configurations/TagConfiguration.cs

using DefaPress.Domain; 
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
 
namespace DefaPress.Infrastructure.Persistence.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> b)
        {
            b.HasKey(t => t.TagId);
            b.Property(t => t.Name).IsRequired().HasMaxLength(100);
            b.HasIndex(t => t.Name).IsUnique();
        }
    }
}