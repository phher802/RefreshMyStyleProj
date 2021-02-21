using Microsoft.EntityFrameworkCore.Migrations;

namespace RefreshMyStyleApp.Migrations
{
    public partial class EditAttendEventModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d8afac3-049e-4b63-91c6-d317d783b839");

            migrationBuilder.AlterColumn<int>(
                name: "AttendeeId",
                table: "Attendees",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2cc0d86c-7c58-495e-bdcd-aa7b6ca113ee", "e48dc2af-31c1-4c44-a430-be7759c9ac69", "ApplicationUser", "APPLICATIONUSER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2cc0d86c-7c58-495e-bdcd-aa7b6ca113ee");

            migrationBuilder.AlterColumn<int>(
                name: "AttendeeId",
                table: "Attendees",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2d8afac3-049e-4b63-91c6-d317d783b839", "41ed7f24-f579-450f-931a-56252dc618d7", "ApplicationUser", "APPLICATIONUSER" });
        }
    }
}
