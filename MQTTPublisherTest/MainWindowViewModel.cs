﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M2MQTTtest2;
using System.Windows;
using Livet;
using Livet.Commands;

namespace MQTTPublisherTest
{
	class MainWindowViewModel : ViewModel
	{
		public string HostName { get; set; } = "192.168.10.11";
		public string Topic { get; set; }
		public string Contents { get; set; }

		private ViewModelCommand _publish;
		public ViewModelCommand Publish => (_publish = _publish ?? new ViewModelCommand(ExecutePublish));

		private void ExecutePublish()
		{
			var _m2mqtt = new MqttClientWrapper(HostName);

			// ブローカー接続
			if (!_m2mqtt.ConnectToBroker() || !_m2mqtt.IsConnected())
			{
				MessageBox.Show("Connection Failed.");
			}

			_m2mqtt.Publish(Topic, Contents);

			_m2mqtt.Disconnect();
		}

		public MainWindowViewModel()
		{
		}

		~MainWindowViewModel()
		{
		}
	}
}
