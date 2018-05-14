namespace CodeSharp.MessageDelegates
{
	public class MessageDelegateMonitor : IMonitor
	{
		readonly MessageDelegate msgDelegate;

		public MessageDelegateMonitor (MessageDelegate msgDelegate)
		{
			this.msgDelegate = msgDelegate;
		}

		public void WriteLine (string message)
		{
			msgDelegate.WriteLog (message);
		}
	}
}
