using System;
using RabbitMQ.Client;

namespace NETCore.RabbitMQExtensions
{
    public interface IRabbitMQProvider : IDisposable
    {
        RabbitMQOptions Options { get; }
        IConnection Connection { get; }
        IModel Model(string key);
    }
}
