using Microsoft.EntityFrameworkCore.Migrations;

namespace ShoppingListAPI.Migrations
{
    public partial class IndependentRoleClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId",
                unique: true);
        }
    }
}
