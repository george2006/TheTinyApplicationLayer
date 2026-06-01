using Microsoft.EntityFrameworkCore;
using TheTinyApplicationLayer.Application.Users;
using TinyEvents.SqlServer.EntityFrameworkCore;

namespace TheTinyApplicationLayer.Application.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserRow> Users => Set<UserRow>();

    public DbSet<WelcomeEmailLogRow> WelcomeEmailLogs => Set<WelcomeEmailLogRow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRow>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(user => user.Id);
            entity.Property(user => user.Email).IsRequired().HasMaxLength(320);
            entity.Property(user => user.DisplayName).IsRequired().HasMaxLength(120);
            entity.Property(user => user.RegisteredAtUtc).IsRequired();
            entity.HasIndex(user => user.Email).IsUnique();
        });

        modelBuilder.Entity<WelcomeEmailLogRow>(entity =>
        {
            entity.ToTable("WelcomeEmailLogs");
            entity.HasKey(log => log.Id);
            entity.Property(log => log.Email).IsRequired().HasMaxLength(320);
            entity.Property(log => log.Message).IsRequired().HasMaxLength(500);
            entity.Property(log => log.CreatedAtUtc).IsRequired();
        });

        modelBuilder.UseTinyEventsOutbox();
    }
}

