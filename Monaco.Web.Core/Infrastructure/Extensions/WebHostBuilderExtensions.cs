using System;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Monaco.Web.Core.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of web host builder
    /// </summary>
    public static class WebHostBuilderExtensions
    {
        /// <summary>
        /// Use Serilog with ColorConsole for structured application logging.
        /// </summary>
        /// <param name="hostBuilder">the web host builder to configure</param>
        public static IWebHostBuilder UseColorConsoleSerilog(this IWebHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((hostingContext, loggerConfiguration) =>
                    loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .Enrich.FromLogContext()
                        .WriteTo.ColoredConsole());

            return hostBuilder;
        }

        /// <summary>
        /// Use Serilog with SEQ for structured application logging.
        /// </summary>
        /// <param name="hostBuilder">the web host builder to configure</param>
        public static IWebHostBuilder UseSEQSerilog(this IWebHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((hostingContext, loggerConfiguration) =>
                    loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .Enrich.FromLogContext()
                        .WriteTo.Seq(
                            serverUrl: hostingContext.Configuration["SEQ:Server"],
                            apiKey: hostingContext.Configuration["SEQ:APIKey"]));

            return hostBuilder;
        }

        /// <summary>
        /// Use Serilog with ElasticSearch for structured application logging.
        /// </summary>
        /// <param name="hostBuilder">the web host builder to configure</param>
        public static IWebHostBuilder UseElasticSearchSerilog(this IWebHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((hostingContext, loggerConfiguration) =>
                    loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .Enrich.FromLogContext()
                        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(hostingContext.Configuration["ElasticSearch:Server"]))
                        {
                            AutoRegisterTemplate = true,
                            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6
                        }));

            return hostBuilder;
        }
    }
}
