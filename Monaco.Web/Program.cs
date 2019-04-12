using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Monaco.Web.Core.Infrastructure.Extensions;

namespace Monaco.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options => options.AddServerHeader = false)
                .UseStartup<Startup>()
                .UseMonacoSerilog(SerilogType.ColoredConsole | SerilogType.ElasticSearch | SerilogType.SEQ)
                .Build();

            host.Run();
        }
    }
}
