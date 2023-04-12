using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PresenceOWO.OWOSystem;

namespace PresenceOWO.IO
{
    public static class SavingManager
    {
        public static string CurrentDir { get; private set; }

        public static void Initialize()
        {
            CurrentDir = Environment.CurrentDirectory;
        }

        public static T LoadFromJson<T>(string filename, bool nullableFields = true) where T : class 
        {
            string reqPath = Path.Combine(CurrentDir, filename);
            if (!File.Exists(reqPath))
                return null;
            
            string text = File.ReadAllText(reqPath);
            var deSSettings = new JsonSerializerSettings()
            {
                NullValueHandling = nullableFields ? NullValueHandling.Include : NullValueHandling.Ignore,
            };
            return JsonConvert.DeserializeObject<T>(text, deSSettings);
        }

        public static void SaveToJson<T>(string filename, T obj) where T : class 
        {
            string reqPath = Path.Combine(CurrentDir, filename);
            using var sw = File.CreateText(reqPath);
            var jw = new JsonSerializer();
            jw.Serialize(sw, obj);
        }
    }
}
