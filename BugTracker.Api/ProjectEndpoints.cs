using BugTracker.Domain;
using BugTracker.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Api;

public static class ProjectEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/projects", async (BugTrackerDbContext db) =>
        {
            return await db.Projects.Include(p => p.Issues).ToListAsync();
        });

        app.MapPost("/project", async (Project project, BugTrackerDbContext db) =>
        {
            await db.Projects.AddAsync(project);
            await db.SaveChangesAsync();
        });
    }
}