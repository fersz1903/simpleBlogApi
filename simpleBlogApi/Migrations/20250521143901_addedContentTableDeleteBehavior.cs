using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace simpleBlogApi.Migrations
{
    /// <inheritdoc />
    public partial class addedContentTableDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Posts_Contents_ContentId", table: "Posts");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Contents_ContentId",
                table: "Posts",
                column: "ContentId",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Posts_Contents_ContentId", table: "Posts");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Contents_ContentId",
                table: "Posts",
                column: "ContentId",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull
            );
        }
    }
}
