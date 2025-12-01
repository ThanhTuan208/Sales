using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD_asp.netMVC.Migrations
{
    /// <inheritdoc />
    public partial class updateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "paidAmount",
                table: "Payment",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDay",
                table: "Payment",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "StartMonth",
                table: "Payment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartYear",
                table: "Payment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StartMonth",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDay",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                computedColumnSql: "CONVERT(DATE, [StartDate]) PERSISTED");

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
                name: "IX_Users_UserByDay",
                table: "AspNetUsers",
                column: "StartDay");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserByQuarter",
                table: "AspNetUsers",
                columns: new[] { "StartQuarter", "StartYear" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserByYear",
                table: "AspNetUsers",
                column: "StartYear");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_UserByDay",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserByQuarter",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserByYear",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StartDay",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StartQuarter",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StartYear",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StartDay",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "StartMonth",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "StartYear",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "StartMonth",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<double>(
                name: "paidAmount",
                table: "Payment",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "Orders",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }
    }
}
