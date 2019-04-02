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
        /// Use Serilog for structured application logging.
        /// </summary>
        /// <param name="hostBuilder">the web host builder to configure</param>
        public static IWebHostBuilder UseSEQSerilog(this IWebHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((hostingContext, loggerConfiguration) =>
                    loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .Enrich.FromLogContext()
                        .WriteTo.ColoredConsole()
                        .WriteTo.Seq(
                            serverUrl: hostingContext.Configuration["SEQ:Server"],
                            apiKey: hostingContext.Configuration["SEQ:APIKey"]));

            return hostBuilder;
        }
    }
}
