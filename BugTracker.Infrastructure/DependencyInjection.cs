using BugTracker.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IssueDbContext>(opt => 
            opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                //.UseSnakeCaseNamingConvention()
            );

        return services;
    }
}