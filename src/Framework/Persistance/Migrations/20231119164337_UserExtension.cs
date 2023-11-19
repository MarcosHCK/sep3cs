using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Framework.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class UserExtension : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Cards_FavoriteCardId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Nickname",
                table: "Players");

            migrationBuilder.AlterColumn<long>(
                name: "FavoriteCardId",
                table: "Players",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<long>(
                name: "PlayerId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PlayerId",
                table: "AspNetUsers",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Players_PlayerId",
                table: "AspNetUsers",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Cards_FavoriteCardId",
                table: "Players",
                column: "FavoriteCardId",
                principalTable: "Cards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Players_PlayerId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Cards_FavoriteCardId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PlayerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<long>(
                name: "FavoriteCardId",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nickname",
                table: "Players",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Cards_FavoriteCardId",
                table: "Players",
                column: "FavoriteCardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
