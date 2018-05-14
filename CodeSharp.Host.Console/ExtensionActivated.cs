using System;

namespace CodeSharp.MessageDelegates
{
	public class ExtensionActivated : ExtensionMessageDelegate
	{
		public override void OnActivate ()
		{
			WriteLog ("this method is called when your extension is activated!");
			VSCode.Console.Log ("this method is called when your extension is activated!");
		}

		public override void OnDeactivate ()
		{
			WriteLog ("this method is called when your extension is deactivated!");
			VSCode.Console.Log ("this method is called when your extension is activated!");
		}
	}

	public class SayHelloCommand : CommandMessageDelegate
	{
		public SayHelloCommand (string id) : base (id)
		{
			
		}
		public override void OnExecuted ()
		{
			WriteLog ($"[{Id}] Executed!");
			//This sends log output in VSCode
			VSCode.Console.Log ($"[{Id}] Executed from C#!");

			//Opens a information message in VSCode
			VSCode.Window.ShowInformationMessage ("Hello World from C#!");
		}
	}
}