using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IncidentMapAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddYourNewProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "PromotionTable",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "PromotionTable");
        }
    }
}
