using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace NETCore.RabbitMQExtensions.Service.Impl
{
    public class PublishService : IPublishService
    {
        private readonly IRabbitMQProvider _rabbitMQProvider;

        public PublishService(IRabbitMQProvider rabbitMQProvider)
        {
            _rabbitMQProvider = rabbitMQProvider;
        }

        public void Publish<TMessage>(string routingKey, TMessage message)
        {
            Publish(_rabbitMQProvider.Options.Queue, routingKey, message);
        }

        public void Publish<TMessage>(string queue, string routingKey, TMessage message)
        {
            Publish(_rabbitMQProvider.Options.Exchange, queue, routingKey, message);
        }

        public void Publish<TMessage>(string exchange, string queue, string routingKey, TMessage message)
        {
            var connection = _rabbitMQProvider.Connection;
            var channel = _rabbitMQProvider.Model(routingKey);

            var dictArgs = new Dictionary<string, object>();
            if (_rabbitMQProvider.Options.AndCreateDlxExchange)
            {
                // 设置死信队列
                dictArgs.Add("x-dead-letter-exchange", _rabbitMQProvider.Options.DlxExchange);
                channel.ExchangeDeclare(_rabbitMQProvider.Options.DlxExchange, _rabbitMQProvider.Options.Type, true, false);
                channel.QueueDeclare(_rabbitMQProvider.Options.DlxQueue, true, false, false);
                channel.QueueBind(_rabbitMQProvider.Options.DlxQueue, _rabbitMQProvider.Options.DlxExchange, routingKey);
            }
            if (_rabbitMQProvider.Options.TTL > 0)
            {
                // 设置过期事件
                dictArgs.Add("x-message-ttl", _rabbitMQProvider.Options.TTL);
            }
            if (_rabbitMQProvider.Options.Length > 0)
            {
                // 设置队列长度
                dictArgs.Add("x-max-length", _rabbitMQProvider.Options.Length);
            }

            channel.ExchangeDeclare(exchange, _rabbitMQProvider.Options.Type, true, false);
            channel.QueueDeclare(queue, true, false, false, dictArgs);
            channel.QueueBind(queue, exchange, routingKey);

            // 持久化消息
            var basicProperties = channel.CreateBasicProperties();
            basicProperties.Persistent = true;
            // 可在headers添加自定义信息，如：重试次数
            //basicProperties.Headers
            // 消息过期时间
            //basicProperties.Expiration

            // 开启消息发送确认
            //channel.ConfirmSelect();
            var bytes = message as byte[] ?? JsonSerializer.SerializeToUtf8Bytes<TMessage>(message);
            channel.BasicPublish(exchange, routingKey, basicProperties, bytes);
            //if (channel.WaitForConfirms())
            //{
            //}
            //else
            //{
            //}
        }

        public void PublishAsync<TMessage>(string routingKey, TMessage message)
        {
            Task.Factory.StartNew(() =>
            {
                Publish(_rabbitMQProvider.Options.Queue, routingKey, message);
            });
        }

        public void PublishAsync<TMessage>(string queue, string routingKey, TMessage message)
        {
            Task.Factory.StartNew(() =>
            {
                Publish(_rabbitMQProvider.Options.Exchange, queue, routingKey, message);
            });
        }

        public void PublishAsync<TMessage>(string exchange, string queue, string routingKey, TMessage message)
        {
            Task.Factory.StartNew(() =>
            {
                Publish(exchange, queue, routingKey, message);
            });
        }
    }
}
