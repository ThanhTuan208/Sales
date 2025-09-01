using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CRUD_asp.netMVC.Migrations
{
    /// <inheritdoc />
    public partial class initialFirstDB : Migration
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
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PicturePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PicturePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Color",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Featured",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Featured", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Gender",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gender", x => x.ID);
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
                name: "Material",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material", x => x.ID);
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
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ConfirmPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                name: "Size",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Size", x => x.ID);
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
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProfileImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoleID = table.Column<int>(type: "int", nullable: false),
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
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PicturePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NormalizedDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldPrice = table.Column<double>(type: "float", nullable: false),
                    NewPrice = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FeaturedID = table.Column<int>(type: "int", nullable: false),
                    GenderID = table.Column<int>(type: "int", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_Products_Featured_FeaturedID",
                        column: x => x.FeaturedID,
                        principalTable: "Featured",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Gender_GenderID",
                        column: x => x.GenderID,
                        principalTable: "Gender",
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
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalPrice = table.Column<double>(type: "float", nullable: false)
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
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    SelectColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                name: "ProductColor",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    ColorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductColor", x => new { x.ProductID, x.ColorID });
                    table.ForeignKey(
                        name: "FK_ProductColor_Color_ColorID",
                        column: x => x.ColorID,
                        principalTable: "Color",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductColor_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PathNameImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductMaterial",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    MaterialID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMaterial", x => new { x.ProductID, x.MaterialID });
                    table.ForeignKey(
                        name: "FK_ProductMaterial_Material_MaterialID",
                        column: x => x.MaterialID,
                        principalTable: "Material",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductMaterial_Products_ProductID",
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
                name: "ProductSize",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    SizeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSize", x => new { x.ProductID, x.SizeID });
                    table.ForeignKey(
                        name: "FK_ProductSize_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSize_Size_SizeID",
                        column: x => x.SizeID,
                        principalTable: "Size",
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
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Orders_OrderID",
                        column: x => x.OrderID,
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
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    paymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    paidAmount = table.Column<double>(type: "float", nullable: false),
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
                    { 1, null, "Admin", "ADMIN" },
                    { 2, null, "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "Brand",
                columns: new[] { "ID", "Description", "Name", "PicturePath" },
                values: new object[,]
                {
                    { 1, "Thương hiệu giày thể thao và trang phục thể thao nổi tiếng", "Nike", "images/logo/logo_nike.gif" },
                    { 2, "Chuyên cung cấp giày dép và trang phục thể thao", "Adidas", "images/logo/logo_adidas.png" },
                    { 3, "Thương hiệu thời trang cao cấp với thiết kế hiện đại", "Zara", "images/logo/logo_zara.png" },
                    { 4, "H&M cung cấp thời trang giá cả phải chăng và phong cách hiện đại", "H&M", "images/logo/logo_HM.webp" },
                    { 5, "Thương hiệu Nhật Bản nổi tiếng với phong cách tối giản và chất liệu tốt", "Uniqlo", "images/logo/logo_uniqlo.png" },
                    { 6, "Thương hiệu cao cấp đến từ Ý với các sản phẩm thời trang xa xỉ", "Gucci", "images/logo/logo_gucci.jpg" },
                    { 7, "Hãng thời trang nổi tiếng với quần jeans và phong cách cổ điển Mỹ", "Levi's", "images/logo/logo_levis.png" },
                    { 8, "Phong cách thể thao thanh lịch đến từ Pháp", "Lacoste", "images/logo/logo_lacoste.png" },
                    { 9, "Cung cấp thời trang thể thao, giày dép và phụ kiện chất lượng", "Puma", "images/logo/logo_puma.jpg" },
                    { 10, "Biểu tượng thời trang xa xỉ với những thiết kế đẳng cấp, cổ điển", "Chanel", "images/logo/logo_chanel.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "ID", "Name", "PicturePath" },
                values: new object[,]
                {
                    { 1, "Áo khoác", "images/Category/jacket1.png" },
                    { 2, "Quần", "images/Category/trousers1.png" },
                    { 3, "Giày", "images/Category/shoes1.png" },
                    { 4, "Váy", "images/Category/skirt1.png" },
                    { 5, "Áo thun", "images/Category/tshirt1.png" },
                    { 6, "Đồng hồ", "images/Category/wristwatch1.png" }
                });

            migrationBuilder.InsertData(
                table: "Color",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "Đen" },
                    { 2, "Xám" },
                    { 3, "Trắng" },
                    { 4, "Hồng" },
                    { 5, "Xanh navy" },
                    { 6, "Trắng/Xanh" },
                    { 7, "Hồng pastel" },
                    { 8, "Đỏ đậm" }
                });

            migrationBuilder.InsertData(
                table: "Featured",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "Nổi bật" },
                    { 2, "Không nổi bật" }
                });

            migrationBuilder.InsertData(
                table: "Gender",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "Nam" },
                    { 2, "Nữ" },
                    { 3, "Unisex" }
                });

            migrationBuilder.InsertData(
                table: "Material",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "Cotton" },
                    { 2, "Polyester" },
                    { 3, "Silk" },
                    { 4, "Leather" },
                    { 5, "Wool" }
                });

            migrationBuilder.InsertData(
                table: "Season",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "Mùa xuân" },
                    { 2, "Mùa hạ" },
                    { 3, "Mùa thu" },
                    { 4, "Mùa đông" }
                });

            migrationBuilder.InsertData(
                table: "Size",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "S" },
                    { 2, "M" },
                    { 3, "L" },
                    { 4, "XL" },
                    { 5, "36" },
                    { 6, "37" },
                    { 7, "38" },
                    { 8, "39" },
                    { 9, "40" },
                    { 10, "41" },
                    { 11, "42" },
                    { 12, "43" },
                    { 13, "44" },
                    { 14, "Free Size" }
                });

            migrationBuilder.InsertData(
                table: "Style",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "Basic" },
                    { 2, "Sport" },
                    { 3, "Formal" },
                    { 4, "Streetwear" },
                    { 5, "Vintage" },
                    { 6, "Casual" }
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
                columns: new[] { "ID", "BrandID", "CateID", "Created", "Description", "FeaturedID", "GenderID", "Name", "NewPrice", "NormalizedDescription", "NormalizedName", "OldPrice", "PicturePath", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Áo khoác nam chính hãng Nike, phong cách thể thao", 2, 1, "Áo khoác nam Nike", 1500000.0, "ao khoac nam chinh hang nike, phong cach the thao", "ao khoac nam nike", 1800000.0, "nike_jacket.jpg", 100 },
                    { 2, 1, 2, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Quần thể thao Nike chất liệu thoáng mát, phù hợp tập luyện", 2, 1, "Quần thể thao Nike", 900000.0, "quan the thao nike chat lieu thoang mat, phu hop tap luyen", "quan the thao nike", 0.0, "nike_pants.webp", 80 },
                    { 3, 1, 3, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Giày thể thao Nike Air Max thoải mái và bền bỉ", 2, 1, "Giày Nike Air Max", 2800000.0, "giay the thao nike air max thoai mai va ben bi", "giay nike air max", 3000000.0, "nike_shoes.jpg", 60 },
                    { 4, 1, 4, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Váy thể thao nữ năng động, thiết kế hiện đại", 2, 2, "Váy thể thao Nike nữ", 1100000.0, "vay the thao nu nang đong, thiet ke hien đai", "vay the thao nike nu", 0.0, "nike_dress.jpg", 40 },
                    { 5, 1, 5, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Áo thun nam Nike cổ tròn, chất liệu cotton thoải mái", 2, 1, "Áo thun Nike basic", 590000.0, "ao thun nam nike co tron, chat lieu cotton thoai mai", "ao thun nike basic", 650000.0, "nike_tshirt.jpg", 200 },
                    { 6, 1, 6, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đồng hồ thể thao phong cách Nike, chống nước, dây silicon", 2, 3, "Đồng hồ thể thao Nike", 1900000.0, "đong ho the thao phong cach nike, chong nuoc, day silicon", "đong ho the thao nike", 0.0, "nike_watch.jpg", 25 },
                    { 7, 2, 1, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế cổ điển, chống gió và giữ ấm tốt", 2, 1, "Áo khoác Adidas Originals", 1450000.0, "thiet ke co đien, chong gio va giu am tot", "ao khoac adidas originals", 1600000.0, "adidas_jacket.jpg", 70 },
                    { 8, 2, 2, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Quần thể thao thoải mái, phù hợp vận động", 2, 1, "Quần jogger Adidas", 850000.0, "quan the thao thoai mai, phu hop van đong", "quan jogger adidas", 0.0, "adidas_pants.jpg", 90 },
                    { 9, 2, 3, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đệm Boost êm ái, hỗ trợ chạy bộ hiệu quả", 2, 3, "Giày Adidas Ultraboost", 3200000.0, "đem boost em ai, ho tro chay bo hieu qua", "giay adidas ultraboost", 3400000.0, "adidas_shoes.jpg", 50 },
                    { 10, 2, 4, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Váy thể thao nhẹ, thấm hút mồ hôi tốt", 2, 2, "Váy tennis Adidas nữ", 990000.0, "vay the thao nhe, tham hut mo hoi tot", "vay tennis adidas nu", 0.0, "adidas_dress.jpg", 45 },
                    { 11, 2, 5, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Áo thun cổ điển, logo 3 sọc đặc trưng", 2, 1, "Áo thun Adidas cổ tròn", 650000.0, "ao thun co đien, logo 3 soc đac trung", "ao thun adidas co tron", 700000.0, "adidas_tshirt.jpg", 150 },
                    { 12, 2, 6, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế năng động, chống nước tốt", 2, 3, "Đồng hồ thể thao Adidas", 1750000.0, "thiet ke nang đong, chong nuoc tot", "đong ho the thao adidas", 0.0, "adidas_watch.jpg", 30 },
                    { 13, 3, 1, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Áo khoác nữ dáng dài thanh lịch", 2, 2, "Áo khoác Zara Dáng Dài", 1900000.0, "ao khoac nu dang dai thanh lich", "ao khoac zara dang dai", 2000000.0, "zara_jacket.jpg", 60 },
                    { 14, 3, 2, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Phong cách hiện đại, thời trang", 2, 2, "Quần Ống Rộng Zara", 980000.0, "phong cach hien đai, thoi trang", "quan ong rong zara", 0.0, "zara_pants.jpg", 40 },
                    { 15, 3, 3, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế sang trọng, phù hợp tiệc tùng", 2, 2, "Giày Cao Gót Zara", 1200000.0, "thiet ke sang trong, phu hop tiec tung", "giay cao got zara", 1300000.0, "zara_heels.jpg", 55 },
                    { 16, 3, 4, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Váy dài thướt tha, thanh lịch", 2, 2, "Váy Xếp Ly Zara", 1250000.0, "vay dai thuot tha, thanh lich", "vay xep ly zara", 0.0, "zara_dress.jpg", 35 },
                    { 17, 3, 5, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Áo thun đơn giản, dễ phối đồ", 2, 3, "Áo Thun Zara Basic", 450000.0, "ao thun đon gian, de phoi đo", "ao thun zara basic", 500000.0, "zara_tshirt.jpg", 90 },
                    { 18, 3, 6, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế mặt tròn, dây da tinh tế", 2, 2, "Đồng Hồ Kim Zara", 1350000.0, "thiet ke mat tron, day da tinh te", "đong ho kim zara", 0.0, "zara_watch.jpg", 25 },
                    { 19, 4, 1, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chống gió, nhẹ và gọn", 2, 1, "Áo Khoác Dù H&M", 950000.0, "chong gio, nhe va gon", "ao khoac du h&m", 1000000.0, "hm_jacket.jpg", 80 },
                    { 20, 4, 2, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Phong cách công sở lịch sự", 2, 1, "Quần Tây H&M", 700000.0, "phong cach cong so lich su", "quan tay h&m", 0.0, "hm_pants.jpg", 65 },
                    { 21, 4, 3, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Giày đơn giản, năng động", 2, 3, "Giày Thể Thao H&M", 820000.0, "giay đon gian, nang đong", "giay the thao h&m", 900000.0, "hm_shoes.jpg", 100 },
                    { 22, 4, 4, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Váy xinh xắn cho mùa hè", 2, 2, "Váy Ngắn Hoa Nhí H&M", 600000.0, "vay xinh xan cho mua he", "vay ngan hoa nhi h&m", 0.0, "hm_dress.jpg", 45 },
                    { 23, 4, 5, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Áo thun trơn giá rẻ", 2, 1, "Áo Thun Nam Basic H&M", 200000.0, "ao thun tron gia re", "ao thun nam basic h&m", 250000.0, "hm_tshirt.jpg", 150 },
                    { 24, 4, 6, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế đơn giản, hiện đại", 2, 3, "Đồng Hồ Dây Silicon H&M", 550000.0, "thiet ke đon gian, hien đai", "đong ho day silicon h&m", 0.0, "hm_watch.jpg", 40 },
                    { 25, 5, 1, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Giữ ấm nhẹ nhàng, tiện lợi", 2, 1, "Áo khoác lông vũ Uniqlo", 1750000.0, "giu am nhe nhang, tien loi", "ao khoac long vu uniqlo", 1900000.0, "uniqlo_jacket.jpg", 70 },
                    { 26, 5, 2, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jean co giãn, phong cách tối giản", 2, 1, "Quần Jean Uniqlo", 950000.0, "jean co gian, phong cach toi gian", "quan jean uniqlo", 0.0, "uniqlo_jeans.jpg", 50 },
                    { 27, 5, 3, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế tối giản, thoải mái", 2, 1, "Giày Sneaker Uniqlo", 1050000.0, "thiet ke toi gian, thoai mai", "giay sneaker uniqlo", 1150000.0, "uniqlo_shoes.jpg", 60 },
                    { 28, 5, 4, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế tối giản, thoải mái cho mùa hè", 2, 2, "Váy Cotton Uniqlo", 850000.0, "thiet ke toi gian, thoai mai cho mua he", "vay cotton uniqlo", 0.0, "uniqlo_dress.jpg", 40 },
                    { 29, 5, 5, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thoáng mát, nhanh khô, thích hợp mùa hè", 2, 3, "Áo thun Airism Uniqlo", 400000.0, "thoang mat, nhanh kho, thich hop mua he", "ao thun airism uniqlo", 450000.0, "uniqlo_tshirt.jpg", 120 },
                    { 30, 5, 6, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Phong cách tối giản, dây da đen", 2, 1, "Đồng hồ tối giản Uniqlo", 980000.0, "phong cach toi gian, day da đen", "đong ho toi gian uniqlo", 0.0, "uniqlo_watch.jpg", 30 },
                    { 31, 6, 1, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế sang trọng, dành cho thời trang cao cấp", 2, 2, "Áo khoác cao cấp Gucci", 25000000.0, "thiet ke sang trong, danh cho thoi trang cao cap", "ao khoac cao cap gucci", 27000000.0, "gucci_jacket.jpg", 10 },
                    { 32, 6, 2, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Phong cách lịch lãm, chất liệu cao cấp", 2, 1, "Quần tây Gucci nam", 15500000.0, "phong cach lich lam, chat lieu cao cap", "quan tay gucci nam", 0.0, "gucci_pants.jpg", 15 },
                    { 33, 6, 3, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế logo đặc trưng, đẳng cấp", 2, 1, "Giày lười Gucci", 22000000.0, "thiet ke logo đac trung, đang cap", "giay luoi gucci", 24000000.0, "gucci_shoes.jpg", 12 },
                    { 34, 6, 4, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chất liệu mềm mại, sang trọng", 2, 2, "Váy lụa cao cấp Gucci", 30000000.0, "chat lieu mem mai, sang trong", "vay lua cao cap gucci", 0.0, "gucci_dress.jpg", 8 },
                    { 35, 6, 5, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Phong cách trẻ trung, thời thượng", 2, 3, "Áo thun Gucci logo lớn", 9000000.0, "phong cach tre trung, thoi thuong", "ao thun gucci logo lon", 9500000.0, "gucci_tshirt.jpg", 20 },
                    { 36, 6, 6, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đồng hồ xa xỉ với thiết kế tinh xảo", 2, 2, "Đồng hồ đính đá Gucci", 45000000.0, "đong ho xa xi voi thiet ke tinh xao", "đong ho đinh đa gucci", 0.0, "gucci_watch.jpg", 5 },
                    { 37, 7, 1, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chất liệu jean bền, phong cách cổ điển", 2, 1, "Áo khoác jean Levi's", 1450000.0, "chat lieu jean ben, phong cach co đien", "ao khoac jean levi's", 1600000.0, "levis_jacket.jpg", 40 },
                    { 38, 7, 2, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Form dáng chuẩn Mỹ, chất lượng cao", 2, 1, "Quần jeans Levi's 501", 1250000.0, "form dang chuan my, chat luong cao", "quan jeans levi's 501", 0.0, "levis_jeans.jpg", 60 },
                    { 39, 7, 3, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thoải mái và thời trang hàng ngày", 2, 1, "Giày thể thao Levi's", 1150000.0, "thoai mai va thoi trang hang ngay", "giay the thao levi's", 1250000.0, "levis_shoes.jpg", 35 },
                    { 40, 7, 4, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế trẻ trung, năng động", 2, 2, "Váy bò Levi's nữ", 980000.0, "thiet ke tre trung, nang đong", "vay bo levi's nu", 0.0, "levis_dress.jpg", 20 },
                    { 41, 7, 5, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Logo cổ điển, phong cách Mỹ", 2, 3, "Áo thun cổ tròn Levi's", 500000.0, "logo co đien, phong cach my", "ao thun co tron levi's", 550000.0, "levis_tshirt.jpg", 100 },
                    { 42, 7, 6, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đơn giản, mạnh mẽ, cá tính", 2, 1, "Đồng hồ dây da Levi's", 850000.0, "đon gian, manh me, ca tinh", "đong ho day da levi's", 0.0, "levis_watch.jpg", 15 },
                    { 43, 8, 1, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế năng động, thời trang", 2, 3, "Áo khoác thể thao Lacoste", 2700000.0, "thiet ke nang đong, thoi trang", "ao khoac the thao lacoste", 2900000.0, "lacoste_jacket.jpg", 30 },
                    { 44, 8, 2, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Phong cách thể thao, thoáng mát", 2, 1, "Quần short Lacoste", 1450000.0, "phong cach the thao, thoang mat", "quan short lacoste", 0.0, "lacoste_short.jpg", 40 },
                    { 45, 8, 3, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chất liệu cao cấp, thiết kế tinh tế", 2, 1, "Giày sneaker Lacoste", 2200000.0, "chat lieu cao cap, thiet ke tinh te", "giay sneaker lacoste", 2400000.0, "lacoste_shoes.jpg", 25 },
                    { 46, 8, 4, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lịch sự, sang trọng cho nữ", 2, 2, "Váy polo Lacoste", 1900000.0, "lich su, sang trong cho nu", "vay polo lacoste", 0.0, "lacoste_dress.jpg", 18 },
                    { 47, 8, 5, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Biểu tượng nổi bật với logo cá sấu", 2, 1, "Áo thun cá sấu Lacoste", 1100000.0, "bieu tuong noi bat voi logo ca sau", "ao thun ca sau lacoste", 1200000.0, "lacoste_tshirt.jpg", 55 },
                    { 48, 8, 6, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Năng động, trẻ trung, phù hợp với người chơi thể thao", 2, 3, "Đồng hồ thể thao Lacoste", 2600000.0, "nang đong, tre trung, phu hop voi nguoi choi the thao", "đong ho the thao lacoste", 0.0, "lacoste_watch.jpg", 20 },
                    { 49, 9, 1, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế hiện đại, năng động", 2, 1, "Áo khoác thể thao Puma", 1550000.0, "thiet ke hien đai, nang đong", "ao khoac the thao puma", 1700000.0, "puma_jacket.jpg", 50 },
                    { 50, 9, 2, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thoải mái, phù hợp vận động", 2, 1, "Quần jogger Puma", 1100000.0, "thoai mai, phu hop van đong", "quan jogger puma", 0.0, "puma_pants.jpg", 70 },
                    { 51, 9, 3, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế thể thao, hỗ trợ di chuyển", 2, 1, "Giày chạy bộ Puma", 1800000.0, "thiet ke the thao, ho tro di chuyen", "giay chay bo puma", 2000000.0, "puma_shoes.jpg", 40 },
                    { 52, 9, 4, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Phù hợp tập luyện hoặc mặc thường ngày", 2, 2, "Váy thể thao Puma nữ", 950000.0, "phu hop tap luyen hoac mac thuong ngay", "vay the thao puma nu", 0.0, "puma_dress.jpg", 25 },
                    { 53, 9, 5, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chất vải thấm hút, thoáng mát", 2, 3, "Áo thun thể thao Puma", 600000.0, "chat vai tham hut, thoang mat", "ao thun the thao puma", 650000.0, "puma_tshirt.jpg", 90 },
                    { 54, 9, 6, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế mạnh mẽ, phong cách", 2, 1, "Đồng hồ thể thao Puma", 1200000.0, "thiet ke manh me, phong cach", "đong ho the thao puma", 0.0, "puma_watch.jpg", 18 },
                    { 55, 10, 1, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đẳng cấp, thời thượng, thiết kế quý phái", 2, 2, "Áo khoác dạ Chanel", 9500000.0, "đang cap, thoi thuong, thiet ke quy phai", "ao khoac da chanel", 10000000.0, "chanel_jacket.jpg", 15 },
                    { 56, 10, 2, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đường may tinh xảo, chất liệu mềm mịn", 2, 2, "Quần vải cao cấp Chanel", 7200000.0, "đuong may tinh xao, chat lieu mem min", "quan vai cao cap chanel", 0.0, "chanel_pants.jpg", 12 },
                    { 57, 10, 3, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thanh lịch, sang trọng", 2, 2, "Giày cao gót Chanel", 8300000.0, "thanh lich, sang trong", "giay cao got chanel", 9000000.0, "chanel_shoes.jpg", 10 },
                    { 58, 10, 4, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lộng lẫy, thiết kế quyến rũ", 2, 2, "Váy dạ hội Chanel", 13000000.0, "long lay, thiet ke quyen ru", "vay da hoi chanel", 0.0, "chanel_dress.jpg", 8 },
                    { 59, 10, 5, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đơn giản nhưng đầy tinh tế", 2, 2, "Áo thun cao cấp Chanel", 3200000.0, "đon gian nhung đay tinh te", "ao thun cao cap chanel", 3500000.0, "chanel_tshirt.jpg", 20 },
                    { 60, 10, 6, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tinh xảo, sang trọng và nữ tính", 2, 2, "Đồng hồ Chanel sang trọng", 18000000.0, "tinh xao, sang trong va nu tinh", "đong ho chanel sang trong", 0.0, "chanel_watch.jpg", 6 }
                });

            migrationBuilder.InsertData(
                table: "ProductColor",
                columns: new[] { "ColorID", "ProductID" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 5, 1 },
                    { 1, 2 },
                    { 2, 2 },
                    { 3, 2 },
                    { 5, 2 },
                    { 1, 3 },
                    { 2, 3 },
                    { 3, 3 },
                    { 5, 3 },
                    { 1, 4 },
                    { 2, 4 },
                    { 3, 4 },
                    { 5, 4 },
                    { 1, 5 },
                    { 2, 5 },
                    { 3, 5 },
                    { 5, 5 },
                    { 1, 6 },
                    { 2, 6 },
                    { 3, 6 },
                    { 5, 6 },
                    { 1, 7 },
                    { 2, 7 },
                    { 3, 7 },
                    { 5, 7 },
                    { 1, 8 },
                    { 2, 8 },
                    { 3, 8 },
                    { 5, 8 },
                    { 1, 9 },
                    { 2, 9 },
                    { 3, 9 },
                    { 5, 9 },
                    { 1, 10 },
                    { 2, 10 },
                    { 3, 10 },
                    { 5, 10 },
                    { 1, 11 },
                    { 2, 11 },
                    { 3, 11 },
                    { 5, 11 },
                    { 1, 12 },
                    { 2, 12 },
                    { 3, 12 },
                    { 5, 12 },
                    { 1, 13 },
                    { 2, 13 },
                    { 3, 13 },
                    { 5, 13 },
                    { 1, 14 },
                    { 2, 14 },
                    { 3, 14 },
                    { 5, 14 },
                    { 1, 15 },
                    { 2, 15 },
                    { 3, 15 },
                    { 5, 15 },
                    { 1, 16 },
                    { 2, 16 },
                    { 3, 16 },
                    { 5, 16 },
                    { 1, 17 },
                    { 2, 17 },
                    { 3, 17 },
                    { 5, 17 },
                    { 1, 18 },
                    { 2, 18 },
                    { 3, 18 },
                    { 5, 18 },
                    { 1, 19 },
                    { 2, 19 },
                    { 3, 19 },
                    { 5, 19 },
                    { 1, 20 },
                    { 2, 20 },
                    { 3, 20 },
                    { 5, 20 },
                    { 1, 21 },
                    { 2, 21 },
                    { 3, 21 },
                    { 5, 21 },
                    { 1, 22 },
                    { 2, 22 },
                    { 3, 22 },
                    { 5, 22 },
                    { 1, 23 },
                    { 2, 23 },
                    { 3, 23 },
                    { 5, 23 },
                    { 1, 24 },
                    { 2, 24 },
                    { 3, 24 },
                    { 5, 24 },
                    { 1, 25 },
                    { 2, 25 },
                    { 3, 25 },
                    { 5, 25 },
                    { 1, 26 },
                    { 2, 26 },
                    { 3, 26 },
                    { 5, 26 },
                    { 1, 27 },
                    { 2, 27 },
                    { 3, 27 },
                    { 5, 27 },
                    { 1, 28 },
                    { 2, 28 },
                    { 3, 28 },
                    { 5, 28 },
                    { 1, 29 },
                    { 2, 29 },
                    { 3, 29 },
                    { 5, 29 },
                    { 1, 30 },
                    { 2, 30 },
                    { 3, 30 },
                    { 5, 30 },
                    { 1, 31 },
                    { 2, 31 },
                    { 3, 31 },
                    { 5, 31 },
                    { 1, 32 },
                    { 2, 32 },
                    { 3, 32 },
                    { 5, 32 },
                    { 1, 33 },
                    { 2, 33 },
                    { 3, 33 },
                    { 5, 33 },
                    { 1, 34 },
                    { 2, 34 },
                    { 3, 34 },
                    { 5, 34 },
                    { 1, 35 },
                    { 2, 35 },
                    { 3, 35 },
                    { 5, 35 },
                    { 1, 36 },
                    { 2, 36 },
                    { 3, 36 },
                    { 5, 36 },
                    { 1, 37 },
                    { 2, 37 },
                    { 3, 37 },
                    { 5, 37 },
                    { 1, 38 },
                    { 2, 38 },
                    { 3, 38 },
                    { 5, 38 },
                    { 1, 39 },
                    { 2, 39 },
                    { 3, 39 },
                    { 5, 39 },
                    { 1, 40 },
                    { 2, 40 },
                    { 3, 40 },
                    { 5, 40 },
                    { 1, 41 },
                    { 2, 41 },
                    { 3, 41 },
                    { 5, 41 },
                    { 1, 42 },
                    { 2, 42 },
                    { 3, 42 },
                    { 5, 42 },
                    { 1, 43 },
                    { 2, 43 },
                    { 3, 43 },
                    { 5, 43 },
                    { 1, 44 },
                    { 2, 44 },
                    { 3, 44 },
                    { 5, 44 },
                    { 1, 45 },
                    { 2, 45 },
                    { 3, 45 },
                    { 5, 45 },
                    { 1, 46 },
                    { 2, 46 },
                    { 3, 46 },
                    { 5, 46 },
                    { 1, 47 },
                    { 2, 47 },
                    { 3, 47 },
                    { 5, 47 },
                    { 1, 48 },
                    { 2, 48 },
                    { 3, 48 },
                    { 5, 48 },
                    { 1, 49 },
                    { 2, 49 },
                    { 3, 49 },
                    { 5, 49 },
                    { 1, 50 },
                    { 2, 50 },
                    { 3, 50 },
                    { 5, 50 },
                    { 1, 51 },
                    { 2, 51 },
                    { 3, 51 },
                    { 5, 51 },
                    { 1, 52 },
                    { 2, 52 },
                    { 3, 52 },
                    { 5, 52 },
                    { 1, 53 },
                    { 2, 53 },
                    { 3, 53 },
                    { 5, 53 },
                    { 1, 54 },
                    { 2, 54 },
                    { 3, 54 },
                    { 5, 54 },
                    { 1, 55 },
                    { 2, 55 },
                    { 3, 55 },
                    { 5, 55 },
                    { 1, 56 },
                    { 2, 56 },
                    { 3, 56 },
                    { 5, 56 },
                    { 1, 57 },
                    { 2, 57 },
                    { 3, 57 },
                    { 5, 57 },
                    { 1, 58 },
                    { 2, 58 },
                    { 3, 58 },
                    { 5, 58 },
                    { 1, 59 },
                    { 2, 59 },
                    { 3, 59 },
                    { 5, 59 },
                    { 1, 60 },
                    { 2, 60 },
                    { 3, 60 },
                    { 5, 60 }
                });

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

            migrationBuilder.InsertData(
                table: "ProductSeason",
                columns: new[] { "ProductID", "SeasonID" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 },
                    { 5, 1 },
                    { 6, 1 },
                    { 7, 1 },
                    { 8, 1 },
                    { 9, 1 },
                    { 10, 1 },
                    { 11, 1 },
                    { 12, 1 },
                    { 13, 1 },
                    { 14, 1 },
                    { 15, 1 },
                    { 16, 1 },
                    { 17, 1 },
                    { 18, 1 },
                    { 19, 1 },
                    { 20, 1 },
                    { 21, 1 },
                    { 22, 1 },
                    { 23, 1 },
                    { 24, 1 },
                    { 25, 1 },
                    { 26, 1 },
                    { 27, 1 },
                    { 28, 1 },
                    { 29, 1 },
                    { 30, 1 },
                    { 31, 1 },
                    { 32, 1 },
                    { 33, 1 },
                    { 34, 1 },
                    { 35, 1 },
                    { 36, 1 },
                    { 37, 1 },
                    { 38, 1 },
                    { 39, 1 },
                    { 40, 1 },
                    { 41, 1 },
                    { 42, 1 },
                    { 43, 1 },
                    { 45, 1 },
                    { 46, 1 },
                    { 47, 1 },
                    { 48, 1 },
                    { 49, 1 },
                    { 50, 1 },
                    { 51, 1 },
                    { 52, 1 },
                    { 53, 1 },
                    { 54, 1 },
                    { 55, 1 },
                    { 56, 1 },
                    { 57, 1 },
                    { 58, 1 },
                    { 59, 1 },
                    { 60, 1 },
                    { 1, 2 },
                    { 2, 2 },
                    { 3, 2 },
                    { 4, 2 },
                    { 5, 2 },
                    { 6, 2 },
                    { 7, 2 },
                    { 8, 2 },
                    { 9, 2 },
                    { 10, 2 },
                    { 11, 2 },
                    { 12, 2 },
                    { 13, 2 },
                    { 14, 2 },
                    { 15, 2 },
                    { 16, 2 },
                    { 17, 2 },
                    { 18, 2 },
                    { 19, 2 },
                    { 20, 2 },
                    { 21, 2 },
                    { 22, 2 },
                    { 23, 2 },
                    { 24, 2 },
                    { 25, 2 },
                    { 26, 2 },
                    { 27, 2 },
                    { 28, 2 },
                    { 29, 2 },
                    { 30, 2 },
                    { 31, 2 },
                    { 32, 2 },
                    { 33, 2 },
                    { 34, 2 },
                    { 35, 2 },
                    { 36, 2 },
                    { 37, 2 },
                    { 38, 2 },
                    { 39, 2 },
                    { 40, 2 },
                    { 41, 2 },
                    { 42, 2 },
                    { 43, 2 },
                    { 44, 2 },
                    { 45, 2 },
                    { 46, 2 },
                    { 47, 2 },
                    { 48, 2 },
                    { 49, 2 },
                    { 50, 2 },
                    { 51, 2 },
                    { 52, 2 },
                    { 53, 2 },
                    { 54, 2 },
                    { 55, 2 },
                    { 56, 2 },
                    { 57, 2 },
                    { 58, 2 },
                    { 59, 2 },
                    { 60, 2 },
                    { 1, 3 },
                    { 2, 3 },
                    { 3, 3 },
                    { 5, 3 },
                    { 6, 3 },
                    { 7, 3 },
                    { 8, 3 },
                    { 9, 3 },
                    { 11, 3 },
                    { 12, 3 },
                    { 13, 3 },
                    { 14, 3 },
                    { 17, 3 },
                    { 18, 3 },
                    { 19, 3 },
                    { 20, 3 },
                    { 21, 3 },
                    { 23, 3 },
                    { 24, 3 },
                    { 25, 3 },
                    { 26, 3 },
                    { 27, 3 },
                    { 29, 3 },
                    { 30, 3 },
                    { 31, 3 },
                    { 32, 3 },
                    { 33, 3 },
                    { 35, 3 },
                    { 36, 3 },
                    { 37, 3 },
                    { 38, 3 },
                    { 39, 3 },
                    { 41, 3 },
                    { 42, 3 },
                    { 43, 3 },
                    { 45, 3 },
                    { 47, 3 },
                    { 48, 3 },
                    { 49, 3 },
                    { 50, 3 },
                    { 51, 3 },
                    { 53, 3 },
                    { 54, 3 },
                    { 55, 3 },
                    { 56, 3 },
                    { 58, 3 },
                    { 59, 3 },
                    { 60, 3 },
                    { 1, 4 },
                    { 2, 4 },
                    { 3, 4 },
                    { 6, 4 },
                    { 7, 4 },
                    { 8, 4 },
                    { 9, 4 },
                    { 12, 4 },
                    { 13, 4 },
                    { 14, 4 },
                    { 18, 4 },
                    { 19, 4 },
                    { 20, 4 },
                    { 21, 4 },
                    { 24, 4 },
                    { 25, 4 },
                    { 26, 4 },
                    { 27, 4 },
                    { 30, 4 },
                    { 31, 4 },
                    { 32, 4 },
                    { 33, 4 },
                    { 36, 4 },
                    { 37, 4 },
                    { 38, 4 },
                    { 39, 4 },
                    { 42, 4 },
                    { 43, 4 },
                    { 45, 4 },
                    { 48, 4 },
                    { 49, 4 },
                    { 50, 4 },
                    { 51, 4 },
                    { 54, 4 },
                    { 55, 4 },
                    { 56, 4 },
                    { 60, 4 }
                });

            migrationBuilder.InsertData(
                table: "ProductSize",
                columns: new[] { "ProductID", "SizeID" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 1, 4 },
                    { 2, 1 },
                    { 2, 2 },
                    { 2, 3 },
                    { 2, 4 },
                    { 3, 1 },
                    { 3, 2 },
                    { 3, 3 },
                    { 3, 4 },
                    { 4, 1 },
                    { 4, 2 },
                    { 4, 3 },
                    { 4, 4 },
                    { 5, 1 },
                    { 5, 2 },
                    { 5, 3 },
                    { 5, 4 },
                    { 6, 1 },
                    { 6, 2 },
                    { 6, 3 },
                    { 6, 4 },
                    { 7, 1 },
                    { 7, 2 },
                    { 7, 3 },
                    { 7, 4 },
                    { 8, 1 },
                    { 8, 2 },
                    { 8, 3 },
                    { 8, 4 },
                    { 9, 1 },
                    { 9, 2 },
                    { 9, 3 },
                    { 9, 4 },
                    { 10, 1 },
                    { 10, 2 },
                    { 10, 3 },
                    { 10, 4 },
                    { 11, 1 },
                    { 11, 2 },
                    { 11, 3 },
                    { 11, 4 },
                    { 12, 1 },
                    { 12, 2 },
                    { 12, 3 },
                    { 12, 4 },
                    { 13, 1 },
                    { 13, 2 },
                    { 13, 3 },
                    { 13, 4 },
                    { 14, 1 },
                    { 14, 2 },
                    { 14, 3 },
                    { 14, 4 },
                    { 15, 1 },
                    { 15, 2 },
                    { 15, 3 },
                    { 15, 4 },
                    { 16, 1 },
                    { 16, 2 },
                    { 16, 3 },
                    { 16, 4 },
                    { 17, 1 },
                    { 17, 2 },
                    { 17, 3 },
                    { 17, 4 },
                    { 18, 1 },
                    { 18, 2 },
                    { 18, 3 },
                    { 18, 4 },
                    { 19, 1 },
                    { 19, 2 },
                    { 19, 3 },
                    { 19, 4 },
                    { 20, 1 },
                    { 20, 2 },
                    { 20, 3 },
                    { 20, 4 },
                    { 21, 1 },
                    { 21, 2 },
                    { 21, 3 },
                    { 21, 4 },
                    { 22, 1 },
                    { 22, 2 },
                    { 22, 3 },
                    { 22, 4 },
                    { 23, 1 },
                    { 23, 2 },
                    { 23, 3 },
                    { 23, 4 },
                    { 24, 1 },
                    { 24, 2 },
                    { 24, 3 },
                    { 24, 4 },
                    { 25, 1 },
                    { 25, 2 },
                    { 25, 3 },
                    { 25, 4 },
                    { 26, 1 },
                    { 26, 2 },
                    { 26, 3 },
                    { 26, 4 },
                    { 27, 1 },
                    { 27, 2 },
                    { 27, 3 },
                    { 27, 4 },
                    { 28, 1 },
                    { 28, 2 },
                    { 28, 3 },
                    { 28, 4 },
                    { 29, 1 },
                    { 29, 2 },
                    { 29, 3 },
                    { 29, 4 },
                    { 30, 1 },
                    { 30, 2 },
                    { 30, 3 },
                    { 30, 4 },
                    { 31, 1 },
                    { 31, 2 },
                    { 31, 3 },
                    { 31, 4 },
                    { 32, 1 },
                    { 32, 2 },
                    { 32, 3 },
                    { 32, 4 },
                    { 33, 1 },
                    { 33, 2 },
                    { 33, 3 },
                    { 33, 4 },
                    { 34, 1 },
                    { 34, 2 },
                    { 34, 3 },
                    { 34, 4 },
                    { 35, 1 },
                    { 35, 2 },
                    { 35, 3 },
                    { 35, 4 },
                    { 36, 1 },
                    { 36, 2 },
                    { 36, 3 },
                    { 36, 4 },
                    { 37, 1 },
                    { 37, 2 },
                    { 37, 3 },
                    { 37, 4 },
                    { 38, 1 },
                    { 38, 2 },
                    { 38, 3 },
                    { 38, 4 },
                    { 39, 1 },
                    { 39, 2 },
                    { 39, 3 },
                    { 39, 4 },
                    { 40, 1 },
                    { 40, 2 },
                    { 40, 3 },
                    { 40, 4 },
                    { 41, 1 },
                    { 41, 2 },
                    { 41, 3 },
                    { 41, 4 },
                    { 42, 1 },
                    { 42, 2 },
                    { 42, 3 },
                    { 42, 4 },
                    { 43, 1 },
                    { 43, 2 },
                    { 43, 3 },
                    { 43, 4 },
                    { 44, 1 },
                    { 44, 2 },
                    { 44, 3 },
                    { 44, 4 },
                    { 45, 1 },
                    { 45, 2 },
                    { 45, 3 },
                    { 45, 4 },
                    { 46, 1 },
                    { 46, 2 },
                    { 46, 3 },
                    { 46, 4 },
                    { 47, 1 },
                    { 47, 2 },
                    { 47, 3 },
                    { 47, 4 },
                    { 48, 1 },
                    { 48, 2 },
                    { 48, 3 },
                    { 48, 4 },
                    { 49, 1 },
                    { 49, 2 },
                    { 49, 3 },
                    { 49, 4 },
                    { 50, 1 },
                    { 50, 2 },
                    { 50, 3 },
                    { 50, 4 },
                    { 51, 1 },
                    { 51, 2 },
                    { 51, 3 },
                    { 51, 4 },
                    { 52, 1 },
                    { 52, 2 },
                    { 52, 3 },
                    { 52, 4 },
                    { 53, 1 },
                    { 53, 2 },
                    { 53, 3 },
                    { 53, 4 },
                    { 54, 1 },
                    { 54, 2 },
                    { 54, 3 },
                    { 54, 4 },
                    { 55, 1 },
                    { 55, 2 },
                    { 55, 3 },
                    { 55, 4 },
                    { 56, 1 },
                    { 56, 2 },
                    { 56, 3 },
                    { 56, 4 },
                    { 57, 1 },
                    { 57, 2 },
                    { 57, 3 },
                    { 57, 4 },
                    { 58, 1 },
                    { 58, 2 },
                    { 58, 3 },
                    { 58, 4 },
                    { 59, 1 },
                    { 59, 2 },
                    { 59, 3 },
                    { 59, 4 },
                    { 60, 1 },
                    { 60, 2 },
                    { 60, 3 },
                    { 60, 4 }
                });

            migrationBuilder.InsertData(
                table: "ProductStyle",
                columns: new[] { "ProductID", "StyleID" },
                values: new object[,]
                {
                    { 23, 1 },
                    { 29, 1 },
                    { 1, 2 },
                    { 3, 2 },
                    { 4, 2 },
                    { 5, 2 },
                    { 6, 2 },
                    { 7, 2 },
                    { 9, 2 },
                    { 10, 2 },
                    { 11, 2 },
                    { 12, 2 },
                    { 13, 2 },
                    { 19, 2 },
                    { 25, 2 },
                    { 43, 2 },
                    { 44, 2 },
                    { 49, 2 },
                    { 51, 2 },
                    { 52, 2 },
                    { 53, 2 },
                    { 54, 2 },
                    { 15, 3 },
                    { 31, 3 },
                    { 32, 3 },
                    { 35, 3 },
                    { 36, 3 },
                    { 55, 3 },
                    { 56, 3 },
                    { 57, 3 },
                    { 58, 3 },
                    { 59, 3 },
                    { 60, 3 },
                    { 21, 4 },
                    { 27, 4 },
                    { 33, 4 },
                    { 35, 4 },
                    { 39, 4 },
                    { 45, 4 },
                    { 59, 4 },
                    { 37, 5 },
                    { 38, 5 },
                    { 1, 6 },
                    { 2, 6 },
                    { 7, 6 },
                    { 8, 6 },
                    { 13, 6 },
                    { 14, 6 },
                    { 16, 6 },
                    { 17, 6 },
                    { 18, 6 },
                    { 19, 6 },
                    { 20, 6 },
                    { 21, 6 },
                    { 22, 6 },
                    { 24, 6 },
                    { 25, 6 },
                    { 26, 6 },
                    { 27, 6 },
                    { 28, 6 },
                    { 30, 6 },
                    { 33, 6 },
                    { 34, 6 },
                    { 37, 6 },
                    { 38, 6 },
                    { 39, 6 },
                    { 40, 6 },
                    { 41, 6 },
                    { 42, 6 },
                    { 43, 6 },
                    { 45, 6 },
                    { 46, 6 },
                    { 47, 6 },
                    { 48, 6 },
                    { 49, 6 },
                    { 50, 6 }
                });

            migrationBuilder.InsertData(
                table: "ProductTag",
                columns: new[] { "ProductID", "TagID" },
                values: new object[,]
                {
                    { 14, 1 },
                    { 23, 1 },
                    { 39, 1 },
                    { 47, 1 },
                    { 57, 1 },
                    { 1, 2 },
                    { 2, 2 },
                    { 4, 2 },
                    { 6, 2 },
                    { 7, 2 },
                    { 8, 2 },
                    { 10, 2 },
                    { 12, 2 },
                    { 13, 2 },
                    { 15, 2 },
                    { 19, 2 },
                    { 27, 2 },
                    { 31, 2 },
                    { 37, 2 },
                    { 41, 2 },
                    { 45, 2 },
                    { 49, 2 },
                    { 52, 2 },
                    { 55, 2 },
                    { 60, 2 },
                    { 3, 3 },
                    { 9, 3 },
                    { 13, 3 },
                    { 21, 3 },
                    { 29, 3 },
                    { 35, 3 },
                    { 43, 3 },
                    { 48, 3 },
                    { 54, 3 },
                    { 4, 4 },
                    { 10, 4 },
                    { 16, 4 },
                    { 20, 4 },
                    { 24, 4 },
                    { 30, 4 },
                    { 34, 4 },
                    { 50, 4 },
                    { 59, 4 },
                    { 1, 5 },
                    { 2, 5 },
                    { 3, 5 },
                    { 5, 5 },
                    { 7, 5 },
                    { 11, 5 },
                    { 15, 5 },
                    { 17, 5 },
                    { 22, 5 },
                    { 25, 5 },
                    { 28, 5 },
                    { 32, 5 },
                    { 36, 5 },
                    { 42, 5 },
                    { 46, 5 },
                    { 51, 5 },
                    { 58, 5 },
                    { 16, 6 },
                    { 26, 6 },
                    { 33, 6 },
                    { 40, 6 },
                    { 53, 6 },
                    { 18, 7 },
                    { 44, 7 },
                    { 6, 8 },
                    { 9, 8 },
                    { 12, 8 },
                    { 38, 8 },
                    { 56, 8 }
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
                name: "IX_OrderDetail_OrderID",
                table: "OrderDetail",
                column: "OrderID");

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
                name: "IX_ProductColor_ColorID",
                table: "ProductColor",
                column: "ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductID",
                table: "ProductImages",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMaterial_MaterialID",
                table: "ProductMaterial",
                column: "MaterialID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandID",
                table: "Products",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CateID",
                table: "Products",
                column: "CateID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_FeaturedID",
                table: "Products",
                column: "FeaturedID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_GenderID",
                table: "Products",
                column: "GenderID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSeason_ProductID",
                table: "ProductSeason",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSize_SizeID",
                table: "ProductSize",
                column: "SizeID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStyle_ProductID",
                table: "ProductStyle",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTag_ProductID",
                table: "ProductTag",
                column: "ProductID");

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
                name: "ProductColor");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductMaterial");

            migrationBuilder.DropTable(
                name: "ProductSeason");

            migrationBuilder.DropTable(
                name: "ProductSize");

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
                name: "Color");

            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropTable(
                name: "Season");

            migrationBuilder.DropTable(
                name: "Size");

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
                name: "Featured");

            migrationBuilder.DropTable(
                name: "Gender");

            migrationBuilder.DropTable(
                name: "AspNetRoles");
        }
    }
}
