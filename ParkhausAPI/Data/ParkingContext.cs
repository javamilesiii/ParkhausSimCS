using Microsoft.EntityFrameworkCore;
using ParkhausAPI.Models;

namespace ParkhausAPI.Data
{
    public class ParkingContext : DbContext
    {
        public ParkingContext(DbContextOptions<ParkingContext> options) : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Id).HasMaxLength(50);
                entity.Property(t => t.Spot).IsRequired();
                entity.Property(t => t.PurchaseTime).IsRequired();
                entity.Property(t => t.IsPaid).IsRequired().HasDefaultValue(false);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}