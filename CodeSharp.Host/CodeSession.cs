using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using CodeSharp.MessageDelegates;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace CodeSharp
{
	public class CodeSession : IDisposable
	{
		const string DefaultBrokerHost = "localhost";

		public string[] AllowedHosts { get; set; }

		internal const int Port = 51826;
		public List<MessageDelegate> MessageDelegates { get; private set; } = new List<MessageDelegate> ();

		readonly IMonitor monitor;

		public bool Debug { get; internal set; }

		MessageService client;

		public bool IsConnected => client.IsConnected;

		public CodeSession (IMonitor monitor)
		{
			this.monitor = monitor;
		}

		public void CheckHost ()
		{
			//Is our device allowed to execute this host
			if (AllowedHosts != null) {
				if (!AllowedHosts.Contains(Environment.MachineName)) {
					throw new UnauthorizedAccessException("Your device is not allowed to execute this Host session.");
				}
			}
		}

		public void Start(string brokerHost = DefaultBrokerHost)
		{
			CheckHost ();

			monitor.WriteLine ("     _-----_     ╭──────────────────────────╮");
			monitor.WriteLine ("    |       |    │        Welcome  to       │");
			monitor.WriteLine ("    |--(o)--|    │    .Net Host extension   │");
			monitor.WriteLine ("   `---------´   │        for VSCode        │");
			monitor.WriteLine ("    ( _´U`_ )    ╰──────────────────────────╯");
			monitor.WriteLine ("    /___A___\\   /");
			monitor.WriteLine ("     |  ~  |     ");
			monitor.WriteLine ("   __'.___.'__   ");
			monitor.WriteLine (" ´   `  |° ´ Y ` ");
			monitor.WriteLine ("");
			monitor.WriteLine ("");

			//Kill current user node processes
			//ProcessService.CleanProcessesInMemory ();

			//Connection to current MQTT broker
			if (client != null) {
				client.MqttMsgPublishReceived -= client_MqttMsgPublishReceived;
				client.Disconnect ();
			}

			client = new MessageService (monitor);
			client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

			client.Connect (brokerHost);
			MessageService.Current = client;

			//We need subscribe to all topics
			SubscribeAllTopics ();

			monitor.WriteLine ($"[Net] Host started in port: {Port}");
		}

		void SubscribeAllTopics ()
		{
			foreach (var msgDelegate in MessageDelegates) {
				Subscribe (msgDelegate.Topic);
				msgDelegate.SendMessage += (s, e) => {
					client.SendMessage (e.Item1, System.Text.Encoding.Default.GetBytes (e.Item2));
				};
			}
		}

		void Subscribe (string topic)
		{
			monitor.WriteLine ("[Net] Suscribed to: " + topic);
			client.Subscribe (topic);
		}

		void client_MqttMsgPublishReceived (object sender, MqttMsgPublishEventArgs e)
		{
			MessageDelegate msg;
			var message = System.Text.Encoding.Default.GetString (e.Message);
			if (e.Topic == "command") {
				msg = MessageDelegates.OfType<CommandMessageDelegate> ().FirstOrDefault (s => s.Id == message);
			} else {
				msg = MessageDelegates.FirstOrDefault (s => s.Topic == e.Topic);
			}
			if (msg != null) {
				msg.RaiseMessageReceived (e.Topic, e.Message);
				msg.RaiseMessageReceived (e.Topic, message);
			} else {
				throw new NotImplementedException (e.Topic);
			}
		}

		internal void Stop ()
		{
			client.Disconnect ();
			//KillProcess (process);
		}

		public void Dispose ()
		{
			Stop ();
		}
	}
}
