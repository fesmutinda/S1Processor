using log4net;
using S1Processor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S1Processor.Client_Services
{
    class ProcessWithdrawals : SERVICE
    {
        #region "Declarations"
        private static readonly ILog _log = LogManager.GetLogger(typeof(ProcessorService));
        private AutoResetEvent ar = new AutoResetEvent(false);
        System.Net.NetworkCredential cd;
        #endregion  
        public ProcessWithdrawals()
        {
            clientCode = "100";
            SaccoName = "Polytech Sacco";
        }
        public void RunService()
        {
            Errorlog.LogEntryOnFile("Started ProcessWithdrawals");
            Console.WriteLine("Started ProcessWithdrawals");
            while (Shared.StopService == false)
            {
                try
                {
                    Console.WriteLine(DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff") + "\ttop of loop....");
                    Parallel.Invoke(
                        () =>
                        {
                            Main();
                        },
                        () =>
                        {
                            Main();
                        }
                    );
                }
                catch (Exception ex)
                {
                    _log.Info("Polytech Sacco | 10000 |ERR :\t" + ex);
                    LogError(ex);
                }
                finally
                {
                }
                try
                {
                    ar.WaitOne(1800);
                }
                catch (Exception ex)
                {
                    ar.WaitOne(5000);
                }
            }
        }
    
        private void Main()
        {
            Console.WriteLine(DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff") + "\tprocessing withdrawals -START....");
            try
            {
                var client = Config.GetMobileServiceClient();
                var paybillResponse = client.ProcessWithdrawalsAsync();
                Console.WriteLine("Withdrawals processed: " + paybillResponse);
            }
            catch (Exception ex)
            {
                Errorlog.LogEntryOnFile(clientCode + ": " + ex.Message);
                if (ex.InnerException != null)
                {
                    if ((ex.InnerException.Message != null))
                    {
                        Errorlog.LogEntryOnFile(ex.InnerException.Message);
                        Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                    }
                }
            }
            Console.WriteLine(DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff") + "\tprocessing withdrawals postings -DONE....");
        }
    }
}
