﻿using log4net;
using S1Processor.Client_Services;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using Topshelf;

namespace S1Processor
{
    class Program
    {
        private static PF2TransService _service;

        static void Main(string[] args)
        {
            //while (true)
            //{
            //    new LoanProcessor().RunService();
            //    //new PaybillSwitch().RunService();
            //}

            // self service installer/uninstaller
            if (args != null && args.Length == 1 && (args[0][0] == '-' || args[0][0] == '/'))
            {
                switch (args[0].Substring(1).ToLower())
                {
                    case "install":
                    case "i":
                        if (!ServiceInstallerUtility.InstallMe())
                            //Logger.Fatal("Failed to install service");
                            UTILS.WriteLog(string.Format("{0}\tFailed to install service", DateTime.Now));
                        break;
                    case "uninstall":
                    case "u":
                        if (!ServiceInstallerUtility.UninstallMe())
                            //Logger.Fatal("Failed to uninstall service");
                            UTILS.WriteLog(string.Format("{0}\tFailed to uninstall service", DateTime.Now));
                        break;
                    default:
                        //Logger.Error("Unrecognized parameters (allowed: /install and /uninstall, shorten /i and /u)");
                        UTILS.WriteLog(string.Format("{0}\tUnrecognized parameters (allowed: /install and /uninstall, shorten /i and /u)", DateTime.Now));
                        break;
                }
                Environment.Exit(0);
            }


            _service = new PF2TransService();
            var servicesToRun = new ServiceBase[] { _service };


            // console mode
            if (Environment.UserInteractive)
            {
                // register console close event
                _consoleHandler = ConsoleEventHandler;
                SetConsoleCtrlHandler(_consoleHandler, true);

                //Console.Title = AppDomain.CurrentDomain.FriendlyName;

                //Logger.Debug("Running in console mode");
                UTILS.WriteLog(string.Format("{0}\tRunning in console mode", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")));
                _service.Start();

                Console.WriteLine("Press any key to stop the service...");
                Console.Read();

                _service.Stop();
            }
            else
            {
                // service mode
                //Logger.Debug("Running in service mode");
                ServiceBase.Run(servicesToRun);
            }

        }


        #region Page Event Setup
        enum ConsoleCtrlHandlerCode : uint
        {
            // ReSharper disable InconsistentNaming
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
            // ReSharper restore InconsistentNaming
        }
        delegate bool ConsoleCtrlHandlerDelegate(ConsoleCtrlHandlerCode eventCode);
        [DllImport("kernel32.dll")]
        static extern bool SetConsoleCtrlHandler(ConsoleCtrlHandlerDelegate handlerProc, bool add);
        static ConsoleCtrlHandlerDelegate _consoleHandler;
        #endregion

        #region Page Events
        static bool ConsoleEventHandler(ConsoleCtrlHandlerCode eventCode)
        {
            // Handle close event here...
            switch (eventCode)
            {
                case ConsoleCtrlHandlerCode.CTRL_C_EVENT:
                case ConsoleCtrlHandlerCode.CTRL_CLOSE_EVENT:
                case ConsoleCtrlHandlerCode.CTRL_BREAK_EVENT:
                case ConsoleCtrlHandlerCode.CTRL_LOGOFF_EVENT:
                case ConsoleCtrlHandlerCode.CTRL_SHUTDOWN_EVENT:

                    _service.Stop();

                    Environment.Exit(0);
                    break;
            }

            return (false);
        }
        #endregion
    }
}
