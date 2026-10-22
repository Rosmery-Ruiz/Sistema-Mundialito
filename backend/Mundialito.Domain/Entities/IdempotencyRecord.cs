using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Domain.Entities
{
    public class IdempotencyRecord
    {
        public int Id { get; set; }

        public string Key { get; set; } = string.Empty;

        public string ResponseBody { get; set; } = string.Empty;

        public int StatusCode { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}