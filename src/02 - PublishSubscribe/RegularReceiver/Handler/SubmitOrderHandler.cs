using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared;
using Shared.Messages;

namespace RegularReceiver.Handler
{
    public class SubmitOrderHandler : IHandleMessages<OrderSubmitted>
    {
        readonly ILog log = LogManager.GetLogger<SubmitOrderHandler>();

        public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
        {
            if (!Customers.GetPriorityCustomers().Contains(message.CustomerId))
            {
                log.Info($"Message received with CustomerId [{message.CustomerId}]");

                // Emulate a delay as if RegularReceiver is slower than StrategicReceiver
                await Task.Delay(250);
            }
        }
    }
}
