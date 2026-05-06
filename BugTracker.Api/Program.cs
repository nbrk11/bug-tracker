using BugTracker.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/issues", async (IssueDbContext db) =>
{
    return await db.Issues.ToListAsync();
});

app.Run();
