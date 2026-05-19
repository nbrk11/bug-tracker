using BugTracker.Domain;

namespace BugTracker.Application.DTOs;

public class IssueDto
{
    public int ProjectId { get; set; }
    public string Description { get; set; } = string.Empty;
    public IssueStatus Status { get; set; }
    public IssuePriority Priority { get; set; }
    public DateTime CreatedDate { get; init; }
}