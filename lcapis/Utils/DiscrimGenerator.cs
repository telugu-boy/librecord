using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lcapis.Utils
{
    public class DiscrimGenerator
    {
        public static int GenerateDiscrim()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
    }
}
