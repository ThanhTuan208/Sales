using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CRUD_asp.netMVC.Migrations
{
    /// <inheritdoc />
    public partial class initialSecond : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductMaterial",
                columns: new[] { "MaterialID", "ProductID" },
                values: new object[,]
                {
                    { 2, 1 },
                    { 5, 1 },
                    { 1, 2 },
                    { 2, 2 },
                    { 2, 3 },
                    { 4, 3 },
                    { 1, 4 },
                    { 2, 4 },
                    { 1, 5 },
                    { 2, 5 },
                    { 2, 6 },
                    { 2, 7 },
                    { 5, 7 },
                    { 1, 8 },
                    { 2, 8 },
                    { 2, 9 },
                    { 4, 9 },
                    { 1, 10 },
                    { 2, 10 },
                    { 1, 11 },
                    { 2, 11 },
                    { 4, 12 },
                    { 2, 13 },
                    { 5, 13 },
                    { 1, 14 },
                    { 2, 14 },
                    { 4, 15 },
                    { 1, 16 },
                    { 2, 16 },
                    { 1, 17 },
                    { 4, 18 },
                    { 2, 19 },
                    { 5, 19 },
                    { 1, 20 },
                    { 2, 20 },
                    { 4, 21 },
                    { 1, 22 },
                    { 2, 22 },
                    { 1, 23 },
                    { 2, 24 },
                    { 2, 25 },
                    { 5, 25 },
                    { 1, 26 },
                    { 2, 26 },
                    { 4, 27 },
                    { 1, 28 },
                    { 2, 28 },
                    { 1, 29 },
                    { 4, 30 },
                    { 2, 31 },
                    { 3, 31 },
                    { 5, 31 },
                    { 3, 32 },
                    { 5, 32 },
                    { 4, 33 },
                    { 3, 34 },
                    { 1, 35 },
                    { 3, 36 },
                    { 4, 36 },
                    { 1, 37 },
                    { 2, 37 },
                    { 1, 38 },
                    { 4, 39 },
                    { 1, 40 },
                    { 1, 41 },
                    { 4, 42 },
                    { 2, 43 },
                    { 5, 43 },
                    { 1, 44 },
                    { 2, 44 },
                    { 4, 45 },
                    { 1, 46 },
                    { 2, 46 },
                    { 1, 47 },
                    { 4, 48 },
                    { 2, 49 },
                    { 5, 49 },
                    { 1, 50 },
                    { 2, 50 },
                    { 2, 51 },
                    { 4, 51 },
                    { 1, 52 },
                    { 2, 52 },
                    { 1, 53 },
                    { 2, 53 },
                    { 2, 54 },
                    { 2, 55 },
                    { 3, 55 },
                    { 5, 55 },
                    { 3, 56 },
                    { 5, 56 },
                    { 4, 57 },
                    { 3, 58 },
                    { 1, 59 },
                    { 3, 59 },
                    { 3, 60 },
                    { 4, 60 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 4, 3 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 4 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 5 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 5 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 6 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 7 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 5, 7 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 8 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 8 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 9 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 4, 9 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 10 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 10 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 11 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 11 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 4, 12 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 13 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 5, 13 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 14 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 14 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 4, 15 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 16 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 16 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 17 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 4, 18 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 19 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 5, 19 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 20 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 20 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 4, 21 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 22 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 22 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 23 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 24 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 25 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 5, 25 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 26 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 26 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 4, 27 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 28 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 28 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 29 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 4, 30 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 31 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 3, 31 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 5, 31 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 3, 32 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 5, 32 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 4, 33 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 3, 34 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 35 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 3, 36 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 4, 36 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 37 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 37 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 38 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 4, 39 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 40 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 41 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 4, 42 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 43 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 5, 43 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 44 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 44 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 4, 45 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 46 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 46 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 47 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 4, 48 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 49 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 5, 49 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 50 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 50 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 51 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 4, 51 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 52 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 52 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 53 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 53 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 54 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 2, 55 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 3, 55 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 5, 55 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 3, 56 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 5, 56 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 4, 57 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 3, 58 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 1, 59 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 3, 59 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 3, 60 });

            migrationBuilder.DeleteData(
                table: "ProductMaterial",
                keyColumns: new[] { "MaterialID", "ProductID" },
                keyValues: new object[] { 4, 60 });
        }
    }
}
