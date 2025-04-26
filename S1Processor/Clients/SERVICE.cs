using S1Processor.Helpers;

namespace S1Processor.Clients
{

	public class SERVICE
	{
		public string clientCode;
		public string SaccoName;
		public string CompanyName;
		public string processItem;
		
		public void LogError(string processItem, Exception ex)
		{
			try
			{
				Errorlog.LogEntryOnFile(processItem,string.Format("{0}:{1}", clientCode, ex.Message));
				if (ex.InnerException != null)
				{
					Errorlog.LogEntryOnFile(processItem, string.Format("{0}:{1}", clientCode, ex.InnerException.Message));
				}
				Errorlog.LogEntryOnFile(processItem, string.Format("{0}:{1}", clientCode, ex.StackTrace));
				Errorlog.LogEntryOnFile(processItem, string.Format("{0}:{1}", clientCode, ex.Source));
			}
			catch (Exception e)
			{
			}
		}

	}
}