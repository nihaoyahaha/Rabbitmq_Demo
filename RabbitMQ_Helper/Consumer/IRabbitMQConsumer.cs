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
		//获取消息
        byte[] GetMessage();

		/// <summary>
		/// 开始监听队列
		/// </summary>
		/// <param name="queue">队列名</param>
		/// <returns></returns>
		Task<string> StartConsumingAsync(string queue);

		/// <summary>
		/// 停止监听队列
		/// </summary>
		/// <returns></returns>
		Task StopConsumingAsync();
	}
}
