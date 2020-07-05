using System;
using Microsoft.Extensions.Options;

namespace NETCore.RabbitMQExtensions
{
    public class RabbitMQOptions : IOptions<RabbitMQOptions>
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 服务器端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 队列
        /// </summary>
        public string Queue { get; set; } = "helloworld";

        /// <summary>
        /// 死信队列
        /// </summary>
        public string DlxQueue { get; set; } = "dlx_helloworld";

        /// <summary>
        /// 交换机
        /// </summary>
        public string Exchange { get; set; } = "street77_dotnet";

        /// <summary>
        /// 死信队列交换机
        /// </summary>
        public string DlxExchange { get; set; } = "dlx_street77_dotnet";

        /// <summary>
        /// 交换机类型
        /// 默认：direct
        /// </summary>
        public string Type { get; set; } = ExchangeType.Direct;

        /// <summary>
        /// 创建死信队列
        /// </summary>
        public bool AndCreateDlxExchange { get; set; } = false;

        /// <summary>
        /// 队列的消息过期时间
        /// </summary>
        public int TTL { get; set; } = -1;

        /// <summary>
        /// 队列的消息最大长度
        /// </summary>
        public int Length { get; set; } = -1;

        /// <summary>
        /// 
        /// </summary>
        public RabbitMQOptions Value
        {
            get
            {
                return this;
            }
        }
    }
}
