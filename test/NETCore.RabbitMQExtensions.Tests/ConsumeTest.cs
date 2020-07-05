using System;
using System.Text;
using NETCore.RabbitMQExtensions.Service;
using NETCore.RabbitMQExtensions.Service.Impl;
using Xunit;

namespace NETCore.RabbitMQExtensions.Tests
{
    public class ConsumeTest
    {
        private readonly IConsumeService _consume;

        public ConsumeTest()
        {
            var _rabbitMQProvider = new RabbitMQProvider(new RabbitMQOptions
            {
                Host = "127.0.0.1",
                Port = 5672,
                Username = "admin",
                Password = "admin"
            });
            _consume = new ConsumeService(_rabbitMQProvider);
        }

        [Fact]
        public void Test1()
        {
            _consume.Consume("yellow", message =>
            {
                try
                {
                    string content = Encoding.UTF8.GetString(message.Message);
                    message.BasicAck();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            });
        }
    }
}
