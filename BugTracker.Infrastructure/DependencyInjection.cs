using BugTracker.Application.Interfaces;
using BugTracker.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BugTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BugTrackerDbContext>(opt =>
            opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IIssuesService, IssuesService>();
        services.AddScoped<IProjectsService, ProjectsService>();
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<ICommentsService, CommentsService>();

        return services;
    }
}