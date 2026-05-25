namespace BugTracker.Application.DTOs;

public class CommentDto
{
    public int? AuthorId { get; set; }
    public int IssueId { get; set; }
    public string Content { get; set; } = string.Empty;
}