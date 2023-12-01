using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Framework.Migrations
{
    /// <inheritdoc />
    public partial class BugFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PlayerClans_PlayerId",
                table: "PlayerClans");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerClans_PlayerId",
                table: "PlayerClans",
                column: "PlayerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PlayerClans_PlayerId",
                table: "PlayerClans");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerClans_PlayerId",
                table: "PlayerClans",
                column: "PlayerId");
        }
    }
}
