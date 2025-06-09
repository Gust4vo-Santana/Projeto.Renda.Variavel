using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.MySql.Configuration
{
    public class QuoteConfiguration : IEntityTypeConfiguration<Quote>
    {
        public void Configure(EntityTypeBuilder<Quote> builder)
        {
            builder.ToTable("Quotes");

            builder.HasKey(q => q.Id);

            builder.Property(q => q.Id)
                   .ValueGeneratedOnAdd()
                   .HasColumnName("id");

            builder.Property(q => q.AssetId)
                   .IsRequired()
                   .HasColumnName("asset_id");

            builder.Property(q => q.Price)
                   .IsRequired()
                   .HasColumnType("decimal(10,2)")
                   .HasColumnName("price");

            builder.Property(q => q.Date)
                   .IsRequired()
                   .HasDefaultValueSql("CURRENT_TIMESTAMP")
                   .HasColumnName("date_time");

            builder.HasOne(q => q.Asset)
                   .WithMany(a => a.Quotes)
                   .HasForeignKey(q => q.AssetId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
