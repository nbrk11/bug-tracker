using BugTracker.Application.DTOs;

namespace BugTracker.Application.Interfaces;

public interface IProjectsService
{
    public Task CreateAsync(ProjectDto projectDto);
    public Task ReadAllAsync();
    public Task ReadByIdAsync(int id);
    public Task UpdateAsync(ProjectDto projectDto, int id);
    public Task<ResponseWrapper<int>> DeleteAsync(int id);
}