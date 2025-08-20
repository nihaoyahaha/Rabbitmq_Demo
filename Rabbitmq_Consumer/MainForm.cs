using RabbitMQ_Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Rabbitmq_Consumer
{
    public partial class MainForm: Form
    {
        private readonly IRabbitMQConsumer _consumer_order;
		private readonly IRabbitMQConsumer _consumer_inventory;
		private List<string> _messages = new List<string>();
		public MainForm(IRabbitMQConsumer consumer_order, IRabbitMQConsumer consumer_inventory)
		{
			InitializeComponent();
			_consumer_order = consumer_order;
			_consumer_inventory = consumer_inventory;
			_consumer_order.MessageReceived += OnMessageReceivedAsync;
			_consumer_inventory.MessageReceived += OnMessageReceivedAsync;
		}

		private async void MainForm_Load(object sender, EventArgs e)
		{
			await _consumer_order.StartConsumingAsync("queue_order", "routingKey_exchange_order - Inventory");
			await _consumer_inventory.StartConsumingAsync("queue_inventory", "routingKey_exchange_order - Inventory");
		}

		private async Task<bool> OnMessageReceivedAsync(byte[] messageBody, ulong deliveryTag)
		{
			string message = Encoding.UTF8.GetString(messageBody);
			try
			{
				await Task.Delay(100);
				SafeUpdateText(txt_OrderMessages,message);
				SafeUpdateText(txt_InventoryMessages, message);
				return true;
			}
			catch 
			{
				return false;
			}
		}

		private void SafeUpdateText(TextBox textBox, string text)
		{
			if (textBox.InvokeRequired)
			{
				textBox.BeginInvoke(new Action(() => textBox.Text = text));
			}
			else
			{
				textBox.Text = text;
			}
		}

	}
}
