using BugTracker.Application.Interfaces;
using BugTracker.Domain;
using BugTracker.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Api;

public static class ProjectsEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGroup("/projects")
        .MapProjects()
        .WithTags("Projects");
    }

    public static RouteGroupBuilder MapProjects(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllProjects);
        group.MapGet("/{id}", GetProjectById);
        group.MapPost("/", CreateProject);
        group.MapDelete("/{id}", DeleteProjectById);

        return group;
    }

    private static async Task<IResult> GetAllProjects(BugTrackerDbContext db)
    {
        var projects = await db.Projects.Include(p => p.Issues).Include(p => p.Users).ToListAsync();

        return Results.Ok(projects);
    }

    private static async Task<IResult> GetProjectById(int id, BugTrackerDbContext db)
    {
        var project = await db.Projects.FirstOrDefaultAsync(p => p.Id == id);
        if (project is null)
            return Results.NotFound(null);

        return Results.Ok(project);
    }

    private static async Task<IResult> CreateProject(Project project, BugTrackerDbContext db)
    {
        await db.Projects.AddAsync(project);
        await db.SaveChangesAsync();

        return Results.Created();
    }

    private static async Task<IResult> DeleteProjectById(int id, IProjectsService projectService)
    {
        // delete project
        // what about its' users?
        // users can be reassigned to another project, but in the mean time they have just no project at all (NULL)
        var result = await projectService.DeleteAsync(id);

        if (!result.IsSuccess)
            return Results.NotFound(result);

        return Results.Ok($"Project with id = {id} was deleted.\n{result.Value} number of entities were deleted.");
    }
}