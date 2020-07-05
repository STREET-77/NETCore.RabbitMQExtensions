using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NETCore.RabbitMQExtensions.Service;

namespace NETCore.RabbitMQExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, Action<RabbitMQOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            services.AddOptions();
            services.Configure(options);
            services.Add(ServiceDescriptor.Scoped<IRabbitMQProvider, RabbitMQProvider>());
            services.AddService();

            return services;
        }

        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMQOptions>(configuration);
            services.AddScoped<IRabbitMQProvider, RabbitMQProvider>();
            services.AddService();

            return services;
        }
    }
}
