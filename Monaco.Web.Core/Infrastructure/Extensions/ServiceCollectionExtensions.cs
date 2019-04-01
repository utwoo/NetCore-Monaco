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
        /// Register object-object mapper
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddMonacoMapper(this IServiceCollection services)
        {
            // Use AutoMapper for object-object mapper
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
