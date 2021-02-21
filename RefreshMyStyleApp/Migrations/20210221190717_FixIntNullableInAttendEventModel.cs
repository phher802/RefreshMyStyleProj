using Microsoft.EntityFrameworkCore.Migrations;

namespace RefreshMyStyleApp.Migrations
{
    public partial class FixIntNullableInAttendEventModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2cc0d86c-7c58-495e-bdcd-aa7b6ca113ee");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Attendees",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AttendeeId",
                table: "Attendees",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "543d3aba-fa92-4c6d-9a6c-30a2aee04cd6", "2cf5483c-b764-442f-96dd-374ab10c2c28", "ApplicationUser", "APPLICATIONUSER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "543d3aba-fa92-4c6d-9a6c-30a2aee04cd6");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Attendees",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AttendeeId",
                table: "Attendees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2cc0d86c-7c58-495e-bdcd-aa7b6ca113ee", "e48dc2af-31c1-4c44-a430-be7759c9ac69", "ApplicationUser", "APPLICATIONUSER" });
        }
    }
}
