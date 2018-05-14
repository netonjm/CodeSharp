using System;

namespace CodeSharp.Host.Terminal
{
	partial class MainClass
	{
		class ConsoleMonitor : IMonitor
		{
			public void WriteLine (string message)
			{
				Console.WriteLine (message);
			}
		}
	}
}
