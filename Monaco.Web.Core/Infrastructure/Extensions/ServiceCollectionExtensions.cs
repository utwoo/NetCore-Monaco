using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monaco.Data.Core.DbContexts;

namespace Monaco.Web.Core.Infrastructure.Extensions
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

            // Resolve application configration
            var appConfig = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            // Read Assemblies for Autofac from configuration
            var assemblies =
                appConfig["Autofac:LoadAssemblies"]
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
        /// Register database context
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddMonacoDbContext(this IServiceCollection services)
        {
            // Resolve application configration
            var appConfig = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            // Register database context from application configuration
            services.AddDbContext<MonacoDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseNpgsql(appConfig["Data:ConnectionString"]);
            });
        }
    }
}
