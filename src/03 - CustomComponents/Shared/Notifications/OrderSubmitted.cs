using System;
using MediatR;

namespace Shared.Notifications
{
    public class OrderSubmitted : INotification
    {
        public Guid CustomerIdentifier { get; set; }
    }
}
