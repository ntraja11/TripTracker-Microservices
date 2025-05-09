using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripTracker.Services.ExpenseApi.Migrations
{
    /// <inheritdoc />
    public partial class AddParticipantNameToExpense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParticipantName",
                table: "Expenses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParticipantName",
                table: "Expenses");
        }
    }
}
