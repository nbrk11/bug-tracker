namespace BugTracker.Application.DTOs;

public class ProjectDto
{
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset CreatedDate { get; set; }
}