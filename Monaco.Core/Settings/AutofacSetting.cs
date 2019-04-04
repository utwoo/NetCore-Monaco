using Autofac;
using Monaco.Core.EventPublishers;

namespace Monaco.Core.Settings
{
    public class AutofacSetting : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Event Publisher
            builder.RegisterType<RabbitMQEventPublisher>().As<IEventPublisher>().InstancePerLifetimeScope();
        }
    }
}
