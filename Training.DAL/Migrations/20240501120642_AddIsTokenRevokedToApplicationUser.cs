using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddIsTokenRevokedToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTokenRevoked",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTokenRevoked",
                table: "AspNetUsers");
        }
    }
}
