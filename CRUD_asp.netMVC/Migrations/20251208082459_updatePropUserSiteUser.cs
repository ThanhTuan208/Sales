using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD_asp.netMVC.Migrations
{
    /// <inheritdoc />
    public partial class updatePropUserSiteUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalView",
                table: "SiteUser");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SiteUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "DailyActiveUsers",
                table: "SiteUser",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UniqueVisitors",
                table: "SiteUser",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SiteUser");

            migrationBuilder.DropColumn(
                name: "DailyActiveUsers",
                table: "SiteUser");

            migrationBuilder.DropColumn(
                name: "UniqueVisitors",
                table: "SiteUser");

            migrationBuilder.AddColumn<int>(
                name: "TotalView",
                table: "SiteUser",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
