using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Application.Player.Queries
{
    public class GetPlayersQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public int? TeamId { get; set; }
        public string? Name { get; set; }
        public string SortBy { get; set; } = "Id";
        public string SortDirection { get; set; } = "asc";
    }
}