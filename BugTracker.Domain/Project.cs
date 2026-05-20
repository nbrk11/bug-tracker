namespace BugTracker.Domain;

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset CreatedDate { get; set; }
    public ICollection<Issue> Issues { get; set; } = [];
    public ICollection<User> Users { get; set; } = [];
}