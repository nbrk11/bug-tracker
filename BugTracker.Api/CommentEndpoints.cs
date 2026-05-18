using BugTracker.Domain;
using BugTracker.Infrastructure;
using Microsoft.EntityFrameworkCore;
using BugTracker.Application.DTOs;

namespace BugTracker.Api;

public static class CommentEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/comments", async (BugTrackerDbContext db) =>
        {
            var comments = await db.Comments.Select(c => c).ToListAsync();

            return Results.Ok(comments);
        }).WithTags("Comments");

        app.MapGet("/comment/{id}", async (int id, BugTrackerDbContext db) => {
            var comment = await db.Comments.FirstOrDefaultAsync(c => c.Id == id);

            if (comment is null)
                return Results.NotFound($"Comment with id {id} is not found");

            return Results.Ok(comment);
        })
        .WithName("GetCommentById")
        .WithDescription("Get a comment by it's id")
        .WithTags("Comments")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesValidationProblem();

        app.MapGet("/comments/filter", async (CommentFilterQuery filter, BugTrackerDbContext db) =>
        {
            var comments = db.Comments.AsNoTracking();

            if (filter is null)
                return Results.Ok(await comments.ToListAsync());

            if (filter.AuthorId is not null)
                comments = comments.Where(c => c.AuthorId == filter.AuthorId);

            if (filter.DateFrom is not null) 
                comments = comments.Where(c => c.CreatedDate >= filter.DateFrom);

            if (filter.DateTo is not null)
                comments = comments.Where(c => filter.DateTo >= filter.DateTo);

            var res = await comments.ToArrayAsync();

            return Results.Ok(res);
        })
        .WithTags("Comments")
        .Produces(StatusCodes.Status200OK);

        app.MapPost("/comment", async (CommentDto c, BugTrackerDbContext db) =>
        {
            var comment = new Comment
            {
                Content = c.Content,
            };

            await db.Comments.AddAsync(comment);  
            await db.SaveChangesAsync();

            return Results.Created();
        }).WithTags("Comments");

        app.MapDelete("/comment/{id}", async (int id, BugTrackerDbContext db) => {
            var comment = await db.Comments.FirstOrDefaultAsync(c => c.Id == id);

            if (comment is null)
                return Results.NotFound($"Comment with id {id} is not found");

            db.Comments.Remove(comment);

            await db.SaveChangesAsync();

            return Results.Ok();
        }).WithTags("Comments");

        app.MapPatch("/comment/{id}", async (int id, CommentDto commentDto, BugTrackerDbContext db) =>
        {
            var comment = await db.Comments.FirstOrDefaultAsync(c => c.Id == id);    

            if (comment is null) 
                return Results.NotFound($"Comment with id {id} not found.");

            if (commentDto.Content is not null)
                comment.Content = commentDto.Content;

            await db.SaveChangesAsync();

            return Results.Ok();
        })
        .WithTags("Comments")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}