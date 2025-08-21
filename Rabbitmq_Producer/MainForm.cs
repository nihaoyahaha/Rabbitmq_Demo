using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ_Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;
using IChannel = RabbitMQ.Client.IChannel;

namespace Rabbitmq_Producer
{
	public partial class MainForm : Form
	{
		private readonly ILogger<MainForm> _logger;
		private readonly IRabbitMQProducer _producer;
		public MainForm(IRabbitMQProducer producer, ILogger<MainForm> logger)
		{
			InitializeComponent();
			_producer = producer;
			_logger = logger;
		}

		private async void btn_send_Click(object sender, EventArgs e)
		{
			try
			{
				await _producer.PublishAsync($"[{DateTime.Now .ToString("yyyy/MM/dd HH:mm:ss")}]{txt_Message.Text}", "routingKey_exchange_order-Inventory");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				throw;
			}
		}

		private  void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			//_producer?.DisposeAsync().Wait(TimeSpan.FromSeconds(0.5));
		}
	}
}
