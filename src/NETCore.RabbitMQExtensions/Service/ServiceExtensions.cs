using System;
using Microsoft.Extensions.DependencyInjection;
using NETCore.RabbitMQExtensions.Service.Impl;

namespace NETCore.RabbitMQExtensions.Service
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.Add(ServiceDescriptor.Scoped<IPublishService, PublishService>());
            services.Add(ServiceDescriptor.Scoped<IConsumeService, ConsumeService>());
            return services;
        }
    }
}
