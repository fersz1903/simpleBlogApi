using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace simpleBlogApi.Migrations
{
    /// <inheritdoc />
    public partial class updatedContentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverPicture",
                table: "Contents",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverPicture",
                table: "Contents");
        }
    }
}
