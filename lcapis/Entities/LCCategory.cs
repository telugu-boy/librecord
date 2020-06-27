using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace lcapis.Entities
{
    public class LCCategory
    {
        [Key]
        public long CategoryID { get; set; }
        public long ServerID { get; set; }
        public string Name { get; set; }
        public List<LCChannel> Channels { get; set; }
    }
}
