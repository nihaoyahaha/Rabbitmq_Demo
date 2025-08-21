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
		public MainForm(IRabbitMQConsumer consumer_order, IRabbitMQConsumer consumer_inventory)
		{
			InitializeComponent();
			_consumer_order = consumer_order;
			_consumer_inventory = consumer_inventory;
			_consumer_order.MessageReceived += OnOrderMessageReceivedAsync;
			_consumer_inventory.MessageReceived += OnInventoryMessageReceivedAsync;
		}

		private async void btn_ReceiveMessage_Click(object sender, EventArgs e)
		{
			await _consumer_order.StartConsumingAsync("queue_order", "routingKey_exchange_order - Inventory");
			await _consumer_inventory.StartConsumingAsync("queue_inventory", "routingKey_exchange_order - Inventory");
		}

		private async void btn_StopReceiveMessage_Click(object sender, EventArgs e)
		{
			await _consumer_order.StopConsumingAsync();
			await _consumer_inventory.StopConsumingAsync();
		}

		private async Task<bool> OnOrderMessageReceivedAsync(byte[] messageBody, ulong deliveryTag)
		{
			string message = Encoding.UTF8.GetString(messageBody);
			try
			{
				await Task.Delay(100);
				SafeUpdateText(listBox_OrderMessages,message);
				return true;
			}
			catch 
			{
				return false;
			}
		}

		private async Task<bool> OnInventoryMessageReceivedAsync(byte[] messageBody, ulong deliveryTag)
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

		private void SafeUpdateText(ListBox listBox, string text)
		{
			if (listBox.InvokeRequired)
			{
				listBox.BeginInvoke(new Action(() => {
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

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			//_consumer_order.StopConsumingAsync().Wait(TimeSpan.FromSeconds(0.5));
			//_consumer_inventory.StopConsumingAsync().Wait(TimeSpan.FromSeconds(0.5));
		}


	}
}
