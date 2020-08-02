using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;
using Shared.Messages;
using Shared.Notifications;

namespace StrategicReceiver.Interceptor
{
    public class StrategicInterceptor : INotificationHandler<OrderSubmitted>
    {
        private readonly ILogger<StrategicInterceptor> log;
        private readonly IMessageSession messageSession;

        public StrategicInterceptor(ILogger<StrategicInterceptor> log, IMessageSession messageSession)
        {
            this.log = log;
            this.messageSession = messageSession;
        }

        public async Task Handle(OrderSubmitted notification, CancellationToken cancellationToken)
        {
            if (!Customers.GetPriorityCustomers().Contains(notification.CustomerIdentifier))
                return;

            var message = new SubmitOrder()
            {
                CustomerId = notification.CustomerIdentifier
            };

            var sendOptions = new SendOptions();
            sendOptions.SetDestination("StrategicReceiver");

            await messageSession.Send(message, sendOptions).ConfigureAwait(false);
        }
    }
}
