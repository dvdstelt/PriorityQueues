using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;
using NServiceBus;

namespace Shared.Messages
{
    public class SubmitOrder : IEvent
    {
        public Guid CustomerId { get; set; }
    }
}
