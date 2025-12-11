using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD_asp.netMVC.Migrations
{
    /// <inheritdoc />
    public partial class addIndexingSiteUserEnitty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_UserByQuarter",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserByYear",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StartQuarter",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StartYear",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StartMonth",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Day",
                table: "SiteUser",
                type: "date",
                nullable: false,
                computedColumnSql: "CONVERT(DATE, [Date]) PERSISTED");

            migrationBuilder.CreateIndex(
                name: "IX_SiteUsers_Day",
                table: "SiteUser",
                column: "Day");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SiteUsers_Day",
                table: "SiteUser");

            migrationBuilder.DropColumn(
                name: "Day",
                table: "SiteUser");

            migrationBuilder.AddColumn<int>(
                name: "StartMonth",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartQuarter",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                computedColumnSql: "DATEPART(QUARTER, [StartDate]) PERSISTED");

            migrationBuilder.AddColumn<int>(
                name: "StartYear",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                computedColumnSql: "DATEPART(YEAR, [StartDate]) PERSISTED");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserByQuarter",
                table: "AspNetUsers",
                columns: new[] { "StartQuarter", "StartYear" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserByYear",
                table: "AspNetUsers",
                column: "StartYear");
        }
    }
}
