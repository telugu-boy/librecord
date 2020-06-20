using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lcapis.Entities
{
    public class LCMsg
    {
        public long? Id { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
    }
}
