public class ExtensionWindow
{
	public void ShowInformationMessage (string message)
	{
		MessageService.Current.SendMessage ("window_showInformationMessage", message);
	}
}