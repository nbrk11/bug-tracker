using BugTracker.Domain;
using BugTracker.Infrastructure;
using Microsoft.EntityFrameworkCore;
using BugTracker.Application.DTOs;

namespace BugTracker.Api;

public static class CommentsEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGroup("/comments")
        .MapComments()
        .WithTags("Comments");
    }

    public static RouteGroupBuilder MapComments(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllComments);
        group.MapGet("/{id}", GetCommentById);
        group.MapGet("/filter", GetFilteredComments);
        group.MapPost("/", CreateComment);
        group.MapDelete("/{id}", DeleteCommentById);
        group.MapPatch("/{id}", UpdateCommentById);

        return group;
    }

    private static async Task<IResult> GetAllComments(BugTrackerDbContext db)
    {
        var comments = await db.Comments.Select(c => c).ToListAsync();

        return Results.Ok(comments);
    }

    private static async Task<IResult> GetCommentById(int id, BugTrackerDbContext db)
    {
        var comment = await db.Comments.FirstOrDefaultAsync(c => c.Id == id);

        if (comment is null)
            return Results.NotFound($"Comment with id {id} is not found");

        return Results.Ok(comment);
    }

    private static async Task<IResult> GetFilteredComments(CommentFilterQuery filter, BugTrackerDbContext db)
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
    }

    private static async Task<IResult> CreateComment(CommentDto c, BugTrackerDbContext db)
    {
        var comment = new Comment
        {
            Content = c.Content,
        };

        await db.Comments.AddAsync(comment);
        await db.SaveChangesAsync();

        return Results.Created();
    }

    private static async Task<IResult> DeleteCommentById(int id, BugTrackerDbContext db)
    {
        var comment = await db.Comments.FirstOrDefaultAsync(c => c.Id == id);

        if (comment is null)
            return Results.NotFound($"Comment with id {id} is not found");

        db.Comments.Remove(comment);

        await db.SaveChangesAsync();

        return Results.Ok();
    }

    private static async Task<IResult> UpdateCommentById(int id, CommentDto commentDto, BugTrackerDbContext db)
    {
        var comment = await db.Comments.FirstOrDefaultAsync(c => c.Id == id);

        if (comment is null)
            return Results.NotFound($"Comment with id {id} not found.");

        if (commentDto.Content is not null)
            comment.Content = commentDto.Content;

        await db.SaveChangesAsync();

        return Results.Ok();
    }
}