using System.Collections.Generic;

public static class VSCode
{
	static VSCode ()
	{

	}

	public static List<MessageCommand> Messages { get; set; } = new List<MessageCommand> ();

	public static ExtensionWindow Window { get; set; } = new ExtensionWindow ();

	public static VSConsole Console { get; set; } = new VSConsole ();
}
