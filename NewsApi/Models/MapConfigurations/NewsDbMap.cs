using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NewsApi.Models.MapConfigurations
{
    public class NewsDbMap : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder.HasKey(a => a.NewsId);
            builder.Property(a => a.NewsId).HasMaxLength(36);
            builder.Property(a => a.Title).HasMaxLength(255);
            builder.Property(a => a.CreatedAt).HasColumnType("datetime2");
            builder.Property(a => a.Article).HasColumnType("nvarchar(max)");
        }
    }
}
