using BugTracker.Application.DTOs;
using BugTracker.Application.Interfaces;

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

    private static async Task<IResult> GetAllUsers(IUsersService userService)
    {
        var response = await userService.ReadAllAsync();

        if (!response.IsSuccess)
            return Results.NotFound(response.Error);

        return Results.Ok(response.Value);
    }

    private static async Task<IResult> GetUserById(int id, IUsersService userService)
    {
        var response = await userService.ReadByIdAsync(id);

        if (!response.IsSuccess)
            return Results.NotFound(response.Error);

        return Results.Ok(response.Value);
    }

    private static async Task<IResult> CreateUser(UserDto userDto, IUsersService userService)
    {
        var response = await userService.CreateAsync(userDto);

        if (!response.IsSuccess)
            return Results.NotFound(response.Error);

        return Results.Created();
    }

    private static async Task<IResult> DeleteUserById(int id, IUsersService userService)
    {
        var response = await userService.DeleteAsync(id);

        if (!response.IsSuccess)
            return Results.NotFound(response.Error);

        return Results.Ok($"User with id {id} was deleted.\n{response.Value} number of rows were affected.\n");
    }

    private static async Task<IResult> UpdateUserById(int id, UserDto userDto, IUsersService userService)
    {
        var response = await userService.UpdateAsync(userDto, id);

        if (!response.IsSuccess)
            return Results.NotFound(response.Error);

        return Results.NoContent();
    }
}