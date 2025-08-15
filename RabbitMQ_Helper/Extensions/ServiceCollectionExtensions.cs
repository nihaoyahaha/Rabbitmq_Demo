using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
			services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();

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
		private readonly IRabbitMQProducer _producer;
		private readonly ILogger<RabbitMQProducerHostedService> _logger;

		public RabbitMQProducerHostedService(IRabbitMQProducer producer, ILogger<RabbitMQProducerHostedService> logger)
		{
			_producer = producer;
			_logger = logger;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			try
			{
				await _producer.InitializeAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "RabbitMQ Producer 启动失败");
				throw;
			}
		}

		public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
	}
}
