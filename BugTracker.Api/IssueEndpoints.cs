using BugTracker.Domain;
using BugTracker.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Api;

public static class IssueEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/issues", async (BugTrackerDbContext db) =>
        {
            return await db.Issues.ToListAsync();
        });

        app.MapGet("/issues/filter", async (IssueFilterQuery filter, BugTrackerDbContext db) =>
        {
            if (filter is null)
                return await db.Issues.AsNoTracking().ToListAsync();

            var filteredIssues = db.Issues.AsNoTracking();

            if (filter.Status is not null)
                filteredIssues = filteredIssues.Where(i => i.Status == filter.Status);

            if (filter.Priority is not null)
                filteredIssues = filteredIssues.Where(i => i.Priority == filter.Priority);

            if (filter.DateFrom is not null)
                filteredIssues = filteredIssues.Where(i => i.CreatedDate > filter.DateFrom);

            if (filter.DateTo is not null)
                filteredIssues = filteredIssues.Where(i => filter.DateTo > i.CreatedDate);

            return await filteredIssues.ToListAsync();
        });

        app.MapGet("/issue/{id:int}", async (int id, BugTrackerDbContext db) =>
        {
            return await db.Issues
                .Select(i => i)
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
        });

        app.MapPost("/issue", async (Issue issue, BugTrackerDbContext db) =>
        {
            var project = await db.Projects.FindAsync(issue.ProjectId);
            issue.Project = project;
            await db.Issues.AddAsync(issue);
            await db.SaveChangesAsync();
        });
    }
}