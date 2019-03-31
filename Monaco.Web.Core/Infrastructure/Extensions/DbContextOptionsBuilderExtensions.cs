using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Monaco.Core.Configuration;

namespace Monaco.Web.Core.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of DbContextBuilder
    /// </summary>
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// PostgreSQL Server specific extension method
        /// </summary>
        /// <param name="optionsBuilder">Database context options builder</param>
        /// <param name="services">Collection of service descriptors</param>
        public static void UsePostgreSQLServer(this DbContextOptionsBuilder optionsBuilder, IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetRequiredService<ApplicationConfiguration>();

            //TODO: config sql server setting with datasetting and configuration file.
            optionsBuilder
                .UseNpgsql(config.ConnectionString);
        }
    }
}
