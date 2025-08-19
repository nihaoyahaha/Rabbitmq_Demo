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

namespace Rabbitmq_Consumer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
			//创建 DI 容器（Host）
			IHost host = CreateHostBuilder().Build();

			//启动后台服务（比如自动初始化 RabbitMQ）
			host.Start();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var mainForm = host.Services.GetRequiredService<MainForm>();
			Application.Run(mainForm);

			host.Dispose();
		}

		static IHostBuilder CreateHostBuilder()
		{
			return Host
				.CreateDefaultBuilder()
				.ConfigureAppConfiguration((context, config) =>
				{
					// 加载 appsettings.json
					config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
					config.AddEnvironmentVariables();//从环境变量读取配置,将json配置覆盖
				})
				.ConfigureServices((context, services) =>
				{
					services.AddRabbitMQ(context.Configuration);
					services.AddScoped<MainForm>();
				});
		}
	}
}
