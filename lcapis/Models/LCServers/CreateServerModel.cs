using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace lcapis.Models.LCServers
{
    public class CreateServerModel
    {
        [Required]
        public string ServerName { get; set; }
    }
}
