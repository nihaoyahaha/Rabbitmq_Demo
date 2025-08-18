using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ_Helper.Consumer;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ_Helper
{
	public static class ServiceCollectionServiceExtensions
	{
		/// <summary>
		/// 添加 RabbitMQ Producer 服务
		/// </summary>
		public static IServiceCollection AddRabbitMQProducer(this IServiceCollection services, Action<RabbitMQConfig> configure)
		{
			var config = new RabbitMQConfig();
			configure(config);

			services.AddSingleton(config);
			services.AddSingleton<IRabbitMQInitializer, RabbitMQInitializer>();
			services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();
			services.AddSingleton<IRabbitMQConsumer, EventingBasicConsumer>();

			// 注册为 IHostedService，应用启动时自动初始化
			services.AddHostedService<RabbitMQProducerHostedService>();

			return services;
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
