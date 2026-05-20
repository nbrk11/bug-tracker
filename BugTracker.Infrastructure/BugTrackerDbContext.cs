using BugTracker.Domain;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Infrastructure;

public class BugTrackerDbContext : DbContext
{
    public BugTrackerDbContext(DbContextOptions<BugTrackerDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>()
            .HasKey(p => p.Id)
            .HasName("PK_ProjectId");

        modelBuilder.Entity<Project>()
            .Property(p => p.CreatedDate)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Issue>()
            .HasKey(i => i.Id)
            .HasName("PK_IssueId");

        modelBuilder.Entity<Issue>()
            .Property(i => i.CreatedDate)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<User>()
            .HasKey(u => u.Id)
            .HasName("PK_UserId");

        modelBuilder.Entity<User>()
            .Property(u => u.CreatedDate)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Comment>()
            .HasKey(c => c.Id)
            .HasName("PK_CommentId");

        modelBuilder.Entity<Comment>()
            .Property(c => c.CreatedDate)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Project>()
            .HasMany(p => p.Issues)
            .WithOne(i => i.Project)
            .HasForeignKey(i => i.ProjectId)
            .IsRequired();

        modelBuilder.Entity<User>()
            .HasOne(u => u.Project)
            .WithMany(p => p.Users)
            .HasForeignKey(u => u.ProjectId);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Author)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.SetNull);
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<Issue> Issues { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Comment> Comments { get; set; }
}