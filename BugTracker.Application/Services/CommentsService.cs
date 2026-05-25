using BugTracker.Application.DTOs;
using BugTracker.Application.Queries;
using BugTracker.Application.Interfaces;
using BugTracker.Domain;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Application.Services;

public class CommentsService : ICommentsService
{
    private readonly IBugTrackerDbContext _db;

    public CommentsService(IBugTrackerDbContext db)
    {
        _db = db;
    }

    public async Task<ResponseWrapper<CommentDto>> CreateAsync(CommentDto commentDto)
    {
        var comment = new Comment
        {
            AuthorId = commentDto.AuthorId,
            Content = commentDto.Content,
        };

        await _db.Comments.AddAsync(comment);
        await _db.SaveChangesAsync();

        return ResponseWrapper<CommentDto>.Success(commentDto);
    }

    public async Task<ResponseWrapper<int>> DeleteAsync(int id)
    {
        var comment = await _db.Comments.FirstOrDefaultAsync(c => c.Id == id);

        if (comment is null)
            return ResponseWrapper<int>.Fail($"Comment with id {id} is not found");

        _db.Comments.Remove(comment);

        var result = await _db.SaveChangesAsync();

        return ResponseWrapper<int>.Success(result);
    }

    public async Task<ResponseWrapper<List<CommentDto>>> ReadAllAsync()
    {
        var comments = await _db.Comments
            .AsNoTracking()
            .Select(c => new CommentDto
            {
                AuthorId = c.AuthorId,
                Content = c.Content,
            })
            .ToListAsync();

        return ResponseWrapper<List<CommentDto>>.Success(comments);
    }

    public async Task<ResponseWrapper<CommentDto>> ReadByIdAsync(int id)
    {
        var comment = await _db.Comments
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CommentDto
            {
                AuthorId = c.AuthorId,
                Content = c.Content,
            })
            .FirstOrDefaultAsync();

        if (comment is null)
            return ResponseWrapper<CommentDto>.Fail($"Comment with id {id} is not found");

        return ResponseWrapper<CommentDto>.Success(comment);
    }

    public async Task<ResponseWrapper<List<CommentDto>>> ReadFilteredAsync(CommentFilterQuery filter)
    {
        var comments = _db.Comments.AsNoTracking();
        var result = new List<CommentDto>();

        if (filter is null)
            goto returnResult;

        if (filter.AuthorId is not null)
            comments = comments.Where(c => c.AuthorId == filter.AuthorId);

        if (filter.DateFrom is not null)
            comments = comments.Where(c => c.CreatedDate >= filter.DateFrom);

        if (filter.DateTo is not null)
            comments = comments.Where(c => filter.DateTo >= c.CreatedDate);

        returnResult:
        var res = await comments
            .Select(c => new CommentDto
            {
                Content = c.Content,
                AuthorId = (int)c.AuthorId!
            })
            .ToListAsync();

        return ResponseWrapper<List<CommentDto>>.Success(res);
    }

    public async Task<ResponseWrapper<CommentDto>> UpdateAsync(CommentDto commentPatch, int id)
    {
        var comment = await _db.Comments.FirstOrDefaultAsync(c => c.Id == id);

        if (comment is null)
            return ResponseWrapper<CommentDto>.Fail($"Comment with id {id} not found.");

        if (commentPatch.Content is not null)
            comment.Content = commentPatch.Content;
        if (commentPatch.AuthorId != 0)
            comment.AuthorId = commentPatch.AuthorId;

        await _db.SaveChangesAsync();

        return ResponseWrapper<CommentDto>.Success(commentPatch);
    }
}