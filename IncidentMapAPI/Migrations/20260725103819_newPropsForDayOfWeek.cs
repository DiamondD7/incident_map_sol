using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IncidentMapAPI.Migrations
{
    /// <inheritdoc />
    public partial class newPropsForDayOfWeek : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "DealsTable",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "DealsTable",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValidDays",
                table: "DealsTable",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "DealsTable");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "DealsTable");

            migrationBuilder.DropColumn(
                name: "ValidDays",
                table: "DealsTable");
        }
    }
}
