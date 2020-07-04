using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus.Features;
using NServiceBus.Pipeline;
using NServiceBus.Routing;

namespace StrategicReceiver.Sender
{
    public class StrategicCustomerRoutingBehavior : Behavior<IDispatchContext>
    {
        public override Task Invoke(IDispatchContext context, Func<Task> next)
        {

            //var routingStrategies = context.RoutingStrategies;
            //var routingStrategy = routingStrategies.First();

            foreach (var operation in context.Operations)
            {

                var body = operation.Message.Body;

                if (operation.AddressTag is UnicastAddressTag unicastAddressTag)
                {
                    var oldDestinationEndpoint = unicastAddressTag.Destination;
                }
            }

            return next();
        }
    }

    public class StrategicCustomerRoutingFeature : Feature
    {
        public StrategicCustomerRoutingFeature()
        {
            EnableByDefault();
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
            context.Pipeline.Register(new StrategicCustomerRoutingBehavior(), "Strategic customer routing behavior");
        }
    }
}
