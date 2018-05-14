using System;

public class MessageCommand
{
	public MessageCommand (string id, EventHandler action)
	{
		Id = id;
		Action = action;
	}

	public string Id { get; set; }
	public EventHandler Action { get; set; }
}
