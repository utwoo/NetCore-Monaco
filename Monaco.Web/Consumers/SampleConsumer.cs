using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Monaco.Web.Models;

namespace Monaco.Web.Consumers
{
    public class SampleConsumer : IConsumer<Sample>
    {
        private readonly ILogger<SampleConsumer> _logger;

        public SampleConsumer(
            ILogger<SampleConsumer> logger)
        {
            this._logger = logger;
        }

        public Task Consume(ConsumeContext<Sample> context)
        {
            return Task.Run(() => this._logger.LogInformation($"Sample Value:{context.Message.Value}"));
        }
    }
}
