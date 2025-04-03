using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Assignment.Utils
{
    public class Logger
    {
        private static readonly string logFilePath = "C:\\Users\\91883\\Downloads\\CDAC WORK\\ErrorLog.txt";

        public static void LogException(Exception ex)
        {
            try
            {
                string logMessage = $"[{DateTime.Now}] Error: {ex.Message}\nStackTrace: {ex.StackTrace}\n\n";
                using (StreamWriter writer = new StreamWriter(logFilePath, append: true))
                {
                    writer.WriteLine(logMessage);
                }
            }
            catch (Exception innerEx)
            {
                Console.WriteLine($"Failed to log exception: {innerEx.Message}");
            }
        }
    }
}