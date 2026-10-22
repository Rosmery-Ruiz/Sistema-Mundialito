using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Application.Scorers.Dtos
{
    public class ScorerDto
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; } = string.Empty;

        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;

        public int Goals { get; set; }
    }
}
