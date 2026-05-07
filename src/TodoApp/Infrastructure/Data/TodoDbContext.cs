using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Models;

namespace TodoApp.Infrastructure.Data;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {
    }

    public DbSet<TodoItem> Todos { get; set; } = null!;

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
