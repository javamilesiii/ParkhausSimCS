using Microsoft.EntityFrameworkCore;
using ParkhausAPI.Models;

namespace ParkhausAPI.Data
{
    public class ParkingContext : DbContext
    {
        public ParkingContext(DbContextOptions<ParkingContext> options) : base(options)
        {
        }

        public DbSet<Tickets> Tickets { get; set; }
    }
}