using log4net;
using Newtonsoft.Json;
using S1Processor.Clients;
using S1Processor.Helpers;
using S1Processor.Models;
using SwizzProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace S1Processor.Services
{
    class PinResetRequests : SERVICE
    {
        #region "Declarations"
        private static readonly ILog _log = LogManager.GetLogger(typeof(ProcessorService));
        private AutoResetEvent ar = new AutoResetEvent(false);
        #endregion
        public PinResetRequests()
        {
            clientCode = "100";
            SaccoName = "Polytech Sacco";
            processItem = "PinResetRequests";
        }
        public void RunService()
        {
            Errorlog.LogEntryOnFile(processItem, "Started PinResetRequests");
            Console.WriteLine("Started PinResetRequests");
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
                    LogError(processItem, ex);
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
                var myJsonResponse = await client.GetPinRequestsAsync(5);

                if (!string.IsNullOrWhiteSpace(myJsonResponse.ToString()))
                {
                    try
                    {
                        var activationRqsts = JsonConvert.DeserializeObject<ActivationRequestResponse>(myJsonResponse.return_value);// ToString());

                        if (activationRqsts != null && activationRqsts.StatusCode == "000")
                        {
                            foreach (ActivationRequestsItem memberItem in activationRqsts.ActivationRequests)
                            {
                                await SendToExternalApi(memberItem.Phone);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No pending PIN requests found.");
                        }
                    }
                    catch (Exception exp)
                    {
                        Errorlog.LogEntryOnFile(processItem, "Deserialization error: " + exp.Message);
                    }
                }
                else
                {
                    Console.WriteLine("PinResetRequests: No data found");
                }
            }
            catch (Exception ex)
            {
                Errorlog.LogEntryOnFile(processItem, clientCode + ": " + ex.Message);
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                {
                    Errorlog.LogEntryOnFile(processItem, ex.InnerException.Message);
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            Console.WriteLine(DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff") + "\tprocessing withdrawals postings -DONE....");
        }

        private async Task SendToExternalApi(string phoneNumber)
        {
            using var httpClient = new HttpClient();
            var payload = new { phoneNumber, SaccoCode = "100" };

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await httpClient.PutAsync /*PostAsync*/("http://197.232.170.121:8599/api/registration/changepinRequest", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"✅ Sent PIN reset for: {phoneNumber}");
                }
                else
                {
                    Console.WriteLine($"❌ Failed to send PIN reset for: {phoneNumber}, StatusCode: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Errorlog.LogEntryOnFile(processItem, "API call error for " + phoneNumber + ": " + ex.Message);
            }
        }

    }
}
