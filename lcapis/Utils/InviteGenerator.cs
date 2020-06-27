using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lcapis.Utils
{
    public class InviteGenerator
    {
        private static Random random = new Random();
        public static string GenerateInvite(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
