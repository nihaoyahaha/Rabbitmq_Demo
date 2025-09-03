using DrawKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ_Helper;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Rabbitmq_Producer
{
	public partial class MainForm : Form
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly ILogger<MainForm> _logger;
		private readonly IRabbitMQProducer _producer;
		private readonly IConfiguration _configuration;

		public MainForm(IRabbitMQProducer producer, ILogger<MainForm> logger, IConfiguration configuration, IServiceProvider serviceProvider)
		{
			InitializeComponent();
			_producer = producer;
			_logger = logger;
			_configuration = configuration;
			_serviceProvider = serviceProvider;
		}
		private void MainForm_Load(object sender, EventArgs e)
		{
			LoadRoutingKeys();
		}

		//发送文本
		private async void btn_send_Click(object sender, EventArgs e)
		{
			try
			{
				string message = $"[{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}]{txt_Message.Text},路由规则:{cmb_RoutingKey.Text}";
				//消息体 → 就是你要传的内容（必须是 byte[]）
				byte[] messageBodyBytes = Encoding.UTF8.GetBytes(message);
				await _producer.PublishAsync(messageBodyBytes, cmb_RoutingKey.Text);
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

		//发送图片
		private async void btn_sendPicture_Click(object sender, EventArgs e)
		{
			try
			{
				byte[] messageBodyBytes = ConvertImageToByteArray();
				await _producer.PublishAsync(messageBodyBytes, cmb_RoutingKey.Text);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				throw;
			}
		}

		private byte[] ConvertImageToByteArray()
		{
			using (MemoryStream ms = new MemoryStream())
			{
				pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
				return ms.ToArray();
			}
		}

		private void btn_openFile_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Filter = "PNG Image|*.png|All Files|*.*",
				Title = "打开图片"
			};

			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					using (Bitmap tempBitmap = new Bitmap(openFileDialog.FileName))
					{
						pictureBox1.Image = new Bitmap(tempBitmap);
					}

				}
				catch (Exception ex)
				{
					MessageBox.Show($"无法加载图像：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void btn_drawPicture_Click(object sender, EventArgs e)
		{
			var form = _serviceProvider.GetRequiredService<CanvasForm>();
			form.ShowDialog();
		}
	}
	
}
