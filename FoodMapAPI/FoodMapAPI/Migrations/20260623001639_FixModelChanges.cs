using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodMapAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_Vendors_VendorId",
                table: "MenuItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ViewLogs_Vendors_VendorId",
                table: "ViewLogs");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_VendorId",
                table: "MenuItems");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "ViewLogs",
                newName: "EateryId");

            migrationBuilder.RenameIndex(
                name: "IX_ViewLogs_VendorId",
                table: "ViewLogs",
                newName: "IX_ViewLogs_EateryId");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "MenuItems",
                newName: "IsAvailable");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "MenuItems",
                newName: "EateryId");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "MenuItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "MenuItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Eateries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    PriceRange = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Rating = table.Column<double>(type: "REAL", nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    AudioFilePath = table.Column<string>(type: "TEXT", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: false),
                    OpenTime = table.Column<string>(type: "TEXT", nullable: false),
                    CloseTime = table.Column<string>(type: "TEXT", nullable: false),
                    IsOpenNow = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsApproved = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NarrationText = table.Column<string>(type: "TEXT", nullable: false),
                    NarrationLanguage = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eateries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eateries_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Eateries_Owners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_EateryId",
                table: "MenuItems",
                column: "EateryId");

            migrationBuilder.CreateIndex(
                name: "IX_Eateries_CategoryId",
                table: "Eateries",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Eateries_OwnerId",
                table: "Eateries",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_Eateries_EateryId",
                table: "MenuItems",
                column: "EateryId",
                principalTable: "Eateries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ViewLogs_Eateries_EateryId",
                table: "ViewLogs",
                column: "EateryId",
                principalTable: "Eateries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_Eateries_EateryId",
                table: "MenuItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ViewLogs_Eateries_EateryId",
                table: "ViewLogs");

            migrationBuilder.DropTable(
                name: "Eateries");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_EateryId",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "MenuItems");

            migrationBuilder.RenameColumn(
                name: "EateryId",
                table: "ViewLogs",
                newName: "VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_ViewLogs_EateryId",
                table: "ViewLogs",
                newName: "IX_ViewLogs_VendorId");

            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "MenuItems",
                newName: "VendorId");

            migrationBuilder.RenameColumn(
                name: "EateryId",
                table: "MenuItems",
                newName: "Quantity");

            migrationBuilder.AlterColumn<string>(
                name: "Price",
                table: "MenuItems",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateTable(
                name: "Vendors",
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
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vendors_Owners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_VendorId",
                table: "MenuItems",
                column: "VendorId");

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
    }
}
