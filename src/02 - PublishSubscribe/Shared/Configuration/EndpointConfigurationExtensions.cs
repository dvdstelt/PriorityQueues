using NServiceBus;
using NServiceBus.Logging;

namespace Shared.Configuration
{
    public static class EndpointConfigurationExtensions
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(EndpointConfigurationExtensions));

        public static EndpointConfiguration ApplyDefaultConfiguration(this EndpointConfiguration endpointConfiguration)
        {
            Log.Info("Configuring endpoint...");

            endpointConfiguration.UseTransport<LearningTransport>();
            endpointConfiguration.UsePersistence<LearningPersistence>();

            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            endpointConfiguration.Recoverability().Immediate(c => c.NumberOfRetries(0));
            endpointConfiguration.Recoverability().Delayed(c => c.NumberOfRetries(0));

            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");

            return endpointConfiguration;
        }
    }
}
