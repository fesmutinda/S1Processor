using log4net;
using S1Processor.Clients;
using S1Processor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace S1Processor.Services
{
    class ProcessWithdrawals : SERVICE
    {
        #region Declarations

        private static readonly ILog _log = LogManager.GetLogger(typeof(ProcessWithdrawals));
        private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

        #endregion

        #region Constructor

        public ProcessWithdrawals()
        {
            clientCode = "100";
            SaccoName = "Polytech Sacco";
            processItem = "ProcessWithdrawals";
        }

        #endregion
        
        #region Public Methods

        public void RunService()
        {
            LogInfo("Started ProcessWithdrawals");

            while (!Shared.StopService)
            {
                try
                {
                    LogInfo("Top of loop...");
                    Task.Run(() => ProcessWithdrawalsAsync());
                }
                catch (Exception ex)
                {
                    LogError(ex, "Error occurred in RunService loop.");
                }

                try
                {
                    _autoResetEvent.WaitOne(1800);
                }
                catch (Exception ex)
                {
                    LogError(ex, "Error while waiting in AutoResetEvent.");
                    _autoResetEvent.WaitOne(5000);
                }
            }
        }

        #endregion

        #region Private Methods

        private async Task ProcessWithdrawalsAsync()
        {
            LogInfo("Processing withdrawals - START...");
            Console.WriteLine(DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff") + "\tprocessing withdrawals -START....");

            try
            {
                var client = Config.GetMobileServiceClient();
                var response = await client.ProcessWithdrawalsAsync();

                Console.WriteLine($"Withdrawals processed: {response}");
                LogInfo($"Withdrawals processed: {response}");
            }
            catch (Exception ex)
            {
                Errorlog.LogEntryOnFile(processItem, clientCode + ": " + ex.Message);
                LogError(ex, "Exception during ProcessWithdrawalsAsync");
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.Message != null)
                    {
                        Errorlog.LogEntryOnFile(processItem, ex.InnerException.Message);
                        Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                    }
                }
            }

            LogInfo("Processing withdrawals - DONE...");
            Console.WriteLine(DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff") + "\tprocessing withdrawals postings -DONE....");
        }

        private void LogInfo(string message)
        {
            _log.Info($"{DateTime.Now:yy-MM-dd HH:mm:ss.fff}\t{message}");
            Console.WriteLine($"{DateTime.Now:yy-MM-dd HH:mm:ss.fff}\t{message}");
            Errorlog.LogEntryOnFile(processItem,message);
        }

        private void LogError(Exception ex, string contextMessage = "")
        {
            _log.Error($"{contextMessage}\n{ex}");
            Errorlog.LogEntryOnFile(processItem,$"{clientCode}: {contextMessage} - {ex.Message}");

            if (ex.InnerException?.Message != null)
            {
                Errorlog.LogEntryOnFile(processItem,$"InnerException: {ex.InnerException.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
        }

        #endregion
    }
}
