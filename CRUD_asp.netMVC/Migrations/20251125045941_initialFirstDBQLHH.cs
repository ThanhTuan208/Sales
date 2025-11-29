using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CRUD_asp.netMVC.Migrations
{
    /// <inheritdoc />
    public partial class initialFirstDBQLHH : Migration
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
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
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
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
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
                    CateID = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: true)
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
                name: "Addresses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    RecipientName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Province = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ward = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Addresses_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
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
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Customer_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
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
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manager", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Manager_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
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
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductQty_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductQty_Size_SizeID",
                        column: x => x.SizeID,
                        principalTable: "Size",
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
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                name: "Orders",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    AddressID = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrackingNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShipRecipientName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShipPhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShipStreet = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShipProvince = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShipWard = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShipPostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Orders_Addresses_AddressID",
                        column: x => x.AddressID,
                        principalTable: "Addresses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
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
                    Price = table.Column<double>(type: "float", nullable: true)
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
                columns: new[] { "ID", "BrandID", "CateID", "Created", "Description", "FeaturedID", "GenderID", "Name", "NewPrice", "NormalizedDescription", "NormalizedName", "OldPrice", "PicturePath", "Quantity", "Weight" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Áo khoác nam chính hãng Nike, phong cách thể thao", 2, 1, "Áo khoác nam Nike", 1500000.0, "ao khoac nam chinh hang nike, phong cach the thao", "ao khoac nam nike", 1800000.0, "nike_jacket_1.1_1.1.jpg", 100, 300 },
                    { 2, 1, 2, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Quần thể thao Nike chất liệu thoáng mát, phù hợp tập luyện", 2, 1, "Quần thể thao Nike", 900000.0, "quan the thao nike chat lieu thoang mat, phu hop tap luyen", "quan the thao nike", 0.0, "nike_pants_1.1.jpg", 80, 200 },
                    { 3, 1, 3, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Giày thể thao Nike Air Max thoải mái và bền bỉ", 2, 1, "Giày Nike Air Max", 2800000.0, "giay the thao nike air max thoai mai va ben bi", "giay nike air max", 3000000.0, "nike_shoes_1.1.jpg", 60, 400 },
                    { 4, 1, 4, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Váy thể thao nữ năng động, thiết kế hiện đại", 2, 2, "Váy thể thao Nike nữ", 1100000.0, "vay the thao nu nang đong, thiet ke hien đai", "vay the thao nike nu", 0.0, "nike_dress_1.1.jpg", 40, 150 },
                    { 5, 1, 5, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Áo thun nam Nike cổ tròn, chất liệu cotton thoải mái", 2, 1, "Áo thun Nike basic", 590000.0, "ao thun nam nike co tron, chat lieu cotton thoai mai", "ao thun nike basic", 650000.0, "nike_tshirt_1.1.jpg", 200, 150 },
                    { 6, 1, 6, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Đồng hồ thể thao phong cách Nike, chống nước, dây silicon", 2, 3, "Đồng hồ thể thao Nike", 1900000.0, "đong ho the thao phong cach nike, chong nuoc, day silicon", "đong ho the thao nike", 0.0, "nike_watch_1.1.jpg", 25, 50 },
                    { 7, 2, 1, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Thiết kế cổ điển, chống gió và giữ ấm tốt", 2, 1, "Áo khoác Adidas Originals", 1450000.0, "thiet ke co đien, chong gio va giu am tot", "ao khoac adidas originals", 1600000.0, "adidas_jacket_1.1.jpg", 70, 350 },
                    { 8, 2, 2, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Quần thể thao thoải mái, phù hợp vận động", 2, 1, "Quần jogger Adidas", 850000.0, "quan the thao thoai mai, phu hop van đong", "quan jogger adidas", 0.0, "adidas_pants_1.1.jpg", 90, 200 },
                    { 9, 2, 3, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Đệm Boost êm ái, hỗ trợ chạy bộ hiệu quả", 2, 1, "Giày Adidas Ultraboost", 3200000.0, "đem boost em ai, ho tro chay bo hieu qua", "giay adidas ultraboost", 3400000.0, "adidas_shoes_1.1.jpg", 50, 400 },
                    { 10, 2, 4, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Váy thể thao nhẹ, thấm hút mồ hôi tốt", 2, 2, "Váy tennis Adidas nữ", 990000.0, "vay the thao nhe, tham hut mo hoi tot", "vay tennis adidas nu", 0.0, "adidas_dress_1.1.jpg", 45, 150 },
                    { 11, 2, 5, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Áo thun cổ điển, logo 3 sọc đặc trưng", 2, 1, "Áo thun Adidas cổ tròn", 650000.0, "ao thun co đien, logo 3 soc đac trung", "ao thun adidas co tron", 700000.0, "adidas_tshirt_1.1.jpg", 150, 150 },
                    { 12, 2, 6, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Thiết kế năng động, chống nước tốt", 2, 3, "Đồng hồ thể thao Adidas", 1750000.0, "thiet ke nang đong, chong nuoc tot", "đong ho the thao adidas", 0.0, "adidas_watch_1.1.jpg", 30, 50 },
                    { 13, 3, 1, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Áo khoác nữ dáng dài thanh lịch", 2, 2, "Áo khoác Zara Dáng Dài", 1900000.0, "ao khoac nu dang dai thanh lich", "ao khoac zara dang dai", 2000000.0, "zara_jacket_1.1.jpg", 60, 400 },
                    { 14, 3, 2, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Phong cách hiện đại, thời trang", 2, 2, "Quần Ống Rộng Zara", 980000.0, "phong cach hien đai, thoi trang", "quan ong rong zara", 0.0, "zara_pants_1.1.jpg", 40, 250 },
                    { 15, 3, 3, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Thiết kế sang trọng, phù hợp tiệc tùng", 2, 2, "Giày Cao Gót Zara", 1200000.0, "thiet ke sang trong, phu hop tiec tung", "giay cao got zara", 1300000.0, "zara_heels_1.1.jpg", 55, 300 },
                    { 16, 3, 4, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Váy dài thướt tha, thanh lịch", 2, 2, "Váy Xếp Ly Zara", 1250000.0, "vay dai thuot tha, thanh lich", "vay xep ly zara", 0.0, "zara_dress_1.1.jpg", 35, 200 },
                    { 17, 3, 5, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Áo thun đơn giản, dễ phối đồ", 2, 3, "Áo Thun Zara Basic", 450000.0, "ao thun đon gian, de phoi đo", "ao thun zara basic", 500000.0, "zara_tshirt_1.1.jpg", 90, 150 },
                    { 18, 3, 6, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Thiết kế mặt tròn, dây da tinh tế", 2, 2, "Đồng Hồ Kim Zara", 1350000.0, "thiet ke mat tron, day da tinh te", "đong ho kim zara", 0.0, "zara_watch_1.1.jpg", 25, 50 },
                    { 19, 4, 1, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Chống gió, nhẹ và gọn", 2, 1, "Áo Khoác Dù H&M", 950000.0, "chong gio, nhe va gon", "ao khoac du h&m", 1000000.0, "hm_jacket_1.1.jpg", 80, 300 },
                    { 20, 4, 2, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Phong cách công sở lịch sự", 2, 1, "Quần Tây H&M", 700000.0, "phong cach cong so lich su", "quan tay h&m", 0.0, "hm_pants_1.1.jpg", 65, 250 },
                    { 21, 4, 3, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Giày đơn giản, năng động", 2, 3, "Giày Thể Thao H&M", 820000.0, "giay đon gian, nang đong", "giay the thao h&m", 900000.0, "hm_shoes_1.1.jpg", 100, 350 },
                    { 22, 4, 4, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Váy xinh xắn cho mùa hè", 2, 2, "Váy Ngắn Hoa Nhí H&M", 600000.0, "vay xinh xan cho mua he", "vay ngan hoa nhi h&m", 0.0, "hm_dress_1.1.jpg", 45, 150 },
                    { 23, 4, 5, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Áo thun trơn giá rẻ", 2, 1, "Áo Thun Nam Basic H&M", 200000.0, "ao thun tron gia re", "ao thun nam basic h&m", 250000.0, "hm_tshirt_1.1.jpg", 150, 150 },
                    { 24, 4, 6, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Thiết kế đơn giản, hiện đại", 2, 3, "Đồng Hồ Dây Silicon H&M", 550000.0, "thiet ke đon gian, hien đai", "đong ho day silicon h&m", 0.0, "hm_watch_1.1.jpg", 40, 50 },
                    { 25, 5, 1, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Giữ ấm nhẹ nhàng, tiện lợi", 2, 1, "Áo khoác lông vũ Uniqlo", 1750000.0, "giu am nhe nhang, tien loi", "ao khoac long vu uniqlo", 1900000.0, "uniqlo_jacket_1.1.jpg", 70, 250 },
                    { 26, 5, 2, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Jean co giãn, phong cách tối giản", 2, 1, "Quần Jean Uniqlo", 950000.0, "jean co gian, phong cach toi gian", "quan jean uniqlo", 0.0, "uniqlo_jeans_1.1.jpg", 50, 300 },
                    { 27, 5, 3, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Thiết kế tối giản, thoải mái", 2, 1, "Giày Sneaker Uniqlo", 1050000.0, "thiet ke toi gian, thoai mai", "giay sneaker uniqlo", 1150000.0, "uniqlo_shoes_1.1.jpg", 60, 350 },
                    { 28, 5, 4, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Thiết kế tối giản, thoải mái cho mùa hè", 2, 2, "Váy Cotton Uniqlo", 850000.0, "thiet ke toi gian, thoai mai cho mua he", "vay cotton uniqlo", 0.0, "uniqlo_dress_1.1.jpg", 40, 150 },
                    { 29, 5, 5, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Thoáng mát, nhanh khô, thích hợp mùa hè", 2, 3, "Áo thun Airism Uniqlo", 400000.0, "thoang mat, nhanh kho, thich hop mua he", "ao thun airism uniqlo", 450000.0, "uniqlo_tshirt_1.1.jpg", 120, 150 },
                    { 30, 5, 6, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Phong cách tối giản, dây da đen", 2, 1, "Đồng hồ tối giản Uniqlo", 980000.0, "phong cach toi gian, day da đen", "đong ho toi gian uniqlo", 0.0, "uniqlo_watch_1.1.jpg", 30, 50 },
                    { 31, 6, 1, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Thiết kế sang trọng, dành cho thời trang cao cấp", 2, 2, "Áo khoác cao cấp Gucci", 25000000.0, "thiet ke sang trong, danh cho thoi trang cao cap", "ao khoac cao cap gucci", 27000000.0, "gucci_jacket_1.1.jpg", 10, 500 },
                    { 32, 6, 2, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Phong cách lịch lãm, chất liệu cao cấp", 2, 1, "Quần tây Gucci nam", 15500000.0, "phong cach lich lam, chat lieu cao cap", "quan tay gucci nam", 0.0, "gucci_pants_1.1.jpg", 15, 300 },
                    { 33, 6, 3, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Thiết kế logo đặc trưng, đẳng cấp", 2, 1, "Giày lười Gucci", 22000000.0, "thiet ke logo đac trung, đang cap", "giay luoi gucci", 24000000.0, "gucci_shoes_1.1.jpg", 12, 450 },
                    { 34, 6, 4, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Chất liệu mềm mại, sang trọng", 2, 2, "Váy lụa cao cấp Gucci", 30000000.0, "chat lieu mem mai, sang trong", "vay lua cao cap gucci", 0.0, "gucci_dress_1.1.jpg", 8, 200 },
                    { 35, 6, 5, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Phong cách trẻ trung, thời thượng", 2, 3, "Áo thun Gucci logo lớn", 9000000.0, "phong cach tre trung, thoi thuong", "ao thun gucci logo lon", 9500000.0, "gucci_tshirt_1.1.jpg", 20, 150 },
                    { 36, 6, 6, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Đồng hồ xa xỉ với thiết kế tinh xảo", 2, 2, "Đồng hồ đính đá Gucci", 45000000.0, "đong ho xa xi voi thiet ke tinh xao", "đong ho đinh đa gucci", 0.0, "gucci_watch_1.1.jpg", 5, 100 },
                    { 37, 7, 1, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Chất liệu jean bền, phong cách cổ điển", 2, 1, "Áo khoác jean Levi's", 1450000.0, "chat lieu jean ben, phong cach co đien", "ao khoac jean levi's", 1600000.0, "levis_jacket_1.1.jpg", 40, 400 },
                    { 38, 7, 2, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Form dáng chuẩn Mỹ, chất lượng cao", 2, 1, "Quần jeans Levi's 501", 1250000.0, "form dang chuan my, chat luong cao", "quan jeans levi's 501", 0.0, "levis_jeans_1.1.jpg", 60, 300 },
                    { 39, 7, 3, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Thoải mái và thời trang hàng ngày", 2, 1, "Giày thể thao Levi's", 1150000.0, "thoai mai va thoi trang hang ngay", "giay the thao levi's", 1250000.0, "levis_shoes_1.1.jpg", 35, 350 },
                    { 40, 7, 4, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Thiết kế trẻ trung, năng động", 2, 2, "Váy bò Levi's nữ", 980000.0, "thiet ke tre trung, nang đong", "vay bo levi's nu", 0.0, "levis_dress_1.1.jpg", 20, 250 },
                    { 41, 7, 5, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Logo cổ điển, phong cách Mỹ", 2, 3, "Áo thun cổ tròn Levi's", 500000.0, "logo co đien, phong cach my", "ao thun co tron levi's", 550000.0, "levis_tshirt_1.1.jpg", 100, 150 },
                    { 42, 7, 6, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Đơn giản, mạnh mẽ, cá tính", 2, 1, "Đồng hồ dây da Levi's", 850000.0, "đon gian, manh me, ca tinh", "đong ho day da levi's", 0.0, "levis_watch_1.1.jpg", 15, 50 },
                    { 43, 8, 1, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Thiết kế năng động, thời trang", 2, 3, "Áo khoác thể thao Lacoste", 2700000.0, "thiet ke nang đong, thoi trang", "ao khoac the thao lacoste", 2900000.0, "lacoste_jacket_1.1.jpg", 30, 300 },
                    { 44, 8, 2, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Phong cách thể thao, thoáng mát", 2, 1, "Quần short Lacoste", 1450000.0, "phong cach the thao, thoang mat", "quan short lacoste", 0.0, "lacoste_short_1.1.jpg", 40, 150 },
                    { 45, 8, 3, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Chất liệu cao cấp, thiết kế tinh tế", 2, 1, "Giày sneaker Lacoste", 2200000.0, "chat lieu cao cap, thiet ke tinh te", "giay sneaker lacoste", 2400000.0, "lacoste_shoes_1.1.jpg", 25, 400 },
                    { 46, 8, 4, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Lịch sự, sang trọng cho nữ", 2, 2, "Váy polo Lacoste", 1900000.0, "lich su, sang trong cho nu", "vay polo lacoste", 0.0, "lacoste_dress_1.1.jpg", 18, 150 },
                    { 47, 8, 5, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Biểu tượng nổi bật với logo cá sấu", 2, 1, "Áo thun cá sấu Lacoste", 1100000.0, "bieu tuong noi bat voi logo ca sau", "ao thun ca sau lacoste", 1200000.0, "lacoste_tshirt_1.1.jpg", 55, 150 },
                    { 48, 8, 6, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Năng động, trẻ trung, phù hợp với người chơi thể thao", 2, 3, "Đồng hồ thể thao Lacoste", 2600000.0, "nang đong, tre trung, phu hop voi nguoi choi the thao", "đong ho the thao lacoste", 0.0, "lacoste_watch_1.1.jpg", 20, 50 },
                    { 49, 9, 1, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Thiết kế hiện đại, năng động", 2, 1, "Áo khoác thể thao Puma", 1550000.0, "thiet ke hien đai, nang đong", "ao khoac the thao puma", 1700000.0, "puma_jacket_1.1.jpg", 50, 300 },
                    { 50, 9, 2, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Thoải mái, phù hợp vận động", 2, 1, "Quần jogger Puma", 1100000.0, "thoai mai, phu hop van đong", "quan jogger puma", 0.0, "puma_pants_1.1.jpg", 70, 200 },
                    { 51, 9, 3, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Thiết kế thể thao, hỗ trợ di chuyển", 2, 1, "Giày chạy bộ Puma", 1800000.0, "thiet ke the thao, ho tro di chuyen", "giay chay bo puma", 2000000.0, "puma_shoes_1.1.jpg", 40, 400 },
                    { 52, 9, 4, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Phù hợp tập luyện hoặc mặc thường ngày", 2, 2, "Váy thể thao Puma nữ", 950000.0, "phu hop tap luyen hoac mac thuong ngay", "vay the thao puma nu", 0.0, "puma_dress_1.1.jpg", 25, 150 },
                    { 53, 9, 5, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Chất vải thấm hút, thoáng mát", 2, 3, "Áo thun thể thao Puma", 600000.0, "chat vai tham hut, thoang mat", "ao thun the thao puma", 650000.0, "puma_tshirt_1.1.jpg", 90, 150 },
                    { 54, 9, 6, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Thiết kế mạnh mẽ, phong cách", 2, 1, "Đồng hồ thể thao Puma", 1200000.0, "thiet ke manh me, phong cach", "đong ho the thao puma", 0.0, "puma_watch_1.1.jpg", 18, 50 },
                    { 55, 10, 1, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Đẳng cấp, thời thượng, thiết kế quý phái", 2, 2, "Áo khoác dạ Chanel", 9500000.0, "đang cap, thoi thuong, thiet ke quy phai", "ao khoac da chanel", 10000000.0, "chanel_jacket_1.1.jpg", 15, 450 },
                    { 56, 10, 2, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Đường may tinh xảo, chất liệu mềm mịn", 2, 2, "Quần vải cao cấp Chanel", 7200000.0, "đuong may tinh xao, chat lieu mem min", "quan vai cao cap chanel", 0.0, "chanel_pants_1.1.jpg", 12, 250 },
                    { 57, 10, 3, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Thanh lịch, sang trọng", 2, 2, "Giày cao gót Chanel", 8300000.0, "thanh lich, sang trong", "giay cao got chanel", 9000000.0, "chanel_shoes_1.1.jpg", 10, 300 },
                    { 58, 10, 4, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Lộng lẫy, thiết kế quyến rũ", 2, 2, "Váy dạ hội Chanel", 13000000.0, "long lay, thiet ke quyen ru", "vay da hoi chanel", 0.0, "chanel_dress_1.1.jpg", 8, 200 },
                    { 59, 10, 5, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Đơn giản nhưng đầy tinh tế", 2, 2, "Áo thun cao cấp Chanel", 3200000.0, "đon gian nhung đay tinh te", "ao thun cao cap chanel", 3500000.0, "chanel_tshirt_1.1.jpg", 20, 150 },
                    { 60, 10, 6, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Tinh xảo, sang trọng và nữ tính", 2, 2, "Đồng hồ Chanel sang trọng", 18000000.0, "tinh xao, sang trong va nu tinh", "đong ho chanel sang trong", 0.0, "chanel_watch_1.1.jpg", 6, 100 }
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
                table: "ProductImages",
                columns: new[] { "ID", "PathNameImage", "ProductID" },
                values: new object[,]
                {
                    { 1, "nike_jacket_1.1.jpg", 1 },
                    { 2, "nike_jacket_1.2.jpg", 1 },
                    { 3, "nike_jacket_1.3.jpg", 1 },
                    { 4, "nike_jacket_1.4.jpg", 1 },
                    { 5, "nike_pants_1.1.jpg", 2 },
                    { 6, "nike_pants_1.2.jpg", 2 },
                    { 7, "nike_pants_1.3.jpg", 2 },
                    { 8, "nike_pants_1.4.jpg", 2 },
                    { 9, "nike_shoes_1.1.jpg", 3 },
                    { 10, "nike_shoes_1.2.jpg", 3 },
                    { 11, "nike_shoes_1.3.jpg", 3 },
                    { 12, "nike_shoes_1.4.jpg", 3 },
                    { 13, "nike_dress_1.1.jpg", 4 },
                    { 14, "nike_dress_1.2.jpg", 4 },
                    { 15, "nike_dress_1.3.jpg", 4 },
                    { 16, "nike_dress_1.4.jpg", 4 },
                    { 17, "nike_tshirt_1.1.jpg", 5 },
                    { 18, "nike_tshirt_1.2.jpg", 5 },
                    { 19, "nike_tshirt_1.3.jpg", 5 },
                    { 20, "nike_tshirt_1.4.jpg", 5 },
                    { 21, "nike_watch_1.1.jpg", 6 },
                    { 22, "nike_watch_1.2.jpg", 6 },
                    { 25, "adidas_jacket_1.1.jpg", 7 },
                    { 26, "adidas_jacket_1.2.jpg", 7 },
                    { 27, "adidas_jacket_1.3.jpg", 7 },
                    { 28, "adidas_jacket_1.4.jpg", 7 },
                    { 29, "adidas_pants_1.1.jpg", 8 },
                    { 30, "adidas_pants_1.2.jpg", 8 },
                    { 31, "adidas_pants_1.3.jpg", 8 },
                    { 32, "adidas_pants_1.4.jpg", 8 },
                    { 33, "adidas_shoes_1.1.jpg", 9 },
                    { 34, "adidas_shoes_1.2.jpg", 9 },
                    { 35, "adidas_shoes_1.3.jpg", 9 },
                    { 36, "adidas_shoes_1.4.jpg", 9 },
                    { 37, "adidas_dress_1.1.jpg", 10 },
                    { 38, "adidas_dress_1.2.jpg", 10 },
                    { 39, "adidas_dress_1.3.jpg", 10 },
                    { 40, "adidas_dress_1.4.jpg", 10 },
                    { 41, "adidas_tshirt_1.1.jpg", 11 },
                    { 42, "adidas_tshirt_1.2.jpg", 11 },
                    { 43, "adidas_tshirt_1.3.jpg", 11 },
                    { 44, "adidas_tshirt_1.4.jpg", 11 },
                    { 45, "adidas_watch_1.1.jpg", 12 },
                    { 46, "adidas_watch_1.2.jpg", 12 },
                    { 47, "adidas_watch_1.3.jpg", 12 },
                    { 48, "adidas_watch_1.4.jpg", 12 },
                    { 49, "zara_jacket_1.1.jpg", 13 },
                    { 50, "zara_jacket_1.2.jpg", 13 },
                    { 51, "zara_jacket_1.3.jpg", 13 },
                    { 52, "zara_jacket_1.4.jpg", 13 },
                    { 53, "zara_pants_1.1.jpg", 14 },
                    { 54, "zara_pants_1.2.jpg", 14 },
                    { 55, "zara_pants_1.3.jpg", 14 },
                    { 56, "zara_pants_1.4.jpg", 14 },
                    { 57, "zara_shoes_1.1.jpg", 15 },
                    { 58, "zara_shoes_1.2.jpg", 15 },
                    { 59, "zara_shoes_1.3.jpg", 15 },
                    { 60, "zara_shoes_1.4.jpg", 15 },
                    { 61, "zara_dress_1.1.jpg", 16 },
                    { 62, "zara_dress_1.2.jpg", 16 },
                    { 63, "zara_dress_1.3.jpg", 16 },
                    { 64, "zara_dress_1.4.jpg", 16 },
                    { 65, "zara_tshirt_1.1.jpg", 17 },
                    { 66, "zara_tshirt_1.2.jpg", 17 },
                    { 67, "zara_tshirt_1.3.jpg", 17 },
                    { 68, "zara_tshirt_1.4.jpg", 17 },
                    { 69, "zara_watch_1.1.jpg", 18 },
                    { 70, "zara_watch_1.2.jpg", 18 },
                    { 73, "h&m_jacket_1.1.jpg", 19 },
                    { 74, "h&m_jacket_1.2.jpg", 19 },
                    { 75, "h&m_jacket_1.3.jpg", 19 },
                    { 76, "h&m_jacket_1.4.jpg", 19 },
                    { 77, "h&m_pants_1.1.jpg", 20 },
                    { 78, "h&m_pants_1.2.jpg", 20 },
                    { 79, "h&m_pants_1.3.jpg", 20 },
                    { 80, "h&m_pants_1.4.jpg", 20 },
                    { 81, "h&m_shoes_1.1.jpg", 21 },
                    { 82, "h&m_shoes_1.2.jpg", 21 },
                    { 83, "h&m_shoes_1.3.jpg", 21 },
                    { 84, "h&m_shoes_1.4.jpg", 21 },
                    { 85, "h&m_dress_1.1.jpg", 22 },
                    { 86, "h&m_dress_1.2.jpg", 22 },
                    { 87, "h&m_dress_1.3.jpg", 22 },
                    { 88, "h&m_dress_1.4.jpg", 22 },
                    { 89, "h&m_tshirt_1.1.jpg", 23 },
                    { 90, "h&m_tshirt_1.2.jpg", 23 },
                    { 91, "h&m_tshirt_1.3.jpg", 23 },
                    { 92, "h&m_tshirt_1.4.jpg", 23 },
                    { 93, "h&m_watch_1.1.jpg", 24 },
                    { 94, "h&m_watch_1.2.jpg", 24 },
                    { 95, "h&m_watch_1.3.jpg", 24 },
                    { 97, "uniqlo_jacket_1.1.jpg", 25 },
                    { 98, "uniqlo_jacket_1.2.jpg", 25 },
                    { 99, "uniqlo_jacket_1.3.jpg", 25 },
                    { 100, "uniqlo_jacket_1.4.jpg", 25 },
                    { 101, "uniqlo_pants_1.1.jpg", 26 },
                    { 102, "uniqlo_pants_1.2.jpg", 26 },
                    { 103, "uniqlo_pants_1.3.jpg", 26 },
                    { 104, "uniqlo_pants_1.4.jpg", 26 },
                    { 105, "uniqlo_shoes_1.1.jpg", 27 },
                    { 106, "uniqlo_shoes_1.2.jpg", 27 },
                    { 107, "uniqlo_shoes_1.3.jpg", 27 },
                    { 108, "uniqlo_shoes_1.4.jpg", 27 },
                    { 109, "uniqlo_dress_1.1.jpg", 28 },
                    { 110, "uniqlo_dress_1.2.jpg", 28 },
                    { 111, "uniqlo_dress_1.3.jpg", 28 },
                    { 112, "uniqlo_dress_1.4.jpg", 28 },
                    { 113, "uniqlo_tshirt_1.1.jpg", 29 },
                    { 114, "uniqlo_tshirt_1.2.jpg", 29 },
                    { 115, "uniqlo_tshirt_1.3.jpg", 29 },
                    { 116, "uniqlo_tshirt_1.4.jpg", 29 },
                    { 117, "uniqlo_watch_1.1.jpg", 30 },
                    { 118, "uniqlo_watch_1.2.jpg", 30 },
                    { 119, "uniqlo_watch_1.3.jpg", 30 },
                    { 121, "gucci_jacket_1.1.jpg", 31 },
                    { 122, "gucci_jacket_1.2.jpg", 31 },
                    { 123, "gucci_jacket_1.3.jpg", 31 },
                    { 124, "gucci_jacket_1.4.jpg", 31 },
                    { 125, "gucci_pants_1.1.jpg", 32 },
                    { 126, "gucci_pants_1.2.jpg", 32 },
                    { 127, "gucci_pants_1.3.jpg", 32 },
                    { 128, "gucci_pants_1.4.jpg", 32 },
                    { 129, "gucci_shoes_1.1.jpg", 33 },
                    { 130, "gucci_shoes_1.2.jpg", 33 },
                    { 131, "gucci_shoes_1.3.jpg", 33 },
                    { 132, "gucci_shoes_1.4.jpg", 33 },
                    { 133, "gucci_dress_1.1.jpg", 34 },
                    { 134, "gucci_dress_1.2.jpg", 34 },
                    { 135, "gucci_dress_1.3.jpg", 34 },
                    { 136, "gucci_dress_1.4.jpg", 34 },
                    { 137, "gucci_tshirt_1.1.jpg", 35 },
                    { 138, "gucci_tshirt_1.2.jpg", 35 },
                    { 139, "gucci_tshirt_1.3.jpg", 35 },
                    { 140, "gucci_tshirt_1.4.jpg", 35 },
                    { 141, "gucci_watch_1.1.jpg", 36 },
                    { 142, "gucci_watch_1.2.jpg", 36 },
                    { 143, "levis_jacket_1.1.jpg", 37 },
                    { 144, "levis_jacket_1.2.jpg", 37 },
                    { 145, "levis_jacket_1.3.jpg", 37 },
                    { 146, "levis_jacket_1.4.jpg", 37 },
                    { 147, "levis_pants_1.1.jpg", 38 },
                    { 148, "levis_pants_1.2.jpg", 38 },
                    { 149, "levis_pants_1.3.jpg", 38 },
                    { 150, "levis_pants_1.4.jpg", 38 },
                    { 151, "levis_shoes_1.1.jpg", 39 },
                    { 152, "levis_shoes_1.2.jpg", 39 },
                    { 153, "levis_shoes_1.3.jpg", 39 },
                    { 154, "levis_shoes_1.4.jpg", 39 },
                    { 155, "levis_dress_1.1.jpg", 40 },
                    { 156, "levis_dress_1.2.jpg", 40 },
                    { 157, "levis_dress_1.3.jpg", 40 },
                    { 158, "levis_dress_1.4.jpg", 40 },
                    { 159, "levis_tshirt_1.1.jpg", 41 },
                    { 160, "levis_tshirt_1.2.jpg", 41 },
                    { 161, "levis_tshirt_1.3.jpg", 41 },
                    { 162, "levis_tshirt_1.4.jpg", 41 },
                    { 163, "levis_watch_1.1.jpg", 42 },
                    { 164, "levis_watch_1.2.jpg", 42 },
                    { 165, "levis_watch_1.3.jpg", 42 },
                    { 166, "lacoste_jacket_1.1.jpg", 43 },
                    { 167, "lacoste_jacket_1.2.jpg", 43 },
                    { 168, "lacoste_jacket_1.3.jpg", 43 },
                    { 169, "lacoste_jacket_1.4.jpg", 43 },
                    { 170, "lacoste_pants_1.1.jpg", 44 },
                    { 171, "lacoste_pants_1.2.jpg", 44 },
                    { 172, "lacoste_pants_1.3.jpg", 44 },
                    { 173, "lacoste_pants_1.4.jpg", 44 },
                    { 174, "lacoste_shoes_1.1.jpg", 45 },
                    { 175, "lacoste_shoes_1.2.jpg", 45 },
                    { 176, "lacoste_shoes_1.3.jpg", 45 },
                    { 177, "lacoste_shoes_1.4.jpg", 45 },
                    { 178, "lacoste_dress_1.1.jpg", 46 },
                    { 179, "lacoste_dress_1.2.jpg", 46 },
                    { 180, "lacoste_dress_1.3.jpg", 46 },
                    { 181, "lacoste_dress_1.4.jpg", 46 },
                    { 182, "lacoste_tshirt_1.1.jpg", 47 },
                    { 183, "lacoste_tshirt_1.2.jpg", 47 },
                    { 184, "lacoste_tshirt_1.3.jpg", 47 },
                    { 185, "lacoste_tshirt_1.4.jpg", 47 },
                    { 186, "lacoste_watch_1.1.jpg", 48 },
                    { 187, "lacoste_watch_1.2.jpg", 48 },
                    { 188, "lacoste_watch_1.3.jpg", 48 },
                    { 189, "puma_jacket_1.1.jpg", 49 },
                    { 190, "puma_jacket_1.2.jpg", 49 },
                    { 191, "puma_jacket_1.3.jpg", 49 },
                    { 192, "puma_jacket_1.4.jpg", 49 },
                    { 193, "puma_pants_1.1.jpg", 50 },
                    { 194, "puma_pants_1.2.jpg", 50 },
                    { 195, "puma_pants_1.3.jpg", 50 },
                    { 196, "puma_pants_1.4.jpg", 50 },
                    { 197, "puma_shoes_1.1.jpg", 51 },
                    { 198, "puma_shoes_1.2.jpg", 51 },
                    { 199, "puma_shoes_1.3.jpg", 51 },
                    { 200, "puma_shoes_1.4.jpg", 51 },
                    { 201, "puma_dress_1.1.jpg", 52 },
                    { 202, "puma_dress_1.2.jpg", 52 },
                    { 203, "puma_dress_1.3.jpg", 52 },
                    { 204, "puma_dress_1.4.jpg", 52 },
                    { 205, "puma_tshirt_1.1.jpg", 53 },
                    { 206, "puma_tshirt_1.2.jpg", 53 },
                    { 207, "puma_tshirt_1.3.jpg", 53 },
                    { 208, "puma_tshirt_1.4.jpg", 53 },
                    { 209, "puma_watch_1.1.jpg", 54 },
                    { 210, "puma_watch_1.2.jpg", 54 },
                    { 211, "puma_watch_1.3.jpg", 54 },
                    { 212, "chanel_jacket_1.1.jpg", 55 },
                    { 213, "chanel_jacket_1.2.jpg", 55 },
                    { 214, "chanel_jacket_1.3.jpg", 55 },
                    { 215, "chanel_jacket_1.4.jpg", 55 },
                    { 216, "chanel_pants_1.1.jpg", 56 },
                    { 217, "chanel_pants_1.2.jpg", 56 },
                    { 218, "chanel_pants_1.3.jpg", 56 },
                    { 219, "chanel_pants_1.4.jpg", 56 },
                    { 220, "chanel_shoes_1.1.jpg", 57 },
                    { 221, "chanel_shoes_1.2.jpg", 57 },
                    { 222, "chanel_shoes_1.3.jpg", 57 },
                    { 223, "chanel_shoes_1.4.jpg", 57 },
                    { 224, "chanel_dress_1.1.jpg", 58 },
                    { 225, "chanel_dress_1.2.jpg", 58 },
                    { 226, "chanel_dress_1.3.jpg", 58 },
                    { 227, "chanel_dress_1.4.jpg", 58 },
                    { 228, "chanel_tshirt_1.1.jpg", 59 },
                    { 229, "chanel_tshirt_1.2.jpg", 59 },
                    { 230, "chanel_tshirt_1.3.jpg", 59 },
                    { 231, "chanel_tshirt_1.4.jpg", 59 },
                    { 232, "chanel_watch_1.1.jpg", 60 },
                    { 233, "chanel_watch_1.2.jpg", 60 },
                    { 234, "chanel_watch_1.3.jpg", 60 }
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
                table: "ProductQty",
                columns: new[] { "ColorID", "ProductID", "SizeID", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 4, 31 },
                    { 6, 1, 4, 22 },
                    { 8, 1, 4, 28 },
                    { 1, 1, 5, 37 },
                    { 6, 1, 5, 35 },
                    { 8, 1, 5, 16 },
                    { 1, 1, 7, 38 },
                    { 6, 1, 7, 14 },
                    { 8, 1, 7, 39 },
                    { 1, 1, 11, 32 },
                    { 6, 1, 11, 16 },
                    { 8, 1, 11, 26 },
                    { 2, 2, 3, 39 },
                    { 6, 2, 3, 20 },
                    { 7, 2, 3, 5 },
                    { 2, 2, 9, 44 },
                    { 6, 2, 9, 9 },
                    { 7, 2, 9, 5 },
                    { 2, 2, 12, 37 },
                    { 6, 2, 12, 23 },
                    { 7, 2, 12, 41 },
                    { 3, 3, 9, 32 },
                    { 4, 3, 9, 16 },
                    { 7, 3, 9, 18 },
                    { 8, 3, 9, 17 },
                    { 3, 3, 12, 24 },
                    { 4, 3, 12, 25 },
                    { 7, 3, 12, 10 },
                    { 8, 3, 12, 22 },
                    { 1, 4, 5, 20 },
                    { 3, 4, 5, 32 },
                    { 5, 4, 5, 32 },
                    { 6, 4, 5, 12 },
                    { 8, 4, 5, 24 },
                    { 1, 4, 10, 29 },
                    { 3, 4, 10, 25 },
                    { 5, 4, 10, 40 },
                    { 6, 4, 10, 12 },
                    { 8, 4, 10, 35 },
                    { 1, 4, 13, 49 },
                    { 3, 4, 13, 5 },
                    { 5, 4, 13, 43 },
                    { 6, 4, 13, 41 },
                    { 8, 4, 13, 39 },
                    { 1, 5, 8, 29 },
                    { 2, 5, 8, 29 },
                    { 3, 5, 8, 17 },
                    { 4, 5, 8, 38 },
                    { 5, 5, 8, 8 },
                    { 8, 5, 8, 19 },
                    { 1, 5, 12, 43 },
                    { 2, 5, 12, 6 },
                    { 3, 5, 12, 42 },
                    { 4, 5, 12, 21 },
                    { 5, 5, 12, 23 },
                    { 8, 5, 12, 40 },
                    { 1, 6, 3, 23 },
                    { 2, 6, 3, 39 },
                    { 5, 6, 3, 10 },
                    { 6, 6, 3, 43 },
                    { 7, 6, 3, 47 },
                    { 1, 6, 8, 23 },
                    { 2, 6, 8, 21 },
                    { 5, 6, 8, 6 },
                    { 6, 6, 8, 41 },
                    { 7, 6, 8, 9 },
                    { 1, 7, 1, 10 },
                    { 2, 7, 1, 35 },
                    { 4, 7, 1, 8 },
                    { 5, 7, 1, 48 },
                    { 6, 7, 1, 17 },
                    { 7, 7, 1, 22 },
                    { 1, 7, 2, 40 },
                    { 2, 7, 2, 21 },
                    { 4, 7, 2, 33 },
                    { 5, 7, 2, 33 },
                    { 6, 7, 2, 8 },
                    { 7, 7, 2, 8 },
                    { 1, 7, 5, 25 },
                    { 2, 7, 5, 33 },
                    { 4, 7, 5, 20 },
                    { 5, 7, 5, 17 },
                    { 6, 7, 5, 29 },
                    { 7, 7, 5, 17 },
                    { 1, 7, 12, 26 },
                    { 2, 7, 12, 20 },
                    { 4, 7, 12, 36 },
                    { 5, 7, 12, 7 },
                    { 6, 7, 12, 7 },
                    { 7, 7, 12, 6 },
                    { 1, 8, 1, 46 },
                    { 2, 8, 1, 21 },
                    { 3, 8, 1, 10 },
                    { 4, 8, 1, 42 },
                    { 6, 8, 1, 40 },
                    { 8, 8, 1, 27 },
                    { 1, 8, 6, 26 },
                    { 2, 8, 6, 22 },
                    { 3, 8, 6, 35 },
                    { 4, 8, 6, 47 },
                    { 6, 8, 6, 18 },
                    { 8, 8, 6, 22 },
                    { 1, 8, 8, 30 },
                    { 2, 8, 8, 11 },
                    { 3, 8, 8, 26 },
                    { 4, 8, 8, 42 },
                    { 6, 8, 8, 6 },
                    { 8, 8, 8, 19 },
                    { 1, 8, 9, 17 },
                    { 2, 8, 9, 26 },
                    { 3, 8, 9, 48 },
                    { 4, 8, 9, 5 },
                    { 6, 8, 9, 5 },
                    { 8, 8, 9, 14 },
                    { 1, 8, 11, 31 },
                    { 2, 8, 11, 44 },
                    { 3, 8, 11, 18 },
                    { 4, 8, 11, 30 },
                    { 6, 8, 11, 13 },
                    { 8, 8, 11, 27 },
                    { 3, 9, 3, 25 },
                    { 4, 9, 3, 26 },
                    { 5, 9, 3, 38 },
                    { 6, 9, 3, 49 },
                    { 7, 9, 3, 37 },
                    { 3, 9, 6, 35 },
                    { 4, 9, 6, 48 },
                    { 5, 9, 6, 8 },
                    { 6, 9, 6, 27 },
                    { 7, 9, 6, 5 },
                    { 3, 9, 7, 49 },
                    { 4, 9, 7, 37 },
                    { 5, 9, 7, 40 },
                    { 6, 9, 7, 8 },
                    { 7, 9, 7, 44 },
                    { 3, 9, 9, 12 },
                    { 4, 9, 9, 28 },
                    { 5, 9, 9, 13 },
                    { 6, 9, 9, 9 },
                    { 7, 9, 9, 21 },
                    { 3, 9, 13, 42 },
                    { 4, 9, 13, 45 },
                    { 5, 9, 13, 29 },
                    { 6, 9, 13, 28 },
                    { 7, 9, 13, 6 },
                    { 1, 10, 2, 36 },
                    { 2, 10, 2, 37 },
                    { 3, 10, 2, 8 },
                    { 4, 10, 2, 44 },
                    { 5, 10, 2, 10 },
                    { 8, 10, 2, 17 },
                    { 1, 10, 5, 10 },
                    { 2, 10, 5, 27 },
                    { 3, 10, 5, 10 },
                    { 4, 10, 5, 25 },
                    { 5, 10, 5, 11 },
                    { 8, 10, 5, 14 },
                    { 1, 10, 7, 13 },
                    { 2, 10, 7, 39 },
                    { 3, 10, 7, 19 },
                    { 4, 10, 7, 33 },
                    { 5, 10, 7, 35 },
                    { 8, 10, 7, 44 },
                    { 1, 10, 12, 11 },
                    { 2, 10, 12, 30 },
                    { 3, 10, 12, 48 },
                    { 4, 10, 12, 15 },
                    { 5, 10, 12, 23 },
                    { 8, 10, 12, 42 },
                    { 1, 11, 11, 40 },
                    { 4, 11, 11, 9 },
                    { 5, 11, 11, 41 },
                    { 6, 11, 11, 12 },
                    { 1, 11, 12, 38 },
                    { 4, 11, 12, 18 },
                    { 5, 11, 12, 16 },
                    { 6, 11, 12, 7 },
                    { 1, 12, 1, 14 },
                    { 4, 12, 1, 16 },
                    { 6, 12, 1, 9 },
                    { 7, 12, 1, 39 },
                    { 8, 12, 1, 22 },
                    { 1, 12, 7, 16 },
                    { 4, 12, 7, 34 },
                    { 6, 12, 7, 21 },
                    { 7, 12, 7, 13 },
                    { 8, 12, 7, 21 },
                    { 1, 12, 11, 44 },
                    { 4, 12, 11, 6 },
                    { 6, 12, 11, 16 },
                    { 7, 12, 11, 19 },
                    { 8, 12, 11, 40 },
                    { 1, 12, 13, 17 },
                    { 4, 12, 13, 7 },
                    { 6, 12, 13, 46 },
                    { 7, 12, 13, 45 },
                    { 8, 12, 13, 48 },
                    { 1, 12, 14, 27 },
                    { 4, 12, 14, 37 },
                    { 6, 12, 14, 22 },
                    { 7, 12, 14, 6 },
                    { 8, 12, 14, 25 },
                    { 2, 13, 2, 16 },
                    { 5, 13, 2, 10 },
                    { 6, 13, 2, 22 },
                    { 7, 13, 2, 16 },
                    { 2, 13, 6, 30 },
                    { 5, 13, 6, 42 },
                    { 6, 13, 6, 40 },
                    { 7, 13, 6, 37 },
                    { 6, 14, 9, 10 },
                    { 7, 14, 9, 7 },
                    { 8, 14, 9, 25 },
                    { 6, 14, 11, 45 },
                    { 7, 14, 11, 42 },
                    { 8, 14, 11, 26 },
                    { 6, 14, 13, 18 },
                    { 7, 14, 13, 40 },
                    { 8, 14, 13, 41 },
                    { 6, 14, 14, 26 },
                    { 7, 14, 14, 9 },
                    { 8, 14, 14, 35 },
                    { 1, 15, 2, 5 },
                    { 2, 15, 2, 33 },
                    { 3, 15, 2, 47 },
                    { 4, 15, 2, 44 },
                    { 5, 15, 2, 49 },
                    { 6, 15, 2, 40 },
                    { 1, 15, 6, 17 },
                    { 2, 15, 6, 22 },
                    { 3, 15, 6, 10 },
                    { 4, 15, 6, 20 },
                    { 5, 15, 6, 41 },
                    { 6, 15, 6, 15 },
                    { 1, 15, 9, 9 },
                    { 2, 15, 9, 5 },
                    { 3, 15, 9, 33 },
                    { 4, 15, 9, 29 },
                    { 5, 15, 9, 41 },
                    { 6, 15, 9, 20 },
                    { 1, 15, 10, 19 },
                    { 2, 15, 10, 47 },
                    { 3, 15, 10, 25 },
                    { 4, 15, 10, 33 },
                    { 5, 15, 10, 8 },
                    { 6, 15, 10, 16 },
                    { 1, 15, 11, 44 },
                    { 2, 15, 11, 13 },
                    { 3, 15, 11, 25 },
                    { 4, 15, 11, 15 },
                    { 5, 15, 11, 37 },
                    { 6, 15, 11, 16 },
                    { 1, 16, 3, 15 },
                    { 2, 16, 3, 27 },
                    { 3, 16, 3, 38 },
                    { 4, 16, 3, 46 },
                    { 8, 16, 3, 26 },
                    { 1, 16, 5, 8 },
                    { 2, 16, 5, 25 },
                    { 3, 16, 5, 29 },
                    { 4, 16, 5, 37 },
                    { 8, 16, 5, 22 },
                    { 1, 16, 10, 26 },
                    { 2, 16, 10, 27 },
                    { 3, 16, 10, 23 },
                    { 4, 16, 10, 29 },
                    { 8, 16, 10, 35 },
                    { 1, 16, 11, 25 },
                    { 2, 16, 11, 41 },
                    { 3, 16, 11, 27 },
                    { 4, 16, 11, 11 },
                    { 8, 16, 11, 27 },
                    { 1, 16, 12, 28 },
                    { 2, 16, 12, 23 },
                    { 3, 16, 12, 10 },
                    { 4, 16, 12, 33 },
                    { 8, 16, 12, 22 },
                    { 1, 17, 3, 6 },
                    { 3, 17, 3, 46 },
                    { 5, 17, 3, 36 },
                    { 6, 17, 3, 12 },
                    { 7, 17, 3, 48 },
                    { 8, 17, 3, 11 },
                    { 1, 17, 4, 41 },
                    { 3, 17, 4, 13 },
                    { 5, 17, 4, 26 },
                    { 6, 17, 4, 26 },
                    { 7, 17, 4, 28 },
                    { 8, 17, 4, 17 },
                    { 1, 17, 13, 20 },
                    { 3, 17, 13, 45 },
                    { 5, 17, 13, 47 },
                    { 6, 17, 13, 24 },
                    { 7, 17, 13, 21 },
                    { 8, 17, 13, 32 },
                    { 1, 18, 1, 28 },
                    { 3, 18, 1, 5 },
                    { 7, 18, 1, 45 },
                    { 1, 18, 13, 23 },
                    { 3, 18, 13, 39 },
                    { 7, 18, 13, 12 },
                    { 1, 19, 2, 5 },
                    { 4, 19, 2, 22 },
                    { 5, 19, 2, 45 },
                    { 6, 19, 2, 41 },
                    { 1, 19, 11, 37 },
                    { 4, 19, 11, 43 },
                    { 5, 19, 11, 32 },
                    { 6, 19, 11, 12 },
                    { 1, 19, 14, 36 },
                    { 4, 19, 14, 16 },
                    { 5, 19, 14, 7 },
                    { 6, 19, 14, 42 },
                    { 3, 20, 3, 13 },
                    { 7, 20, 3, 35 },
                    { 8, 20, 3, 32 },
                    { 3, 20, 9, 38 },
                    { 7, 20, 9, 48 },
                    { 8, 20, 9, 30 },
                    { 1, 21, 1, 12 },
                    { 2, 21, 1, 26 },
                    { 3, 21, 1, 11 },
                    { 5, 21, 1, 26 },
                    { 6, 21, 1, 28 },
                    { 8, 21, 1, 9 },
                    { 1, 21, 5, 5 },
                    { 2, 21, 5, 48 },
                    { 3, 21, 5, 27 },
                    { 5, 21, 5, 21 },
                    { 6, 21, 5, 43 },
                    { 8, 21, 5, 23 },
                    { 1, 21, 13, 26 },
                    { 2, 21, 13, 36 },
                    { 3, 21, 13, 37 },
                    { 5, 21, 13, 41 },
                    { 6, 21, 13, 27 },
                    { 8, 21, 13, 12 },
                    { 1, 21, 14, 37 },
                    { 2, 21, 14, 17 },
                    { 3, 21, 14, 46 },
                    { 5, 21, 14, 23 },
                    { 6, 21, 14, 19 },
                    { 8, 21, 14, 39 },
                    { 2, 22, 1, 43 },
                    { 3, 22, 1, 35 },
                    { 7, 22, 1, 8 },
                    { 2, 22, 11, 31 },
                    { 3, 22, 11, 48 },
                    { 7, 22, 11, 46 },
                    { 2, 22, 14, 42 },
                    { 3, 22, 14, 26 },
                    { 7, 22, 14, 34 },
                    { 1, 23, 2, 10 },
                    { 2, 23, 2, 43 },
                    { 3, 23, 2, 38 },
                    { 4, 23, 2, 44 },
                    { 8, 23, 2, 34 },
                    { 1, 23, 4, 10 },
                    { 2, 23, 4, 12 },
                    { 3, 23, 4, 49 },
                    { 4, 23, 4, 47 },
                    { 8, 23, 4, 38 },
                    { 1, 23, 10, 11 },
                    { 2, 23, 10, 16 },
                    { 3, 23, 10, 43 },
                    { 4, 23, 10, 26 },
                    { 8, 23, 10, 38 },
                    { 1, 23, 11, 24 },
                    { 2, 23, 11, 13 },
                    { 3, 23, 11, 18 },
                    { 4, 23, 11, 17 },
                    { 8, 23, 11, 12 },
                    { 2, 24, 4, 34 },
                    { 3, 24, 4, 42 },
                    { 4, 24, 4, 35 },
                    { 5, 24, 4, 7 },
                    { 2, 24, 8, 43 },
                    { 3, 24, 8, 42 },
                    { 4, 24, 8, 36 },
                    { 5, 24, 8, 15 },
                    { 2, 24, 9, 39 },
                    { 3, 24, 9, 43 },
                    { 4, 24, 9, 23 },
                    { 5, 24, 9, 37 },
                    { 2, 24, 10, 49 },
                    { 3, 24, 10, 32 },
                    { 4, 24, 10, 14 },
                    { 5, 24, 10, 6 },
                    { 2, 24, 12, 43 },
                    { 3, 24, 12, 18 },
                    { 4, 24, 12, 30 },
                    { 5, 24, 12, 36 },
                    { 3, 25, 1, 47 },
                    { 6, 25, 1, 14 },
                    { 8, 25, 1, 13 },
                    { 3, 25, 2, 28 },
                    { 6, 25, 2, 25 },
                    { 8, 25, 2, 46 },
                    { 3, 25, 5, 15 },
                    { 6, 25, 5, 7 },
                    { 8, 25, 5, 29 },
                    { 3, 25, 11, 29 },
                    { 6, 25, 11, 9 },
                    { 8, 25, 11, 20 },
                    { 3, 25, 13, 7 },
                    { 6, 25, 13, 8 },
                    { 8, 25, 13, 47 },
                    { 2, 26, 5, 30 },
                    { 3, 26, 5, 42 },
                    { 5, 26, 5, 14 },
                    { 7, 26, 5, 33 },
                    { 8, 26, 5, 44 },
                    { 2, 26, 6, 10 },
                    { 3, 26, 6, 16 },
                    { 5, 26, 6, 30 },
                    { 7, 26, 6, 10 },
                    { 8, 26, 6, 33 },
                    { 2, 26, 8, 43 },
                    { 3, 26, 8, 44 },
                    { 5, 26, 8, 26 },
                    { 7, 26, 8, 19 },
                    { 8, 26, 8, 43 },
                    { 2, 26, 10, 36 },
                    { 3, 26, 10, 37 },
                    { 5, 26, 10, 37 },
                    { 7, 26, 10, 26 },
                    { 8, 26, 10, 22 },
                    { 2, 27, 3, 45 },
                    { 4, 27, 3, 48 },
                    { 8, 27, 3, 12 },
                    { 2, 27, 8, 46 },
                    { 4, 27, 8, 44 },
                    { 8, 27, 8, 42 },
                    { 2, 27, 10, 9 },
                    { 4, 27, 10, 15 },
                    { 8, 27, 10, 18 },
                    { 2, 28, 7, 33 },
                    { 3, 28, 7, 46 },
                    { 5, 28, 7, 31 },
                    { 8, 28, 7, 27 },
                    { 2, 28, 14, 37 },
                    { 3, 28, 14, 35 },
                    { 5, 28, 14, 49 },
                    { 8, 28, 14, 11 },
                    { 1, 29, 5, 13 },
                    { 2, 29, 5, 22 },
                    { 3, 29, 5, 37 },
                    { 5, 29, 5, 17 },
                    { 7, 29, 5, 37 },
                    { 8, 29, 5, 29 },
                    { 1, 29, 12, 22 },
                    { 2, 29, 12, 20 },
                    { 3, 29, 12, 7 },
                    { 5, 29, 12, 42 },
                    { 7, 29, 12, 5 },
                    { 8, 29, 12, 10 },
                    { 2, 30, 7, 19 },
                    { 3, 30, 7, 16 },
                    { 4, 30, 7, 7 },
                    { 7, 30, 7, 41 },
                    { 2, 30, 8, 6 },
                    { 3, 30, 8, 7 },
                    { 4, 30, 8, 49 },
                    { 7, 30, 8, 48 },
                    { 2, 30, 12, 7 },
                    { 3, 30, 12, 25 },
                    { 4, 30, 12, 48 },
                    { 7, 30, 12, 34 },
                    { 1, 31, 5, 44 },
                    { 2, 31, 5, 8 },
                    { 4, 31, 5, 40 },
                    { 5, 31, 5, 5 },
                    { 6, 31, 5, 49 },
                    { 7, 31, 5, 46 },
                    { 1, 31, 6, 17 },
                    { 2, 31, 6, 27 },
                    { 4, 31, 6, 26 },
                    { 5, 31, 6, 40 },
                    { 6, 31, 6, 12 },
                    { 7, 31, 6, 13 },
                    { 1, 31, 9, 33 },
                    { 2, 31, 9, 27 },
                    { 4, 31, 9, 10 },
                    { 5, 31, 9, 29 },
                    { 6, 31, 9, 19 },
                    { 7, 31, 9, 36 },
                    { 1, 31, 10, 8 },
                    { 2, 31, 10, 44 },
                    { 4, 31, 10, 26 },
                    { 5, 31, 10, 27 },
                    { 6, 31, 10, 17 },
                    { 7, 31, 10, 23 },
                    { 1, 31, 14, 48 },
                    { 2, 31, 14, 30 },
                    { 4, 31, 14, 15 },
                    { 5, 31, 14, 25 },
                    { 6, 31, 14, 40 },
                    { 7, 31, 14, 48 },
                    { 1, 32, 2, 39 },
                    { 3, 32, 2, 25 },
                    { 4, 32, 2, 20 },
                    { 5, 32, 2, 34 },
                    { 7, 32, 2, 9 },
                    { 8, 32, 2, 20 },
                    { 1, 32, 5, 13 },
                    { 3, 32, 5, 27 },
                    { 4, 32, 5, 36 },
                    { 5, 32, 5, 48 },
                    { 7, 32, 5, 34 },
                    { 8, 32, 5, 29 },
                    { 1, 32, 6, 21 },
                    { 3, 32, 6, 39 },
                    { 4, 32, 6, 8 },
                    { 5, 32, 6, 49 },
                    { 7, 32, 6, 29 },
                    { 8, 32, 6, 41 },
                    { 1, 32, 10, 19 },
                    { 3, 32, 10, 19 },
                    { 4, 32, 10, 45 },
                    { 5, 32, 10, 26 },
                    { 7, 32, 10, 37 },
                    { 8, 32, 10, 10 },
                    { 1, 32, 12, 18 },
                    { 3, 32, 12, 37 },
                    { 4, 32, 12, 23 },
                    { 5, 32, 12, 14 },
                    { 7, 32, 12, 17 },
                    { 8, 32, 12, 45 },
                    { 1, 33, 2, 22 },
                    { 2, 33, 2, 35 },
                    { 3, 33, 2, 39 },
                    { 4, 33, 2, 46 },
                    { 5, 33, 2, 49 },
                    { 8, 33, 2, 40 },
                    { 1, 33, 5, 47 },
                    { 2, 33, 5, 49 },
                    { 3, 33, 5, 12 },
                    { 4, 33, 5, 48 },
                    { 5, 33, 5, 26 },
                    { 8, 33, 5, 28 },
                    { 1, 33, 9, 30 },
                    { 2, 33, 9, 30 },
                    { 3, 33, 9, 26 },
                    { 4, 33, 9, 15 },
                    { 5, 33, 9, 6 },
                    { 8, 33, 9, 23 },
                    { 1, 33, 10, 8 },
                    { 2, 33, 10, 17 },
                    { 3, 33, 10, 47 },
                    { 4, 33, 10, 11 },
                    { 5, 33, 10, 17 },
                    { 8, 33, 10, 26 },
                    { 3, 34, 5, 5 },
                    { 4, 34, 5, 30 },
                    { 8, 34, 5, 35 },
                    { 3, 34, 8, 32 },
                    { 4, 34, 8, 18 },
                    { 8, 34, 8, 7 },
                    { 3, 34, 14, 18 },
                    { 4, 34, 14, 44 },
                    { 8, 34, 14, 32 },
                    { 2, 35, 1, 6 },
                    { 3, 35, 1, 25 },
                    { 4, 35, 1, 23 },
                    { 5, 35, 1, 34 },
                    { 7, 35, 1, 10 },
                    { 2, 35, 2, 46 },
                    { 3, 35, 2, 35 },
                    { 4, 35, 2, 40 },
                    { 5, 35, 2, 39 },
                    { 7, 35, 2, 46 },
                    { 2, 35, 6, 27 },
                    { 3, 35, 6, 25 },
                    { 4, 35, 6, 45 },
                    { 5, 35, 6, 9 },
                    { 7, 35, 6, 25 },
                    { 2, 35, 9, 48 },
                    { 3, 35, 9, 34 },
                    { 4, 35, 9, 38 },
                    { 5, 35, 9, 8 },
                    { 7, 35, 9, 46 },
                    { 3, 36, 3, 17 },
                    { 5, 36, 3, 25 },
                    { 7, 36, 3, 47 },
                    { 8, 36, 3, 26 },
                    { 3, 36, 4, 17 },
                    { 5, 36, 4, 18 },
                    { 7, 36, 4, 14 },
                    { 8, 36, 4, 29 },
                    { 3, 36, 9, 42 },
                    { 5, 36, 9, 44 },
                    { 7, 36, 9, 22 },
                    { 8, 36, 9, 44 },
                    { 3, 36, 14, 31 },
                    { 5, 36, 14, 8 },
                    { 7, 36, 14, 24 },
                    { 8, 36, 14, 16 },
                    { 2, 37, 1, 7 },
                    { 4, 37, 1, 38 },
                    { 5, 37, 1, 35 },
                    { 6, 37, 1, 7 },
                    { 7, 37, 1, 19 },
                    { 8, 37, 1, 31 },
                    { 2, 37, 6, 36 },
                    { 4, 37, 6, 48 },
                    { 5, 37, 6, 28 },
                    { 6, 37, 6, 11 },
                    { 7, 37, 6, 10 },
                    { 8, 37, 6, 24 },
                    { 2, 37, 10, 24 },
                    { 4, 37, 10, 10 },
                    { 5, 37, 10, 16 },
                    { 6, 37, 10, 15 },
                    { 7, 37, 10, 31 },
                    { 8, 37, 10, 41 },
                    { 2, 37, 11, 5 },
                    { 4, 37, 11, 38 },
                    { 5, 37, 11, 31 },
                    { 6, 37, 11, 34 },
                    { 7, 37, 11, 12 },
                    { 8, 37, 11, 35 },
                    { 2, 37, 14, 35 },
                    { 4, 37, 14, 19 },
                    { 5, 37, 14, 5 },
                    { 6, 37, 14, 10 },
                    { 7, 37, 14, 9 },
                    { 8, 37, 14, 46 },
                    { 1, 38, 5, 40 },
                    { 3, 38, 5, 33 },
                    { 5, 38, 5, 32 },
                    { 7, 38, 5, 12 },
                    { 1, 38, 7, 35 },
                    { 3, 38, 7, 37 },
                    { 5, 38, 7, 43 },
                    { 7, 38, 7, 44 },
                    { 1, 38, 10, 29 },
                    { 3, 38, 10, 28 },
                    { 5, 38, 10, 22 },
                    { 7, 38, 10, 15 },
                    { 1, 39, 4, 40 },
                    { 2, 39, 4, 31 },
                    { 4, 39, 4, 9 },
                    { 5, 39, 4, 39 },
                    { 6, 39, 4, 38 },
                    { 7, 39, 4, 38 },
                    { 1, 39, 5, 25 },
                    { 2, 39, 5, 37 },
                    { 4, 39, 5, 31 },
                    { 5, 39, 5, 34 },
                    { 6, 39, 5, 11 },
                    { 7, 39, 5, 39 },
                    { 1, 39, 7, 7 },
                    { 2, 39, 7, 13 },
                    { 4, 39, 7, 34 },
                    { 5, 39, 7, 36 },
                    { 6, 39, 7, 38 },
                    { 7, 39, 7, 11 },
                    { 1, 39, 9, 29 },
                    { 2, 39, 9, 36 },
                    { 4, 39, 9, 45 },
                    { 5, 39, 9, 12 },
                    { 6, 39, 9, 15 },
                    { 7, 39, 9, 22 },
                    { 1, 39, 14, 20 },
                    { 2, 39, 14, 33 },
                    { 4, 39, 14, 43 },
                    { 5, 39, 14, 27 },
                    { 6, 39, 14, 35 },
                    { 7, 39, 14, 7 },
                    { 1, 40, 6, 48 },
                    { 4, 40, 6, 18 },
                    { 5, 40, 6, 19 },
                    { 7, 40, 6, 26 },
                    { 8, 40, 6, 13 },
                    { 1, 40, 10, 22 },
                    { 4, 40, 10, 24 },
                    { 5, 40, 10, 48 },
                    { 7, 40, 10, 41 },
                    { 8, 40, 10, 11 },
                    { 3, 41, 1, 38 },
                    { 5, 41, 1, 16 },
                    { 6, 41, 1, 49 },
                    { 7, 41, 1, 49 },
                    { 3, 41, 8, 18 },
                    { 5, 41, 8, 30 },
                    { 6, 41, 8, 21 },
                    { 7, 41, 8, 36 },
                    { 3, 41, 9, 17 },
                    { 5, 41, 9, 31 },
                    { 6, 41, 9, 33 },
                    { 7, 41, 9, 5 },
                    { 3, 41, 13, 8 },
                    { 5, 41, 13, 29 },
                    { 6, 41, 13, 22 },
                    { 7, 41, 13, 29 },
                    { 1, 42, 1, 7 },
                    { 2, 42, 1, 46 },
                    { 4, 42, 1, 18 },
                    { 5, 42, 1, 20 },
                    { 7, 42, 1, 49 },
                    { 8, 42, 1, 38 },
                    { 1, 42, 11, 35 },
                    { 2, 42, 11, 17 },
                    { 4, 42, 11, 28 },
                    { 5, 42, 11, 13 },
                    { 7, 42, 11, 18 },
                    { 8, 42, 11, 21 },
                    { 1, 42, 14, 49 },
                    { 2, 42, 14, 43 },
                    { 4, 42, 14, 31 },
                    { 5, 42, 14, 6 },
                    { 7, 42, 14, 11 },
                    { 8, 42, 14, 43 },
                    { 1, 43, 1, 10 },
                    { 4, 43, 1, 30 },
                    { 7, 43, 1, 25 },
                    { 8, 43, 1, 11 },
                    { 1, 43, 9, 42 },
                    { 4, 43, 9, 11 },
                    { 7, 43, 9, 24 },
                    { 8, 43, 9, 21 },
                    { 2, 44, 5, 30 },
                    { 6, 44, 5, 34 },
                    { 7, 44, 5, 27 },
                    { 8, 44, 5, 39 },
                    { 2, 44, 6, 46 },
                    { 6, 44, 6, 12 },
                    { 7, 44, 6, 45 },
                    { 8, 44, 6, 30 },
                    { 2, 44, 8, 26 },
                    { 6, 44, 8, 17 },
                    { 7, 44, 8, 34 },
                    { 8, 44, 8, 47 },
                    { 2, 44, 9, 10 },
                    { 6, 44, 9, 18 },
                    { 7, 44, 9, 48 },
                    { 8, 44, 9, 46 },
                    { 2, 44, 12, 23 },
                    { 6, 44, 12, 25 },
                    { 7, 44, 12, 35 },
                    { 8, 44, 12, 19 },
                    { 2, 45, 3, 5 },
                    { 5, 45, 3, 27 },
                    { 7, 45, 3, 6 },
                    { 2, 45, 6, 21 },
                    { 5, 45, 6, 30 },
                    { 7, 45, 6, 25 },
                    { 2, 45, 11, 39 },
                    { 5, 45, 11, 23 },
                    { 7, 45, 11, 12 },
                    { 1, 46, 6, 36 },
                    { 3, 46, 6, 16 },
                    { 4, 46, 6, 10 },
                    { 6, 46, 6, 14 },
                    { 7, 46, 6, 37 },
                    { 8, 46, 6, 13 },
                    { 1, 46, 9, 23 },
                    { 3, 46, 9, 9 },
                    { 4, 46, 9, 22 },
                    { 6, 46, 9, 47 },
                    { 7, 46, 9, 41 },
                    { 8, 46, 9, 32 },
                    { 1, 47, 1, 26 },
                    { 2, 47, 1, 18 },
                    { 3, 47, 1, 42 },
                    { 1, 47, 6, 49 },
                    { 2, 47, 6, 40 },
                    { 3, 47, 6, 9 },
                    { 1, 47, 12, 6 },
                    { 2, 47, 12, 39 },
                    { 3, 47, 12, 17 },
                    { 1, 48, 11, 17 },
                    { 5, 48, 11, 43 },
                    { 8, 48, 11, 32 },
                    { 1, 48, 14, 6 },
                    { 5, 48, 14, 20 },
                    { 8, 48, 14, 6 },
                    { 1, 49, 1, 40 },
                    { 2, 49, 1, 25 },
                    { 5, 49, 1, 31 },
                    { 6, 49, 1, 9 },
                    { 1, 49, 3, 37 },
                    { 2, 49, 3, 46 },
                    { 5, 49, 3, 44 },
                    { 6, 49, 3, 5 },
                    { 1, 49, 6, 25 },
                    { 2, 49, 6, 36 },
                    { 5, 49, 6, 25 },
                    { 6, 49, 6, 13 },
                    { 1, 49, 9, 7 },
                    { 2, 49, 9, 38 },
                    { 5, 49, 9, 34 },
                    { 6, 49, 9, 7 },
                    { 1, 50, 4, 17 },
                    { 2, 50, 4, 34 },
                    { 4, 50, 4, 30 },
                    { 7, 50, 4, 43 },
                    { 1, 50, 5, 15 },
                    { 2, 50, 5, 5 },
                    { 4, 50, 5, 14 },
                    { 7, 50, 5, 22 },
                    { 1, 50, 9, 33 },
                    { 2, 50, 9, 49 },
                    { 4, 50, 9, 45 },
                    { 7, 50, 9, 32 },
                    { 1, 50, 10, 8 },
                    { 2, 50, 10, 46 },
                    { 4, 50, 10, 27 },
                    { 7, 50, 10, 7 },
                    { 1, 50, 14, 38 },
                    { 2, 50, 14, 36 },
                    { 4, 50, 14, 18 },
                    { 7, 50, 14, 48 },
                    { 1, 51, 4, 19 },
                    { 4, 51, 4, 42 },
                    { 5, 51, 4, 27 },
                    { 1, 51, 9, 42 },
                    { 4, 51, 9, 47 },
                    { 5, 51, 9, 44 },
                    { 1, 51, 10, 9 },
                    { 4, 51, 10, 7 },
                    { 5, 51, 10, 26 },
                    { 1, 51, 13, 26 },
                    { 4, 51, 13, 35 },
                    { 5, 51, 13, 44 },
                    { 2, 52, 11, 45 },
                    { 5, 52, 11, 10 },
                    { 7, 52, 11, 34 },
                    { 8, 52, 11, 33 },
                    { 2, 52, 12, 6 },
                    { 5, 52, 12, 29 },
                    { 7, 52, 12, 5 },
                    { 8, 52, 12, 46 },
                    { 1, 53, 3, 18 },
                    { 2, 53, 3, 49 },
                    { 3, 53, 3, 19 },
                    { 4, 53, 3, 18 },
                    { 5, 53, 3, 21 },
                    { 6, 53, 3, 31 },
                    { 1, 53, 9, 39 },
                    { 2, 53, 9, 19 },
                    { 3, 53, 9, 47 },
                    { 4, 53, 9, 18 },
                    { 5, 53, 9, 36 },
                    { 6, 53, 9, 13 },
                    { 4, 54, 2, 16 },
                    { 5, 54, 2, 18 },
                    { 6, 54, 2, 27 },
                    { 8, 54, 2, 37 },
                    { 4, 54, 3, 19 },
                    { 5, 54, 3, 35 },
                    { 6, 54, 3, 42 },
                    { 8, 54, 3, 21 },
                    { 4, 54, 8, 20 },
                    { 5, 54, 8, 33 },
                    { 6, 54, 8, 15 },
                    { 8, 54, 8, 27 },
                    { 4, 54, 10, 43 },
                    { 5, 54, 10, 43 },
                    { 6, 54, 10, 46 },
                    { 8, 54, 10, 27 },
                    { 1, 55, 4, 16 },
                    { 2, 55, 4, 38 },
                    { 4, 55, 4, 21 },
                    { 6, 55, 4, 27 },
                    { 1, 55, 8, 45 },
                    { 2, 55, 8, 20 },
                    { 4, 55, 8, 46 },
                    { 6, 55, 8, 32 },
                    { 1, 55, 11, 26 },
                    { 2, 55, 11, 27 },
                    { 4, 55, 11, 11 },
                    { 6, 55, 11, 46 },
                    { 1, 55, 12, 47 },
                    { 2, 55, 12, 12 },
                    { 4, 55, 12, 19 },
                    { 6, 55, 12, 6 },
                    { 1, 56, 3, 36 },
                    { 2, 56, 3, 28 },
                    { 4, 56, 3, 12 },
                    { 5, 56, 3, 28 },
                    { 6, 56, 3, 25 },
                    { 8, 56, 3, 37 },
                    { 1, 56, 5, 40 },
                    { 2, 56, 5, 23 },
                    { 4, 56, 5, 30 },
                    { 5, 56, 5, 23 },
                    { 6, 56, 5, 49 },
                    { 8, 56, 5, 35 },
                    { 1, 56, 6, 16 },
                    { 2, 56, 6, 7 },
                    { 4, 56, 6, 25 },
                    { 5, 56, 6, 33 },
                    { 6, 56, 6, 9 },
                    { 8, 56, 6, 31 },
                    { 1, 56, 7, 17 },
                    { 2, 56, 7, 32 },
                    { 4, 56, 7, 35 },
                    { 5, 56, 7, 34 },
                    { 6, 56, 7, 46 },
                    { 8, 56, 7, 27 },
                    { 3, 57, 2, 23 },
                    { 4, 57, 2, 39 },
                    { 6, 57, 2, 40 },
                    { 7, 57, 2, 34 },
                    { 3, 57, 4, 35 },
                    { 4, 57, 4, 7 },
                    { 6, 57, 4, 40 },
                    { 7, 57, 4, 13 },
                    { 3, 57, 7, 32 },
                    { 4, 57, 7, 34 },
                    { 6, 57, 7, 35 },
                    { 7, 57, 7, 18 },
                    { 3, 57, 8, 8 },
                    { 4, 57, 8, 29 },
                    { 6, 57, 8, 23 },
                    { 7, 57, 8, 38 },
                    { 3, 57, 9, 37 },
                    { 4, 57, 9, 38 },
                    { 6, 57, 9, 30 },
                    { 7, 57, 9, 19 },
                    { 1, 58, 1, 22 },
                    { 2, 58, 1, 17 },
                    { 3, 58, 1, 23 },
                    { 1, 58, 4, 8 },
                    { 2, 58, 4, 7 },
                    { 3, 58, 4, 23 },
                    { 1, 58, 5, 38 },
                    { 2, 58, 5, 29 },
                    { 3, 58, 5, 31 },
                    { 1, 58, 11, 37 },
                    { 2, 58, 11, 29 },
                    { 3, 58, 11, 40 },
                    { 1, 58, 14, 22 },
                    { 2, 58, 14, 16 },
                    { 3, 58, 14, 38 },
                    { 2, 59, 1, 7 },
                    { 3, 59, 1, 13 },
                    { 5, 59, 1, 9 },
                    { 6, 59, 1, 9 },
                    { 7, 59, 1, 47 },
                    { 8, 59, 1, 42 },
                    { 2, 59, 3, 5 },
                    { 3, 59, 3, 25 },
                    { 5, 59, 3, 35 },
                    { 6, 59, 3, 41 },
                    { 7, 59, 3, 31 },
                    { 8, 59, 3, 49 },
                    { 2, 59, 4, 33 },
                    { 3, 59, 4, 46 },
                    { 5, 59, 4, 24 },
                    { 6, 59, 4, 35 },
                    { 7, 59, 4, 13 },
                    { 8, 59, 4, 16 },
                    { 2, 59, 13, 9 },
                    { 3, 59, 13, 27 },
                    { 5, 59, 13, 6 },
                    { 6, 59, 13, 49 },
                    { 7, 59, 13, 11 },
                    { 8, 59, 13, 27 },
                    { 2, 59, 14, 31 },
                    { 3, 59, 14, 21 },
                    { 5, 59, 14, 48 },
                    { 6, 59, 14, 39 },
                    { 7, 59, 14, 23 },
                    { 8, 59, 14, 40 },
                    { 3, 60, 1, 25 },
                    { 4, 60, 1, 43 },
                    { 7, 60, 1, 15 },
                    { 3, 60, 2, 45 },
                    { 4, 60, 2, 19 },
                    { 7, 60, 2, 33 },
                    { 3, 60, 4, 6 },
                    { 4, 60, 4, 29 },
                    { 7, 60, 4, 45 },
                    { 3, 60, 7, 47 },
                    { 4, 60, 7, 10 },
                    { 7, 60, 7, 25 },
                    { 3, 60, 13, 47 },
                    { 4, 60, 13, 39 },
                    { 7, 60, 13, 19 }
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
                name: "IX_Addresses_UserID",
                table: "Addresses",
                column: "UserID");

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
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Manager_UserID",
                table: "Manager",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderID",
                table: "OrderDetail",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ProductID",
                table: "OrderDetail",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AddressID",
                table: "Orders",
                column: "AddressID");

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
                name: "IX_ProductQty_ColorID",
                table: "ProductQty",
                column: "ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductQty_SizeID",
                table: "ProductQty",
                column: "SizeID");

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
                name: "ProductQty");

            migrationBuilder.DropTable(
                name: "ProductSeason");

            migrationBuilder.DropTable(
                name: "ProductSize");

            migrationBuilder.DropTable(
                name: "ProductStyle");

            migrationBuilder.DropTable(
                name: "ProductTag");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropTable(
                name: "Color");

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
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Brand");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Featured");

            migrationBuilder.DropTable(
                name: "Gender");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");
        }
    }
}
