using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmProject.Infrastructure.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class FromRabbitToBreedingRabbit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pairs_Rabbits_FemaleRabbitId",
                table: "Pairs");

            migrationBuilder.DropForeignKey(
                name: "FK_Pairs_Rabbits_MaleRabbitId",
                table: "Pairs");

            migrationBuilder.DropTable(
                name: "Rabbits");

            migrationBuilder.RenameColumn(
                name: "MaleRabbitId",
                table: "Pairs",
                newName: "MaleBreedingRabbitId");

            migrationBuilder.RenameColumn(
                name: "FemaleRabbitId",
                table: "Pairs",
                newName: "FemaleBreedingRabbitId");

            migrationBuilder.RenameIndex(
                name: "IX_Pairs_MaleRabbitId",
                table: "Pairs",
                newName: "IX_Pairs_MaleBreedingRabbitId");

            migrationBuilder.RenameIndex(
                name: "IX_Pairs_FemaleRabbitId",
                table: "Pairs",
                newName: "IX_Pairs_FemaleBreedingRabbitId");

            migrationBuilder.CreateTable(
                name: "BreedingRabbits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(52)", maxLength: 52, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BreedingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Available")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreedingRabbits", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Pairs_BreedingRabbits_FemaleBreedingRabbitId",
                table: "Pairs",
                column: "FemaleBreedingRabbitId",
                principalTable: "BreedingRabbits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pairs_BreedingRabbits_MaleBreedingRabbitId",
                table: "Pairs",
                column: "MaleBreedingRabbitId",
                principalTable: "BreedingRabbits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pairs_BreedingRabbits_FemaleBreedingRabbitId",
                table: "Pairs");

            migrationBuilder.DropForeignKey(
                name: "FK_Pairs_BreedingRabbits_MaleBreedingRabbitId",
                table: "Pairs");

            migrationBuilder.DropTable(
                name: "BreedingRabbits");

            migrationBuilder.RenameColumn(
                name: "MaleBreedingRabbitId",
                table: "Pairs",
                newName: "MaleRabbitId");

            migrationBuilder.RenameColumn(
                name: "FemaleBreedingRabbitId",
                table: "Pairs",
                newName: "FemaleRabbitId");

            migrationBuilder.RenameIndex(
                name: "IX_Pairs_MaleBreedingRabbitId",
                table: "Pairs",
                newName: "IX_Pairs_MaleRabbitId");

            migrationBuilder.RenameIndex(
                name: "IX_Pairs_FemaleBreedingRabbitId",
                table: "Pairs",
                newName: "IX_Pairs_FemaleRabbitId");

            migrationBuilder.CreateTable(
                name: "Rabbits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BreedingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Available"),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(52)", maxLength: 52, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rabbits", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Pairs_Rabbits_FemaleRabbitId",
                table: "Pairs",
                column: "FemaleRabbitId",
                principalTable: "Rabbits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pairs_Rabbits_MaleRabbitId",
                table: "Pairs",
                column: "MaleRabbitId",
                principalTable: "Rabbits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
