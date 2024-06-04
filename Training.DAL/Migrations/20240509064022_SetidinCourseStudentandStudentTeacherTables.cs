using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SetidinCourseStudentandStudentTeacherTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentTeachers",
                table: "StudentTeachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseStudents",
                table: "CourseStudents");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "StudentTeachers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CourseStudents",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentTeachers",
                table: "StudentTeachers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseStudents",
                table: "CourseStudents",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTeachers_StudentID",
                table: "StudentTeachers",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseStudents_CourseID",
                table: "CourseStudents",
                column: "CourseID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentTeachers",
                table: "StudentTeachers");

            migrationBuilder.DropIndex(
                name: "IX_StudentTeachers_StudentID",
                table: "StudentTeachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseStudents",
                table: "CourseStudents");

            migrationBuilder.DropIndex(
                name: "IX_CourseStudents_CourseID",
                table: "CourseStudents");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StudentTeachers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CourseStudents");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentTeachers",
                table: "StudentTeachers",
                columns: new[] { "StudentID", "TeacherID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseStudents",
                table: "CourseStudents",
                columns: new[] { "CourseID", "StudentID" });
        }
    }
}
