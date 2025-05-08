using Microsoft.EntityFrameworkCore;
using TripTracker.Services.ParticipantApi.Models;

namespace TripTracker.Services.ParticipantApi.Data
{
    public class ParticipantDbContext : DbContext
    {
        public ParticipantDbContext(DbContextOptions<ParticipantDbContext> options) : base(options)
        {
        }

        public DbSet<Participant> Participants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }


    }
}
