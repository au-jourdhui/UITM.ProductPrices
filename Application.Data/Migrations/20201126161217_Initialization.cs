using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.Data.Migrations
{
    public partial class Initialization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ProductTypeID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Products_ProductTypes_ProductTypeID",
                        column: x => x.ProductTypeID,
                        principalTable: "ProductTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ProductTypes",
                columns: new[] { "ID", "Name" },
                values: new object[] { 1, "Goods" });

            migrationBuilder.InsertData(
                table: "ProductTypes",
                columns: new[] { "ID", "Name" },
                values: new object[] { 2, "Service" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductTypeID",
                table: "Products",
                column: "ProductTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductTypes");
        }
    }
}
