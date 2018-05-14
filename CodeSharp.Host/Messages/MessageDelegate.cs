using System;

namespace CodeSharp.MessageDelegates
{
	public abstract class MessageDelegate : IDisposable
	{
		protected const string ReceiveTopicNode = "r";

		public event EventHandler ValueChanged;

		protected void OnValueChanged()
		{
			ValueChanged?.Invoke(this, EventArgs.Empty);
		}

		public event EventHandler<Tuple<string, string>> SendMessage;

		readonly public string Topic;

		internal string TopicReceive => Topic + "/" + ReceiveTopicNode;

		protected void OnSendMessage (string topic, string message, int value)
		{
			OnSendMessage (topic, message, value.ToString ());
		}

		protected void OnSendMessage(string topic, string message, bool value)
		{
			OnSendMessage(topic, message, value ? "true" : "false");
		}

		protected void OnSendMessage (string topic, string message, string value)
		{
			OnSendMessage (topic + "/" + ReceiveTopicNode, message + "/" + value);
		}

		void OnSendMessage (string topic, string message)
		{
			SendMessage?.Invoke (this, new Tuple<string, string> (topic, message));
		}

		string baseTopic;

		public MessageDelegate (string topic)
		{
			baseTopic = topic;
			Topic = "vscode/" + topic;
		}

		string GetNormalizedFileName (string name)
		{
			return name.Replace (" ", "");
		}

		internal void RaiseMessageReceived (string topic, string message)
		{
			OnMessageReceived (topic, message);
		}

		internal void RaiseMessageReceived (string topic, byte[] message)
		{
			OnMessageReceived (topic, message);
		}

		protected virtual void OnMessageReceived (string topic, string message)
		{

		}

	
		protected virtual void OnMessageReceived (string topic, byte[] message)
		{

		}

		public void WriteLog (string message) 
		{
			Console.WriteLine($"[Net][{baseTopic}]{message}");
		}

		public virtual void Dispose()
		{
			
		}
	}
}
