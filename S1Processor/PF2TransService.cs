using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace S1Processor
{
    public partial class PF2TransService : ServiceBase
    {
        public PF2TransService()
        {
            InitializeComponent();
        }
        
        // ------------------------------------
        protected override void OnStart(string[] args)
        {

            //while (true)
            //{
            //    try
            //    {
            //        NAVCodeunit.ExecuteCodeunit();
            //    }
            //    catch (Exception ee)
            //    {
            //        ee.Data.Clear();
            //    }
            //    finally
            //    {
            //        Thread.Sleep(1500);
            //    }
            //}


            var sync = new ProcessorService();
            sync.StartServices();

            ////_stop.Reset();
            ////ThreadPool.RegisterWaitForSingleObject(_stop, new WaitOrTimerCallback(PeriodicProcess), null, 1000, true);
        }

        protected override void OnStop()
        {
            Shared.StopService = true;
            _stop.Set();
        }

        public void Start()
        {
            OnStart(null);
        }

        public void Stop()
        {
            OnStop();
        }

        //private void PeriodicProcess(object state, bool timeout)
        //{
        //    if (timeout)
        //    {
        //        // Periodic processing here
        //        //CommonFunctions.WriteLog(string.Format("{0}\tOk....", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")));
        //        new SmsEngine().PollForPendingSms();
        //        // Then queue another wait
        //        ThreadPool.RegisterWaitForSingleObject(_stop, new WaitOrTimerCallback(PeriodicProcess), null, 1000, true);
        //    }
        //}

        private ManualResetEvent _stop = new ManualResetEvent(false);
        // ------------------------------------

    }
}
