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
            return await db.Projects.Include(p => p.Issues).Include(p => p.Users).ToListAsync();
        });

        app.MapPost("/project", async (Project project, BugTrackerDbContext db) =>
        {
            await db.Projects.AddAsync(project);
            await db.SaveChangesAsync();
        });

        app.MapDelete("/project/{id}", async (int id, BugTrackerDbContext db) =>
        {
            // delete project
            // what about its' users?
            // users can be reassigned to another project, but in the mean time they have just no project at all (NULL)
            var project = await db.Projects.Include(p => p.Issues).FirstOrDefaultAsync(p => p.Id == id);

            if (project is null)
                return Results.NotFound($"No project with {id} was found.");

            db.Remove(project);

            var result = await db.SaveChangesAsync();

            return Results.Ok($"Project with id = {id} was deleted.\n{result} number of entities were deleted.");
        });
    }
}