using BugTracker.Domain;
using BugTracker.Application;
using BugTracker.Application.DTOs;
using BugTracker.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Infrastructure.Services;

public class UsersService : IUsersService
{
    private readonly BugTrackerDbContext _db;


    public UsersService(BugTrackerDbContext db)
    {
        _db = db;
    }
    public async Task<ResponseWrapper<UserDto>> CreateAsync(UserDto userDto)
    {
        var user = new User
        {
            FirstName = userDto.FirstName!,
            LastName = userDto.LastName!,
            Email = userDto.Email!,
            ProjectId = userDto.ProjectId
        };

        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();

        return ResponseWrapper<UserDto>.Success(userDto);
    }

    public async Task<ResponseWrapper<int>> DeleteAsync(int id)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
            return ResponseWrapper<int>.Fail($"User with id {id} was not found.");

        _db.Users.Remove(user);
        var result = await _db.SaveChangesAsync();

        return ResponseWrapper<int>.Success(result);
    }

    public async Task<ResponseWrapper<List<UserDto>>> ReadAllAsync()
    {
        var users = await _db.Users
            .AsNoTracking()
            .Select(u => new UserDto
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                ProjectId = u.ProjectId
            })
            .ToListAsync();

        return ResponseWrapper<List<UserDto>>.Success(users);
    }

    public async Task<ResponseWrapper<UserDto>> ReadByIdAsync(int id)
    {
        var user = await _db.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => new UserDto
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                ProjectId = u.ProjectId
            })
            .FirstOrDefaultAsync();

        if (user is null)
            return ResponseWrapper<UserDto>.Fail($"User with id {id} was not found.");

        return ResponseWrapper<UserDto>.Success(user);
    }

    public async Task<ResponseWrapper<UserDto>> UpdateAsync(UserDto userPatch, int id)
    {
        var user = await _db.Users.Include(u => u.Comments).FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
            return ResponseWrapper<UserDto>.Fail($"No user with {id} was found.");

        if (userPatch.FirstName is not null)
            user.FirstName = userPatch.FirstName;
        if (userPatch.LastName is not null)
            user.LastName = userPatch.LastName;
        if (userPatch.Email is not null)
            user.Email = userPatch.Email;
        if (userPatch.ProjectId is not null)
        {
            var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == userPatch.ProjectId);

            if (project is null)
                return ResponseWrapper<UserDto>.Fail($"No project with {userPatch.ProjectId} was found.");

            user.ProjectId = userPatch.ProjectId;
            user.Project = project;
            project.Users.Add(user);
        }

        await _db.SaveChangesAsync();

        return ResponseWrapper<UserDto>.Success(userPatch);
    }
}