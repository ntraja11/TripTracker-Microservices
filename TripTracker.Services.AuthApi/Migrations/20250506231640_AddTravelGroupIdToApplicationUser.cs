using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripTracker.Services.AuthApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTravelGroupIdToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TravelGroupId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TravelGroupId",
                table: "AspNetUsers");
        }
    }
}
