using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Application.Match.Queries
{
    public class GetMatchesQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public DateTime? MatchDate { get; set; }
        public int? TeamId { get; set; }
        public string? Status { get; set; }
        public string SortBy { get; set; } = "MatchDate";
        public string SortDirection { get; set; } = "asc";
    }
}