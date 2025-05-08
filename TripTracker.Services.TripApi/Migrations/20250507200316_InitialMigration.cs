using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TripTracker.Services.TripApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TravelGroupId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    From = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    To = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TotalExpense = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParticipantIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpenseIds = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Trips",
                columns: new[] { "Id", "CreatedByUserEmail", "Description", "EndDate", "ExpenseIds", "From", "Name", "Notes", "ParticipantIds", "StartDate", "Status", "To", "TotalExpense", "TravelGroupId" },
                values: new object[,]
                {
                    { 1, null, "A relaxing summer retreat in the hills of Ooty.", new DateOnly(2025, 5, 20), "[]", "Keelapalur", "Ooty Summer Escape", "Wonderful weather, visited the Botanical Gardens and tea estates.", "[]", new DateOnly(2025, 5, 15), "Completed", "Ooty", 25000m, 1 },
                    { 2, null, "Exploring the lakes and forests of Kodaikanal.", new DateOnly(2025, 6, 14), "[]", "Keelapalur", "Kodaikanal Adventure", "Looking forward to trekking and boating.", "[]", new DateOnly(2025, 6, 10), "Planned", "Kodaikanal", 18000m, 2 },
                    { 3, null, "A cultural trip to explore the royal city of Mysore.", new DateOnly(2025, 4, 7), "[]", "Keelapalur", "Mysore Cultural Tour", "Visited Mysore Palace and Brindavan Gardens.", "[]", new DateOnly(2025, 4, 5), "Completed", "Mysore", 15000m, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trips");
        }
    }
}
