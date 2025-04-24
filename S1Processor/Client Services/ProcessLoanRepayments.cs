using log4net;
using S1Processor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S1Processor.Client_Services
{
    class ProcessLoanRepayments : SERVICE
    {
        #region "Declarations"
        private static readonly ILog _log = LogManager.GetLogger(typeof(ProcessorService));
        private AutoResetEvent ar = new AutoResetEvent(false);
        System.Net.NetworkCredential cd;
        #endregion
        public ProcessLoanRepayments()
        {
            clientCode = "100";
            SaccoName = "Polytech Sacco";
        }
        public void RunService()
        {
            Errorlog.LogEntryOnFile("Started ProcessLoanRepayments");
            Console.WriteLine("Started ProcessLoanRepayments");
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
        private async Task Main()
        {
            Console.WriteLine(DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff") + "\tprocessing withdrawals -START....");
            try
            {
                var client = Config.GetMobileServiceClient();
                var paybillResponse =await client.ProcessLoanRepaymentsAsync();
                Console.WriteLine("Response: " + paybillResponse);
            }
            catch (Exception ex)
            {
                Errorlog.LogEntryOnFile(clientCode + ": " + ex.Message);
                if (ex.InnerException != null)
                {
                    if ((ex.InnerException.Message != null))
                    {
                        Errorlog.LogEntryOnFile(ex.InnerException.Message);
                        Console.WriteLine(ex.InnerException.Message);
                    }
                }
            }
            Console.WriteLine(DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff") + "\tprocessing withdrawals postings -DONE....");
        }
    }
}
