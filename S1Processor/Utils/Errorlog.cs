using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
namespace S1Processor.Utils
{
	public class Errorlog
	{

		private Errorlog()
		{
		}

		public static string LogFileName
		{
			get { return "D:\\SwizzKASH logs\\Errorlogs\\" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".txt"; }
		}

		public static void LogEntryOnFile(string data)
		{
			try
			{
				Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")+":\t"+  data);
			}
			catch (Exception ex)
			{
				//ex.Data.Clear();
			}
		}



	}
}
