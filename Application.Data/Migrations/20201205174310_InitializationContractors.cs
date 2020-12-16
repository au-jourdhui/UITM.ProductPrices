using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.Data.Migrations
{
    public partial class InitializationContractors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Contractors",
                columns: new[] { "ID", "Name" },
                values: new object[] { 1, "Amazon" });

            migrationBuilder.InsertData(
                table: "Contractors",
                columns: new[] { "ID", "Name" },
                values: new object[] { 2, "Google" });

            migrationBuilder.InsertData(
                table: "Contractors",
                columns: new[] { "ID", "Name" },
                values: new object[] { 3, "Orange" });

            migrationBuilder.InsertData(
                table: "Contractors",
                columns: new[] { "ID", "Name" },
                values: new object[] { 4, "MediaMarkt" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Contractors",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Contractors",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Contractors",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Contractors",
                keyColumn: "ID",
                keyValue: 4);
        }
    }
}
