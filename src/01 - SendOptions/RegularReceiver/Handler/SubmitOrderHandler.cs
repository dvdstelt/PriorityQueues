﻿using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared.Messages;

namespace RegularReceiver.Handler
{
    public class SubmitOrderHandler : IHandleMessages<SubmitOrder>
    {
        readonly ILog log = LogManager.GetLogger<SubmitOrderHandler>();

        public async Task Handle(SubmitOrder message, IMessageHandlerContext context)
        {
            log.Info($"Message received with CustomerId [{message.CustomerId}]");

            // Emulate a delay as if RegularReceiver is slower than StrategicReceiver
            await Task.Delay(250);
        }
    }
}
