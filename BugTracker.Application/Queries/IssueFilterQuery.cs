using BugTracker.Domain;
using Microsoft.AspNetCore.Http;

namespace BugTracker.Application.Queries;

public class IssueFilterQuery
{
    public IssuePriority? Priority { get; set; }
    public IssueStatus? Status { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }

    public static ValueTask<IssueFilterQuery?> BindAsync(HttpContext httpContext)
    {
        var query = httpContext.Request.Query;

        var filter = new IssueFilterQuery
        {
            Priority = Enum.TryParse<IssuePriority>(query["priority"], ignoreCase: true, out var priority)
                    ? priority : null,
            Status = Enum.TryParse<IssueStatus>(query["status"], ignoreCase: true, out var status)
                    ? status : null,
            DateFrom = DateTime.TryParse(query["datefrom"], out var dateFrom)
                    ? dateFrom : null,
            DateTo = DateTime.TryParse(query["dateto"], out var dateTo)
                    ? dateTo : null,
        };

        return ValueTask.FromResult<IssueFilterQuery?>(filter);
    }
}