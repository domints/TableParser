using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TableParser
{
    public class Primary
    {
        public DateTime Timestamp { get; set; }
        public string Id { get; set; }
        public bool? IsSent { get; set; }
        public bool? IsExpired { get; set; }
        public string Message { get; set; }
        public string Comment { get; set; }
    }
}