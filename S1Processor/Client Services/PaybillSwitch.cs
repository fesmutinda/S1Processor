using log4net;
using S1Processor.Utils;
using System.Threading.Tasks;

namespace S1Processor.Client_Services
{
    class PaybillSwitch : SERVICE
    {
        #region "Declarations"
        private static readonly ILog _log = LogManager.GetLogger(typeof(ProcessorService));

        private AutoResetEvent ar = new AutoResetEvent(false);
        #endregion  
        public PaybillSwitch()
        {
            clientCode = "100";
            SaccoName = "Polytech Sacco";
        }
        public void RunService()
        {
            Errorlog.LogEntryOnFile("Started PaybillSwitch");
            Console.WriteLine("Started PaybillSwitch");
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

                //Console.WriteLine(DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff") + "\tGoing into Wait mode....");
                try
                {
                    ar.WaitOne(1800);
                }
                catch (Exception ex)
                {
                    ar.WaitOne(5000);
                }
            }
            Errorlog.LogEntryOnFile("Stopped " + SaccoName);
            Console.WriteLine("Stopped " + SaccoName);
        }
        private async Task Main()
        {
            Console.WriteLine(DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff") + "\tprocessing Paybillss -START....");
            try
            {
                var client = Config.GetMobileServiceClient();
                var paybillResponse = await client.PaybillSwitchAsync();
                Console.WriteLine("Paybill Response :: "+paybillResponse);
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
            Console.WriteLine(DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff") + "\tprocessing Paybill postings -DONE....");
        }
    }
}
