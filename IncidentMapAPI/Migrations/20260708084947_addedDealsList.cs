using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IncidentMapAPI.Migrations
{
    /// <inheritdoc />
    public partial class addedDealsList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromotionImages_PromotionTable_PromotionId",
                table: "PromotionImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PromotionImages",
                table: "PromotionImages");

            migrationBuilder.RenameTable(
                name: "PromotionImages",
                newName: "PromotionImagesTable");

            migrationBuilder.RenameIndex(
                name: "IX_PromotionImages_PromotionId",
                table: "PromotionImagesTable",
                newName: "IX_PromotionImagesTable_PromotionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PromotionImagesTable",
                table: "PromotionImagesTable",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DealsTable",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PromotionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DealTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DealDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountPercent = table.Column<int>(type: "int", nullable: true),
                    DealStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DealEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealsTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealsTable_PromotionTable_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "PromotionTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DealsTable_PromotionId",
                table: "DealsTable",
                column: "PromotionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PromotionImagesTable_PromotionTable_PromotionId",
                table: "PromotionImagesTable",
                column: "PromotionId",
                principalTable: "PromotionTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromotionImagesTable_PromotionTable_PromotionId",
                table: "PromotionImagesTable");

            migrationBuilder.DropTable(
                name: "DealsTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PromotionImagesTable",
                table: "PromotionImagesTable");

            migrationBuilder.RenameTable(
                name: "PromotionImagesTable",
                newName: "PromotionImages");

            migrationBuilder.RenameIndex(
                name: "IX_PromotionImagesTable_PromotionId",
                table: "PromotionImages",
                newName: "IX_PromotionImages_PromotionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PromotionImages",
                table: "PromotionImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PromotionImages_PromotionTable_PromotionId",
                table: "PromotionImages",
                column: "PromotionId",
                principalTable: "PromotionTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
