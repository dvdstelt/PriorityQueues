using System;
using System.Threading.Tasks;
using NServiceBus;
using Shared.Configuration;

namespace RegularReceiver
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DisplayHeader();

            var endpointConfiguration = new EndpointConfiguration("RegularReceiver")
                .ApplyDefaultConfiguration();

            var endpointInstance = await Endpoint.Start(endpointConfiguration);

            Console.WriteLine("Press a key to quit...");
            Console.ReadKey(true);

            await endpointInstance.Stop();
        }

        private static void DisplayHeader()
        {
            Console.Title = "RegularReceiver - Priority Queues";

            var backgroundColor = Console.BackgroundColor;
            var foregroundColor = Console.ForegroundColor;
            var windowWith = Console.WindowWidth;

            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("RegularReceiver".PadRight(windowWith - 1));

            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
        }
    }
}
