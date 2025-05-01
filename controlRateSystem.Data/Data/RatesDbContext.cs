using Microsoft.EntityFrameworkCore;
using controlRateSystem.Data.Models;

namespace controlRateSystem.Data.Data
{
    public class RatesDbContext : DbContext
    {
        public RatesDbContext(DbContextOptions<RatesDbContext> options)
            : base(options) { }

        public DbSet<Currency> Currencies => Set<Currency>();
        public DbSet<Category> Categories => Set<Category>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Currency>()
                .HasOne(c => c.Category)
                .WithMany(cat => cat.Currencies)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
