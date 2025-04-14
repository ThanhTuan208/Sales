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
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_RoleID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RoleID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "AspNetRoles");

            migrationBuilder.AddColumn<int>(
                name: "RolesId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "RolesId",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                column: "RolesId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RolesId",
                table: "AspNetUsers",
                column: "RolesId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_RolesId",
                table: "AspNetUsers",
                column: "RolesId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_RolesId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RolesId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RolesId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "AspNetRoles",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "UserID",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "UserID",
                value: 2);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RoleID",
                table: "AspNetUsers",
                column: "RoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_RoleID",
                table: "AspNetUsers",
                column: "RoleID",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
