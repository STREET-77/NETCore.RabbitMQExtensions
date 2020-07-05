using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace NETCore.RabbitMQExtensions
{
    public class RabbitMQProvider : IRabbitMQProvider
    {
        private ConcurrentDictionary<string, IModel> _channelDict;

        public RabbitMQProvider(IOptions<RabbitMQOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Options = options.Value;
        }

        public RabbitMQOptions Options { get; }

        public IConnection Connection
        {
            get
            {
                return new Lazy<IConnection>(InitConnection).Value;
            }
        }

        public void Dispose()
        {
            if (Connection != null)
            {
                Connection.Close();
            }
        }

        public IModel Model(string key)
        {
            return _channelDict.GetOrAdd(key, l =>
            {
                return Connection.CreateModel();
            });
        }

        private IConnection InitConnection()
        {
            if (string.IsNullOrEmpty(Options.Host))
            {
                throw new ArgumentNullException(nameof(Options.Host));
            }

            var connectionFactory = new ConnectionFactory
            {
                HostName = Options.Host,
                AutomaticRecoveryEnabled = true
            };

            if (Options.Port > 0)
            {
                connectionFactory.Port = Options.Port;
            }
            if (!string.IsNullOrEmpty(Options.Username))
            {
                connectionFactory.UserName = Options.Username;
            }
            if (!string.IsNullOrEmpty(Options.Password))
            {
                connectionFactory.Password = Options.Password;
            }

            _channelDict = new ConcurrentDictionary<string, IModel>();
            return connectionFactory.CreateConnection();
        }
    }
}
