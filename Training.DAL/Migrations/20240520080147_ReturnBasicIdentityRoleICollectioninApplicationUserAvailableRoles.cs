using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ReturnBasicIdentityRoleICollectioninApplicationUserAvailableRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "AspNetRoles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_ApplicationUserId1",
                table: "AspNetRoles",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_AspNetUsers_ApplicationUserId1",
                table: "AspNetRoles",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_AspNetUsers_ApplicationUserId1",
                table: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_ApplicationUserId1",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "AspNetRoles");
        }
    }
}
