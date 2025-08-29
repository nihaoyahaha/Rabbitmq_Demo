using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace RabbitMQ_Helper
{
	internal class RabbitMQInitializer : IRabbitMQInitializer,IAsyncDisposable
	{
		private readonly RabbitMQConfig _config;
		private readonly ILogger<RabbitMQInitializer> _logger;

		private IConnection _connection;

		//rabbitmq 服务器是否成功连接
		private bool _isInitialized = false;
		
		public IConnection Connection => _connection;

		/// <summary>
		/// 交换机名
		/// </summary>
		public string MainExchangeName => _config.MainExchange;

		/// <summary>
		/// 死信交换机名
		/// </summary>
		public string DeadLetterExchangeName => _config.DeadLetterExchange;

		public RabbitMQInitializer(RabbitMQConfig config, ILogger<RabbitMQInitializer> logger)
		{
			_config = config;
			_logger = logger;
		}

		/// <summary>
		/// 初始化连接和拓扑结构
		/// </summary>
		public async Task CreateConnectionAsync()
		{
			if (_isInitialized && _connection?.IsOpen == true) return;
			try
			{
				var factory = new ConnectionFactory()
				{
					HostName = _config.HostName,
					Port = _config.Port,
					UserName = _config.UserName,
					Password = _config.Password,
					AutomaticRecoveryEnabled = true, // 启用自动恢复（断线重连）
					TopologyRecoveryEnabled =true,//在连接断开重连后，是否自动重新声明你之前创建的交换机、队列、绑定和消费者。
					NetworkRecoveryInterval = TimeSpan.FromSeconds(30)//设置重试间隔（默认 5 秒）
				};

				_logger.LogInformation("正在连接 RabbitMQ...");
				_connection = await factory.CreateConnectionAsync("RabbitMQInitializer");

				_isInitialized = true;
				_logger.LogInformation("RabbitMQ 连接创建成功。");

				await InitializeTopologyAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError("RabbitMQ 连接失败失败,{param}",ex.ToString());
				throw;
			}
		}

		/// <summary>
		/// 初始化所有交换机、队列、死信队列等拓扑结构
		/// </summary>
		/// <returns></returns>
		private async Task InitializeTopologyAsync()
		{
			using (var channel = await _connection.CreateChannelAsync())
			{
				// 声明主交换机
				await channel.ExchangeDeclareAsync(
					_config.MainExchange,
					ExchangeType.Direct,//直连型:按“路由键”精确匹配、Topic:模糊匹配、Fanout:广播！所有队列都发一份
					durable: true);//持久化

				_logger.LogInformation("Rabbit 成功声明交换机,exchangeName:{param}", _config.MainExchange);

				// 声明死信交换机
				await channel.ExchangeDeclareAsync(
					_config.DeadLetterExchange,
					ExchangeType.Direct,
					durable: true);

				_logger.LogInformation("Rabbit 成功声明死信交换机,exchangeName:{param}", _config.DeadLetterExchange);

				foreach (var queueConfig in _config.Queues)
				{
					//声明绑定队列
					await DeclareQueueAndBindAsync(channel, queueConfig);
				}
			}
		}

		/// <summary>
		/// 创建信道
		/// </summary>
		/// <returns></returns>
		public async Task<IChannel> CreateChannelAsync()
		{
			if (_connection == null || !_connection.IsOpen)
			{
				await CreateConnectionAsync();
			}
			try
			{
				_logger.LogInformation("RabbitMQ 信道创建中...");
				IChannel channel = await _connection.CreateChannelAsync();
				_logger.LogInformation("RabbitMQ 信道创建成功。");
				return channel;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "创建 RabbitMQ 信道失败");
				throw;
			}
			

			#region 扩展 被动模式
			/*
			 “普通声明” vs “被动声明”
			  操作	           普通声明 QueueDeclare	            被动声明 QueueDeclarePassive
			  目的	        “我要用这个队列，没有就建一个”	    “我只想知道这个队列存不存在”
			  行为	        不存在 → 创建；存在 → 什么也不做	    不存在 → 报错；存在 → 返回信息
			  是否创建队列	✅ 是	                        ❌ 否
			  适用场景	    应用启动时初始化	                监控、健康检查、调试

			try
			{
				//被动声明
				QueueDeclareOk response = await _channel.QueueDeclarePassiveAsync(_queueName);
				uint messageCount = response.MessageCount;//队列中“就绪状态”的消息数量（还没被消费的）
				if (messageCount > 1000)
				{
					Console.WriteLine("⚠️ 订单积压严重！可能消费者挂了");
				}
				uint consumerCount = response.ConsumerCount;//当前有多少个消费者在监听这个队列
				if (consumerCount == 0)
				{
					Console.WriteLine("🔔 没有消费者！消息会一直堆积");
				}
			}
			catch (Exception )
			{
				Console.WriteLine("❌ 队列不存在！系统可能没启动或配置错了");
			}
			 */
			#endregion

			#region 扩展 无等待模式
			/*
			 标准模式 vs 无等待模式
					  操作	            标准模式（noWait: false）	    无等待模式（noWait: true）
				 是否等待服务器回复	           ✅ 是	                     ❌ 否
				 性能	                       稍慢（有网络往返）	         更快（发完就走）
				 安全性	                       高（知道操作成功）	         低（不知道是否成功）
				 适用场景	                       大多数情况	                 高频变动、性能敏感

			✅ 优点：更快！没有网络等待，吞吐量更高
			❌ 缺点：你不知道队列到底有没有建成功

			只有在极少数高性能、拓扑频繁变动的场景下才用：

			✅ 场景 1：每秒创建成千上万个临时队列（罕见！）
			比如你在一个动态路由系统中，每个用户都有一个临时队列，用完就删。
			for (int i = 0; i < 10000; i++)
			{
				channel.QueueDeclareAsync($"temp_queue_{i}", ..., noWait: true);
			}
			这时省去 10000 次网络等待，性能提升明显。

			✅ 场景 2：你100% 确信队列已经存在
			比如你在一个集群中，由一个“初始化服务”提前建好了所有队列，其他服务只是“假装声明一下”。

			但这种情况不如直接跳过声明。

			await _channel.QueueDeclareAsync(
				_queueName, 
				durable: false, 
				exclusive: false,
				autoDelete: false, 
				arguments: null,
				noWait :true);//⚠️ 不等服务器回复
			 */
			#endregion

			#region 删除队列

			//强制删除（不管有没有人用、有没有消息）
			//⚠️ 危险！ 会把未处理的消息直接丢弃。
			//await _channel.QueueDeleteAsync(_queueName, ifUnused: false, ifEmpty: false);

			//只有队列为空才删
			//✅ 安全，避免误删未处理消息。
			//await _channel.QueueDeleteAsync(_queueName, ifUnused: false, ifEmpty: true);

			//只有未被使用才删(必须没人订阅这个队列（没有消费者），才能删)
			//✅ 常用于动态队列清理，比如临时队列用完就删。
			//await _channel.QueueDeleteAsync(_queueName, ifUnused: true, ifEmpty: false);

			//又空又没人用才删（最安全）
			//await _channel.QueueDeleteAsync(_queueName, ifUnused: true, ifEmpty: true);

			//清空队列
			//删除队列中所有“就绪状态”的消息,队列本身还存在
			//可以继续接收新消息
			//await _channel.QueuePurgeAsync(_queueName);

			//删除交换机，所有绑定它的队列就收不到消息了
			//await _channel.ExchangeDeleteAsync(_queueName);
			#endregion		
		}

		/// <summary>
		/// 声明队列和绑定
		/// </summary>
		/// <param name="channel">信道</param>
		/// <param name="exchange">交换机</param>
		/// <param name="queueName">队列名称</param>
		/// <param name="routingKey">路由规则</param>
		/// <returns></returns>
		private async Task DeclareQueueAndBindAsync(IChannel channel,QueueConfig queueConfig)
		{
			Dictionary<string, object> args = new Dictionary<string, object>();
			args["x-retry-count"] = 0;
			args["isUseDeadLetter"] = queueConfig.UseDeadLetter;//是否使用死信队列
			if (!string.IsNullOrEmpty(_config.DeadLetterExchange))
			{
				args["x-dead-letter-exchange"] = _config.DeadLetterExchange;
			}
			if (!string.IsNullOrEmpty(queueConfig.DLRoutingKey))
			{
				args["x-dead-letter-routing-key"] = queueConfig.DLRoutingKey;
			}

			//声明订单队列（作用:是消息的“暂存地”,等着对应的消费者来取任务处理,一个队列可以有多个消费者一起取任务处理) 如果这个队列不存在 → 创建它,如果已经存在 → 直接用
			await channel.QueueDeclareAsync(
				queueConfig.QueueName, //订单队列队列名
				durable: true, //不持久化 → 重启后队列消失
				exclusive: false, //不独占 → 其他连接也能用 
				autoDelete: false, //不自动删除 → 即使没消费者也不删
				arguments: args);//无额外参数(它允许你为队列（或交换机、绑定）添加额外的、自定义的配置参数，就像给队列“贴标签”或“加插件”。)

			_logger.LogInformation("rabbitMQ 成功声明队列,队列名:{param}", queueConfig.QueueName);

			//绑定队列-定规则，队列愿意接收什么样的消息， 意思是：“请把交换机x 中 routingKey = '规则x' 的消息，转发到 队列x”
			await channel.QueueBindAsync(
				queueConfig.QueueName,  //队列名
				_config.MainExchange,// //交换机名
			    queueConfig.RoutingKey);//路由规则 

			_logger.LogInformation("rabbitMQ 队列成功绑定交换机,交换机:{exchange},队列:{queue},规则:{routingKey}", _config.MainExchange, queueConfig.QueueName, queueConfig.RoutingKey);

			//声明死信队列
			await channel.QueueDeclareAsync(
				queueConfig.DeadLetterQueueName, 
				durable: true, 
				exclusive: false, 
				autoDelete: false, 
				arguments: null);

			_logger.LogInformation("rabbitMQ 成功声明死信队列,死信队列名:{queue}", queueConfig.DeadLetterQueueName);

			//死信队列绑定规则
			await channel.QueueBindAsync(
				queueConfig.DeadLetterQueueName,  //队列名
				_config.DeadLetterExchange,// //交换机名
				queueConfig.DLRoutingKey);//路由规则 
			_logger.LogInformation("rabbitMQ 死信队列成功绑定交换机,死信交换机:{exchange},死信队列:{queue},规则:{routingKey}", _config.DeadLetterExchange, queueConfig.DeadLetterQueueName, queueConfig.DLRoutingKey);
		}

		/// <summary>
		/// 释放资源
		/// </summary>
		public async ValueTask DisposeAsync()
		{
			if (_connection != null)
			{
				try
				{
					if (_connection.IsOpen)
					{
						await _connection.CloseAsync();
						_logger.LogInformation("RabbitMQ连接已关闭!");
					} 
					
					await _connection.DisposeAsync();
					_logger.LogInformation("RabbitMQ连接已销毁!");
				}
				catch (Exception ex)
				{
					_logger.LogWarning(ex, "关闭连接时出错");
				}
				finally
				{
					_connection = null; //防止后续误用
					_isInitialized = false;
				}
			}
		}
	}
}
