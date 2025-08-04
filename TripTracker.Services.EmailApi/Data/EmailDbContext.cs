using Microsoft.EntityFrameworkCore;
using TripTracker.Services.EmailApi.Models;

namespace TripTracker.Services.TripApi.Data
{
    public class EmailDbContext : DbContext
    {
        public EmailDbContext(DbContextOptions<EmailDbContext> options) : base(options)
        {
        }

        //public DbSet<EmailLogger> EmailLoggers { get; set; }       


    }
}
