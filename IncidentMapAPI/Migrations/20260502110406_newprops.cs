using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IncidentMapAPI.Migrations
{
    /// <inheritdoc />
    public partial class newprops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PromotionTable",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedAt",
                table: "PromotionTable",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PromotionTable");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "PromotionTable");
        }
    }
}
