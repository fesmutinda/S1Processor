using log4net;
using S1Processor.Clients;
using S1Processor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S1Processor.Services
{
    class LoanProcessor : SERVICE
    {
        #region "Declarations"
        private static readonly ILog _log = LogManager.GetLogger(typeof(ProcessorService));
        private AutoResetEvent ar = new AutoResetEvent(false);
        #endregion
        public LoanProcessor()
        {
            clientCode = "100";
            SaccoName = "Polytech Sacco";
            processItem = "LoanProcessor";
        }
        public void RunService()
        {
            Errorlog.LogEntryOnFile(processItem,"Started LoanProcessor");
            Console.WriteLine("Started LoanProcessor");
            while (Shared.StopService == false)
            {
                try
                {
                    Console.WriteLine(DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff") + "\ttop of loop....");
                    Parallel.Invoke(
                        //() =>
                        //{
                        //    Main();
                        //},
                        () =>
                        {
                            Main();
                        }
                    );
                }
                catch (Exception ex)
                {
                    _log.Info("Polytech Sacco | 10000 |ERR :\t" + ex);
                    LogError(processItem,ex);
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
            Console.WriteLine(DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff") + "\tprocessing Loans Applications -START....");
            try
            {
                var client = Config.GetMobileServiceClient();
                var paybillResponse =await client.ProcessLoanApplicationsAsync();
                Console.WriteLine("Loan Applications Processed: " + paybillResponse);
            }
            catch (Exception ex)
            {
                Errorlog.LogEntryOnFile(processItem,clientCode + ": " + ex.Message);
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.Message != null)
                    {
                        Errorlog.LogEntryOnFile(processItem,ex.InnerException.Message);
                        Console.WriteLine(ex.InnerException.Message);
                    }
                }
            }
            Console.WriteLine(DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff") + "\tprocessing Loans Applications postings -DONE....");
        }
    }
}
