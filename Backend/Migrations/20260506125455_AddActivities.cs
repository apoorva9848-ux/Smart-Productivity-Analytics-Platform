using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailyTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class AddActivities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DSA",
                table: "DailyLogs");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "DailyLogs");

            migrationBuilder.DropColumn(
                name: "Project",
                table: "DailyLogs");

            migrationBuilder.DropColumn(
                name: "Theory",
                table: "DailyLogs");

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DailyLogId = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    DurationInMinutes = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_DailyLogs_DailyLogId",
                        column: x => x.DailyLogId,
                        principalTable: "DailyLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5027));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5031));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5033));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5035));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5037));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5040));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5041));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5043));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5044));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5047));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5048));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5049));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5051));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5052));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5054));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5055));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5057));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5059));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5060));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5062));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5063));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5064));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5066));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5067));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5068));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5070));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5071));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5072));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5074));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5075));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5077));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5078));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5079));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5081));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5083));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 12, 54, 54, 129, DateTimeKind.Utc).AddTicks(5084));

            migrationBuilder.CreateIndex(
                name: "IX_Activities_DailyLogId",
                table: "Activities",
                column: "DailyLogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.AddColumn<string>(
                name: "DSA",
                table: "DailyLogs",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "DailyLogs",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Project",
                table: "DailyLogs",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Theory",
                table: "DailyLogs",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5445));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5449));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5451));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5452));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5453));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5456));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5457));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5458));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5459));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5461));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5462));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5463));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5465));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5466));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5467));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5469));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5470));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5472));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5473));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5474));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5475));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5476));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5477));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5478));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5479));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5481));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5482));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5483));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5484));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5485));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5486));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5520));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5521));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5523));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5524));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5525));
        }
    }
}
