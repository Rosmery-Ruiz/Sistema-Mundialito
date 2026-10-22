using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Application.Scorers.Queries
{
    public class GetScorersQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string SortBy { get; set; } = "Goals";
        public string SortDirection { get; set; } = "desc";
    }
}