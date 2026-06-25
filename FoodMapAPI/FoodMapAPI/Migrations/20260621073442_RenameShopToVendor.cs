using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodMapAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameShopToVendor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_Shops_ShopId",
                table: "MenuItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ViewLogs_Shops_ShopId",
                table: "ViewLogs");

            migrationBuilder.DropTable(
                name: "Shops");

            migrationBuilder.RenameColumn(
                name: "ShopId",
                table: "ViewLogs",
                newName: "VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_ViewLogs_ShopId",
                table: "ViewLogs",
                newName: "IX_ViewLogs_VendorId");

            migrationBuilder.RenameColumn(
                name: "ShopId",
                table: "MenuItems",
                newName: "VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_MenuItems_ShopId",
                table: "MenuItems",
                newName: "IX_MenuItems_VendorId");

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    PriceRange = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    AudioFilePath = table.Column<string>(type: "TEXT", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: false),
                    OpenTime = table.Column<string>(type: "TEXT", nullable: false),
                    CloseTime = table.Column<string>(type: "TEXT", nullable: false),
                    IsOpenNow = table.Column<bool>(type: "INTEGER", nullable: false),
                    NarrationText = table.Column<string>(type: "TEXT", nullable: false),
                    NarrationLanguage = table.Column<string>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vendors_Owners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_OwnerId",
                table: "Vendors",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_Vendors_VendorId",
                table: "MenuItems",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ViewLogs_Vendors_VendorId",
                table: "ViewLogs",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_Vendors_VendorId",
                table: "MenuItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ViewLogs_Vendors_VendorId",
                table: "ViewLogs");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "ViewLogs",
                newName: "ShopId");

            migrationBuilder.RenameIndex(
                name: "IX_ViewLogs_VendorId",
                table: "ViewLogs",
                newName: "IX_ViewLogs_ShopId");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "MenuItems",
                newName: "ShopId");

            migrationBuilder.RenameIndex(
                name: "IX_MenuItems_VendorId",
                table: "MenuItems",
                newName: "IX_MenuItems_ShopId");

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: false),
                    AudioFilePath = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    CloseTime = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: false),
                    IsOpenNow = table.Column<bool>(type: "INTEGER", nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    NarrationLanguage = table.Column<string>(type: "TEXT", nullable: false),
                    NarrationText = table.Column<string>(type: "TEXT", nullable: false),
                    OpenTime = table.Column<string>(type: "TEXT", nullable: false),
                    PriceRange = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shops_Owners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shops_OwnerId",
                table: "Shops",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_Shops_ShopId",
                table: "MenuItems",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ViewLogs_Shops_ShopId",
                table: "ViewLogs",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
