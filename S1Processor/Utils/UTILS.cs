using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace S1Processor
{
    class UTILS
    {
        public static void WriteLog(string logText)
        {
            try
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ":\t" + logText );

                //DateTime now = DateTime.Now;

                //string folder = @"E:\HOSTED\SyncTransactionsService";
                //string filename = "log_" + now.ToString("yyyy-MM-dd HH") + ".txt";
                //string path2 = folder + "\\" + filename;

                //if (!Directory.Exists(folder))
                //{
                //    Directory.CreateDirectory(folder);
                //}
                //if (!File.Exists(path2))
                //{
                //    File.Create(path2);
                //}
                //ProcessWrite(path2, logText + "\n").Wait();
            }
            //catch (Exception ex)
            //{
            //    /*----------*/
            //    ex.Data.Clear();
            //}
            finally
            {
                //--
            }
        }

        private static Task ProcessWrite(string filePath, string logText)
        {
            return WriteTextAsync(filePath, logText);
        }
        private static async Task WriteTextAsync(string filePath, string text)
        {
            byte[] encodedText = Encoding.Unicode.GetBytes(text);

            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Append, FileAccess.Write, FileShare.Write,
                bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }
        //-------------------------------------------------------------------------

    }

}
