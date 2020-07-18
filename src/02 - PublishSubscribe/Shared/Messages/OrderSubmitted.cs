using System;
using NServiceBus;

namespace Shared.Messages
{
    public class OrderSubmitted : IEvent
    {
        public Guid CustomerId { get; set; }
    }
}
