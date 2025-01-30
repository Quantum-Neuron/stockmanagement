using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stockmanagementapi.Migrations
{
    /// <inheritdoc />
    public partial class StockImageAdjustment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockImageId",
                table: "StockItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StockImageId",
                table: "StockItems",
                type: "int",
                nullable: true);
        }
    }
}
