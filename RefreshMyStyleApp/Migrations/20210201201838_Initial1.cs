using Microsoft.EntityFrameworkCore.Migrations;

namespace RefreshMyStyleApp.Migrations
{
    public partial class Initial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c59fc846-319f-4e0a-8c6a-bf536cd67471");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "43b264b0-6f61-44b1-8860-2c45b57ce697", "213963e3-0b8d-4d65-89b1-1594b8a857d7", "ClothingEnthusiast", "CLOTHINGENTHUSIAST" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "43b264b0-6f61-44b1-8860-2c45b57ce697");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c59fc846-319f-4e0a-8c6a-bf536cd67471", "900c0a02-d982-45e1-b677-eb6c250324e1", "ClothingEnthusiast", "CLOTHINGENTHUSIAST" });
        }
    }
}
