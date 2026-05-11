using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Application.DTO.Features.Posts
{
    public class PagedResult<T>
    {
        public required IReadOnlyList<T> Items { get; init; }
        public required int PageNumber { get; init; }
        public required int PageSize
        {
            get;
            init;
        }
        public required int TotalCount { get; init; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber * PageSize < TotalCount;
    }
}
