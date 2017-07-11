
using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace M2MQTTtest2
{
    public class MqttClientWrapper
	{
		private MqttClient _client;

		#region Methods

		public MqttClientWrapper(string brokerHost)
		{
			//_client = new MqttClient(brokerHost, MqttSettings.MQTT_BROKER_DEFAULT_SSL_PORT, true, MqttSslProtocols.TLSv1_2, null, null);
			_client = new MqttClient(brokerHost);

			// ログ用
			receiveLogSetting();
		}

		public bool ConnectToBroker()
		{
			var ret = _client.Connect(Guid.NewGuid().ToString());
			Debug.WriteLine("Connected with result code {0}", ret);

			return ret == MqttMsgConnack.CONN_ACCEPTED;
		}

		public bool ConnectToBroker(string username, string password)
		{
			var ret = _client.Connect(Guid.NewGuid().ToString(), username, password);
			Debug.WriteLine("Connected with result code {0}", ret);

			return ret == MqttMsgConnack.CONN_ACCEPTED;
		}

		public bool Subscribe(string [] topics)
		{
			var ret = _client.Subscribe(topics, new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

			Debug.WriteLine("Subscribe {0}.", ret);

			return ret == 1;
		}

		public bool Unsubscribe(string [] topics)
		{
			var ret = _client.Unsubscribe(topics);

			Debug.WriteLine("Unsubscribe {0}.", ret);

			return ret == 2;
		}

		public void AddSubscribeAction(Action<string, string> func)
		{
			_client.MqttMsgPublishReceived += (sender, eventArgs) =>
			{
				var msg = Encoding.UTF8.GetString(eventArgs.Message);
				var topic = eventArgs.Topic;

				func(topic, msg);
			};
		}

		public bool Publish(string topic, string msg)
		{
			_client.Publish(topic, Encoding.UTF8.GetBytes(msg));

			Debug.WriteLine("Published {0}:{1}.", topic, msg);

			return true;
		}

		public bool IsConnected()
		{
			return _client.IsConnected;
		}

		public void Disconnect()
		{
			if (IsConnected()) _client.Disconnect();
		}

		private void receiveLogSetting()
		{
			if (_client == null) return;

			_client.MqttMsgPublishReceived += (sender, eventArgs) =>
			{
				var msg = Encoding.UTF8.GetString(eventArgs.Message);
				var topic = eventArgs.Topic;

				Debug.WriteLine(topic + ", " + msg);
			};
		}

		#endregion
	}
}
