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

        public static ulong StartTime { get; set; }

        public static ulong? LastUpdateTime { get; set; }

        public static string SimplifySpace(string str)
        {
            if(string.IsNullOrEmpty(str)) return "";
            return regex.Replace(str.Trim(), "\t");
        }

        /// <summary>
        /// Converts local DateTime to utc timestamp
        /// </summary>
        /// <param name="date">Local Date</param>
        /// <param name="time">Local Time</param>
        /// <returns>UTC Timestamp</returns>
        public static ulong DateTimeToTimestamp(DateTime date, DateTime time)
        {
            DateTime selected = DateTime.Parse(date.Date.ToShortDateString() + "\t" + time.TimeOfDay.ToString()).ToUniversalTime();
            return (ulong)((DateTimeOffset)selected).ToUnixTimeSeconds();
            
        }
    }
}
