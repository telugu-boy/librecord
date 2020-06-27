using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace lcapis.Entities
{
    public class LCInvite
    {
        [Key]
        public string InviteCode { get; set; }
        [DataType(DataType.Date)]
        public virtual DateTime Expiry { get; set; }
        public virtual LCServer Server { get; set; }
    }
}
