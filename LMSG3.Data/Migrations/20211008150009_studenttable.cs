using Microsoft.EntityFrameworkCore.Migrations;

namespace LMSG3.Data.Migrations
{
    public partial class studenttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Course_CourseId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Document_Course_CourseId",
                table: "Document");

            migrationBuilder.DropForeignKey(
                name: "FK_Literature_LiteratureLevel_LiteratureLevelId",
                table: "Literature");

            migrationBuilder.DropForeignKey(
                name: "FK_Literature_LiteratureType_LiteratureTypeId",
                table: "Literature");

            migrationBuilder.DropForeignKey(
                name: "FK_Literature_Subject_SubjectId",
                table: "Literature");

            migrationBuilder.DropForeignKey(
                name: "FK_LiteratureLiteratureAuthor_Literature_LiteraturesId",
                table: "LiteratureLiteratureAuthor");

            migrationBuilder.DropForeignKey(
                name: "FK_Module_Course_CourseId",
                table: "Module");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CourseId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Literature",
                table: "Literature");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Course",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "Literature",
                newName: "Literatures");

            migrationBuilder.RenameTable(
                name: "Course",
                newName: "Courses");

            migrationBuilder.RenameIndex(
                name: "IX_Literature_SubjectId",
                table: "Literatures",
                newName: "IX_Literatures_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Literature_LiteratureTypeId",
                table: "Literatures",
                newName: "IX_Literatures_LiteratureTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Literature_LiteratureLevelId",
                table: "Literatures",
                newName: "IX_Literatures_LiteratureLevelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Literatures",
                table: "Literatures",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Student_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Student_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Student_CourseId",
                table: "Student",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Courses_CourseId",
                table: "Document",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LiteratureLiteratureAuthor_Literatures_LiteraturesId",
                table: "LiteratureLiteratureAuthor",
                column: "LiteraturesId",
                principalTable: "Literatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Literatures_LiteratureLevel_LiteratureLevelId",
                table: "Literatures",
                column: "LiteratureLevelId",
                principalTable: "LiteratureLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Literatures_LiteratureType_LiteratureTypeId",
                table: "Literatures",
                column: "LiteratureTypeId",
                principalTable: "LiteratureType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Literatures_Subject_SubjectId",
                table: "Literatures",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Module_Courses_CourseId",
                table: "Module",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Courses_CourseId",
                table: "Document");

            migrationBuilder.DropForeignKey(
                name: "FK_LiteratureLiteratureAuthor_Literatures_LiteraturesId",
                table: "LiteratureLiteratureAuthor");

            migrationBuilder.DropForeignKey(
                name: "FK_Literatures_LiteratureLevel_LiteratureLevelId",
                table: "Literatures");

            migrationBuilder.DropForeignKey(
                name: "FK_Literatures_LiteratureType_LiteratureTypeId",
                table: "Literatures");

            migrationBuilder.DropForeignKey(
                name: "FK_Literatures_Subject_SubjectId",
                table: "Literatures");

            migrationBuilder.DropForeignKey(
                name: "FK_Module_Courses_CourseId",
                table: "Module");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Literatures",
                table: "Literatures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            migrationBuilder.RenameTable(
                name: "Literatures",
                newName: "Literature");

            migrationBuilder.RenameTable(
                name: "Courses",
                newName: "Course");

            migrationBuilder.RenameIndex(
                name: "IX_Literatures_SubjectId",
                table: "Literature",
                newName: "IX_Literature_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Literatures_LiteratureTypeId",
                table: "Literature",
                newName: "IX_Literature_LiteratureTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Literatures_LiteratureLevelId",
                table: "Literature",
                newName: "IX_Literature_LiteratureLevelId");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Literature",
                table: "Literature",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Course",
                table: "Course",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CourseId",
                table: "AspNetUsers",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Course_CourseId",
                table: "AspNetUsers",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Course_CourseId",
                table: "Document",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Literature_LiteratureLevel_LiteratureLevelId",
                table: "Literature",
                column: "LiteratureLevelId",
                principalTable: "LiteratureLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Literature_LiteratureType_LiteratureTypeId",
                table: "Literature",
                column: "LiteratureTypeId",
                principalTable: "LiteratureType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Literature_Subject_SubjectId",
                table: "Literature",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LiteratureLiteratureAuthor_Literature_LiteraturesId",
                table: "LiteratureLiteratureAuthor",
                column: "LiteraturesId",
                principalTable: "Literature",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Module_Course_CourseId",
                table: "Module",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
