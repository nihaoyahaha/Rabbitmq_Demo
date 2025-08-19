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
	internal class EventingBasicConsumer : IRabbitMQConsumer
	{
		private readonly ILogger<EventingBasicConsumer> _logger;
		private readonly IChannel _channel;
		private AsyncEventingBasicConsumer _consumer;
		private string _consumerTag;

		public event Func<byte[], ulong, Task<bool>> MessageReceived;

		public EventingBasicConsumer(ILogger<EventingBasicConsumer> logger, IRabbitMQInitializer rabbitInitializer)
		{
			_logger = logger;
			_channel = rabbitInitializer.Channel;
			_consumer = new AsyncEventingBasicConsumer(_channel);
			_consumer.ReceivedAsync += OnMessageReceivedAsync;
		}

		private async Task OnMessageReceivedAsync(object sender, BasicDeliverEventArgs  eventArgs)
		{
			byte[] body = eventArgs.Body.ToArray();
			ulong deliveryTag = eventArgs.DeliveryTag;
			_logger.LogInformation("收到新消息，DeliveryTag: {Tag}", deliveryTag);
			try
			{
				bool success;
				if (MessageReceived == null) success = await Task.FromResult(false);
				// 调用客户端事件处理器
				success = await MessageReceived?.Invoke(body, deliveryTag);
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

		//开始监听队列
		public async Task StartConsumingAsync(string queue)
		{
			_logger.LogInformation("正在启动消费者，监听队列: {Queue}", queue);

			_consumerTag = await _channel.BasicConsumeAsync(
				queue: queue,//要监听的队列名字，这个队列必须已经存在
				autoAck: false,   // 手动确认，确认机制 true:消息一收到，RabbitMQ 就认为“处理完了”，立刻删除,如果程序崩溃，消息就丢了 false:收到消息后，必须手动调用 BasicAck 告诉 RabbitMQ“我处理完了”,安全 ✅，即使程序崩溃，消息会重新投递
				consumer: _consumer);//消费者对象
			
			_logger.LogInformation("消费者已启动，ConsumerTag: {Tag}", _consumerTag);
		}

		//停止监听队列
		public async Task StopConsumingAsync()
		{
			if (!string.IsNullOrEmpty(_consumerTag))
			{
				await _channel.BasicCancelAsync(_consumerTag);
				_logger.LogInformation($"消费者已停止: {_consumerTag}");
			}
		}

	}
}
