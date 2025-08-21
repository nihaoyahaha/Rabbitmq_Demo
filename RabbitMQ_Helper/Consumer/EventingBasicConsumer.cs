using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace RabbitMQ_Helper.Consumer
{
	internal class EventingBasicConsumer : IRabbitMQConsumer, IAsyncDisposable
	{
		private readonly ILogger<EventingBasicConsumer> _logger;
		private readonly IRabbitMQInitializer _rabbitInitializer;

		private IChannel _channel;
		private AsyncEventingBasicConsumer _consumer;
		private string _consumerTag;
		private bool _isConsuming = false;

		public event Func<byte[], ulong, Task<bool>> MessageReceived;

		public EventingBasicConsumer(ILogger<EventingBasicConsumer> logger, IRabbitMQInitializer rabbitInitializer)
		{
			_logger = logger;
			_rabbitInitializer = rabbitInitializer;
		}

		//开始监听队列
		public async Task StartConsumingAsync(string queue, string routingKey)
		{
			if (_isConsuming)
			{
				_logger.LogWarning("消费者已在运行，跳过启动");
				return;
			}

			try
			{
				_logger.LogInformation("正在启动消费者，监听队列: {Queue}", queue);

				// 消费者自己创建通道
				_channel = await _rabbitInitializer.CreateChannelAsync();

				// 消费者负责声明队列和绑定（这是它的职责！）
				await _rabbitInitializer.DeclareQueueAndBindAsync(_channel, queue, routingKey);

				// 创建消费者对象
				_consumer = new AsyncEventingBasicConsumer(_channel);
				_consumer.ReceivedAsync += OnMessageReceivedAsync;

				//BasicConsume（推送）才是 高效、推荐 的实时消息处理方式,BasicGetAsync（拉取/轮询）是 低效、不推荐 的消费方式
				_consumerTag = await _channel.BasicConsumeAsync(
					queue: queue,//要监听的队列名字，这个队列必须已经存在
					autoAck: false,   // 手动确认，确认机制 true:消息一收到，RabbitMQ 就认为“处理完了”，立刻删除,如果程序崩溃，消息就丢了 false:收到消息后，必须手动调用 BasicAck 告诉 RabbitMQ“我处理完了”,安全 ✅，即使程序崩溃，消息会重新投递
					consumer: _consumer);//消费者对象

				_isConsuming = true;

				_logger.LogInformation("消费者已启动，ConsumerTag: {Tag}", _consumerTag);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "启动消费者失败");
				await DisposeChannelAsync();
				throw;
			}
		}

		//停止监听队列
		public async Task StopConsumingAsync()
		{
			if (!_isConsuming || string.IsNullOrEmpty(_consumerTag))
			{
				_logger.LogWarning("消费者已停止运行，跳过停止");
				return;
			}

			try
			{
				await _channel.BasicCancelAsync(_consumerTag);
				_logger.LogInformation("消费者已停止: {Tag}", _consumerTag);
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "取消消费者时出错");
			}
			finally
			{
				_isConsuming = false;
				_consumerTag = null;
			}
		}

		private async Task OnMessageReceivedAsync(object sender, BasicDeliverEventArgs eventArgs)
		{
			byte[] body = eventArgs.Body.ToArray();
			//RabbitMQ 向消费者推送消息时给每条消息分配的投递标签
			ulong deliveryTag = eventArgs.DeliveryTag;

			_logger.LogInformation("收到新消息，DeliveryTag: {Tag}", deliveryTag);

			try
			{
				if (MessageReceived == null) return;
				// 调用客户端事件处理器处理业务逻辑
				bool success = await MessageReceived?.Invoke(body, deliveryTag);
				if (success)
				{
					// 客户端处理成功，ACK
					await _channel.BasicAckAsync(deliveryTag, false);
					_logger.LogInformation("消息已确认 (ACK), DeliveryTag: {Tag}", deliveryTag);
				}
				else
				{
					// 客户端处理失败，NACK（可选择是否重回队列）
					await _channel.BasicNackAsync(
						deliveryTag: deliveryTag,
						multiple: false,
						requeue: true); // true: 重回队列；false: 进入死信队列
					_logger.LogWarning("消息处理失败，已 NACK (重回队列), DeliveryTag: {Tag}", deliveryTag);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "处理消息时发生异常, DeliveryTag: {Tag}", deliveryTag);
				await _channel.BasicNackAsync(deliveryTag, multiple: false, requeue: true);
			}
		}

		private async Task DisposeChannelAsync()
		{
			if (_channel == null) return;
			try
			{
				if (_channel.IsOpen)
				{
					await _channel.CloseAsync();
					_logger.LogInformation("信道已关闭");
				}
				await _channel.DisposeAsync();
				_logger.LogInformation("信道已销毁");
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "关闭消费者通道时出错");
			}
			finally
			{
				_channel = null;
			}
		}

		public async ValueTask DisposeAsync()
		{
			await StopConsumingAsync();
			await DisposeChannelAsync();
		}
	}
}
