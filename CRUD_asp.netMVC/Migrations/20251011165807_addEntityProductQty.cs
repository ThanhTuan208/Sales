using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD_asp.netMVC.Migrations
{
    /// <inheritdoc />
    public partial class addEntityProductQty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductQty",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    ColorID = table.Column<int>(type: "int", nullable: false),
                    SizeID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductQty", x => new { x.ProductID, x.SizeID, x.ColorID });
                    table.ForeignKey(
                        name: "FK_ProductQty_Color_ColorID",
                        column: x => x.ColorID,
                        principalTable: "Color",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductQty_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductQty_Size_SizeID",
                        column: x => x.SizeID,
                        principalTable: "Size",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductQty_ColorID",
                table: "ProductQty",
                column: "ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductQty_SizeID",
                table: "ProductQty",
                column: "SizeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductQty");
        }
    }
}
