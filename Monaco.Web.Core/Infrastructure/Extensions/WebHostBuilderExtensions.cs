using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace Monaco.Web.Core.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of web host builder
    /// </summary>
    public static class WebHostBuilderExtensions
    {
        /// <summary>
        /// Register object-object mapper
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static IWebHostBuilder UseSEQSerilog(this IWebHostBuilder hostBuilder)
        {
            // Use AutoMapper for object-object mapper
            hostBuilder.UseSerilog((hostingContext, loggerConfiguration) =>
                    loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .Enrich.FromLogContext()
                        .WriteTo.Seq(
                            serverUrl: hostingContext.Configuration["SEQ:Server"],
                            apiKey: hostingContext.Configuration["SEQ:APIKey"]));

            return hostBuilder;
        }
    }
}
