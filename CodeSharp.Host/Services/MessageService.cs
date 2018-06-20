using System;
using CodeSharp;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class MessageService
{
	const int DefaultConnectionRetries = 5;

	public int ConnectionRetries { get; set; } = DefaultConnectionRetries;

	public static MessageService Current {
		get;
		set;
	}

	MqttClient client;

	public EventHandler<MqttMsgPublishEventArgs> MqttMsgPublishReceived;

	public string BrokerHost { get; private set; }

	public bool IsConnected => client.IsConnected;

	static CodeSession _session;

	readonly IMonitor monitor;
	public MessageService (IMonitor monitor)
	{
		this.monitor = monitor;
	}

	public void Connect (string host)
	{
		if (client != null) {
			client.MqttMsgPublishReceived -= client_MqttMsgPublishReceived;
			client.Disconnect ();
		}

		BrokerHost = host;

		client = new MqttClient (host);
		client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

		string clientId = Guid.NewGuid ().ToString ();

		Exception lastConnectionException = null;
		for (int i = 1; i <= ConnectionRetries && !client.IsConnected; i++) {
			try {
				monitor.WriteLine ($"[Net] Connecting to Broker ({host}) with client id: {clientId} ({i}/{ConnectionRetries}) ... ");
				client.Connect (clientId);

			} catch (Exception ex) {
				monitor.WriteLine ($"[Net] Error connecting! Check if your Broker is up!");
				lastConnectionException = ex;
				System.Threading.Thread.Sleep (2000);
			}
		}

		if (!client.IsConnected) {
			throw lastConnectionException;
		}
	}

	void client_MqttMsgPublishReceived (object sender, MqttMsgPublishEventArgs e)
	{
		MqttMsgPublishReceived?.Invoke (this, e);
	}

	static void Initialize (CodeSession session)
	{
		_session = session;
	}


	public void Subscribe (params string[] topic)
	{
		client.Subscribe (topic, new byte [] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
	}

	public void SendMessage (string topic, string message)
	{
		SendMessage (topic, System.Text.Encoding.Default.GetBytes (message));
	}

	public void SendMessage (string topic, byte[] message)
	{
		client.Publish (topic, message);
	}

	public void Disconnect ()
	{
		if (client.IsConnected) {
			client.Disconnect ();
		}
	}
}
