using System;
using NETCore.RabbitMQExtensions.Service;
using NETCore.RabbitMQExtensions.Service.Impl;
using Xunit;

namespace NETCore.RabbitMQExtensions.Tests
{
    public class PublishTest
    {
        private readonly IPublishService _publish;

        public PublishTest()
        {
            var _rabbitMQProvider = new RabbitMQProvider(new RabbitMQOptions
            {
                Host = "127.0.0.1",
                Port = 5672,
                Username = "admin",
                Password = "admin"
            });
            _publish = new PublishService(_rabbitMQProvider);
        }

        [Fact]
        public void Test1()
        {
            _publish.Publish<string>("yellow", $"test_{DateTime.Now.ToString()}");
        }
    }
}
