using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ_Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Rabbitmq_Consumer
{
	public partial class MainForm : Form
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly IConfiguration _configuration;
		//消费者对象，队列名，路由规则映射
		private List<(IRabbitMQConsumer Consumer, string QueueName, string RoutingKey)> _consumers = new List<(IRabbitMQConsumer, string, string)>();

		public MainForm(IConfiguration configuration, IServiceProvider serviceProvider)
		{
			InitializeComponent();
			_configuration = configuration;
			_serviceProvider = serviceProvider;
			InitializeConsumer();
		}

		//动态创建消费者
		private void InitializeConsumer()
		{
			var queues = _configuration.GetSection("RabbitMQ:Queues").GetChildren();
			if (queues == null || !queues.Any())
			{
				MessageBox.Show("未找到队列配置！");
				return;
			}

			foreach (var queue in queues)
			{
				string queueName = queue.GetValue<string>("QueueName");
				string routingKey = queue.GetValue<string>("RoutingKey");
				var consumer = _serviceProvider.GetRequiredService<IRabbitMQConsumer>();

				if (queueName.Contains("order")) consumer.MessageReceived += ReceiveMessageFormOrderQueue;
				else if (queueName.Contains("inventory")) consumer.MessageReceived += ReceiveMessageFromInventoryQueue;
				else if (queueName.Contains("image")) consumer.MessageReceived += ReceiveMessageFromImageQueue;
				
				_consumers.Add((consumer, queueName, routingKey));
			}
		}

		//接收订单队列的消息
		private async Task<bool> ReceiveMessageFormOrderQueue(byte[] messageBody, ulong deliveryTag)
		{
			string message = Encoding.UTF8.GetString(messageBody);
			try
			{
				await Task.Delay(100);
				SafeUpdateText(listBox_OrderMessages, message);
				return true;
			}
			catch
			{
				return false;
			}
		}

		//接收库存队列的消息
		private async Task<bool> ReceiveMessageFromInventoryQueue(byte[] messageBody, ulong deliveryTag)
		{
			string message = Encoding.UTF8.GetString(messageBody);
			try
			{
				await Task.Delay(100);
				SafeUpdateText(listBox_InventoryMessages, message);
				return true;
			}
			catch
			{
				return false;
			}
		}

		//接收图片队列的消息
		private async Task<bool> ReceiveMessageFromImageQueue(byte[] imageBytes, ulong deliveryTag)
		{
			try
			{
				await Task.Delay(100);
				using (MemoryStream ms = new MemoryStream(imageBytes))
				{
					Bitmap bitmap = new Bitmap(Image.FromStream(ms));
					pictureBox1.BeginInvoke(new Action(() => {
						pictureBox1.Image = bitmap;
					}));			
				}
				return true;
			}
			catch
			{
				return false;
			}

		}

		//开始接收消息
		private async void btn_ReceiveMessage_Click(object sender, EventArgs e)
		{
			foreach (var consumer in _consumers)
			{
				await consumer.Consumer.StartConsumingAsync(consumer.QueueName, consumer.RoutingKey);
			}
		}

		//停止接收消息
		private async void btn_StopReceiveMessage_Click(object sender, EventArgs e)
		{
			foreach (var consumer in _consumers)
			{
				await consumer.Consumer.StopConsumingAsync();
			}
		}

		//设置文本
		private void SafeUpdateText(ListBox listBox, string text)
		{
			if (listBox.InvokeRequired)
			{
				listBox.BeginInvoke(new Action(() =>
				{
					listBox.Items.Add(text);
					listBox.TopIndex = listBox.Items.Count - 1;
				}));
			}
			else
			{
				listBox.Items.Add(text);
				listBox.TopIndex = listBox.Items.Count - 1;
			}
		}
	}
}
