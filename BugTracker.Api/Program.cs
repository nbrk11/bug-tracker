using BugTracker.Infrastructure;
using BugTracker.Api;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.KebabCaseLower, false));
});

builder.Services.AddOpenApi();

// builder.Services.AddOpenApiDocument(config =>
// {
//     config.DocumentName = "BugTracker";
//     config.Title = "BugTracker API";
//     config.Version = "v1";
// });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // app.UseOpenApi();
    // app.UseSwaggerUi();
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/", () => "Hello World!");

IssueEndpoints.Map(app);
ProjectEndpoints.Map(app);

app.Run();