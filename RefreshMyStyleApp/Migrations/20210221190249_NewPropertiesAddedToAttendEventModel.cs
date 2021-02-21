using Microsoft.EntityFrameworkCore.Migrations;

namespace RefreshMyStyleApp.Migrations
{
    public partial class NewPropertiesAddedToAttendEventModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "951cca11-b6be-422c-be81-9ee05978fab1");

            migrationBuilder.AddColumn<int>(
                name: "AttendeeId",
                table: "Attendees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Attendees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2d8afac3-049e-4b63-91c6-d317d783b839", "41ed7f24-f579-450f-931a-56252dc618d7", "ApplicationUser", "APPLICATIONUSER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d8afac3-049e-4b63-91c6-d317d783b839");

            migrationBuilder.DropColumn(
                name: "AttendeeId",
                table: "Attendees");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Attendees");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "951cca11-b6be-422c-be81-9ee05978fab1", "69fd6ce4-ec45-4804-a974-1001e0a0614b", "ApplicationUser", "APPLICATIONUSER" });
        }
    }
}
