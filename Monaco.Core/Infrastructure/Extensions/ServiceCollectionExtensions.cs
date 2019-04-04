using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using MassTransit;
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
                    .Select(name => Assembly.Load(name))
                    .ToArray();
            // Register Autofac modules
            builder.RegisterAssemblyModules(assemblies);
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
        /// Register RabbitMQ
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Application configuration</param>
        public static void AddMonacoRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            // Register MassTransit component
            services.AddMassTransit(config =>
            {
                config.AddBus(context =>
                {
                    // Create bus control by factory method
                    return Bus.Factory.CreateUsingRabbitMq(server =>
                    {
                        server.Host(new Uri(configuration["RabbitMQ:Server"]), host =>
                        {
                            host.Username(configuration["RabbitMQ:Username"]);
                            host.Password(configuration["RabbitMQ:Password"]);
                        });
                    });
                });
            });
        }
    }
}
