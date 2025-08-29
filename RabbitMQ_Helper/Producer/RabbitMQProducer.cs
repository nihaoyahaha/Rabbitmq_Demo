using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace RabbitMQ_Helper
{
	internal class RabbitMQProducer : IRabbitMQProducer
	{
		private readonly ILogger<RabbitMQProducer> _logger;
		private readonly IRabbitMQInitializer _rabbitInitializer;
		
		public RabbitMQProducer(ILogger<RabbitMQProducer> logger, IRabbitMQInitializer rabbitInitializer)
		{
			_logger = logger;
			_rabbitInitializer = rabbitInitializer;
		}

		//消息先到交换机，再按绑定规则投递。绑定在哪台交换机，就必须把消息发给那台交换机
		public async Task PublishAsync(string message, string routingKey, string messageId = null)
		{
			if (string.IsNullOrEmpty(message)) 
				throw new ArgumentException("消息不能为空", nameof(message));

			using (var channel = await _rabbitInitializer.CreateChannelAsync())
			{
				try
				{
					//注册处理无法路由的消息的事件
					channel.BasicReturnAsync += BasicReturnAsync;

					//消息体 → 就是你要传的内容（必须是 byte[]）
					byte[] messageBodyBytes = Encoding.UTF8.GetBytes(message);
					BasicProperties props = CreateBasicProperties(messageId);

					await channel.BasicPublishAsync(
						exchange: _rabbitInitializer.MainExchangeName,
						routingKey: routingKey,
						mandatory: true,// ⚠️ 强制投递  false（默认）：消息发出去就不管了、true：必须成功投递到至少一个队列，否则触发 Return 事件
						basicProperties: props,
						body: messageBodyBytes);



					_logger.LogInformation($"消息已发布: RoutingKey='{routingKey}', MessageId='{props.MessageId}'");
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "发送消息失败: {Message}", ex.Message);
					throw;
				}

			}
		}

		//设置消息属性
		private BasicProperties CreateBasicProperties(string messageId)
		{
            /*
              设置消息属性 IBasicProperties
              BasicProperties 是消息的“元数据”，就像快递包裹上的标签。
              属性	                作用	                      示例
              ContentType	     消息内容类型	      "text/plain", "application/json"
              DeliveryMode	     是否持久化	      2 = 持久化，1 = 非持久化
              MessageId	         消息唯一 ID	      用于去重
              Timestamp	         时间戳	          记录发送时间
              Expiration	     过期时间（毫秒）	  "3600000" = 1小时
              Headers	         自定义键值对	      地理位置、用户ID等
              ReplyTo	         回复地址	          用于“请求-响应”模式
              CorrelationId	     关联 ID    	      跟踪一次调用链
            */
			//消息的“身份证”和“说明书”（属性）
			BasicProperties props = new BasicProperties();
			props.ContentType = "text/plain";

			//发一个“持久化”的消息
			props.DeliveryMode = DeliveryModes.Persistent;// ⭐1:不持久化 2:持久化  持久化: 即使 RabbitMQ 重启，消息也不丢

			//唯一ID
			props.MessageId = messageId ?? Guid.NewGuid().ToString();

			//发送时间
			props.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

			//设置消息过期时间（TTL）单位:毫秒,1分钟 = 60000毫秒
			//_props.Expiration = "60000"; // 1分钟过期

			//发消息带“自定义头”（Headers）
			props.Headers = new Dictionary<string, object>() {
						{ "latitude", 51.5252949 },
						{"longitude", -0.0905493 },
						{ "Test", "TestData"}
					};

			return props;
		}

		//没有队列能接收消息时,这条消息原路退回
		private Task BasicReturnAsync(object sender, BasicReturnEventArgs eventArgs)
		{
			string exchange = eventArgs.Exchange;
			string routingKey = eventArgs.RoutingKey;
			string replyText = eventArgs.ReplyText;  // 比如 "NO_ROUTE"
			byte[] body = eventArgs.Body.ToArray();

			_logger.LogWarning($"交换机[{exchange}]无法路由规则为[{ routingKey}]的消息，消息退回！原因: {replyText}");
			_logger.LogWarning($"原始消息: {Encoding.UTF8.GetString(body)}");
			return Task.CompletedTask;
		}

	}
}
