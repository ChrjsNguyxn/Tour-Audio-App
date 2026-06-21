using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodMapAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerStatusAndShopHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CloseTime",
                table: "Shops",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsOpenNow",
                table: "Shops",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OpenTime",
                table: "Shops",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Owners",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseTime",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "IsOpenNow",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "OpenTime",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Owners");
        }
    }
}
