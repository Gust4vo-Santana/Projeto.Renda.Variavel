using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.MySql.Configuration
{
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.ToTable("Positions");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                   .ValueGeneratedOnAdd()
                   .HasColumnName("id");

            builder.Property(p => p.UserId)
                   .IsRequired()
                   .HasColumnName("user_id");

            builder.Property(p => p.AssetId)
                   .IsRequired()
                   .HasColumnName("asset_id");

            builder.Property(p => p.Quantity)
                   .IsRequired()
                   .HasColumnType("decimal(10,2)")
                   .HasColumnName("quantity");

            builder.Property(p => p.AveragePrice)
                   .IsRequired()
                   .HasColumnType("decimal(10,2)")
                   .HasColumnName("average_price");

            builder.Property(p => p.PAndL)
                   .IsRequired()
                   .HasColumnType("decimal(10,2)")
                   .HasColumnName("p_and_l");

            builder.HasOne(p => p.User)
                   .WithMany(u => u.Positions)
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Asset)
                   .WithMany(a => a.Positions)
                   .HasForeignKey(p => p.AssetId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
