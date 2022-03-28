using System;
using System.Threading;
using System.Diagnostics;
using NUnit.Framework;

namespace WebPortalAutomation
{
    public static class Logger
    {
        private static bool logToStandarOutput = SetLogToStandarOutput();

        private static bool SetLogToStandarOutput()
        {
            string stdout = (string)TestContext.Parameters["STDOUT"] ?? "false";
            return bool.Parse(stdout.Trim().ToLower());
        }

        public static void Error(string message, string prefix = null)
        {
            Log("ERROR", prefix, message);
        }

        public static void Info(string message, string prefix = null)
        {
            Log("INFO", prefix, message);
        }

        public static void Critical(string message, string prefix = null)
        {
            Log("CRITICAL", prefix, message);
        }

        public static void Warning(string message, string prefix = null)
        {
            Log("WARNING", prefix, message);
        }

        public static void Log(string level, string prefix, string message)
        {
            prefix = (prefix is null) ? "" : (prefix + "|");
            string logMessage = $"{prefix}{Utils.ContextToFileName()}|{level}: {message}";

            TestContext.Progress.WriteLine(logMessage);

            if(logToStandarOutput) { Console.WriteLine(logMessage); }
        }
    }
}
