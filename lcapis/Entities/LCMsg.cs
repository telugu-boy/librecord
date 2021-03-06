﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace lcapis.Entities
{
    public class LCMsg
    {
        [Key]
        public long MsgID { get; set; }
        public long UserID { get; set; }
        public long ChannelID { get; set; }
        public long ServerID { get; set; }
        public string Content { get; set; }
    }
}
