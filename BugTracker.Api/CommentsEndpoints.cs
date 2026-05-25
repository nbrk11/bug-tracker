using BugTracker.Application.DTOs;
using BugTracker.Application.Interfaces;
using BugTracker.Application.Queries;

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

    private static async Task<IResult> GetAllComments(ICommentsService commentsService)
    {
        var response = await commentsService.ReadAllAsync();

        if (!response.IsSuccess)
            return Results.InternalServerError(response.Error);

        return Results.Ok(response.Value);
    }

    private static async Task<IResult> GetCommentById(int id, ICommentsService commentsService)
    {
        var response = await commentsService.ReadByIdAsync(id);

        if (!response.IsSuccess)
            return Results.InternalServerError(response.Error);

        return Results.Ok(response.Value);
    }

    private static async Task<IResult> GetFilteredComments(CommentFilterQuery filter, ICommentsService commentsService)
    {
        var response = await commentsService.ReadFilteredAsync(filter);

        if (!response.IsSuccess)
            return Results.InternalServerError(response.Error);

        return Results.Ok(response.Value);

    }

    private static async Task<IResult> CreateComment(CommentDto commentDto, ICommentsService commentsService)
    {
        var response = await commentsService.CreateAsync(commentDto);

        if (!response.IsSuccess)
            return Results.InternalServerError(response.Error);

        return Results.Ok(response.Value);
    }

    private static async Task<IResult> DeleteCommentById(int id, ICommentsService commentsService)
    {
        var response = await commentsService.DeleteAsync(id);

        if (!response.IsSuccess)
            return Results.InternalServerError(response.Error);

        return Results.Ok(response.Value);
    }

    private static async Task<IResult> UpdateCommentById(int id, CommentDto commentDto, ICommentsService commentsService)
    {
        var response = await commentsService.UpdateAsync(commentDto, id);

        if (!response.IsSuccess)
            return Results.InternalServerError(response.Error);

        return Results.Ok(response.Value);
    }
}