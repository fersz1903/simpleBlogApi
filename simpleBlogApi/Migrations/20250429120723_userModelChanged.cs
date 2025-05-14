using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace simpleBlogApi.Migrations
{
    /// <inheritdoc />
    public partial class userModelChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_roleid",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "roleid",
                table: "Users",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "lastname",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "firstname",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameIndex(
                name: "IX_Users_roleid",
                table: "Users",
                newName: "IX_Users_RoleId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Roles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Roles",
                newName: "Id");

            migrationBuilder.AddColumn<Guid>(
                name: "PublicId",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "Users",
                newName: "roleid");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "lastname");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "firstname");

            migrationBuilder.RenameIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                newName: "IX_Users_roleid");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Roles",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Roles",
                newName: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_roleid",
                table: "Users",
                column: "roleid",
                principalTable: "Roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
