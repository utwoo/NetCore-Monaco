using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        public static IServiceProvider AddMonacoAutoFac(this IServiceCollection services, IConfiguration configuration)
        {
            // Create the container builder.
            var builder = new ContainerBuilder();
            // Read Assemblies for Autofac from configuration
            var assemblies =
                configuration["Autofac:LoadAssemblies"]
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
    }
}
