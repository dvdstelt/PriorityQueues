using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared;
using Shared.Messages;

namespace StrategicReceiver.Handlers
{
    public class SubmitOrderHandler : IHandleMessages<SubmitOrder>
    {
        readonly ILog log = LogManager.GetLogger<SubmitOrderHandler>();

        public Task Handle(SubmitOrder message, IMessageHandlerContext context)
        {
            log.Info($"Message received with CustomerId [{message.CustomerId}]");

            return Task.CompletedTask;
        }
    }
}
