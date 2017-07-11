using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M2MQTTtest2;
using System.Windows;
using Livet;
using System.Security.Cryptography.X509Certificates;

namespace MQTTSubscriberTest
{
	class MainWindowViewModel : ViewModel
	{
		private string _subscribeLog;
		public string SubscribeLog
		{
			get { return _subscribeLog; }
			set
			{
				_subscribeLog = value;
				RaisePropertyChanged(nameof(SubscribeLog));
			}
		}
		public string HostName { get; set; }
		public string Topic { get; set; }

		private bool _isSubscribe;
		public bool IsSubscribe
		{
			get { return _isSubscribe; }
			set
			{
				if (value == true)
				{
					if (Topic == null) return;
					if (Topic.Length == 0) return;

					_m2mqtt = new MqttClientWrapper(HostName);

					// ブローカー接続
					if (!_m2mqtt.ConnectToBroker() || !_m2mqtt.IsConnected())
					{
						MessageBox.Show("Connection Failed.");
					}

					_m2mqtt.AddSubscribeAction(SubscribeAction);

					// Subscribe開始
					_m2mqtt.Subscribe(new string[] { Topic });
				}
				else
				{
					// Subscribe終了
					_m2mqtt.Unsubscribe(new string[] { Topic });

					_m2mqtt.Disconnect();
				}
				_isSubscribe = value;
			}
		}

		private MqttClientWrapper _m2mqtt;

		public MainWindowViewModel()
		{
		}

		~MainWindowViewModel()
		{
		}

		private void SubscribeAction(string topic, string msg)
		{
			var log = topic + " : " + msg + "\n";
			SubscribeLog += log;
		}
	}
}
