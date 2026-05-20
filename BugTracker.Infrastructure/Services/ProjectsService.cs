using BugTracker.Application.Interfaces;
using BugTracker.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using BugTracker.Application;
using BugTracker.Domain;

namespace BugTracker.Infrastructure.Services;

public class ProjectsService : IProjectsService
{
    private BugTrackerDbContext _db;

    public ProjectsService(BugTrackerDbContext db)
    {
        _db = db;
    }

    public async Task<ResponseWrapper<ProjectDto>> CreateAsync(ProjectDto projectDto)
    {
        var project = new Project
        {
            Title = projectDto.Title,   
        };

        await _db.Projects.AddAsync(project);
        await _db.SaveChangesAsync();

        return ResponseWrapper<ProjectDto>.Success(projectDto);
    }


    public async Task<ResponseWrapper<List<ProjectDto>>> ReadAllAsync()
    {
        var projects = await _db.Projects
            .AsNoTracking()
            .Select(p => new ProjectDto
            {
                Title = p.Title,
                CreatedDate = p.CreatedDate,
            })
            .ToListAsync();

        return ResponseWrapper<List<ProjectDto>>.Success(projects);
    }

    public async Task<ResponseWrapper<ProjectDto>> ReadByIdAsync(int id)
    {
        var project = await _db.Projects
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProjectDto
            {
                Title = p.Title,
                CreatedDate = p.CreatedDate,
            })
            .FirstOrDefaultAsync();

        if (project is null)
            return ResponseWrapper<ProjectDto>.Fail($"Project with id {id} was not found");

        return ResponseWrapper<ProjectDto>.Success(project);
    }

    public async Task<ResponseWrapper<ProjectDto>> UpdateAsync(ProjectDto projectPatch, int id)
    {
        var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == id);

        if (project is null)
            return ResponseWrapper<ProjectDto>.Fail($"Project with id {id} was not found");

        if (projectPatch.Title != string.Empty)
            project.Title = projectPatch.Title;

        await _db.SaveChangesAsync();

        return ResponseWrapper<ProjectDto>.Success(projectPatch);
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
}