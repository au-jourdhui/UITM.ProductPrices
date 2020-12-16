using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.Data.Migrations
{
    public partial class Contractors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contractors",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contractors", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ContractorProductPrices",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContractorID = table.Column<int>(type: "INTEGER", nullable: true),
                    ProductID = table.Column<int>(type: "INTEGER", nullable: true),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractorProductPrices", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ContractorProductPrices_Contractors_ContractorID",
                        column: x => x.ContractorID,
                        principalTable: "Contractors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractorProductPrices_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractorProductPrices_ContractorID",
                table: "ContractorProductPrices",
                column: "ContractorID");

            migrationBuilder.CreateIndex(
                name: "IX_ContractorProductPrices_ProductID",
                table: "ContractorProductPrices",
                column: "ProductID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractorProductPrices");

            migrationBuilder.DropTable(
                name: "Contractors");
        }
    }
}
