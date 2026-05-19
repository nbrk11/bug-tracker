using BugTracker.Application.DTOs;
using BugTracker.Application.Queries;

namespace BugTracker.Application.Interfaces;

public interface IIssuesService
{
    public Task<ResponseWrapper<IssueDto>> CreateAsync(IssueDto issueDto);
    public Task<ResponseWrapper<List<IssueDto>>> ReadAllAsync();
    public Task<ResponseWrapper<IssueDto>> ReadByIdAsync(int id);
    public Task<ResponseWrapper<List<IssueDto>>> ReadFilteredAsync(IssueFilterQuery filter);
    public Task<ResponseWrapper<IssueDto>> UpdateAsync(IssueDto issueDto, int id);
    public Task<ResponseWrapper<int>> DeleteAsync(int id);
}