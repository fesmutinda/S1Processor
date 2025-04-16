using S1Processor.Utils;
namespace S1Processor
{

	public class SERVICE
	{
		public string clientCode;
		public string SaccoName;
		public string CompanyName;	

		
		public void LogError(Exception ex)
		{
			try
			{
				Errorlog.LogEntryOnFile(string.Format("{0}:{1}", clientCode, ex.Message));
				if (ex.InnerException != null)
				{
					Errorlog.LogEntryOnFile(string.Format("{0}:{1}", clientCode, ex.InnerException.Message));
				}
				Errorlog.LogEntryOnFile(string.Format("{0}:{1}", clientCode, ex.StackTrace));
				Errorlog.LogEntryOnFile(string.Format("{0}:{1}", clientCode, ex.Source));
			}
			catch (Exception e)
			{
			}
		}

	}
}