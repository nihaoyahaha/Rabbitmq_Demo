using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using RabbitMQ_Helper;
using Microsoft.Extensions.Configuration;

namespace Rabbitmq_Producer
{
    static class Program
	{
		private static IHost _host;

		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
        static void Main()
        {
			// 1. 创建 DI 容器（Host）
			_host = CreateHostBuilder().Build();

			// 2. 启动后台服务（比如自动初始化 RabbitMQ）
			_host.Start();

			Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

			// 4. 从 DI 容器中获取 MainForm（自动注入依赖）
			var mainForm = _host.Services.GetRequiredService<MainForm>();
			// 5. 运行窗体应用
			Application.Run(mainForm);

			// 6. 应用退出后释放资源
			_host.Dispose();
		}

		static IHostBuilder CreateHostBuilder()
		{
			
			var settings = new RabbitmqConfig();
			new ConfigurationBuilder()
				.AddJsonFile("RabbitmqConfig/RabbitmqConfig.json", optional: false, reloadOnChange: true)
				.Build()
				.Bind(settings);

			return Host
				.CreateDefaultBuilder()
				.ConfigureServices((context, services) =>
				{
					services.AddRabbitMQProducer(config =>
					{
						config.HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME") ?? "localhost";
						config.Port = settings.Port;
						config.UserName = settings.UserName;
						config.Password = settings.Password;
					});

					services.AddScoped<MainForm>();
				});
		}

	}
}
