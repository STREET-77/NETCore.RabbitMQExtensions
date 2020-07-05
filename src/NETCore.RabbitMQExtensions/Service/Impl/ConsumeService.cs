using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NETCore.RabbitMQExtensions.Service.Impl
{
    public class ConsumeService : IConsumeService
    {
        private readonly IRabbitMQProvider _rabbitMQProvider;

        public ConsumeService(IRabbitMQProvider rabbitMQProvider)
        {
            _rabbitMQProvider = rabbitMQProvider;
        }

        public void Consume(string routingKey, Action<ConsumerEventMessage> consumer)
        {
            Consume(_rabbitMQProvider.Options.Queue, routingKey, consumer);
        }

        public void Consume(string queue, string routingKey, Action<ConsumerEventMessage> consumer)
        {
            var connection = _rabbitMQProvider.Connection;
            var channel = _rabbitMQProvider.Model(routingKey);

            var event_consumer = new EventingBasicConsumer(channel);
            event_consumer.Received += (ch, ea) =>
            {
                var consumerEventMessage = new ConsumerEventMessage
                {
                    Message = ea.Body.ToArray()
                };
                consumerEventMessage.OnBasicAck += () =>
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                };
                consumerEventMessage.OnBasicNack += (requeue) =>
                {
                    channel.BasicNack(ea.DeliveryTag, false, requeue);
                };

                consumer.Invoke(consumerEventMessage);
            };

            // 限流
            // 每次仅处理一条消息
            channel.BasicQos(0, 1, false);
            // 开启消息确认: autoAsk=false
            channel.BasicConsume(queue, false, event_consumer);
        }
    }
}
