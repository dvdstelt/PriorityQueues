using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;
using Shared.Messages;
using Shared.Notifications;

namespace RegularReceiver.Interceptor
{
    public class RegularInterceptor : INotificationHandler<OrderSubmitted>
    {
        private readonly ILogger<RegularInterceptor> log;
        private readonly IMessageSession messageSession;

        public RegularInterceptor(ILogger<RegularInterceptor> log, IMessageSession messageSession)
        {
            this.log = log;
            this.messageSession = messageSession;
        }

        public async Task Handle(OrderSubmitted notification, CancellationToken cancellationToken)
        {
            if (Customers.GetPriorityCustomers().Contains(notification.CustomerIdentifier))
                return;

            var message = new SubmitOrder()
            {
                CustomerId = notification.CustomerIdentifier
            };

            var sendOptions = new SendOptions();
            sendOptions.SetDestination("RegularReceiver");

            await messageSession.Send(message, sendOptions).ConfigureAwait(false);

        }
    }
}
