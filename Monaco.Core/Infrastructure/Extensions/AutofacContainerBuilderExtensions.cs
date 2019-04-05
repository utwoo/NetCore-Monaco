using System;
using System.Linq;
using System.Reflection;
using Autofac;
using MassTransit;
using MassTransit.AutofacIntegration;
using Microsoft.Extensions.Configuration;
using Monaco.Core.EventConsumer;

namespace Monaco.Core.Infrastructure.Extensions
{
    public static class AutofacContainerBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        public static void RegisterRabbitMQComponents(this ContainerBuilder builder, IConfiguration configuration)
        {
            // Register Consumers Factory
            builder.RegisterGeneric(typeof(AutofacConsumerFactory<>)).As(typeof(IConsumerFactory<>)).InstancePerLifetimeScope();
            // Register Bus Control
            builder.Register((context) =>
            {
                // Create bus control by factory method
                var busControl = Bus.Factory.CreateUsingRabbitMq(server =>
                {
                    var host = server.Host(new Uri(configuration["RabbitMQ:Server"]), cfg =>
                    {
                        cfg.Username(configuration["RabbitMQ:Username"]);
                        cfg.Password(configuration["RabbitMQ:Password"]);
                    });

                    // Recieve Event Consumers Setting Types
                    var rabbitEventConsumersSettings =
                        configuration["Autofac:LoadAssemblies"]
                        .Split(';')
                        .Select(assemblyName =>
                        {
                            var assembly = Assembly.Load(assemblyName);
                            return assembly.GetTypes()
                                    .Where(type => !type.IsInterface && type.GetInterfaces()
                                    .Contains(typeof(IRabbitEventConsumerRegistrar)));
                        })
                        .SelectMany(types => types)
                        .Select(type => (IRabbitEventConsumerRegistrar)Activator.CreateInstance(type))
                        .OrderBy(setting => setting.Order);

                    // Register Event Consumer
                    foreach (var rabbitEventConsumersSetting in rabbitEventConsumersSettings)
                        rabbitEventConsumersSetting.RegisterEventConsumers(server, host, context);
                });
                // Start Bus Control
                busControl.StartAsync().Wait();
                return busControl;
            })
            .As<IBusControl>().
            SingleInstance();
        }
    }
}
