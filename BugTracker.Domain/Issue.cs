using System.Text.Json.Serialization;

namespace BugTracker.Domain;

public enum IssueStatus
{
    NONE = -1,
    OPEN = 0,
    [JsonStringEnumMemberName("in-progress")]
    IN_PROGRESS = 1,
    DONE = 2
}

public enum IssuePriority
{
    NONE = -1,
    LOW = 0,
    MEDIUM = 1,
    HIGH = 2,
    BLOCKER = 3
}

public class Issue
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Project? Project { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset CreatedDate { get; set; }
    public IssueStatus Status { get; set; } = IssueStatus.OPEN;
    public IssuePriority Priority { get; set; } = IssuePriority.MEDIUM;
}
