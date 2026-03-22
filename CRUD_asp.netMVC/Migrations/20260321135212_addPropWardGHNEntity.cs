using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD_asp.netMVC.Migrations
{
    /// <inheritdoc />
    public partial class addPropWardGHNEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GovernmentCode",
                table: "WardGHN",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GovernmentCode",
                table: "WardGHN");
        }
    }
}
