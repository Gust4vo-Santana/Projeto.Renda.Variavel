using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.MySql.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                   .ValueGeneratedOnAdd()
                   .HasColumnName("id");

            builder.Property(u => u.Name)
                   .IsRequired()
                   .HasMaxLength(100)
                   .HasColumnName("name");

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(100)
                   .HasColumnName("email");

            builder.HasIndex(u => u.Email)
                   .IsUnique();

            builder.Property(u => u.BrokerageFee)
                   .IsRequired()
                   .HasColumnType("decimal(10,2)")
                   .HasColumnName("brokerage_fee");
        }
    }
}