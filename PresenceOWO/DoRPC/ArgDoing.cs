using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace PresenceOWO.DoRPC
{
    public static class ArgDoing
    {
        public const string ArgsPath = "args.json";
        private static Regex whiteSpaceRegEx = new Regex(@" +");

        public static DateTime StartTime { get; set; }

        public static DateTime? LastUpdateTime { get; set; }

        public static string SimplifySpace(string str)
        {
            if(string.IsNullOrEmpty(str)) return "";
            return whiteSpaceRegEx.Replace(str.Trim(), "\t");
        }

        /// <summary>
        /// Converts local DateTime to utc timestamp
        /// </summary>
        /// <param name="date">Local Date</param>
        /// <param name="time">Local Time</param>
        /// <returns>UTC Timestamp</returns>
        public static ulong DateTimeToTimestamp(DateTime date, DateTime time)
        {
            DateTime selected = CombineDateTime(date, time).ToUniversalTime();
            return (ulong)((DateTimeOffset)selected).ToUnixTimeSeconds();
            
        }

        public static ulong DateTimeToTimestamp(DateTime dateTime)
        {
            return (ulong)((DateTimeOffset)dateTime).ToUniversalTime().ToUnixTimeSeconds();

        }

        /// <summary>
        /// Combines two datetime into one
        /// </summary>
        /// <param name="date">Date of DateTime</param>
        /// <param name="time">Time of DateTime</param>
        /// <returns>Combined DateTime in local</returns>
        public static DateTime CombineDateTime(DateTime date, DateTime time)
        {
            return DateTime.Parse(date.Date.ToShortDateString() + "\t" + time.TimeOfDay.ToString());
        }

        public static RPArgs DefaultArgs()
        {
            return new RPArgs()
            {
                Details = "OWO",
                State = "owo?",
                LargeImageText = "! OWO !",
                SmallImageText = ".w.",
                TimestampModeNumber = 1,
                BtnText1 = "Click to see pussy",
                BtnUrl1 = @"https://www.youtube.com/watch?v=uwmeH6Rnj2E",
            };
        }
    }
}
