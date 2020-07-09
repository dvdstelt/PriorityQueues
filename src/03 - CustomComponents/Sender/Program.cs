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

            // Self DI

            var services = new ServiceCollection();
            services.AddTransient<ConsoleApplication>();
            services.AddMediatR(assemblyScannerResults.Assemblies.ToArray());
            services.AddLogging(configure => configure.AddConsole());
            
            await services.BuildServiceProvider().GetService<ConsoleApplication>().Run();

            return;
            

            /// Generic Host

            var builder = Host.CreateDefaultBuilder(args);
            builder.UseNServiceBus(ctx =>
            {
                var endpointConfiguration = new EndpointConfiguration("Sender");
                endpointConfiguration.ApplyDefaultConfiguration();

                endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);

                return endpointConfiguration;
            });
            builder.ConfigureServices((ctx, services) =>
            {
                services.AddMediatR(assemblyScannerResults.Assemblies.ToArray());
            });
            builder.UseConsoleLifetime();

            builder.ConfigureLogging((ctx, logging) =>
            {
                logging.AddConsole();
            });

            await builder.RunConsoleAsync(CancellationToken.None);

            //builder.Build().Run();
        }

        private static async Task OnCriticalError(ICriticalErrorContext context)
        {
            var fatalMessage =
                $"The following critical error was encountered:{Environment.NewLine}{context.Error}{Environment.NewLine}Process is shutting down. StackTrace: {Environment.NewLine}{context.Exception.StackTrace}";
            
            Console.WriteLine(fatalMessage);

            try
            {
                await context.Stop().ConfigureAwait(false);
            }
            finally
            {
                Environment.FailFast(fatalMessage, context.Exception);
            }
        }
    }

    internal class ConsoleApplication
    {
        private readonly IMediator mediator;

        public ConsoleApplication(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task Run()
        {
            await mediator.Publish(new OrderSubmitted() {CustomerIdentifier = Guid.NewGuid()});

            Console.WriteLine("Dude");
            Console.ReadKey();
        }

    }
}
