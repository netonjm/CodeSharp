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
}