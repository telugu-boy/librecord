using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lcapis.Models.LCUsers
{
    public class UserModel
    {
        public long ID { get; set; }
        public string Username { get; set; }
        public int Discrim { get; set; }
    }
}
