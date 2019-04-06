using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Monaco.Web.Models;

namespace Monaco.Web.Consumers
{
    public class SampleConsumer2 : IConsumer<Sample>
    {
        private readonly ILogger<SampleConsumer> _logger;

        public SampleConsumer2(
            ILogger<SampleConsumer> logger)
        {
            this._logger = logger;
        }

        public Task Consume(ConsumeContext<Sample> context)
        {
            return Task.Run(() => this._logger.LogInformation($"Sample2 Value:{context.Message.Value + 10}"));
        }
    }
}
