using System;
using System.IO;
using CodeSharp.MessageDelegates;

namespace CodeSharp.Host.Terminal
{
	partial class MainClass
	{
		public static void Main(string[] args)
		{
			//This class provides the handling of the output log messages
			var monitor = new ConsoleMonitor();

			//Our HAP session manages our runner, this step only adds our prefered monitor
			var session = new CodeSession (monitor);

			var message = new ExtensionActivated ();
			session.MessageDelegates.Add (message);

			var command = new SayHelloCommand ("extension.sayHello");
			session.MessageDelegates.Add (command);

			var xamlCompletionItemProvider = new XamlCompletionItemProvider ("completion");
			session.MessageDelegates.Add (xamlCompletionItemProvider);

			session.Start ();

			Console.ReadKey ();
		}
	}
}
