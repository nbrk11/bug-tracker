using BugTracker.Application.DTOs;

namespace BugTracker.Application.Interfaces;

public interface IProjectsService
{
    public Task<ResponseWrapper<ProjectDto>> CreateAsync(ProjectDto projectDto);
    public Task<ResponseWrapper<List<ProjectDto>>> ReadAllAsync();
    public Task<ResponseWrapper<ProjectDto>> ReadByIdAsync(int id);
    public Task<ResponseWrapper<ProjectDto>> UpdateAsync(ProjectDto projectPatch, int id);
    public Task<ResponseWrapper<int>> DeleteAsync(int id);
}