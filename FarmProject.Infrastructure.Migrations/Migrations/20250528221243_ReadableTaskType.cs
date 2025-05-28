using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmProject.Infrastructure.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class ReadableTaskType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FarmEventType",
                table: "FarmEvents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FarmEventType",
                table: "FarmEvents",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
