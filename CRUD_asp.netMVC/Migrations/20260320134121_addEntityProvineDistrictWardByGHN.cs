using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD_asp.netMVC.Migrations
{
    /// <inheritdoc />
    public partial class addEntityProvineDistrictWardByGHN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProvinceGHN",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProvinceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProvinceID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvinceGHN", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DistrictGHN",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistrictName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DistrictID = table.Column<int>(type: "int", nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistrictGHN", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DistrictGHN_ProvinceGHN_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "ProvinceGHN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WardGHN",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WardCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false),
                    WardName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WardGHN", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WardGHN_DistrictGHN_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "DistrictGHN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DistrictGHN_ProvinceId",
                table: "DistrictGHN",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_WardGHN_DistrictId",
                table: "WardGHN",
                column: "DistrictId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WardGHN");

            migrationBuilder.DropTable(
                name: "DistrictGHN");

            migrationBuilder.DropTable(
                name: "ProvinceGHN");
        }
    }
}
