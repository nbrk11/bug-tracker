using System.Text.Json.Serialization;

namespace BugTracker.Domain;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set;} = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int? ProjectId { get; set; }
    public Project? Project { get; set; }
    public ICollection<Comment> Comments { get; set; } = [];
}