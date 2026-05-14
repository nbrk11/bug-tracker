using BugTracker.Api.DTOs;
using BugTracker.Domain;
using BugTracker.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Api;

public static class UserEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/users", async (BugTrackerDbContext db) =>
        {
            return await db.Users.AsNoTracking().Include(u => u.Comments).ToListAsync();
        });

        app.MapPost("/user", async (User user, BugTrackerDbContext db) =>
        {
            await db.Users.AddAsync(user);    
            await db.SaveChangesAsync();
        });

        app.MapDelete("/user/{id}", async (int id, BugTrackerDbContext db) =>
        {
            var user = await db.Users.Include(u => u.Comments).FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
                return Results.NotFound($"No user with {id} was found.");

            db.Remove(user);

            var result = await db.SaveChangesAsync();

            return Results.Ok($"User with id = {id} was deleted.\n{result} number of entities were deleted.");
        });

        app.MapPatch("/user/{id}", async (int id, UserDto userDto, BugTrackerDbContext db) =>
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
        });
    }
}