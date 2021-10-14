using Microsoft.EntityFrameworkCore.Migrations;

namespace LMSG3.Data.Migrations
{
    public partial class init3 : Migration
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
