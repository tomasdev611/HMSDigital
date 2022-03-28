using System;
using System.IO; // File, Path defined here
using System.Linq;
using System.Threading;
using System.Diagnostics; 
using System.Collections.Generic; // Dictionary type defined here
using System.Runtime.CompilerServices; // CallerFilePath defined here

using NUnit.Framework;

using Newtonsoft.Json; // JsonConvert defined here
using Newtonsoft.Json.Linq; // JObject y otros

namespace WebPortalAutomation
{
    public static class Utils
    {

        // Jsons
        public static string CurrentDirectory([CallerFilePath] string callerFile = "")
        {
            return Path.GetDirectoryName(callerFile);
        }

        public static Dictionary<string, object> LoadJson(
            string jsonFileName,
            [CallerFilePath] string codeFileName = ""
        )
        { 
            string directoryPath = Path.GetDirectoryName(codeFileName);
            string json = File.ReadAllText(Path.Join(directoryPath, jsonFileName));
            
            return (Dictionary<string, object>)recursivelyDeserializeJson(JsonConvert.DeserializeObject(json));
        }

        private static object recursivelyDeserializeJson(object json)
        {
            switch(json)
            {
                // structure types
                case JObject jObject:
                    return (Dictionary<string, object>)
                             ((IEnumerable<KeyValuePair<string, JToken>>) jObject).ToDictionary(
                                j => j.Key, j => recursivelyDeserializeJson(j.Value)
                            );
                case JArray jArray:
                    return (List<object>)jArray.Select(recursivelyDeserializeJson).ToList();
                
                // primitive types
                case JValue jValue:
                    return (object)jValue.Value;

                default:
                    throw new Exception($"Unsupported type: {json.GetType()}"); 
            }
        }

        // Dates
        private static Dictionary<string, string> monthMapper = new Dictionary<string, string>(){
            { "Jan", "01" }, { "Feb", "02" }, { "Mar", "03" }, { "Apr", "04" },
            { "May", "05" }, { "Jun", "06" }, { "Jul", "07" }, { "Aug", "08" },
            { "Sep", "09" }, { "Oct", "10" }, { "Nov", "11" }, { "Dec", "12" }
        };

        // This asumes that the date format in the page is not gonna change
        public static DateTime CastToDateTime(string stringDateTime)
        {
            // 21 => needs 0 padd, 22 already padded
            if(stringDateTime.Length == 21)
            {
                stringDateTime = stringDateTime.Substring(0, 14) + "0" + stringDateTime.Substring(14, 7);
            }

            // Replace month in chars for numbers (example: Apr -> 04)
            string monthNumber = monthMapper[stringDateTime.Substring(0, 3)];
            stringDateTime = stringDateTime.Remove(0, 3).Insert(0, monthNumber);

            return DateTime.ParseExact(stringDateTime, "MM dd, yyyy, HH:mm tt", null); 
        }

        public static string ContextToFileName()
        {
            string datetime = DateTime.UtcNow.ToString(
                "yyyy-MM-dd_HH.mm.ss.ffff_zz"
            );

            int processID = System.Environment.ProcessId;
            int threadID = Thread.CurrentThread.ManagedThreadId;

            string testID = TestContext.CurrentContext.Test.ID;
            string testName = TestContext.CurrentContext.Test.Name;

            return $"PID_{processID}|TID_{threadID}|{datetime}|{testName}({testID})";
        }
    }
}
