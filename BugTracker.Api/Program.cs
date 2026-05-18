using BugTracker.Infrastructure;
using BugTracker.Api;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using BugTracker.Application.Interfaces;
using BugTracker.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<IProjectService, ProjectService>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.KebabCaseLower, false));
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/", () => "Hello World!");

IssueEndpoints.Map(app);
ProjectEndpoints.Map(app);
UserEndpoints.Map(app);
CommentEndpoints.Map(app);

app.Run();