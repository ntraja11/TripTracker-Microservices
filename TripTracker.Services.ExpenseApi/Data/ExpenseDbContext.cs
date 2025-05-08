using Microsoft.EntityFrameworkCore;
using TripTracker.Services.ExpenseApi.Models;

namespace TripTracker.Services.ExpenseApi.Data
{
    public class ExpenseDbContext : DbContext
    {
        public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options)
        {
        }

        public DbSet<Expense> Expenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



        }


    }
}
