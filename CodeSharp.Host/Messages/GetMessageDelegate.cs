using System;

namespace CodeSharp.MessageDelegates
{

	public abstract class GetMessageDelegate<T> : MessageDelegate
	{
		const string TopicGet = "get";

		protected GetMessageDelegate (string topic) : base (topic)
		{
		}

		public abstract T OnGetMessageReceived ();

		protected override void OnMessageReceived (string topic, string message)
		{
			if (message == TopicGet) {
				var value = OnGetMessageReceived ();
				var type = value.GetType ();

				if (type == typeof (bool)) {
					OnSendMessage (topic, message, (bool)(object)value);
				} else if (type == typeof (int)) {
					OnSendMessage (topic, message, (int)(object)value);
				} else {
					throw new NotImplementedException (type.ToString ());
				}
			} else {
				throw new NotImplementedException (message);
			}
		}
	}
}