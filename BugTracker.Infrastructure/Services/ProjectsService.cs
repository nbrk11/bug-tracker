using BugTracker.Application.Interfaces;
using BugTracker.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using BugTracker.Application;

namespace BugTracker.Infrastructure.Services;

public class ProjectsService : IProjectsService
{
    private BugTrackerDbContext _db;

    public ProjectsService(BugTrackerDbContext db)
    {
        _db = db;
    }

    public async Task CreateAsync(ProjectDto projectDto)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseWrapper<int>> DeleteAsync(int id)
    {
        var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == id);

        if (project is null)
        {
            return ResponseWrapper<int>.Fail($"Project with {id} id was not found");
        }

        _db.Projects.Remove(project);
        var result = await _db.SaveChangesAsync();

        return ResponseWrapper<int>.Success(result);
    }

    public Task ReadAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task ReadByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(ProjectDto projectDto, int id)
    {
        throw new NotImplementedException();
    }
}