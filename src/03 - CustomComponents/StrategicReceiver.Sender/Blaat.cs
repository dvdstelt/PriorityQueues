using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Notifications;

namespace StrategicReceiver.Interceptor
{
    public class Blaat : INotificationHandler<OrderSubmitted>
    {
        private readonly ILogger<Blaat> log;

        public Blaat(ILogger<Blaat> log)
        {
            this.log = log;
        }

        public Task Handle(OrderSubmitted notification, CancellationToken cancellationToken)
        {
            log.LogInformation("Jaaaaa, version 2.0!!!");

            return Task.CompletedTask;
        }
    }
}
