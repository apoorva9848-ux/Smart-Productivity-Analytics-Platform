using DailyTracker.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DailyTracker.API.Data;

/// <summary>
/// Main database context for the Daily Tracker application.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// DbSet for DailyLog entities.
    /// </summary>
    public DbSet<DailyLog> DailyLogs { get; set; } = null!;

    /// <summary>
    /// DbSet for Activity entities.
    /// </summary>
    public DbSet<Activity> Activities { get; set; } = null!;

    /// <summary>
    /// DbSet for DSAProblem entities.
    /// </summary>
    public DbSet<DSAProblem> DSAProblems { get; set; } = null!;

    /// <summary>
    /// DbSet for Topic entities.
    /// </summary>
    public DbSet<Topic> Topics { get; set; } = null!;

    /// <summary>
    /// Configure model relationships and constraints.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure DailyLog entity
        modelBuilder.Entity<DailyLog>(entity =>
        {
            entity.ToTable("DailyLogs");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Date)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.Property(e => e.UpdatedAt);

            entity.HasMany(e => e.Activities)
                .WithOne(a => a.DailyLog)
                .HasForeignKey(a => a.DailyLogId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.Date)
                .IsUnique();
        });

        // Configure Activity entity
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.ToTable("Activities");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Type)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsRequired();

            entity.Property(e => e.DurationInMinutes);

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.HasIndex(e => e.DailyLogId);
        });

        // Configure DSAProblem entity
        modelBuilder.Entity<DSAProblem>(entity =>
        {
            entity.ToTable("DSAProblems");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(e => e.DateSolved)
                .IsRequired();

            entity.Property(e => e.Link)
                .HasMaxLength(500);

            entity.Property(e => e.Difficulty)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.Pattern)
                .HasMaxLength(100);

            entity.Property(e => e.Notes)
                .HasMaxLength(1000);

            entity.Property(e => e.CreatedAt);

            // Create index on DateSolved for faster queries
            entity.HasIndex(e => e.DateSolved);

            // Create index on Difficulty for filtering
            entity.HasIndex(e => e.Difficulty);
        });

        // Configure Topic entity
        modelBuilder.Entity<Topic>(entity =>
        {
            entity.ToTable("Topics");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.IsCompleted)
                .IsRequired();

            entity.Property(e => e.DateCompleted);

            entity.Property(e => e.Notes)
                .HasMaxLength(1000);

            entity.Property(e => e.CreatedAt);

            // Create index on Category for filtering
            entity.HasIndex(e => e.Category);

            // Create index on IsCompleted for progress calculations
            entity.HasIndex(e => e.IsCompleted);
        });

        // Seed initial topics if table is empty
        SeedTopics(modelBuilder);
    }

    private void SeedTopics(ModelBuilder modelBuilder)
    {
        var topics = new List<Topic>
        {
            // Angular Topics
            new Topic { Id = 1, Name = "Component architecture and communication", Category = "Angular", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 2, Name = "Data binding (one-way, two-way, event binding)", Category = "Angular", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 3, Name = "Lifecycle hooks and when they run", Category = "Angular", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 4, Name = "Services and dependency injection", Category = "Angular", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 5, Name = "Observables vs promises", Category = "Angular", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 6, Name = "HTTP client and API integration", Category = "Angular", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 7, Name = "Routing and navigation", Category = "Angular", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 8, Name = "Route parameters and guards", Category = "Angular", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 9, Name = "Reactive vs template-driven forms", Category = "Angular", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 10, Name = "Form validation", Category = "Angular", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 11, Name = "Change detection basics", Category = "Angular", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 12, Name = "Lazy loading modules", Category = "Angular", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 13, Name = "Error handling in API calls", Category = "Angular", IsCompleted = false, CreatedAt = DateTime.UtcNow },

            // .NET Topics
            new Topic { Id = 14, Name = "REST API fundamentals", Category = ".NET", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 15, Name = "HTTP verbs and status codes", Category = ".NET", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 16, Name = "Controllers and routing", Category = ".NET", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 17, Name = "Model binding", Category = ".NET", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 18, Name = "Dependency injection", Category = ".NET", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 19, Name = "Layered architecture (controller/service/repository)", Category = ".NET", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 20, Name = "CRUD operations", Category = ".NET", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 21, Name = "Async/await usage", Category = ".NET", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 22, Name = "Exception handling", Category = ".NET", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 23, Name = "DTO usage", Category = ".NET", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 24, Name = "Authentication basics (JWT flow)", Category = ".NET", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 25, Name = "Validation", Category = ".NET", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 26, Name = "Database integration (EF/ADO.NET basics)", Category = ".NET", IsCompleted = false, CreatedAt = DateTime.UtcNow },

            // SQL Topics
            new Topic { Id = 27, Name = "SELECT queries", Category = "SQL", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 28, Name = "WHERE filtering", Category = "SQL", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 29, Name = "ORDER BY", Category = "SQL", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 30, Name = "GROUP BY", Category = "SQL", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 31, Name = "Aggregate functions (COUNT, SUM, AVG)", Category = "SQL", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 32, Name = "JOINS", Category = "SQL", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 33, Name = "Primary and foreign keys", Category = "SQL", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 34, Name = "Basic indexing concept", Category = "SQL", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 35, Name = "Stored procedures basics", Category = "SQL", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 36, Name = "Insert/update/delete operations", Category = "SQL", IsCompleted = false, CreatedAt = DateTime.UtcNow }
        };

        modelBuilder.Entity<Topic>().HasData(topics);
    }
}
