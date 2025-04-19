using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CRUD_asp.netMVC.Migrations
{
    /// <inheritdoc />
    public partial class initialFirst : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brand",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brand", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RememberMe = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Season",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Season", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Style",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Style", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoleID = table.Column<int>(type: "int", nullable: true),
                    RolesId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_AspNetRoles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Register",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfirmPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    RolesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_Register_AspNetRoles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PicturePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Material = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BrandID = table.Column<int>(type: "int", nullable: false),
                    CateID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Products_Brand_BrandID",
                        column: x => x.BrandID,
                        principalTable: "Brand",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Category_CateID",
                        column: x => x.CateID,
                        principalTable: "Category",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Customer_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Manager",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manager", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Manager_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    totalPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Staff_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Carts_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carts_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSeason",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    SeasonID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSeason", x => new { x.SeasonID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_ProductSeason_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSeason_Season_SeasonID",
                        column: x => x.SeasonID,
                        principalTable: "Season",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductStyle",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    StyleID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStyle", x => new { x.StyleID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_ProductStyle_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductStyle_Style_StyleID",
                        column: x => x.StyleID,
                        principalTable: "Style",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTag",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    TagID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTag", x => new { x.TagID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_ProductTag_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductTag_Tag_TagID",
                        column: x => x.TagID,
                        principalTable: "Tag",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    orderID = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Orders_orderID",
                        column: x => x.orderID,
                        principalTable: "Orders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    OrderID = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    paymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    paidAmount = table.Column<double>(type: "float", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Payment_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, null, "Manager", null },
                    { 2, null, "Customer", null }
                });

            migrationBuilder.InsertData(
                table: "Brand",
                columns: new[] { "ID", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Thương hiệu giày thể thao và trang phục thể thao nổi tiếng", "Nike" },
                    { 2, "Chuyên cung cấp giày dép và trang phục thể thao", "Adidas" },
                    { 3, "Thương hiệu thời trang cao cấp với thiết kế hiện đại", "Zara" },
                    { 4, "H&M cung cấp thời trang giá cả phải chăng và phong cách hiện đại", "H&M" },
                    { 5, "Thương hiệu Nhật Bản nổi tiếng với phong cách tối giản và chất liệu tốt", "Uniqlo" },
                    { 6, "Thương hiệu cao cấp đến từ Ý với các sản phẩm thời trang xa xỉ", "Gucci" },
                    { 7, "Hãng thời trang nổi tiếng với quần jeans và phong cách cổ điển Mỹ", "Levi's" },
                    { 8, "Phong cách thể thao thanh lịch đến từ Pháp", "Lacoste" },
                    { 9, "Cung cấp thời trang thể thao, giày dép và phụ kiện chất lượng", "Puma" },
                    { 10, "Biểu tượng thời trang xa xỉ với những thiết kế đẳng cấp, cổ điển", "Chanel" }
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "Áo khoác" },
                    { 2, "Quần" },
                    { 3, "Giày" },
                    { 4, "Váy" },
                    { 5, "Áo thun" },
                    { 6, "Đồng hồ" }
                });

            migrationBuilder.InsertData(
                table: "Tag",
                columns: new[] { "ID", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Các sản phẩm liên quan đến xu hướng thời trang", "Thời trang" },
                    { 2, "Trang phục và phụ kiện dùng trong thể thao", "Thể thao" },
                    { 3, "Các loại giày dành cho vận động, thể thao", "Giày thể thao" },
                    { 4, "Sản phẩm dành riêng cho nữ giới", "Nữ" },
                    { 5, "Sản phẩm dành riêng cho nam giới", "Nam" },
                    { 6, "Thời trang cho môi trường làm việc, công sở", "Công sở" },
                    { 7, "Thời trang năng động, thoải mái khi đi chơi", "Dạo phố" },
                    { 8, "Phù hợp cho cả nam và nữ", "Unisex" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ID", "BrandID", "CateID", "Color", "Created", "Description", "Gender", "Material", "Name", "PicturePath", "Price", "Quantity", "Size" },
                values: new object[,]
                {
                    { 1, 1, 1, "Đen", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Áo khoác nam chính hãng Nike, màu đen, phong cách thể thao", "Nam", "Vải co giãn", "Áo khoác nam Nike", "jacket1.jpg", 1500000.0, 100, "L" },
                    { 2, 1, 2, "Xám", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Quần thể thao Nike chất liệu thoáng mát, phù hợp tập luyện", "Nam", "Polyester", "Quần thể thao Nike", "pants1.webp", 900000.0, 80, "M" },
                    { 3, 1, 3, "Trắng", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Giày thể thao Nike Air Max thoải mái và bền bỉ", "Nam", "Vải lưới", "Giày Nike Air Max", "shoes1.avif", 2800000.0, 60, "42" },
                    { 4, 1, 4, "Hồng", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Váy thể thao nữ năng động, thiết kế hiện đại", "Nữ", "Thun lạnh", "Váy thể thao Nike nữ", "dress1.jpg", 1100000.0, 40, "S" },
                    { 5, 1, 5, "Trắng", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Áo thun nam Nike cổ tròn, chất liệu cotton thoải mái", "Nam", "Cotton", "Áo thun Nike basic", "tshirt1.jpg", 590000.0, 200, "L" },
                    { 6, 1, 6, "Đen", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đồng hồ thể thao phong cách Nike, chống nước, dây silicon", "Unisex", "Nhựa cao cấp", "Đồng hồ thể thao Nike", "watch1.jpg", 1900000.0, 25, "Free" },
                    { 7, 2, 1, "Xanh navy", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế cổ điển, chống gió và giữ ấm tốt", "Nam", "Polyester", "Áo khoác Adidas Originals", "adidas_jacket.jpg", 1450000.0, 70, "L" },
                    { 8, 2, 2, "Xám", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Quần thể thao thoải mái, phù hợp vận động", "Nam", "Cotton pha", "Quần jogger Adidas", "adidas_pants.jpg", 850000.0, 90, "M" },
                    { 9, 2, 3, "Trắng/Xanh", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đệm Boost êm ái, hỗ trợ chạy bộ hiệu quả", "Unisex", "Vải Primeknit", "Giày Adidas Ultraboost", "adidas_shoes.webp", 3200000.0, 50, "41" },
                    { 10, 2, 4, "Hồng pastel", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Váy thể thao nhẹ, thấm hút mồ hôi tốt", "Nữ", "Polyester", "Váy tennis Adidas nữ", "adidas_dress.jpg", 990000.0, 45, "S" },
                    { 11, 2, 5, "Đen", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Áo thun cổ điển, logo 3 sọc đặc trưng", "Nam", "Cotton", "Áo thun Adidas cổ tròn", "adidas_tshirt.jpg", 650000.0, 150, "XL" },
                    { 12, 2, 6, "Đỏ đậm", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế năng động, chống nước tốt", "Unisex", "Silicon", "Đồng hồ thể thao Adidas", "adidas_watch.jpg", 1750000.0, 30, "Free" },
                    { 13, 3, 1, "Be", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Áo khoác nữ dáng dài thanh lịch", "Nữ", "Len pha", "Áo khoác Zara Dáng Dài", "zara_jacket.jpg", 1900000.0, 60, "M" },
                    { 14, 3, 2, "Đen", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Phong cách hiện đại, thời trang", "Nữ", "Vải tuyết mưa", "Quần Ống Rộng Zara", "zara_pants.jpg", 980000.0, 40, "S" },
                    { 15, 3, 3, "Kem", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế sang trọng, phù hợp tiệc tùng", "Nữ", "Da tổng hợp", "Giày Cao Gót Zara", "zara_heels.jpg", 1200000.0, 55, "38" },
                    { 16, 3, 4, "Trắng", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Váy dài thướt tha, thanh lịch", "Nữ", "Voan", "Váy Xếp Ly Zara", "zara_dress.jpg", 1250000.0, 35, "M" },
                    { 17, 3, 5, "Xám", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Áo thun đơn giản, dễ phối đồ", "Unisex", "Cotton", "Áo Thun Zara Basic", "zara_tshirt.jpg", 450000.0, 90, "M" },
                    { 18, 3, 6, "Nâu", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế mặt tròn, dây da tinh tế", "Nữ", "Thép không gỉ & da", "Đồng Hồ Kim Zara", "zara_watch.jpg", 1350000.0, 25, "Free" },
                    { 19, 4, 1, "Xanh rêu", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chống gió, nhẹ và gọn", "Nam", "Nylon", "Áo Khoác Dù H&M", "hm_jacket.jpg", 950000.0, 80, "L" },
                    { 20, 4, 2, "Xám đậm", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Phong cách công sở lịch sự", "Nam", "Polyester", "Quần Tây H&M", "hm_pants.jpg", 700000.0, 65, "M" },
                    { 21, 4, 3, "Trắng", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Giày đơn giản, năng động", "Unisex", "Canvas", "Giày Thể Thao H&M", "hm_shoes.jpg", 820000.0, 100, "42" },
                    { 22, 4, 4, "Hồng", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Váy xinh xắn cho mùa hè", "Nữ", "Cotton", "Váy Ngắn Hoa Nhí H&M", "hm_dress.jpg", 600000.0, 45, "S" },
                    { 23, 4, 5, "Xanh dương", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Áo thun trơn giá rẻ", "Nam", "Cotton", "Áo Thun Nam Basic H&M", "hm_tshirt.jpg", 200000.0, 150, "L" },
                    { 24, 4, 6, "Đen", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế đơn giản, hiện đại", "Unisex", "Silicon", "Đồng Hồ Dây Silicon H&M", "hm_watch.jpg", 550000.0, 40, "Free" },
                    { 25, 5, 1, "Xanh navy", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Giữ ấm nhẹ nhàng, tiện lợi", "Nam", "Lông vũ", "Áo khoác lông vũ Uniqlo", "uniqlo_jacket.jpg", 1750000.0, 70, "M" },
                    { 26, 5, 2, "Xanh đậm", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jean co giãn, phong cách tối giản", "Nam", "Denim", "Quần Jean Uniqlo", "uniqlo_jeans.jpg", 950000.0, 50, "L" },
                    { 27, 5, 3, "Trắng", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế tối giản, thoải mái", "Nam", "Vải dệt", "Giày Sneaker Uniqlo", "uniqlo_shoes.jpg", 1050000.0, 60, "41" },
                    { 28, 5, 4, "Be", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế tối giản, thoải mái cho mùa hè", "Nữ", "Cotton", "Váy Cotton Uniqlo", "uniqlo_dress.jpg", 850000.0, 40, "S" },
                    { 29, 5, 5, "Xám", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thoáng mát, nhanh khô, thích hợp mùa hè", "Unisex", "Vải Airism", "Áo thun Airism Uniqlo", "uniqlo_tshirt.jpg", 400000.0, 120, "L" },
                    { 30, 5, 6, "Đen", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Phong cách tối giản, dây da đen", "Nam", "Kim loại", "Đồng hồ tối giản Uniqlo", "uniqlo_watch.jpg", 980000.0, 30, "Free" },
                    { 31, 6, 1, "Đỏ đô", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế sang trọng, dành cho thời trang cao cấp", "Nữ", "Lông cừu", "Áo khoác cao cấp Gucci", "gucci_jacket.jpg", 25000000.0, 10, "M" },
                    { 32, 6, 2, "Đen", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Phong cách lịch lãm, chất liệu cao cấp", "Nam", "Lụa pha", "Quần tây Gucci nam", "gucci_pants.jpg", 15500000.0, 15, "L" },
                    { 33, 6, 3, "Nâu", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế logo đặc trưng, đẳng cấp", "Nam", "Da thật", "Giày lười Gucci", "gucci_shoes.jpg", 22000000.0, 12, "42" },
                    { 34, 6, 4, "Vàng", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chất liệu mềm mại, sang trọng", "Nữ", "Lụa", "Váy lụa cao cấp Gucci", "gucci_dress.jpg", 30000000.0, 8, "S" },
                    { 35, 6, 5, "Trắng", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Phong cách trẻ trung, thời thượng", "Unisex", "Cotton Ý", "Áo thun Gucci logo lớn", "gucci_tshirt.jpg", 9000000.0, 20, "L" },
                    { 36, 6, 6, "Vàng hồng", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đồng hồ xa xỉ với thiết kế tinh xảo", "Nữ", "Kim cương nhân tạo", "Đồng hồ đính đá Gucci", "gucci_watch.jpg", 45000000.0, 5, "Free" },
                    { 37, 7, 1, "Xanh jean", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chất liệu jean bền, phong cách cổ điển", "Nam", "Denim", "Áo khoác jean Levi's", "levis_jacket.jpg", 1450000.0, 40, "M" },
                    { 38, 7, 2, "Xanh", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Form dáng chuẩn Mỹ, chất lượng cao", "Nam", "Denim", "Quần jeans Levi's 501", "levis_jeans.jpg", 1250000.0, 60, "L" },
                    { 39, 7, 3, "Trắng", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thoải mái và thời trang hàng ngày", "Nam", "Vải canvas", "Giày thể thao Levi's", "levis_shoes.jpg", 1150000.0, 35, "42" },
                    { 40, 7, 4, "Xanh jean", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế trẻ trung, năng động", "Nữ", "Denim", "Váy bò Levi's nữ", "levis_dress.jpg", 980000.0, 20, "S" },
                    { 41, 7, 5, "Trắng", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Logo cổ điển, phong cách Mỹ", "Unisex", "Cotton", "Áo thun cổ tròn Levi's", "levis_tshirt.jpg", 500000.0, 100, "M" },
                    { 42, 7, 6, "Nâu", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đơn giản, mạnh mẽ, cá tính", "Nam", "Thép + da", "Đồng hồ dây da Levi's", "levis_watch.jpg", 850000.0, 15, "Free" },
                    { 43, 8, 1, "Trắng", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế năng động, thời trang", "Nam", "Polyester", "Áo khoác thể thao Lacoste", "lacoste_jacket.jpg", 2700000.0, 30, "L" },
                    { 44, 8, 2, "Xanh dương", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Phong cách thể thao, thoáng mát", "Nam", "Cotton", "Quần short Lacoste", "lacoste_shorts.jpg", 1450000.0, 40, "M" },
                    { 45, 8, 3, "Trắng", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chất liệu cao cấp, thiết kế tinh tế", "Nam", "Da", "Giày sneaker Lacoste", "lacoste_shoes.jpg", 2200000.0, 25, "41" },
                    { 46, 8, 4, "Hồng", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lịch sự, sang trọng cho nữ", "Nữ", "Cotton pha", "Váy polo Lacoste", "lacoste_dress.jpg", 1900000.0, 18, "S" },
                    { 47, 8, 5, "Xanh lá", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Biểu tượng nổi bật với logo cá sấu", "Nam", "Cotton", "Áo thun cá sấu Lacoste", "lacoste_tshirt.jpg", 1100000.0, 55, "L" },
                    { 48, 8, 6, "Xanh navy", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Năng động, trẻ trung, phù hợp với người chơi thể thao", "Unisex", "Nhựa + kim loại", "Đồng hồ thể thao Lacoste", "lacoste_watch.jpg", 2600000.0, 20, "Free" },
                    { 49, 9, 1, "Xám", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế hiện đại, năng động", "Nam", "Polyester", "Áo khoác thể thao Puma", "puma_jacket.jpg", 1550000.0, 50, "L" },
                    { 50, 9, 2, "Đen", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thoải mái, phù hợp vận động", "Nam", "Thun co giãn", "Quần jogger Puma", "puma_pants.jpg", 1100000.0, 70, "M" },
                    { 51, 9, 3, "Xanh", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế thể thao, hỗ trợ di chuyển", "Nam", "Vải + Cao su", "Giày chạy bộ Puma", "puma_shoes.jpg", 1800000.0, 40, "42" },
                    { 52, 9, 4, "Hồng", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Phù hợp tập luyện hoặc mặc thường ngày", "Nữ", "Cotton", "Váy thể thao Puma nữ", "puma_dress.jpg", 950000.0, 25, "S" },
                    { 53, 9, 5, "Trắng", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chất vải thấm hút, thoáng mát", "Unisex", "Polyester", "Áo thun thể thao Puma", "puma_tshirt.jpg", 600000.0, 90, "M" },
                    { 54, 9, 6, "Đen", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế mạnh mẽ, phong cách", "Nam", "Nhựa cao cấp", "Đồng hồ thể thao Puma", "puma_watch.jpg", 1200000.0, 18, "Free" },
                    { 55, 10, 1, "Đen", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đẳng cấp, thời thượng, thiết kế quý phái", "Nữ", "Dạ cao cấp", "Áo khoác dạ Chanel", "chanel_jacket.jpg", 9500000.0, 15, "S" },
                    { 56, 10, 2, "Be", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đường may tinh xảo, chất liệu mềm mịn", "Nữ", "Vải lụa", "Quần vải cao cấp Chanel", "chanel_pants.jpg", 7200000.0, 12, "M" },
                    { 57, 10, 3, "Đỏ đô", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thanh lịch, sang trọng", "Nữ", "Da thật", "Giày cao gót Chanel", "chanel_heels.jpg", 8300000.0, 10, "37" },
                    { 58, 10, 4, "Trắng", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lộng lẫy, thiết kế quyến rũ", "Nữ", "Lụa", "Váy dạ hội Chanel", "chanel_dress.jpg", 13000000.0, 8, "S" },
                    { 59, 10, 5, "Xám", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đơn giản nhưng đầy tinh tế", "Nữ", "Cotton", "Áo thun cao cấp Chanel", "chanel_tshirt.jpg", 3200000.0, 20, "M" },
                    { 60, 10, 6, "Vàng", new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tinh xảo, sang trọng và nữ tính", "Nữ", "Thép mạ vàng", "Đồng hồ Chanel sang trọng", "chanel_watch.jpg", 18000000.0, 6, "Free" }
                });

            migrationBuilder.InsertData(
                table: "ProductTag",
                columns: new[] { "ProductID", "TagID" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RolesId",
                table: "AspNetUsers",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ProductID",
                table: "Carts",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserID",
                table: "Carts",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_UserID",
                table: "Customer",
                column: "UserID",
                unique: true,
                filter: "[UserID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Manager_UserID",
                table: "Manager",
                column: "UserID",
                unique: true,
                filter: "[UserID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_orderID",
                table: "OrderDetail",
                column: "orderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ProductID",
                table: "OrderDetail",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserID",
                table: "Orders",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_OrderID",
                table: "Payment",
                column: "OrderID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandID",
                table: "Products",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CateID",
                table: "Products",
                column: "CateID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSeason_ProductID",
                table: "ProductSeason",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStyle_ProductID",
                table: "ProductStyle",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTag_ProductID",
                table: "ProductTag",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Register_RolesId",
                table: "Register",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductID",
                table: "Reviews",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserID",
                table: "Reviews",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_UserID",
                table: "Staff",
                column: "UserID",
                unique: true,
                filter: "[UserID] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Login");

            migrationBuilder.DropTable(
                name: "Manager");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "ProductSeason");

            migrationBuilder.DropTable(
                name: "ProductStyle");

            migrationBuilder.DropTable(
                name: "ProductTag");

            migrationBuilder.DropTable(
                name: "Register");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Season");

            migrationBuilder.DropTable(
                name: "Style");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Brand");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "AspNetRoles");
        }
    }
}
