using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YuGiOh.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveArchetypeTableDescriptionParameter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Archetypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Archetypes",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}
