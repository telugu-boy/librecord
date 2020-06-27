using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace lcapis.Entities
{
    public class LCRole
    {
        [Key]
        public long RoleID { get; set; }
        public string Name { get; set; }
        public long Colour { get; set; }
        public bool CanBan { get; set; }
        public bool CanKick { get; set; }
        //public List<LCUser> Users { get; set; }
    }
}