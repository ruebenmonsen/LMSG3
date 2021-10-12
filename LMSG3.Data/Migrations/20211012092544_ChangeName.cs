using Microsoft.EntityFrameworkCore.Migrations;

namespace LMSG3.Data.Migrations
{
    public partial class ChangeName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiteratureLiteratureAuthor_LiteratureAuthor_AuthorsId",
                table: "LiteratureLiteratureAuthor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LiteratureAuthor",
                table: "LiteratureAuthor");

            migrationBuilder.RenameTable(
                name: "LiteratureAuthor",
                newName: "LiteratureAuthors");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LiteratureAuthors",
                table: "LiteratureAuthors",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LiteratureLiteratureAuthor_LiteratureAuthors_AuthorsId",
                table: "LiteratureLiteratureAuthor",
                column: "AuthorsId",
                principalTable: "LiteratureAuthors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiteratureLiteratureAuthor_LiteratureAuthors_AuthorsId",
                table: "LiteratureLiteratureAuthor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LiteratureAuthors",
                table: "LiteratureAuthors");

            migrationBuilder.RenameTable(
                name: "LiteratureAuthors",
                newName: "LiteratureAuthor");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LiteratureAuthor",
                table: "LiteratureAuthor",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LiteratureLiteratureAuthor_LiteratureAuthor_AuthorsId",
                table: "LiteratureLiteratureAuthor",
                column: "AuthorsId",
                principalTable: "LiteratureAuthor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
