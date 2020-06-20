using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace lcapis.Entities
{
    public class LCUser
    {
        public long? ID { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public string Username { get; set; }
        public int Discrim { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
