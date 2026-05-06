namespace BugTracker.Domain;

#nullable disable

public enum IssueStatus
{
    OPEN = 0,
    IN_PROGRESS = 1,
    DONE = 2
}

public enum IssuePriority
{
    LOW,
    MEDIUM,
    HIGH,
    BLOCKER
}

public class Issue
{
    public int Id { get; set;}
    public string Description { get; set; }
    public IssueStatus Status { get; set; } = IssueStatus.OPEN;
    public IssuePriority Priority { get; set; } = IssuePriority.MEDIUM;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}
