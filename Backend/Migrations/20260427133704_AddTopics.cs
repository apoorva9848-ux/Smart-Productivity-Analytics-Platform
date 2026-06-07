using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DailyTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTopics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCompleted = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "Category", "CreatedAt", "DateCompleted", "IsCompleted", "Name", "Notes", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Angular", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5445), null, false, "Component architecture and communication", null, null },
                    { 2, "Angular", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5449), null, false, "Data binding (one-way, two-way, event binding)", null, null },
                    { 3, "Angular", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5451), null, false, "Lifecycle hooks and when they run", null, null },
                    { 4, "Angular", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5452), null, false, "Services and dependency injection", null, null },
                    { 5, "Angular", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5453), null, false, "Observables vs promises", null, null },
                    { 6, "Angular", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5456), null, false, "HTTP client and API integration", null, null },
                    { 7, "Angular", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5457), null, false, "Routing and navigation", null, null },
                    { 8, "Angular", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5458), null, false, "Route parameters and guards", null, null },
                    { 9, "Angular", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5459), null, false, "Reactive vs template-driven forms", null, null },
                    { 10, "Angular", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5461), null, false, "Form validation", null, null },
                    { 11, "Angular", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5462), null, false, "Change detection basics", null, null },
                    { 12, "Angular", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5463), null, false, "Lazy loading modules", null, null },
                    { 13, "Angular", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5465), null, false, "Error handling in API calls", null, null },
                    { 14, ".NET", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5466), null, false, "REST API fundamentals", null, null },
                    { 15, ".NET", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5467), null, false, "HTTP verbs and status codes", null, null },
                    { 16, ".NET", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5469), null, false, "Controllers and routing", null, null },
                    { 17, ".NET", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5470), null, false, "Model binding", null, null },
                    { 18, ".NET", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5472), null, false, "Dependency injection", null, null },
                    { 19, ".NET", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5473), null, false, "Layered architecture (controller/service/repository)", null, null },
                    { 20, ".NET", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5474), null, false, "CRUD operations", null, null },
                    { 21, ".NET", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5475), null, false, "Async/await usage", null, null },
                    { 22, ".NET", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5476), null, false, "Exception handling", null, null },
                    { 23, ".NET", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5477), null, false, "DTO usage", null, null },
                    { 24, ".NET", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5478), null, false, "Authentication basics (JWT flow)", null, null },
                    { 25, ".NET", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5479), null, false, "Validation", null, null },
                    { 26, ".NET", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5481), null, false, "Database integration (EF/ADO.NET basics)", null, null },
                    { 27, "SQL", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5482), null, false, "SELECT queries", null, null },
                    { 28, "SQL", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5483), null, false, "WHERE filtering", null, null },
                    { 29, "SQL", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5484), null, false, "ORDER BY", null, null },
                    { 30, "SQL", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5485), null, false, "GROUP BY", null, null },
                    { 31, "SQL", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5486), null, false, "Aggregate functions (COUNT, SUM, AVG)", null, null },
                    { 32, "SQL", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5520), null, false, "JOINS", null, null },
                    { 33, "SQL", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5521), null, false, "Primary and foreign keys", null, null },
                    { 34, "SQL", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5523), null, false, "Basic indexing concept", null, null },
                    { 35, "SQL", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5524), null, false, "Stored procedures basics", null, null },
                    { 36, "SQL", new DateTime(2026, 4, 27, 13, 37, 3, 567, DateTimeKind.Utc).AddTicks(5525), null, false, "Insert/update/delete operations", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Topics_Category",
                table: "Topics",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_IsCompleted",
                table: "Topics",
                column: "IsCompleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Topics");
        }
    }
}
