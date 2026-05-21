using BugTracker.Application.DTOs;
using BugTracker.Application.Queries;

namespace BugTracker.Application.Interfaces;

public interface ICommentsService
{
    public Task<ResponseWrapper<CommentDto>> CreateAsync(CommentDto issueDto);
    public Task<ResponseWrapper<List<CommentDto>>> ReadAllAsync();
    public Task<ResponseWrapper<CommentDto>> ReadByIdAsync(int id);
    public Task<ResponseWrapper<List<CommentDto>>> ReadFilteredAsync(CommentFilterQuery filter);
    public Task<ResponseWrapper<CommentDto>> UpdateAsync(CommentDto issueDto, int id);
    public Task<ResponseWrapper<int>> DeleteAsync(int id);
}