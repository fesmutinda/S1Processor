using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
namespace S1Processor.Helpers
{
	public class Errorlog
	{
        public static string logpath = @"C:\SwizzProcessor LOGS\";

        private Errorlog()
		{
		}

		public static string LogFileName
		{
			get { return "D:\\SwizzKASH logs\\Errorlogs\\" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".txt"; }
		}

        public static void LogEntryOnFile(string processItem, string processData)
        {
            string logFileName = $"S1ProcessorLogs-{DateTime.Now:yyyy-MM-dd}-{processItem} .txt";
            //string logFileName = $"SaccoLinkLogs-{DateTime.Now:yyyy-MM-dd}.txt";
            string logFilePath = Path.Combine(logpath, logFileName);

            if (!Directory.Exists(logpath))
            {
                Directory.CreateDirectory(logpath);
            }
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - [{Thread.CurrentThread.ManagedThreadId}]-[{processItem}] {processData}");
            }

        }


    }
}
