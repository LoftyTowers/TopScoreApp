using Microsoft.EntityFrameworkCore;
using TopScore.Core.Models;

namespace TopScore.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<WordEntry> WordEntries => Set<WordEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WordEntry>()
            .HasIndex(w => w.Word)
            .IsUnique();

        modelBuilder.Entity<WordEntry>()
            .Property(w => w.Word)
            .IsRequired()
            .HasMaxLength(500);
    }
}
