using System;
using System.Linq;

namespace CodeSharp.MessageDelegates
{
	public abstract class TextDocumentContentProviderMessageDelegate : MessageDelegate
	{
		//readonly EventHandler eventHandler;
		public TextDocumentContentProviderMessageDelegate (string id)
			: base ("renderer")
		{
			
		}
		protected override void OnMessageReceived (string topic, string message)
		{
			//We need check at this point if the html is correct
			//Create a view service to parse the stuff??
			var parsedCode = GetParsedCode (message);

			OnSendMessage (topic, parsedCode);
		}

		public abstract string GetParsedCode (string html);
	}

	public class CompletionItemArgs
	{
		public string First { get; set; }

		public string Second { get; set; }
	}

	public class CompletionItem
	{
		public string Name { get; set; }
	}

	public abstract class CompletionItemProviderMessageDelegate : MessageDelegate
	{
		public string Id {
			get;
			private set;
		}
		//readonly EventHandler eventHandler;
		protected CompletionItemProviderMessageDelegate (string id)
			: base ("completion")
		{
			this.Id = id;
			//this.eventHandler = eventHandler;
		}

		protected override void OnMessageReceived (string topic, string message)
		{
			var parsedMessage = message.Split ('|');
			if (parsedMessage.Length != 2) {
				throw new ArgumentOutOfRangeException ("wrong arguments: " + parsedMessage);
			}
			var args = new CompletionItemArgs () { First = parsedMessage [0], Second = parsedMessage [1] };
			var items = GetCompletionItems (args);
			var result = GetParsedString (items);

			OnSendMessage (TopicReceive, result);
		}

		public string GetParsedString (CompletionItem[] completionItems) 
		{
			return string.Join (",", completionItems.Select (s => s.Name).ToArray ());
		}

		public abstract CompletionItem [] GetCompletionItems (CompletionItemArgs args); 
	}

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