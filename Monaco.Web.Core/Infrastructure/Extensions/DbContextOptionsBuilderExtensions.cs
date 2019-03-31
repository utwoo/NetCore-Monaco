using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            var appConfig = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            //TODO: config sql server setting with datasetting and configuration file.
            optionsBuilder
                .UseNpgsql(appConfig["Data:ConnectionString"]);
        }
    }
}
