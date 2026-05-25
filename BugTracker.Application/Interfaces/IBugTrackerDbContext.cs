using BugTracker.Domain;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Application.Interfaces;

public interface IBugTrackerDbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<Issue> Issues { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public Task<int> SaveChangesAsync(CancellationToken token = default);
}