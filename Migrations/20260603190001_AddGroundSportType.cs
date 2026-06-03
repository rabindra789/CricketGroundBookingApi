using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CricketGroundBookingApi.Migrations
{
    /// <inheritdoc />
    public partial class AddGroundSportType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SportType",
                table: "Grounds",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Multi-Sport");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SportType",
                table: "Grounds");
        }
    }
}
