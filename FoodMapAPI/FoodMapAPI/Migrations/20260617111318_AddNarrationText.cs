using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodMapAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddNarrationText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NarrationLanguage",
                table: "Shops",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NarrationText",
                table: "Shops",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NarrationLanguage",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "NarrationText",
                table: "Shops");
        }
    }
}
