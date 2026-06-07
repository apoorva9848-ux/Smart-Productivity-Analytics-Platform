using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailyTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDSAProblems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DSAProblems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    DateSolved = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Link = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Difficulty = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Pattern = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DSAProblems", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DSAProblems_DateSolved",
                table: "DSAProblems",
                column: "DateSolved");

            migrationBuilder.CreateIndex(
                name: "IX_DSAProblems_Difficulty",
                table: "DSAProblems",
                column: "Difficulty");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DSAProblems");
        }
    }
}
