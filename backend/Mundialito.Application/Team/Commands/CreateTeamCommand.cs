using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Application.Team.Command
{
    public class CreateTeamCommand
    {
        public string Name { get; set; } = string.Empty;
        public string IdempotencyKey { get; set; } = string.Empty;
    }
}
