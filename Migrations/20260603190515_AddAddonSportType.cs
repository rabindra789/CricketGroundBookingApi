using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CricketGroundBookingApi.Migrations
{
    /// <inheritdoc />
    public partial class AddAddonSportType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SportType",
                table: "Addons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Multi-Sport");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SportType",
                table: "Addons");
        }
    }
}
