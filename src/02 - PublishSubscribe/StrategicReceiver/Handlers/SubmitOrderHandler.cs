using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared;
using Shared.Messages;

namespace StrategicReceiver.Handlers
{
    public class SubmitOrderHandler : IHandleMessages<OrderSubmitted>
    {
        readonly ILog log = LogManager.GetLogger<SubmitOrderHandler>();

        public Task Handle(OrderSubmitted message, IMessageHandlerContext context)
        {
            if (Customers.GetPriorityCustomers().Contains(message.CustomerId))
            {
                log.Info($"Message received with CustomerId [{message.CustomerId}]");
            }

            return Task.CompletedTask;
        }
    }
}
