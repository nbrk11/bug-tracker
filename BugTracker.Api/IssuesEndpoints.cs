using BugTracker.Domain;
using BugTracker.Infrastructure;
using Microsoft.EntityFrameworkCore;

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

    private static async Task<IResult> GetAllIssues(BugTrackerDbContext db)
    {
        var issues = await db.Issues.AsNoTracking().ToListAsync();
        return Results.Ok(issues);
    }

    private static async Task<IResult> GetIssueById(int id, BugTrackerDbContext db)
    {
        var issue = await db.Issues
                .FirstOrDefaultAsync(i => i.Id == id);
        
        if (issue is null)
            return Results.NotFound($"Issue with id {id} was not found.");

        return Results.Ok(issue);
    }

    private static async Task<IResult> GetFilteredIssues(IssueFilterQuery filter, BugTrackerDbContext db)
    {
        var filteredIssues = db.Issues.AsNoTracking();
        var result = new List<Issue>();

        if (filter is null)
            goto returnResult;


        if (filter.Status is not null)
            filteredIssues = filteredIssues.Where(i => i.Status == filter.Status);

        if (filter.Priority is not null)
            filteredIssues = filteredIssues.Where(i => i.Priority == filter.Priority);

        if (filter.DateFrom is not null)
            filteredIssues = filteredIssues.Where(i => i.CreatedDate > filter.DateFrom);

        if (filter.DateTo is not null)
            filteredIssues = filteredIssues.Where(i => filter.DateTo > i.CreatedDate);

    returnResult:
        result = await filteredIssues.ToListAsync();
        return Results.Ok(result);
    }

    private static async Task<IResult> CreateIssue(Issue issue, BugTrackerDbContext db)
    {
        var project = await db.Projects.FindAsync(issue.ProjectId);
        issue.Project = project;
        await db.Issues.AddAsync(issue);
        await db.SaveChangesAsync();

        return Results.Created();
    }
}