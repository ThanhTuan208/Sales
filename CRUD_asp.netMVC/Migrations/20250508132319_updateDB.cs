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
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "OldPrice");

            migrationBuilder.AddColumn<double>(
                name: "NewPrice",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "SelectColor",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "selectSize",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 1500000.0, 1800000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 900000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 2800000.0, 3000000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 1100000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 5,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 590000.0, 650000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 6,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 1900000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 7,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 1450000.0, 1600000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 8,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 850000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 9,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 3200000.0, 3400000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 10,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 990000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 11,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 650000.0, 700000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 12,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 1750000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 13,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 1900000.0, 2000000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 14,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 980000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 15,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 1200000.0, 1300000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 16,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 1250000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 17,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 450000.0, 500000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 18,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 1350000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 19,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 950000.0, 1000000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 20,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 700000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 21,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 820000.0, 900000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 22,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 600000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 23,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 200000.0, 250000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 24,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 550000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 25,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 1750000.0, 1900000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 26,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 950000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 27,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 1050000.0, 1150000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 28,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 850000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 29,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 400000.0, 450000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 30,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 980000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 31,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 25000000.0, 27000000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 32,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 15500000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 33,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 22000000.0, 24000000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 34,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 30000000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 35,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 9000000.0, 9500000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 36,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 45000000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 37,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 1450000.0, 1600000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 38,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 1250000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 39,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 1150000.0, 1250000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 40,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 980000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 41,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 500000.0, 550000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 42,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 850000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 43,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 2700000.0, 2900000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 44,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 1450000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 45,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 2200000.0, 2400000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 46,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 1900000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 47,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 1100000.0, 1200000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 48,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 2600000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 49,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 1550000.0, 1700000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 50,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 1100000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 51,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 1800000.0, 2000000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 52,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 950000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 53,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 600000.0, 650000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 54,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 1200000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 55,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 9500000.0, 10000000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 56,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 7200000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 57,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 8300000.0, 9000000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 58,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 13000000.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 59,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 3200000.0, 3500000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 60,
                columns: new[] { "NewPrice", "OldPrice" },
                values: new object[] { 18000000.0, 0.0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewPrice",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "OldPrice",
                table: "Products",
                newName: "Price");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 1,
                column: "Price",
                value: 1500000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 2,
                column: "Price",
                value: 900000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 3,
                column: "Price",
                value: 2800000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 4,
                column: "Price",
                value: 1100000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 5,
                column: "Price",
                value: 590000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 6,
                column: "Price",
                value: 1900000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 7,
                column: "Price",
                value: 1450000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 8,
                column: "Price",
                value: 850000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 9,
                column: "Price",
                value: 3200000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 10,
                column: "Price",
                value: 990000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 11,
                column: "Price",
                value: 650000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 12,
                column: "Price",
                value: 1750000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 13,
                column: "Price",
                value: 1900000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 14,
                column: "Price",
                value: 980000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 15,
                column: "Price",
                value: 1200000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 16,
                column: "Price",
                value: 1250000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 17,
                column: "Price",
                value: 450000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 18,
                column: "Price",
                value: 1350000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 19,
                column: "Price",
                value: 950000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 20,
                column: "Price",
                value: 700000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 21,
                column: "Price",
                value: 820000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 22,
                column: "Price",
                value: 600000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 23,
                column: "Price",
                value: 200000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 24,
                column: "Price",
                value: 550000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 25,
                column: "Price",
                value: 1750000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 26,
                column: "Price",
                value: 950000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 27,
                column: "Price",
                value: 1050000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 28,
                column: "Price",
                value: 850000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 29,
                column: "Price",
                value: 400000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 30,
                column: "Price",
                value: 980000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 31,
                column: "Price",
                value: 25000000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 32,
                column: "Price",
                value: 15500000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 33,
                column: "Price",
                value: 22000000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 34,
                column: "Price",
                value: 30000000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 35,
                column: "Price",
                value: 9000000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 36,
                column: "Price",
                value: 45000000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 37,
                column: "Price",
                value: 1450000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 38,
                column: "Price",
                value: 1250000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 39,
                column: "Price",
                value: 1150000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 40,
                column: "Price",
                value: 980000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 41,
                column: "Price",
                value: 500000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 42,
                column: "Price",
                value: 850000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 43,
                column: "Price",
                value: 2700000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 44,
                column: "Price",
                value: 1450000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 45,
                column: "Price",
                value: 2200000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 46,
                column: "Price",
                value: 1900000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 47,
                column: "Price",
                value: 1100000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 48,
                column: "Price",
                value: 2600000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 49,
                column: "Price",
                value: 1550000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 50,
                column: "Price",
                value: 1100000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 51,
                column: "Price",
                value: 1800000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 52,
                column: "Price",
                value: 950000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 53,
                column: "Price",
                value: 600000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 54,
                column: "Price",
                value: 1200000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 55,
                column: "Price",
                value: 9500000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 56,
                column: "Price",
                value: 7200000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 57,
                column: "Price",
                value: 8300000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 58,
                column: "Price",
                value: 13000000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 59,
                column: "Price",
                value: 3200000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 60,
                column: "Price",
                value: 18000000.0);
        }
    }
}
