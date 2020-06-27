using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace lcapis.Entities
{
    public class LCChannel
    {
        [Key]
        public long ChannelID { get; set; }
        public long ServerID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public bool IsVoice { get; set; }
        public List<LCMsg> Messages { get; set; }
    }
}
