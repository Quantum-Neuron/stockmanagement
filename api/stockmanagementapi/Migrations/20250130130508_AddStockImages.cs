using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stockmanagementapi.Migrations
{
    /// <inheritdoc />
    public partial class AddStockImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StockImageId",
                table: "StockItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StockImages",
                columns: table => new
                {
                    StockImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageBinary = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    StockItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockImages", x => x.StockImageId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockImages");

            migrationBuilder.DropColumn(
                name: "StockImageId",
                table: "StockItems");
        }
    }
}
