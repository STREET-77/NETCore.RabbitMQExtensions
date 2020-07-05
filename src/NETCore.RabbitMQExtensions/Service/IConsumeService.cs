using System;
namespace NETCore.RabbitMQExtensions.Service
{
    public interface IConsumeService
    {
        void Consume(string routingKey, Action<ConsumerEventMessage> consumer);
        void Consume(string queue, string routingKey, Action<ConsumerEventMessage> consumer);
    }
}
