using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Hosting.Helpers;
using Shared;
using Shared.Configuration;
using Shared.Messages;
using Shared.Notifications;

namespace Sender
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // We're using NServiceBus anyway, so let's use it to scan all assemblies.
            var assemblyScannerResults = new AssemblyScanner().GetScannableAssemblies();

            var services = new ServiceCollection();
            services.AddTransient<Worker>();
            services.AddMediatR(assemblyScannerResults.Assemblies.ToArray());
            services.AddLogging(configure => configure.AddConsole());

            var endpointConfiguration = new EndpointConfiguration("Sender");
            endpointConfiguration.ApplyDefaultConfiguration();

            var endpointInstance = await Endpoint.Start(endpointConfiguration);
            services.AddSingleton<IMessageSession>(endpointInstance);

            await services.BuildServiceProvider().GetService<Worker>().Run();
        }
    }
}
