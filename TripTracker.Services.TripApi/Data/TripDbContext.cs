using Microsoft.EntityFrameworkCore;
using TripTracker.Services.TripApi.Models;

namespace TripTracker.Services.TripApi.Data
{
    public class TripDbContext : DbContext
    {
        public TripDbContext(DbContextOptions<TripDbContext> options) : base(options)
        {
        }

        public DbSet<Trip> Trips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Trip>()
                .HasData(
                    new Trip
                    {
                        Id = 1,
                        Name = "Ooty Summer Escape",
                        TravelGroupId = 1,
                        Description = "A relaxing summer retreat in the hills of Ooty.",
                        From = "Keelapalur",
                        To = "Ooty",
                        StartDate = new DateOnly(2025, 5, 15),
                        EndDate = new DateOnly(2025, 5, 20),
                        TotalExpense = 25000,
                        Status = "Completed",
                        Notes = "Wonderful weather, visited the Botanical Gardens and tea estates.",
                    },
                    new Trip
                    {
                        Id = 2,
                        Name = "Kodaikanal Adventure",
                        TravelGroupId = 2,
                        Description = "Exploring the lakes and forests of Kodaikanal.",
                        From = "Keelapalur",
                        To = "Kodaikanal",
                        StartDate = new DateOnly(2025, 6, 10),
                        EndDate = new DateOnly(2025, 6, 14),
                        TotalExpense = 18000,
                        Status = "Planned",
                        Notes = "Looking forward to trekking and boating.",
                    },
                    new Trip
                    {
                        Id = 3,
                        Name = "Mysore Cultural Tour",
                        TravelGroupId = 3,
                        Description = "A cultural trip to explore the royal city of Mysore.",
                        From = "Keelapalur",
                        To = "Mysore",
                        StartDate = new DateOnly(2025, 4, 5),
                        EndDate = new DateOnly(2025, 4, 7),
                        TotalExpense = 15000,
                        Status = "Completed",
                        Notes = "Visited Mysore Palace and Brindavan Gardens.",
                    }
                );
                
        }


    }
}
