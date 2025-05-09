using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripTracker.Services.ExpenseApi.Migrations
{
    /// <inheritdoc />
    public partial class RenameExpenseDateToDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpenseDate",
                table: "Expenses",
                newName: "Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Expenses",
                newName: "ExpenseDate");
        }
    }
}
