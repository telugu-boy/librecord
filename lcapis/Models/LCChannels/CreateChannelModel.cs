using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace lcapis.Models.LCChannels
{
    public class CreateChannelModel
    {
        [Required]
        public string ChannelName { get; set; }
    }
}
