using Autofac;
using Monaco.Web.Consumers;

namespace Monaco.Web.Settings
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register event consumers
            builder.RegisterType<SampleConsumer>().InstancePerLifetimeScope();
        }
    }
}
