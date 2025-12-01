using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD_asp.netMVC.Migrations
{
    /// <inheritdoc />
    public partial class addIndexingPayemnt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StartYear",
                table: "Payment",
                type: "int",
                nullable: false,
                computedColumnSql: "DATEPART(YEAR, [PaymentDate]) PERSISTED",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "StartMonth",
                table: "Payment",
                type: "int",
                nullable: false,
                computedColumnSql: "DATEPART(MONTH, [PaymentDate]) PERSISTED",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "StartDay",
                table: "Payment",
                type: "date",
                nullable: false,
                computedColumnSql: "CONVERT(DATE, [PaymentDate]) PERSISTED",
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PaymentByDay",
                table: "Payment",
                column: "StartDay");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PaymentByMonth",
                table: "Payment",
                columns: new[] { "StartMonth", "StartYear" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payment_PaymentByDay",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_PaymentByMonth",
                table: "Payment");

            migrationBuilder.AlterColumn<int>(
                name: "StartYear",
                table: "Payment",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComputedColumnSql: "DATEPART(YEAR, [PaymentDate]) PERSISTED");

            migrationBuilder.AlterColumn<int>(
                name: "StartMonth",
                table: "Payment",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComputedColumnSql: "DATEPART(MONTH, [PaymentDate]) PERSISTED");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "StartDay",
                table: "Payment",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldComputedColumnSql: "CONVERT(DATE, [PaymentDate]) PERSISTED");
        }
    }
}
