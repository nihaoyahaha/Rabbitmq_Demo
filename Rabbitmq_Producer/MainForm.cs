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
		private readonly IConfiguration _configuration;
		public MainForm(IRabbitMQProducer producer, ILogger<MainForm> logger, IConfiguration configuration)
		{
			InitializeComponent();
			_producer = producer;
			_logger = logger;
			_configuration = configuration;
		}
		private void MainForm_Load(object sender, EventArgs e)
		{
			LoadRoutingKeys();
		}

		private async void btn_send_Click(object sender, EventArgs e)
		{
			try
			{
				await _producer.PublishAsync($"[{DateTime.Now .ToString("yyyy/MM/dd HH:mm:ss")}]{txt_Message.Text},路由规则:{cmb_RoutingKey.Text}", cmb_RoutingKey.Text);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				throw;
			}
		}

		private void LoadRoutingKeys()
		{
			// 读取 RabbitMQ -> Queues 数组
			var queues = _configuration.GetSection("RabbitMQ:Queues").GetChildren();

			foreach (var queue in queues)
			{
				string routingKey = queue.GetValue<string>("RoutingKey");
				if (!string.IsNullOrEmpty(routingKey))
				{
					cmb_RoutingKey.Items.Add(routingKey);
				}
			}
			if (cmb_RoutingKey.Items.Count > 0) cmb_RoutingKey.SelectedIndex = 0;
		}


	}
}
