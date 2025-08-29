using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Helper
{
    internal class RabbitMQConfig
    {
		public RabbitMQConfig() { }

		/// <summary>
		/// RabbitMQ服务器地址
		/// </summary>
		public string HostName { get; set; }

		/// <summary>
		/// 端口号
		/// </summary>
		public int Port { get; set; }

		/// <summary>
		/// 用户名
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// 主交换机
		/// </summary>
		public string MainExchange { get; set; }

		/// <summary>
		/// 队列集
		/// </summary>
		public List<QueueConfig> Queues { get; set; }

		/// <summary>
		/// 死信交换机
		/// </summary>
		public string DeadLetterExchange { get; set; }
	}

	internal class QueueConfig
	{
		/// <summary>
		/// 队列名
		/// </summary>
		public string QueueName { get; set; }

		/// <summary>
		/// 路由规则
		/// </summary>
		public string RoutingKey { get; set; }

		/// <summary>
		/// 是否使用死信队列
		/// </summary>
		public bool UseDeadLetter { get; set; } = true;

		/// <summary>
		/// 死信队列路由规则
		/// </summary>
		public string DLRoutingKey { get; set; }

		/// <summary>
		/// 死信队列
		/// </summary>
		public string DeadLetterQueueName
		{
			get { return $"{QueueName}.dlq";} 
		}
	}
}
