using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Framework.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class WarSpan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BeginDay",
                table: "Wars",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeginDay",
                table: "Wars");
        }
    }
}
