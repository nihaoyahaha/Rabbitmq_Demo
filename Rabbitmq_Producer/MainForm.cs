using Microsoft.Extensions.Configuration;
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
		private readonly IRabbitMQProducer _producer;
		public MainForm(IRabbitMQProducer producer)
		{
			InitializeComponent();
			_producer = producer;
		}

		private async void btn_send_Click(object sender, EventArgs e)
		{
			try
			{
				await _producer.PublishAsync(txt_Message.Text, "routingKey_exchange_order-Inventory");
				MessageBox.Show("消息发送成功！");
			}
			catch (Exception ex)
			{
				MessageBox.Show("发送失败：" + ex.Message);
			}
		}


	}
}
