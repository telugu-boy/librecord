using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace lcapis.Entities
{
    public class UserServer
    {
        [Column("ServerID")]
        public long UserID { get; set; }
        public virtual LCUser User { get; set; }
        [Column("UserID")]
        public long ServerID { get; set; }
        public virtual LCServer Server { get; set; }
    }
    public class LCServer
    {
        [Key]
        public long ServerID { get; set; }
        public long UserID { get; set; } //for owner
        public string ServerName { get; set; }
        public string Description { get; set; }

        public virtual List<UserServer> UserServers { get; set; }
        /*
        public List<LCCategory> Categories { get; set; }
        public List<LCRole> Roles { get; set; }
        */
        public virtual List<LCUser> UserBans { get; set; }
        public virtual List<LCInvite> Invites { get; set; }
    }

}
