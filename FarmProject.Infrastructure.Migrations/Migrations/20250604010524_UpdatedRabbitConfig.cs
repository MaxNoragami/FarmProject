using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmProject.Infrastructure.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedRabbitConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BreedingRabbits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(52)", maxLength: 52, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BreedingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Available"),
                    CageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreedingRabbits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FarmTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FarmTaskType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MaleBreedingRabbitId = table.Column<int>(type: "int", nullable: true),
                    FemaleBreedingRabbitId = table.Column<int>(type: "int", nullable: true),
                    OffspringCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OffspringType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cages_BreedingRabbits_FemaleBreedingRabbitId",
                        column: x => x.FemaleBreedingRabbitId,
                        principalTable: "BreedingRabbits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cages_BreedingRabbits_MaleBreedingRabbitId",
                        column: x => x.MaleBreedingRabbitId,
                        principalTable: "BreedingRabbits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Pairs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaleBreedingRabbitId = table.Column<int>(type: "int", nullable: true),
                    FemaleBreedingRabbitId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PairingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Active")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pairs_BreedingRabbits_FemaleBreedingRabbitId",
                        column: x => x.FemaleBreedingRabbitId,
                        principalTable: "BreedingRabbits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pairs_BreedingRabbits_MaleBreedingRabbitId",
                        column: x => x.MaleBreedingRabbitId,
                        principalTable: "BreedingRabbits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BreedingRabbits_CageId",
                table: "BreedingRabbits",
                column: "CageId",
                filter: "[CageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Cages_FemaleBreedingRabbitId",
                table: "Cages",
                column: "FemaleBreedingRabbitId");

            migrationBuilder.CreateIndex(
                name: "IX_Cages_MaleBreedingRabbitId",
                table: "Cages",
                column: "MaleBreedingRabbitId");

            migrationBuilder.CreateIndex(
                name: "IX_Pairs_FemaleBreedingRabbitId",
                table: "Pairs",
                column: "FemaleBreedingRabbitId");

            migrationBuilder.CreateIndex(
                name: "IX_Pairs_MaleBreedingRabbitId",
                table: "Pairs",
                column: "MaleBreedingRabbitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cages");

            migrationBuilder.DropTable(
                name: "FarmTasks");

            migrationBuilder.DropTable(
                name: "Pairs");

            migrationBuilder.DropTable(
                name: "BreedingRabbits");
        }
    }
}
