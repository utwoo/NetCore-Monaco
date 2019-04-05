using Autofac;
using MassTransit.RabbitMqTransport;

namespace Monaco.Core.EventConsumer
{
    public interface IRabbitEventConsumerRegistrar
    {
        void RegisterEventConsumers(IRabbitMqBusFactoryConfigurator configuration, IRabbitMqHost host, IComponentContext context);
        int Order { get; }
    }
}
