using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.Data.Migrations
{
    public partial class FixPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "ContractorProductPrices",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "ContractorProductPrices",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");
        }
    }
}
