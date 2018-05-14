public class VSConsole
{
	public void Log (string message)
	{
		MessageService.Current.SendMessage ("console_log", message);
	}
}
