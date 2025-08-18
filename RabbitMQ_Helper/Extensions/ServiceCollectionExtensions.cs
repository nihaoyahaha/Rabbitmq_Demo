using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ_Helper.Consumer;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ_Helper
{
	public static class ServiceCollectionServiceExtensions
	{
		/// <summary>
		/// 添加 RabbitMQ Producer 服务
		/// </summary>
		public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration, string sectionName = "RabbitMQ")
		{
			RabbitMQConfig config = configuration.GetSection(sectionName).Get<RabbitMQConfig>()
			?? throw new InvalidOperationException($"找不到配置节点: {sectionName}");

			ValidateConfig(config);

			services.AddSingleton(config);
			services.AddSingleton<IRabbitMQInitializer, RabbitMQInitializer>();
			services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();
			services.AddSingleton<IRabbitMQConsumer, EventingBasicConsumer>();

			// 注册为 IHostedService，应用启动时自动初始化
			services.AddHostedService<RabbitMQProducerHostedService>();

			return services;
		}

		private static void ValidateConfig(RabbitMQConfig config)
		{
			if (string.IsNullOrEmpty(config.HostName))
				throw new ArgumentException("HostName 不能为空");
			if (config.Port <= 0 || config.Port > 65535)
				throw new ArgumentException("Port 必须在 1-65535 之间");
			if (string.IsNullOrEmpty(config.UserName))
				throw new ArgumentException("UserName 不能为空");
			if (string.IsNullOrEmpty(config.Password))
				throw new ArgumentException("Password 不能为空");
		}
	}



	/// <summary>
	/// 后台服务：在应用启动时初始化 Producer
	/// </summary>
	internal class RabbitMQProducerHostedService : IHostedService
	{
		private readonly IRabbitMQInitializer _initializer;
		private readonly ILogger<RabbitMQProducerHostedService> _logger;

		public RabbitMQProducerHostedService(IRabbitMQInitializer initializer, ILogger<RabbitMQProducerHostedService> logger)
		{
			_initializer = initializer;
			_logger = logger;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			try
			{
				await _initializer.InitializeAsync();
				_logger.LogInformation("RabbitMQ 初始化成功!");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "RabbitMQ 初始化失败!");
				throw;
			}
		}

		public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
	}
}
