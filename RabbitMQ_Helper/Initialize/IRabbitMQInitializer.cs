using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Helper
{
    public interface IRabbitMQInitializer
    {
		//交换机名
		string ExchangeName { get; }

		IConnection Connection { get; }
	
		//创建信道
		Task<IChannel> CreateChannelAsync(string type = ExchangeType.Direct);

		//声明队列和绑定
		Task DeclareQueueAndBindAsync(IChannel channel, string queueName, string routingKey);

		//初始化Rabbitmq连接
		Task CreateConnectionAsync();

	}
}
