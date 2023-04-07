using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PresenceOWO.DoRPC
{
    public static class ArgDoing
    {
        private static Regex regex = new Regex(@" +");
        public static string SimplifySpace(string str)
        {
            if(string.IsNullOrEmpty(str)) return "";
            return regex.Replace(str.Trim(), " ");
        }
    }
}
