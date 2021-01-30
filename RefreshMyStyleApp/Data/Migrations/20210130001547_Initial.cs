using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RefreshMyStyleApp.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.CreateTable(
                name: "FriendsList",
                columns: table => new
                {
                    FriendsListId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendsList", x => x.FriendsListId);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ImageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageTitle = table.Column<string>(nullable: true),
                    ImageData = table.Column<byte[]>(nullable: true),
                    ClothingCategory = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    Size = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ToShare = table.Column<bool>(nullable: false),
                    ToGiveAway = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ImageId);
                });

            migrationBuilder.CreateTable(
                name: "profileImages",
                columns: table => new
                {
                    ProfileImageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileImageTitle = table.Column<string>(nullable: true),
                    ProfileImageData = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profileImages", x => x.ProfileImageId);
                });

            migrationBuilder.CreateTable(
                name: "EventList",
                columns: table => new
                {
                    EventListId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventList", x => x.EventListId);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    EventId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatePosted = table.Column<DateTime>(nullable: true),
                    EventDate = table.Column<DateTime>(nullable: true),
                    EventTitle = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    EventListId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_Event_EventList_EventListId",
                        column: x => x.EventListId,
                        principalTable: "EventList",
                        principalColumn: "EventListId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClothingEnthusiast",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FName = table.Column<string>(nullable: true),
                    LName = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    ProfileImageId = table.Column<int>(nullable: true),
                    ImageId = table.Column<int>(nullable: true),
                    EventId = table.Column<int>(nullable: true),
                    FriendsListId = table.Column<int>(nullable: true),
                    IdentityUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClothingEnthusiast", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_ClothingEnthusiast_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClothingEnthusiast_FriendsList_FriendsListId",
                        column: x => x.FriendsListId,
                        principalTable: "FriendsList",
                        principalColumn: "FriendsListId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClothingEnthusiast_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClothingEnthusiast_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "ImageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClothingEnthusiast_profileImages_ProfileImageId",
                        column: x => x.ProfileImageId,
                        principalTable: "profileImages",
                        principalColumn: "ProfileImageId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3529b2d0-3803-4cf7-b6e4-ecafbddcc3e9", "ef822aeb-68e6-43e0-9365-a5cef6b0c7c4", "User", "USER" });

            migrationBuilder.CreateIndex(
                name: "IX_ClothingEnthusiast_EventId",
                table: "ClothingEnthusiast",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_ClothingEnthusiast_FriendsListId",
                table: "ClothingEnthusiast",
                column: "FriendsListId",
                unique: true,
                filter: "[FriendsListId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClothingEnthusiast_IdentityUserId",
                table: "ClothingEnthusiast",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClothingEnthusiast_ImageId",
                table: "ClothingEnthusiast",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ClothingEnthusiast_ProfileImageId",
                table: "ClothingEnthusiast",
                column: "ProfileImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_EventListId",
                table: "Event",
                column: "EventListId");

            migrationBuilder.CreateIndex(
                name: "IX_EventList_UserId",
                table: "EventList",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventList_ClothingEnthusiast_UserId",
                table: "EventList",
                column: "UserId",
                principalTable: "ClothingEnthusiast",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClothingEnthusiast_Event_EventId",
                table: "ClothingEnthusiast");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "EventList");

            migrationBuilder.DropTable(
                name: "ClothingEnthusiast");

            migrationBuilder.DropTable(
                name: "FriendsList");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "profileImages");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3529b2d0-3803-4cf7-b6e4-ecafbddcc3e9");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
