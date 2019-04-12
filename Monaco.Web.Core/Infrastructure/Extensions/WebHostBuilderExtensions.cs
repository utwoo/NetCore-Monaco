using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
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
        /// <param name="serilogType">the type of log output</param>
        public static IWebHostBuilder UseMonacoSerilog(this IWebHostBuilder hostBuilder, SerilogType serilogType)
        {
            Action<WebHostBuilderContext, LoggerConfiguration> serilogConfiguration =
                (hostingContext, loggerConfiguration) =>
                   {
                       var configration = loggerConfiguration
                           .ReadFrom.Configuration(hostingContext.Configuration)
                           .Enrich.FromLogContext();

                       if ((serilogType & SerilogType.ColoredConsole) != 0)
                           configration.UseColoredConsole();

                       if ((serilogType & SerilogType.ElasticSearch) != 0)
                           configration.UseElasticSearch(hostingContext.Configuration);

                       if ((serilogType & SerilogType.SEQ) != 0)
                           configration.UseSEQ(hostingContext.Configuration);
                   };

            return hostBuilder.UseSerilog(serilogConfiguration);
        }

        /// <summary>
        /// Use Serilog with Colored Console for structured application logging.
        /// </summary>
        /// <param name="loggerConfiguration">Logger Configuration</param>
        private static void UseColoredConsole(this LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration
                .WriteTo.ColoredConsole();
        }

        /// <summary>
        /// Use Serilog with SEQ for structured application logging.
        /// </summary>
        /// <param name="loggerConfiguration">Logger Configuration</param>
        /// <param name="configuration">Application Configuration</param>
        private static void UseSEQ(this LoggerConfiguration loggerConfiguration, IConfiguration configuration)
        {
            loggerConfiguration
                .WriteTo.Seq(
                    serverUrl: configuration["SEQ:Server"],
                    apiKey: configuration["SEQ:APIKey"]);
        }

        /// <summary>
        /// Use Serilog with ElasticSearch for structured application logging.
        /// </summary>
        /// <param name="loggerConfiguration">Logger Configuration</param>
        /// <param name="configuration">Application Configuration</param>
        private static void UseElasticSearch(this LoggerConfiguration loggerConfiguration, IConfiguration configuration)
        {
            loggerConfiguration
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration["ElasticSearch:Server"]))
                {
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6
                });
        }
    }

    [Flags]
    public enum SerilogType
    {
        None = 0,
        ColoredConsole = 1,
        ElasticSearch = 2,
        SEQ = 4
    }
}
