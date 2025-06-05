using Domain.Entities;
using Infrastructure.MySql.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MySql.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Asset> Assets { get; set; }

        public DbSet<Operation> Operations { get; set; }

        public DbSet<Quote> Quotes { get; set; }

        public DbSet<Position> Positions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssetConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OperationConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PositionConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(QuoteConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
