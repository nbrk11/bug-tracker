using System.Text.Json.Serialization;

namespace BugTracker.Domain;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    // In the future the user's timezone info will be needed to properly convert it to local time
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public int? AuthorId { get; set; }
    public User? Author { get; set; }
    // TODO: Add relation to the Issues
}