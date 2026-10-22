using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Domain.Entities
{
    public class Player
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ShirtNumber { get; set; }
    }
}
