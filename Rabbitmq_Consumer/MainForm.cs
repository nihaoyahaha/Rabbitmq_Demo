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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace Rabbitmq_Consumer
{
    public partial class MainForm: Form
    {
        private readonly IRabbitMQConsumer _consumer;
		private List<string> _messages = new List<string>();
		public MainForm(IRabbitMQConsumer consumer)
		{
			InitializeComponent();
			_consumer = consumer;
			_consumer.MessageReceived += OnMessageReceivedAsync;
		}

		private async void MainForm_Load(object sender, EventArgs e)
		{
			await _consumer.StartConsumingAsync("queue_order");
		}

		private async Task<bool> OnMessageReceivedAsync(byte[] messageBody, ulong deliveryTag)
		{
			string message = Encoding.UTF8.GetString(messageBody);
			try
			{
				await Task.Delay(100);
				SafeUpdateText(txt_Messages,message);
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
