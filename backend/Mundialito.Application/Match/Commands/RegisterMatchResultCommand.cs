using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Application.Match.Commands
{
    public class RegisterMatchResultCommand
    {
        public int MatchId { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }

        public List<int> HomeScorerIds { get; set; } = [];
        public List<int> AwayScorerIds { get; set; } = [];
    }
}