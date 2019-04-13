using Autofac;
using Monaco.Core.Caching;
using Monaco.Web.Core.Caching;

namespace Monaco.Web.Core.Settings
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //cache manager
            builder.RegisterType<HttpContextCacheManager>().As<ILocalCacheManager>().InstancePerLifetimeScope();
        }
    }
}
