using BugTracker.Application.DTOs;
using BugTracker.Application.Interfaces;

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

    private static async Task<IResult> GetAllProjects(IProjectsService projectsService)
    {
        var response = await projectsService.ReadAllAsync(); 

        return Results.Ok(response.Value);
    }

    private static async Task<IResult> GetProjectById(int id, IProjectsService projectsService)
    {
        var response = await projectsService.ReadByIdAsync(id);

        if (!response.IsSuccess)
            return Results.NotFound(response.Error);

        return Results.Ok(response.Value);
    }

    private static async Task<IResult> CreateProject(ProjectDto project, IProjectsService projectsService)
    {
        var response = await projectsService.CreateAsync(project);

        if (!response.IsSuccess)
            return Results.InternalServerError(response.Error);

        return Results.Created();
    }

    private static async Task<IResult> DeleteProjectById(int id, IProjectsService projectService)
    {
        // delete project
        // what about its' users?
        // users can be reassigned to another project, but in the mean time they have just no project at all (NULL)
        var response = await projectService.DeleteAsync(id);

        if (!response.IsSuccess)
            return Results.NotFound(response);

        return Results.Ok($"Project with id = {id} was deleted.\n{response.Value} number of entities were deleted.");
    }
}