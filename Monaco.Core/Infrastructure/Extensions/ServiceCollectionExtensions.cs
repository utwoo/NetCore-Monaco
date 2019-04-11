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
        /// <param name="configuration">Application configuration</param>
        public static IServiceProvider AddMonacoAutoFac(this IServiceCollection services, MonacoConfiguration configuration)
        {
            // Create the container builder.
            var builder = new ContainerBuilder();
            // Read Assemblies for Autofac from configuration
            var assemblies =
                configuration.AutofacConfig.LoadAssemblies
                    .Split(';')
                    .Select(assemblyName => Assembly.Load(assemblyName))
                    .ToArray();
            // Register Autofac modules
            builder.RegisterAssemblyModules(assemblies);
            // Register RabbitMQ Components
            builder.RegisterRabbitMQComponents(configuration);
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
            //and register it as a service
            services.AddSingleton(config);

            return config;
        }

        /// <summary>
        /// Initialize Monaco Configurations
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Set of key/value application configuration properties</param>
        /// <returns>Monaco Configurations</returns>
        public static MonacoConfiguration InitializeConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var config = new MonacoConfiguration
            {
                // TODO: Register [Autofac] Configurations
                DataConfig = services.BindConfiguration<DataConfiguration>(configuration.GetSection("Data")),
                // TODO: Register [Autofac] Configurations
                AutofacConfig = services.BindConfiguration<AutofacConfiguration>(configuration.GetSection("Autofac")),
                // TODO: Register [RedLock] Configurations
                RedLockConfig = services.BindConfiguration<RedLockConfiguration>(configuration.GetSection("RedLock")),
                // TODO: Register [Caching] Configurations
                CachingConfig = services.BindConfiguration<CachingConfiguration>(configuration.GetSection("Caching")),
                // TODO: Register [RabbitMQ] Configurations
                RabbitMQConfig = services.BindConfiguration<RabbitMQConfiguration>(configuration.GetSection("RabbitMQ")),
                // TODO: Register [SEQ] Configurations
                SEQConfig = services.BindConfiguration<SEQConfiguration>(configuration.GetSection("SEQ"))
            };

            return config;
        }
    }
}
