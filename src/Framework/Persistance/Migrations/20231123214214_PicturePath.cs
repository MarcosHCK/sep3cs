using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Framework.Migrations
{
    /// <inheritdoc />
    public partial class PicturePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Cards",
                newName: "Picture");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Picture",
                table: "Cards",
                newName: "ImagePath");
        }
    }
}
