﻿using Microsoft.Extensions.DependencyInjection;
using Monaco.Data.Core.DbContexts;

namespace Monaco.Web.Core.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register base object context
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddMonacoDbContext(this IServiceCollection services)
        {
            services.AddDbContext<MonacoDbContext>(optionsBuilder =>
            {
                optionsBuilder.UsePostgreSQLServer(services);
            });
        }
    }
}
