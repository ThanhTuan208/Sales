using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD_asp.netMVC.Migrations
{
    /// <inheritdoc />
    public partial class initialSecond : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PicturePath",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 1,
                column: "PicturePath",
                value: "images/logo/logo_nike.gif");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 2,
                column: "PicturePath",
                value: "images/logo/logo_adidas.png");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 3,
                column: "PicturePath",
                value: "images/logo/logo_zara.png");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 4,
                column: "PicturePath",
                value: "images/logo/logo_HM.webp");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 5,
                column: "PicturePath",
                value: "images/logo/logo_uniqlo.png");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 6,
                column: "PicturePath",
                value: "images/logo/logo_gucci.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 7,
                column: "PicturePath",
                value: "images/logo/logo_levis.png");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 8,
                column: "PicturePath",
                value: "images/logo/logo_lacoste.png");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 9,
                column: "PicturePath",
                value: "images/logo/logo_puma.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 10,
                column: "PicturePath",
                value: "images/logo/logo_chanel.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PicturePath",
                table: "Products");
        }
    }
}
