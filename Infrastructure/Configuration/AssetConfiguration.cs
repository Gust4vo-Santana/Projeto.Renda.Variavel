using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.MySql.Configuration
{
    public class AssetConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.ToTable("Assets");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                   .ValueGeneratedOnAdd()
                   .HasColumnName("id");

            builder.Property(a => a.Code)
                   .IsRequired()
                   .HasMaxLength(10)
                   .HasColumnName("code");

            builder.HasIndex(a => a.Code)
                   .IsUnique();

            builder.Property(a => a.Name)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasColumnName("name");

            builder.HasIndex(a => a.Name)
                   .IsUnique();
        }
    }
}
