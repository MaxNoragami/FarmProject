using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmProject.Infrastructure.Migrations.Migrations.Domain
{
    /// <inheritdoc />
    public partial class UpdateFarmTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CageId",
                table: "FarmTasks",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CageId",
                table: "FarmTasks");
        }
    }
}
