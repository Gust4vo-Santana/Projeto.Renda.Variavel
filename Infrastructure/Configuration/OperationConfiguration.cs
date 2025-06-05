using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.MySql.Configuration
{
    public class OperationConfiguration : IEntityTypeConfiguration<Operation>
    {
        public void Configure(EntityTypeBuilder<Operation> builder)
        {
            builder.ToTable("Operations");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                   .ValueGeneratedOnAdd()
                   .HasColumnName("id");

            builder.Property(o => o.UserId)
                   .IsRequired()
                   .HasColumnName("user_id");

            builder.Property(o => o.AssetId)
                   .IsRequired()
                   .HasColumnName("asset_id");

            builder.Property(o => o.Quantity)
                   .IsRequired()
                   .HasColumnType("decimal(10,2)")
                   .HasColumnName("quantity");

            builder.Property(o => o.Price)
                   .IsRequired()
                   .HasColumnType("decimal(10,2)")
                   .HasColumnName("price");

            builder.Property(o => o.OperationType)
                   .IsRequired()
                   .HasConversion(
                        v => v.ToString(),
                        v => (OperationType)Enum.Parse(typeof(OperationType), v))
                   .HasMaxLength(10)
                   .HasColumnName("operation_type");

            builder.Property(o => o.BrokerageFee)
                   .IsRequired()
                   .HasColumnType("decimal(10,2)")
                   .HasColumnName("brokerage_fee");

            builder.Property(o => o.Date)
                   .IsRequired()
                   .HasDefaultValueSql("CURRENT_TIMESTAMP")
                   .HasColumnName("date");

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(o => o.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Asset>()
                   .WithMany()
                   .HasForeignKey(o => o.AssetId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
