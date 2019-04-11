using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monaco.Core.Autofac;
using Monaco.Core.Caching;
using Monaco.Core.Configurations;
using Monaco.Core.Data;
using Monaco.Core.MessageQueue;
using Monaco.Core.SEQ;

namespace Monaco.Core.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register Autofac IOC container 
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static IServiceProvider AddMonacoAutoFac(this IServiceCollection services)
        {
            // Create the container builder.
            var builder = new ContainerBuilder();
            // Read Assemblies for Autofac from configuration
            var assemblies =
                MonacoConfiguration.Instance.AutofacConfig.LoadAssemblies
                    .Split(';')
                    .Select(assemblyName => Assembly.Load(assemblyName))
                    .ToArray();
            // Register Autofac modules
            builder.RegisterAssemblyModules(assemblies);
            // Register RabbitMQ Components
            builder.RegisterRabbitMQComponents();
            // Populate MVC services to Autofac container build
            builder.Populate(services);
            // Create container and return provider 
            return new AutofacServiceProvider(builder.Build());
        }

        /// <summary>
        /// Register object-object mapper
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddMonacoMapper(this IServiceCollection services)
        {
            // Register mapper configurations
            services.AddAutoMapper();
        }

        /// <summary>
        /// Create, bind and register as service the specified configuration parameters 
        /// </summary>
        /// <typeparam name="TConfig">Configuration parameters</typeparam>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Set of key/value application configuration properties</param>
        /// <returns>Instance of configuration parameters</returns>
        public static TConfig BindConfiguration<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            //create instance of config
            var config = new TConfig();
            //bind it to the appropriate section of configuration
            configuration.Bind(config);

            return config;
        }

        /// <summary>
        /// Initialize Monaco Configurations
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Set of key/value application configuration properties</param>
        public static void InitializeConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            
            // TODO: Register [Autofac] Configurations
            MonacoConfiguration.Instance.DataConfig = services.BindConfiguration<DataConfiguration>(configuration.GetSection("Data"));
            // TODO: Register [Autofac] Configurations
            MonacoConfiguration.Instance.AutofacConfig = services.BindConfiguration<AutofacConfiguration>(configuration.GetSection("Autofac"));
            // TODO: Register [RedLock] Configurations
            MonacoConfiguration.Instance.RedLockConfig = services.BindConfiguration<RedLockConfiguration>(configuration.GetSection("RedLock"));
            // TODO: Register [Caching] Configurations
            MonacoConfiguration.Instance.CachingConfig = services.BindConfiguration<CachingConfiguration>(configuration.GetSection("Caching"));
            // TODO: Register [RabbitMQ] Configurations
            MonacoConfiguration.Instance.RabbitMQConfig = services.BindConfiguration<RabbitMQConfiguration>(configuration.GetSection("RabbitMQ"));
            // TODO: Register [SEQ] Configurations
            MonacoConfiguration.Instance.SEQConfig = services.BindConfiguration<SEQConfiguration>(configuration.GetSection("SEQ"));
        }
    }
}
