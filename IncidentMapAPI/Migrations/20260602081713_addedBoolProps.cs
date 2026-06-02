using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IncidentMapAPI.Migrations
{
    /// <inheritdoc />
    public partial class addedBoolProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasPromotion",
                table: "PromotionTable",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAnAestheticShop",
                table: "PromotionTable",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasPromotion",
                table: "PromotionTable");

            migrationBuilder.DropColumn(
                name: "IsAnAestheticShop",
                table: "PromotionTable");
        }
    }
}
