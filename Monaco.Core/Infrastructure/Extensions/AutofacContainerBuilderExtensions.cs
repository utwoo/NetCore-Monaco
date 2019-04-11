using System;
using System.Linq;
using System.Reflection;
using Autofac;
using MassTransit;
using Monaco.Core.Configurations;
using Monaco.Core.EventConsumer;
using Monaco.Core.EventPublisher;

namespace Monaco.Core.Infrastructure.Extensions
{
    public static class AutofacContainerBuilderExtensions
    {
        /// <summary>
        /// Register RabbitMQ Components in Autofac container
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="configuration">application configuration</param>
        public static void RegisterRabbitMQComponents(this ContainerBuilder builder, MonacoConfiguration configuration)
        {
            // Register Event Publisher
            builder.RegisterType<RabbitMQEventPublisher>().As<IEventPublisher>().InstancePerLifetimeScope();
            // Register Bus Control
            builder.AddMassTransit((containerConfiguration) =>
            {
                // Register bus control by factory method
                containerConfiguration.AddBus(context =>
                {
                    var bus = Bus.Factory.CreateUsingRabbitMq(busConfiguration =>
                    {
                        // Create host through application configurations
                        var host = busConfiguration.Host(new Uri(configuration.RabbitMQConfig.Server), hostConfiguration =>
                         {
                             hostConfiguration.Username(configuration.RabbitMQConfig.Username);
                             hostConfiguration.Password(configuration.RabbitMQConfig.Password);
                         });

                        // Recieve Event Consumers Setting Types
                        var rabbitEventConsumersSettings =
                             configuration.AutofacConfig.LoadAssemblies
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
                            rabbitEventConsumersSetting.RegisterEventConsumers(busConfiguration, context);
                    });
                    // Start Bus
                    bus.StartAsync().Wait();
                    return bus;
                });
            });
        }
    }
}
