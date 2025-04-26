namespace S1Processor.Services
{
	public class ProcessorService
	{
		public void StartServices()
		{

			Thread LoanProcessorthread = null;
			Thread PaybillSwitchThread = null;
			Thread PinResetRequestsThread = null;
			Thread ProcessLoanRepaymentsThread = null;
            Thread ProcessWithdrawalsThread = null;

            try
			{
				LoanProcessor startLoan = new LoanProcessor();
				LoanProcessorthread = new Thread(startLoan.RunService);
				LoanProcessorthread.Priority = ThreadPriority.Normal;
				LoanProcessorthread.Start();

                PaybillSwitch startPaybill = new PaybillSwitch();
                PaybillSwitchThread = new Thread(startPaybill.RunService);
                PaybillSwitchThread.Priority = ThreadPriority.Normal;
                PaybillSwitchThread.Start();

                PinResetRequests startPinReset = new PinResetRequests();
                PinResetRequestsThread = new Thread(startPinReset.RunService);
                PinResetRequestsThread.Priority = ThreadPriority.Normal;
                PinResetRequestsThread.Start();

                ProcessLoanRepayments startProcessLoanRepayments = new ProcessLoanRepayments();
                ProcessLoanRepaymentsThread = new Thread(startProcessLoanRepayments.RunService);
                ProcessLoanRepaymentsThread.Priority = ThreadPriority.Normal;
                ProcessLoanRepaymentsThread.Start();

                ProcessWithdrawals startProcessWithdrawals = new ProcessWithdrawals();
                ProcessWithdrawalsThread = new Thread(startProcessWithdrawals.RunService);
                ProcessWithdrawalsThread.Priority = ThreadPriority.Normal;
                ProcessWithdrawalsThread.Start();
            }
			catch (Exception ex)
			{
				UTILS.WriteLog(string.Format("Start Service:{0}\\n{1}\\", ex.Message, ex.StackTrace));
			}
		}
	}
}
