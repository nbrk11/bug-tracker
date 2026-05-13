using System.Text.Json.Serialization;

namespace BugTracker.Domain;

public class Comment
{
    public int Id { get; set;}
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    [JsonIgnore]
    public int AuthorId { get; set;}
    [JsonIgnore]
    public User Author { get; set;}
}