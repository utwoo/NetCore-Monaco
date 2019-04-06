using Autofac;
using MassTransit.RabbitMqTransport;

namespace Monaco.Core.EventConsumer
{
    /// <summary>
    /// Register RabbitMQ Event Consumer
    /// </summary>
    public interface IRabbitEventConsumerRegistrar
    {
        /// <summary>
        /// Register event consumers
        /// </summary>
        /// <param name="configuration">Bus Control factory configuration</param>
        /// <param name="context">Autofac dependency context</param>
        void RegisterEventConsumers(IRabbitMqBusFactoryConfigurator configuration, IComponentContext context);

        /// <summary>
        /// Gets order of registrar implementation
        /// </summary>
        int Order { get; }
    }
}
