using BugTracker.Application.Interfaces;
using BugTracker.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BugTracker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IIssuesService, IssuesService>();
        services.AddScoped<IProjectsService, ProjectsService>();
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<ICommentsService, CommentsService>();

        return services;
    }
}