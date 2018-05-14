using System;

namespace CodeSharp.MessageDelegates
{
	public abstract class CommandMessageDelegate : MessageDelegate
	{
		public string Id {
			get;
			private set;
		}
		//readonly EventHandler eventHandler;
		public CommandMessageDelegate (string id)
			: base ("command")
		{
			this.Id = id;
			//this.eventHandler = eventHandler;
		}

		protected override void OnMessageReceived (string topic, string message)
		{
			OnExecuted ();
		}

		public abstract void OnExecuted ();
	}

	public abstract class ExtensionMessageDelegate : MessageDelegate
	{
		//readonly EventHandler eventHandler;
		public ExtensionMessageDelegate ()
			: base ("extension")
		{
			//this.eventHandler = eventHandler;
		}

		protected override void OnMessageReceived (string topic, string message)
		{
			//eventHandler?.Invoke (this, EventArgs.Empty);
			if (message == "activate") {
				OnActivate ();
				return;
			}
			if (message == "deactivate") {
				OnDeactivate ();
				return;
			}
			throw new NotImplementedException ($"{topic} : {message}");
		}

		public abstract void OnActivate ();

		public abstract void OnDeactivate ();
	}
}