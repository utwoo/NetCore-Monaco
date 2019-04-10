using Autofac;
using Monaco.Core.Caching;

namespace Monaco.Core.Settings
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register lock
            builder.RegisterType<RedLockManager>().As<ILockManager>().SingleInstance();
        }
    }
}
