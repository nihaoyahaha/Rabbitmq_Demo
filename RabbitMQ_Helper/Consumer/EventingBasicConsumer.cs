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
		private readonly ILogger<RabbitMQProducer> _logger;
		private readonly IChannel _channel;
		private AsyncEventingBasicConsumer _consumer;
		private byte[] _body;
		private string _consumerTag;
		public EventingBasicConsumer(ILogger<RabbitMQProducer> logger, IRabbitMQInitializer rabbitInitializer)
		{
			_logger = logger;
			_channel = rabbitInitializer.Channel;
			_consumer = new AsyncEventingBasicConsumer(_channel);
			_consumer.ReceivedAsync += ReceivedAsync;
		}

		private async Task ReceivedAsync(object sender, BasicDeliverEventArgs eventArgs)
		{
			//获取消息体
			_body = eventArgs.Body.ToArray();
			_logger.LogInformation("成功获取消息");
			//处理完消息后，发送确认（ACK）告诉 RabbitMQ：“这条消息我处理完了，可以删除了”
			await _channel.BasicAckAsync(eventArgs.DeliveryTag, false);
			_logger.LogInformation("消息处理确认");
		}

		public byte[] GetMessage() => _body;

		//开始监听队列
		public async Task<string> StartConsumingAsync(string queue)
		{
			_logger.LogInformation("正在启动消费者，监听队列: {Queue}", queue);

			_consumerTag = await _channel.BasicConsumeAsync(
				queue: "my_queue",//要监听的队列名字，这个队列必须已经存在
				autoAck: false,   // 手动确认，确认机制 true:消息一收到，RabbitMQ 就认为“处理完了”，立刻删除,如果程序崩溃，消息就丢了 false:收到消息后，必须手动调用 BasicAck 告诉 RabbitMQ“我处理完了”,安全 ✅，即使程序崩溃，消息会重新投递
				consumer: _consumer);//消费者对象
			
			_logger.LogInformation("消费者已启动，ConsumerTag: {Tag}", _consumerTag);
			return _consumerTag;
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
