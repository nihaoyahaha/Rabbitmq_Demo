using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Helper
{
    public interface IRabbitMQConsumer
	{
		//消息接收事件 参数：消息体、DeliveryTag
		event Func<byte[], ulong, Task<bool>> MessageReceived;

		/// <summary>
		/// 开始监听队列
		/// </summary>
		/// <param name="queue">队列名</param>
		/// <returns></returns>
		Task StartConsumingAsync(string queue);

		/// <summary>
		/// 停止监听队列
		/// </summary>
		/// <returns></returns>
		Task StopConsumingAsync();
	}
}
