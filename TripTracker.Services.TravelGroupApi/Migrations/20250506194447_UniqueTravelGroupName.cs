using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripTracker.Services.TravelGroupApi.Migrations
{
    /// <inheritdoc />
    public partial class UniqueTravelGroupName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TravelGroups",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_TravelGroups_Name",
                table: "TravelGroups",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TravelGroups_Name",
                table: "TravelGroups");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TravelGroups",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
