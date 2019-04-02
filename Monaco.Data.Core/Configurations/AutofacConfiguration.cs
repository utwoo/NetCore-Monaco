using Autofac;
using Monaco.Data.Core.Repository;

namespace Monaco.Data.Core.Configurations
{
    public class AutofacConfiguration : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register repositories
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
        }
    }
}
