using Microsoft.AspNetCore.Http;

namespace BugTracker.Application.Queries;

public class CommentFilterQuery
{
    public int? AuthorId { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }

    public static ValueTask<CommentFilterQuery?> BindAsync(HttpContext httpContext)
    {
        var query = httpContext.Request.Query;

        var filter = new CommentFilterQuery
        {
            AuthorId = int.TryParse(query["authorid"], out var authorId)
                    ? authorId : null,
            DateFrom = DateTime.TryParse(query["datefrom"], out var dateFrom)
                    ? dateFrom : null,
            DateTo = DateTime.TryParse(query["dateto"], out var dateTo)
                    ? dateTo : null,
        };

        return ValueTask.FromResult<CommentFilterQuery?>(filter);
    }
}