using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Helper
{
    public interface IRabbitMQProducer
	{
		Task PublishAsync(byte [] message, string routingKey, string messageId = null);
	}
}
