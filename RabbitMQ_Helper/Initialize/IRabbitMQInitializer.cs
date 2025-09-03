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
		string MainExchangeName { get; }

		//死信交换机名
		string DeadLetterExchangeName { get; }

		//创建信道
		Task<IChannel> CreateChannelAsync();

		//初始化Rabbitmq连接
		Task CreateConnectionAsync();
		
		//是否启用死信队列
		bool IsUseDeadLetter(string routingKey);
	}
}
