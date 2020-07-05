using System;
namespace NETCore.RabbitMQExtensions.Service
{
    public interface IPublishService
    {
        void Publish<TMessage>(string routingKey, TMessage message);
        void Publish<TMessage>(string queue, string routingKey, TMessage message);
        void Publish<TMessage>(string exchange, string queue, string routingKey, TMessage message);
        void PublishAsync<TMessage>(string routingKey, TMessage message);
        void PublishAsync<TMessage>(string queue, string routingKey, TMessage message);
        void PublishAsync<TMessage>(string exchange, string queue, string routingKey, TMessage message);
    }
}
