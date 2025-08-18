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
		
		//信道
		IChannel Channel { get; }

		//初始化Rabbitmq
		Task InitializeAsync();

		//关闭连接
        Task DisposeAsync();
	}
}
