using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMSG3.Api.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LiteratureAuthors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiteratureAuthors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "literatureLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_literatureLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Literatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubId = table.Column<int>(type: "int", nullable: false),
                    LiteraLevelId = table.Column<int>(type: "int", nullable: false),
                    LiteraTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Literatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LiteratureSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiteratureSubjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "literatureTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_literatureTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LiteratureLiteratureAuthor",
                columns: table => new
                {
                    AuthorsId = table.Column<int>(type: "int", nullable: false),
                    LiteraturesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiteratureLiteratureAuthor", x => new { x.AuthorsId, x.LiteraturesId });
                    table.ForeignKey(
                        name: "FK_LiteratureLiteratureAuthor_LiteratureAuthors_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "LiteratureAuthors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LiteratureLiteratureAuthor_Literatures_LiteraturesId",
                        column: x => x.LiteraturesId,
                        principalTable: "Literatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LiteratureLiteratureAuthor_LiteraturesId",
                table: "LiteratureLiteratureAuthor",
                column: "LiteraturesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "literatureLevels");

            migrationBuilder.DropTable(
                name: "LiteratureLiteratureAuthor");

            migrationBuilder.DropTable(
                name: "LiteratureSubjects");

            migrationBuilder.DropTable(
                name: "literatureTypes");

            migrationBuilder.DropTable(
                name: "LiteratureAuthors");

            migrationBuilder.DropTable(
                name: "Literatures");
        }
    }
}
