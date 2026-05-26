using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IncidentMapAPI.Migrations
{
    /// <inheritdoc />
    public partial class addedDiscountPercentProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountPercent",
                table: "PromotionTable",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountPercent",
                table: "PromotionTable");
        }
    }
}
