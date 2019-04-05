using System;
using Autofac;
using MassTransit;
using MassTransit.AutofacIntegration;
using Monaco.Core.EventPublisher;

namespace Monaco.Core.Settings
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Event Publisher
            builder.RegisterType<RabbitMQEventPublisher>().As<IEventPublisher>().InstancePerLifetimeScope();
        }
    }
}
