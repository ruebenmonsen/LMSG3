using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMSG3.Api.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LiteratureAuthor",
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
                    table.PrimaryKey("PK_LiteratureAuthor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LiteratureLevel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiteratureLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LiteratureType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiteratureType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.Id);
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
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    LiteratureLevelId = table.Column<int>(type: "int", nullable: false),
                    LiteratureTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Literatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Literatures_LiteratureLevel_LiteratureLevelId",
                        column: x => x.LiteratureLevelId,
                        principalTable: "LiteratureLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Literatures_LiteratureType_LiteratureTypeId",
                        column: x => x.LiteratureTypeId,
                        principalTable: "LiteratureType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Literatures_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        name: "FK_LiteratureLiteratureAuthor_LiteratureAuthor_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "LiteratureAuthor",
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

            migrationBuilder.CreateIndex(
                name: "IX_Literatures_LiteratureLevelId",
                table: "Literatures",
                column: "LiteratureLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Literatures_LiteratureTypeId",
                table: "Literatures",
                column: "LiteratureTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Literatures_SubjectId",
                table: "Literatures",
                column: "SubjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LiteratureLiteratureAuthor");

            migrationBuilder.DropTable(
                name: "LiteratureAuthor");

            migrationBuilder.DropTable(
                name: "Literatures");

            migrationBuilder.DropTable(
                name: "LiteratureLevel");

            migrationBuilder.DropTable(
                name: "LiteratureType");

            migrationBuilder.DropTable(
                name: "Subject");
        }
    }
}
