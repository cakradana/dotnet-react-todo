using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Models;

namespace TodoApp.Infrastructure.Data;

public class TodoDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of <see cref="TodoDbContext"/> using the provided options.
    /// </summary>
    /// <param name="options">The options to configure the context (e.g., provider, connection string, and other EF Core settings).</param>
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {
    }

    public DbSet<TodoItem> Todos { get; set; } = null!;

    /// <summary>
    /// Configures Entity Framework Core mappings for the TodoItem entity and its owned Category value object.
    /// </summary>
    /// <param name="modelBuilder">The <see cref="ModelBuilder"/> used to configure entity types and relationships.</param>
    /// <remarks>
    /// - Sets Id as the primary key and configures it to be generated on add.
    /// - Configures Category as an owned type and maps its properties to the columns
    ///   "CategoryName" and "CategoryColor".
    /// </remarks>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TodoItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Generate ID automatically
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            // Category is an owned type (Value Object)
            entity.OwnsOne(e => e.Category, category =>
            {
                category.Property(c => c.Name).HasColumnName("CategoryName");
                category.Property(c => c.Color).HasColumnName("CategoryColor");
            });
        });
    }
}
