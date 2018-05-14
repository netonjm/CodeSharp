namespace CodeSharp.MessageDelegates
{
	public abstract partial class MyWindow : OpenWindowMessage
	{
		public override void OnInitialize ()
		{

		}
	}

	public abstract partial class OpenWindowMessage : MessageDelegate
	{
		//readonly EventHandler eventHandler;

		protected OpenWindowMessage ()
			: base ("window")
		{
			//this.eventHandler = eventHandler;
		}

		protected override void OnMessageReceived (string topic, string message)
		{
			//eventHandler?.Invoke (this, EventArgs.Empty);
			OnInitialize ();
		}

		public abstract void OnInitialize ();
	}
}