using Microsoft.EntityFrameworkCore;
using TripTracker.Services.TravelGroupApi.Model;

namespace TripTracker.Services.TravelGroupApi.Data
{
    public class TravelGroupDbContext : DbContext
    {
        public TravelGroupDbContext(DbContextOptions<TravelGroupDbContext> options) : base(options)
        {
        }

        public DbSet<TravelGroup> TravelGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TravelGroup>()
                .HasData(

                new TravelGroup
                {
                    Id = 1,
                    Name = "Friends Group",
                    Place = "Keelapalur/Varanasi",
                    Description = "Friends from palur and varanasi.",
                    CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                },
                new TravelGroup
                {
                    Id = 2,
                    Name = "Aero Group",
                    Place = "Chennai",
                    Description = "Friends from Aeronautical education.",
                    CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                }
            );
        }


    }
}
