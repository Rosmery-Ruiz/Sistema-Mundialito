using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Application.Player.Commands
{
    public class CreatePlayerCommand
    {
        public int TeamId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ShirtNumber { get; set; }
    }
}
