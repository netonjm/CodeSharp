using System;

namespace CodeSharp.MessageDelegates
{
	public class MyContentProviderMessageDelegate : TextDocumentContentProviderMessageDelegate
	{
		public MyContentProviderMessageDelegate (string id) : base (id)
		{
		}

		public override string GetParsedCode (string html)
		{
			return @"<body> Hello!!!!! </ body >";
		}
	}

	public class XamlCompletionItemProvider : CompletionItemProviderMessageDelegate
	{
		public XamlCompletionItemProvider (string id) : base (id)
		{

		}

		public override CompletionItem [] GetCompletionItems (CompletionItemArgs args)
		{
			
			return new CompletionItem [0];
		}

		public override string OnGetMessageReceived ()
		{
			return "";
		}

		public override void OnSendMessageReceived (string value)
		{
			var test = "";
			Console.WriteLine ("");
		}
	}
}