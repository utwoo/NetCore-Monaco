using System;
using Microsoft.Extensions.Configuration;

namespace Monaco.Core.EventPublishers
{
    public class RabbitMQEventPublisher : IEventPublisher
    {
        public void Publish<T>(T eventMessage)
        {
            throw new NotImplementedException();
        }
    }
}
