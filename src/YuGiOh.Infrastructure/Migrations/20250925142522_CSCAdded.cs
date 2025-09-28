using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YuGiOh.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CSCAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Addresses");

            migrationBuilder.AddColumn<string>(
                name: "CityIso2",
                table: "Addresses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CountryIso2",
                table: "Addresses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StateIso2",
                table: "Addresses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityIso2",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CountryIso2",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "StateIso2",
                table: "Addresses");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Addresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Addresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StateId",
                table: "Addresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
