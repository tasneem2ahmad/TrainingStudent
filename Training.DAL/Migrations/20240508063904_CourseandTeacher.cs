using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CourseandTeacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseTeacherId",
                table: "Teachers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_CourseTeacherId",
                table: "Teachers",
                column: "CourseTeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Courses_CourseTeacherId",
                table: "Teachers",
                column: "CourseTeacherId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Courses_CourseTeacherId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_CourseTeacherId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "CourseTeacherId",
                table: "Teachers");
        }
    }
}
