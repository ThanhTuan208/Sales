using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CRUD_asp.netMVC.Migrations
{
    /// <inheritdoc />
    public partial class addSeedDataUserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 3, null, "Guest", "GUEST" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "Gender", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImage", "RoleId", "RolesId", "SecurityStamp", "StartDate", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { 8, 0, "87fa214c-77b7-49b6-969d-b05cd4b8a85a", new DateTime(1, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "user_demo@gmail.com", true, "Nguyễn", "Nam", "Tuấn", true, new DateTimeOffset(new DateTime(2025, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "USER_DEMO@GMAIL.COM", "USERDTRAI", "AQAAAAIAAYagAAAAEIGrep56jZMETG3pvlfvY8ZuqfD8kcKSnkL2LdTVXMmuouD38WnwdkAp5z3lqzIqCg==", "0358986824", true, "images/avatar/2bc6c1a6-74ab-43e3-9c14-7b900fc59112.jpg", 2, null, "T6GUP2BYL5M7Y63HDKWNDRF2TRBJ3VHW", new DateTime(2025, 7, 25, 12, 6, 38, 0, DateTimeKind.Unspecified), false, "userdtrai" },
                    { 10, 0, "6eef443c-bba9-4122-a821-920e06361f5a", new DateTime(2005, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin_demo@gmail.com", true, "Nguyễn", "Nam", "Tuấn", true, new DateTimeOffset(new DateTime(2025, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "ADMIN_DEMO@GMAIL.COM", "ADMINDTRAI", "AQAAAAIAAYagAAAAEET3xX8a043bIf6s88V8JHayJR91oPDtwcbSwBu4DNGtnr4err11wI/IwyJSUI/l0Q==", "0358986823", true, "images/avatar/2bc6c1a6-74ab-43e3-9c14-7b900fc59112.jpg", 1, null, "4SKEY4LFJ5YFEM5REPISLB4YBHVSTUR3", new DateTime(2025, 11, 26, 19, 44, 21, 0, DateTimeKind.Unspecified), false, "admindtrai" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 2, 8 },
                    { 1, 10 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 8 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 10 });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
