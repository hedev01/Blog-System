using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Application.DTO
{
    public record ListPostsQuery(
        int PageNumber = 1,
        int PageSize = 10,
        int AuthorId = 1,
        string SortOrder = "desc" // "asc" | "desc"
    );
}
