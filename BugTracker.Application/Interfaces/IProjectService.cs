using BugTracker.Application.DTOs;
using ErrorOr;

namespace BugTracker.Application.Interfaces;

public interface IProjectService
{
    public Task Create(ProjectDto projectDto);
    public Task ReadAll();
    public Task ReadById(int id);
    public Task Update(ProjectDto projectDto, int id);
    public Task<ResponseWrapper<int>> Delete(int id);
}