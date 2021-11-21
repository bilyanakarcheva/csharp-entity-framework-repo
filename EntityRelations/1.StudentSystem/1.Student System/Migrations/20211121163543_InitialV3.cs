using Microsoft.EntityFrameworkCore.Migrations;

namespace P01_StudentSystem.Migrations
{
    public partial class InitialV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomeworksSubmissions_Courses_CourseId",
                table: "HomeworksSubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_HomeworksSubmissions_Students_StudentId",
                table: "HomeworksSubmissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HomeworksSubmissions",
                table: "HomeworksSubmissions");

            migrationBuilder.RenameTable(
                name: "HomeworksSubmissions",
                newName: "HomeworkSubmissions");

            migrationBuilder.RenameIndex(
                name: "IX_HomeworksSubmissions_StudentId",
                table: "HomeworkSubmissions",
                newName: "IX_HomeworkSubmissions_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_HomeworksSubmissions_CourseId",
                table: "HomeworkSubmissions",
                newName: "IX_HomeworkSubmissions_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HomeworkSubmissions",
                table: "HomeworkSubmissions",
                column: "HomeworkId");

            migrationBuilder.AddForeignKey(
                name: "FK_HomeworkSubmissions_Courses_CourseId",
                table: "HomeworkSubmissions",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HomeworkSubmissions_Students_StudentId",
                table: "HomeworkSubmissions",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomeworkSubmissions_Courses_CourseId",
                table: "HomeworkSubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_HomeworkSubmissions_Students_StudentId",
                table: "HomeworkSubmissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HomeworkSubmissions",
                table: "HomeworkSubmissions");

            migrationBuilder.RenameTable(
                name: "HomeworkSubmissions",
                newName: "HomeworksSubmissions");

            migrationBuilder.RenameIndex(
                name: "IX_HomeworkSubmissions_StudentId",
                table: "HomeworksSubmissions",
                newName: "IX_HomeworksSubmissions_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_HomeworkSubmissions_CourseId",
                table: "HomeworksSubmissions",
                newName: "IX_HomeworksSubmissions_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HomeworksSubmissions",
                table: "HomeworksSubmissions",
                column: "HomeworkId");

            migrationBuilder.AddForeignKey(
                name: "FK_HomeworksSubmissions_Courses_CourseId",
                table: "HomeworksSubmissions",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HomeworksSubmissions_Students_StudentId",
                table: "HomeworksSubmissions",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
