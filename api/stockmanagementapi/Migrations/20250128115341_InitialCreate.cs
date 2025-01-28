using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stockmanagementapi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockAccessories",
                columns: table => new
                {
                    StockAccessoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAccessories", x => x.StockAccessoryId);
                });

            migrationBuilder.CreateTable(
                name: "StockItems",
                columns: table => new
                {
                    StockItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Make = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModelYear = table.Column<int>(type: "int", nullable: false),
                    KMS = table.Column<int>(type: "int", nullable: false),
                    Colour = table.Column<int>(type: "int", nullable: false),
                    VIN = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockItems", x => x.StockItemId);
                });

            migrationBuilder.CreateTable(
                name: "StockItemAccessories",
                columns: table => new
                {
                    StockItemId = table.Column<int>(type: "int", nullable: false),
                    StockAccessoryId = table.Column<int>(type: "int", nullable: false),
                    StockItemAccessoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockItemAccessories", x => new { x.StockItemId, x.StockAccessoryId });
                    table.ForeignKey(
                        name: "FK_StockItemAccessories_StockAccessories_StockAccessoryId",
                        column: x => x.StockAccessoryId,
                        principalTable: "StockAccessories",
                        principalColumn: "StockAccessoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockItemAccessories_StockItems_StockItemId",
                        column: x => x.StockItemId,
                        principalTable: "StockItems",
                        principalColumn: "StockItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockItemAccessories_StockAccessoryId",
                table: "StockItemAccessories",
                column: "StockAccessoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockItemAccessories");

            migrationBuilder.DropTable(
                name: "StockAccessories");

            migrationBuilder.DropTable(
                name: "StockItems");
        }
    }
}
