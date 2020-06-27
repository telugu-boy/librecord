using lcapis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lcapis.Models.LCUsers
{
    public class UpdateUserModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Discrim { get; set; }
        public List<LCRole> Roles { get; set; }
    }
}
