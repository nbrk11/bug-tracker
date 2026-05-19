using BugTracker.Application.DTOs;
using BugTracker.Domain;
using BugTracker.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Api;

public static class UsersEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGroup("/users")
        .MapUsers()
        .WithTags("Users");
    }

    public static RouteGroupBuilder MapUsers(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllUsers);        
        group.MapGet("/{id}", GetUserById);
        group.MapPost("/", CreateUser);
        group.MapDelete("/{id}", DeleteUserById);
        group.MapPatch("/{id}", UpdateUserById);

        return group;
    }

    private static async Task<IResult> GetAllUsers(BugTrackerDbContext db)
    {
        var users = await db.Users.AsNoTracking().Include(u => u.Comments).ToListAsync();
        return Results.Ok(users);
    }

    private static async Task<IResult> GetUserById(int id, BugTrackerDbContext db)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null)
            return Results.NotFound();

        return Results.Ok(user);
    }

    private static async Task<IResult> CreateUser(User user, BugTrackerDbContext db)
    {
        await db.Users.AddAsync(user);
        await db.SaveChangesAsync();

        return Results.Created();
    }

    private static async Task<IResult> DeleteUserById(int id, BugTrackerDbContext db)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
            return Results.NotFound();

        db.Users.Remove(user);
        var result = await db.SaveChangesAsync();

        return Results.Ok($"User with id {id} was deleted.\n{result} number of rows were affected.\n");
    }

    private static async Task<IResult> UpdateUserById(int id, UserDto userDto, BugTrackerDbContext db)
    {
        var user = await db.Users.Include(u => u.Comments).FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
            return Results.NotFound($"No user with {id} was found.");

        if (userDto.FirstName is not null)
            user.FirstName = userDto.FirstName;
        if (userDto.LastName is not null)
            user.LastName = userDto.LastName;
        if (userDto.Email is not null)
            user.Email = userDto.Email;
        if (userDto.ProjectId is not null)
        {
            var project = await db.Projects.FirstOrDefaultAsync(p => p.Id == userDto.ProjectId);

            if (project is null)
                return Results.NotFound($"No project with {userDto.ProjectId} was found.");

            user.ProjectId = userDto.ProjectId;
            user.Project = project;
            project.Users.Add(user);
        }

        await db.SaveChangesAsync();

        return Results.NoContent();
    }
}