namespace CodeSharp.MessageDelegates
{
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