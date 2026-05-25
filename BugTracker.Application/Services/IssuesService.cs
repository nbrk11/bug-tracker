using BugTracker.Application.DTOs;
using BugTracker.Application.Interfaces;
using BugTracker.Application.Queries;
using BugTracker.Domain;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Application.Services;

public class IssuesService : IIssuesService
{
    private readonly IBugTrackerDbContext _db;

    public IssuesService(IBugTrackerDbContext db)
    {
        _db = db;
    }

    public async Task<ResponseWrapper<IssueDto>> CreateAsync(IssueDto issueDto)
    {
        var project = await _db.Projects.FindAsync(issueDto.ProjectId);
        var issue = new Issue
        {
            ProjectId = issueDto.ProjectId,
            Description = issueDto.Description,
            Priority = issueDto.Priority,
            Status = issueDto.Status,
            Project = project
        };

        await _db.Issues.AddAsync(issue);
        await _db.SaveChangesAsync();


        return ResponseWrapper<IssueDto>.Success(issueDto);
    }

    public async Task<ResponseWrapper<List<IssueDto>>> ReadAllAsync()
    {
        var issues = await _db.Issues
            .AsNoTracking()
            .Select(i => new IssueDto
            {
                ProjectId = i.ProjectId,
                Description = i.Description,
                Priority = i.Priority,
                Status = i.Status,
                CreatedDate = i.CreatedDate
            })
            .ToListAsync();

        return ResponseWrapper<List<IssueDto>>.Success(issues);
    }

    public async Task<ResponseWrapper<IssueDto>> ReadByIdAsync(int id)
    {
        var issue = await _db.Issues
            .AsNoTracking()
            .Where(i => i.Id == id)
            .Select(i => new IssueDto
            {
                ProjectId = i.ProjectId,
                Description = i.Description,
                Priority = i.Priority,
                Status = i.Status,
                CreatedDate = i.CreatedDate
            })
            .FirstOrDefaultAsync();

        if (issue is null)
            return ResponseWrapper<IssueDto>.Fail($"Issue with id {id} was not found.");

        return ResponseWrapper<IssueDto>.Success(issue);
    }

    public async Task<ResponseWrapper<List<IssueDto>>> ReadFilteredAsync(IssueFilterQuery filter)
    {
        var filteredIssues = _db.Issues.AsNoTracking();
        var result = new List<IssueDto>();

        if (filter.Status is not null)
            filteredIssues = filteredIssues.Where(i => i.Status == filter.Status);

        if (filter.Priority is not null)
            filteredIssues = filteredIssues.Where(i => i.Priority == filter.Priority);

        if (filter.DateFrom is not null)
            filteredIssues = filteredIssues.Where(i => i.CreatedDate > filter.DateFrom);

        if (filter.DateTo is not null)
            filteredIssues = filteredIssues.Where(i => filter.DateTo > i.CreatedDate);

        result = await filteredIssues
            .Select(i => new IssueDto
            {
                ProjectId = i.ProjectId,
                Description = i.Description,
                Priority = i.Priority,
                Status = i.Status,
                CreatedDate = i.CreatedDate
            })
            .ToListAsync();
        return ResponseWrapper<List<IssueDto>>.Success(result);
    }

    public Task<ResponseWrapper<IssueDto>> UpdateAsync(IssueDto projectDto, int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseWrapper<int>> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}