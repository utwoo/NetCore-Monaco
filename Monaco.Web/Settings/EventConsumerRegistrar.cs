using Autofac;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Monaco.Core.EventConsumer;
using Monaco.Web.Consumers;

namespace Monaco.Web.Settings
{
    public class EventConsumerRegistrar : IRabbitEventConsumerRegistrar
    {
        public int Order => 0;

        public void RegisterEventConsumers(IRabbitMqBusFactoryConfigurator configuration, IComponentContext context)
        {
            configuration.ReceiveEndpoint("sample.test", e =>
           {
               e.Consumer<SampleConsumer>(context);
               e.Consumer<SampleConsumer2>(context);
           });
        }
    }
}
