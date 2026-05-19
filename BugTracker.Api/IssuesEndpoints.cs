using BugTracker.Application.DTOs;
using BugTracker.Application.Interfaces;
using BugTracker.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Api;

public static class IssuesEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGroup("/issues")
        .MapIssues()
        .WithTags("Issues");
    }

    public static RouteGroupBuilder MapIssues(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllIssues);
        group.MapGet("/{id}", GetIssueById);
        group.MapGet("/filter", GetFilteredIssues);
        group.MapPost("/", CreateIssue);
        
        return group;
    }

    private static async Task<IResult> GetAllIssues(IIssuesService issuesService)
    {
        var issues = await issuesService.ReadAllAsync(); 
        // add check for failure

        return Results.Ok(issues.Value);
    }

    private static async Task<IResult> GetIssueById(int id, IIssuesService issuesService)
    {
        var issue = await issuesService.ReadByIdAsync(id);
        
        if (!issue.IsSuccess)
            return Results.NotFound(issue.Error);

        return Results.Ok(issue.Value);
    }

    private static async Task<IResult> GetFilteredIssues(IssueFilterQuery filter, IIssuesService issuesService)
    {
        var issues = await issuesService.ReadFilteredAsync(filter);
        // add check for failure

        return Results.Ok(issues.Value);
    }

    private static async Task<IResult> CreateIssue(IssueDto issue, IIssuesService issuesService)
    {
        var result = await issuesService.CreateAsync(issue);

        if (!result.IsSuccess)
            return Results.InternalServerError(result.Error);

        return Results.Created();
    }
}