using MassTransit;

namespace Monaco.Core.EventPublishers
{
    public class RabbitMQEventPublisher : IEventPublisher
    {
        // MassTransit bus control
        private readonly IBusControl _busControl;

        public RabbitMQEventPublisher(
            IBusControl busControl
            )
        {
            this._busControl = busControl;
        }

        /// <summary>
        /// Publish event by RabbitMQ
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="eventMessage">Event message</param>
        public void Publish<T>(T eventMessage) where T : class
        {
            this._busControl.Publish<T>(eventMessage);
        }
    }
}
