using Microsoft.EntityFrameworkCore.Migrations;

namespace LMSG3.Data.Migrations
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModuleTeacher");

            migrationBuilder.CreateTable(
                name: "TeacherModule",
                columns: table => new
                {
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    TeacherId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherModule", x => new { x.ModuleId, x.TeacherId });
                    table.ForeignKey(
                        name: "FK_TeacherModule_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Module",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeacherModule_Teacher_TeacherId1",
                        column: x => x.TeacherId1,
                        principalTable: "Teacher",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeacherModule_TeacherId1",
                table: "TeacherModule",
                column: "TeacherId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeacherModule");

            migrationBuilder.CreateTable(
                name: "ModuleTeacher",
                columns: table => new
                {
                    ModulesId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleTeacher", x => new { x.ModulesId, x.TeacherId });
                    table.ForeignKey(
                        name: "FK_ModuleTeacher_Module_ModulesId",
                        column: x => x.ModulesId,
                        principalTable: "Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModuleTeacher_Teacher_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModuleTeacher_TeacherId",
                table: "ModuleTeacher",
                column: "TeacherId");
        }
    }
}
