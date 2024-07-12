using Microsoft.EntityFrameworkCore;

namespace Shared;

public class TodoItem
{
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;

    public Priority Priority { get; set; }
}

public enum Priority
{
    Highest,
    High,
    Normal,
    Low,
    Lowest
}

public class TodoItemDbContext(DbContextOptions<TodoItemDbContext> options) : DbContext(options)
{
    public DbSet<TodoItem> Items => Set<TodoItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<TodoItem>();

        entity.HasKey(p => p.Id);

        entity.Property(p => p.Id).ValueGeneratedOnAdd();

        entity.Property(p => p.Title).HasMaxLength(64);

        entity.Property(p => p.Priority);
    }
}
