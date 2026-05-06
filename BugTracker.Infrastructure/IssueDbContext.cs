using BugTracker.Domain;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Infrastructure;

public class IssueDbContext : DbContext
{
    public IssueDbContext(DbContextOptions<IssueDbContext> options) : base(options)
    {
    }

    public DbSet<Issue> Issues { get; set; }
}
