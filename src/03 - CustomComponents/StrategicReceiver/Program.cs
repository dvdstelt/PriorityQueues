using System;
using System.Threading.Tasks;
using NServiceBus;
using Shared.Configuration;

namespace StrategicReceiver
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DisplayHeader();

            var endpointConfiguration = new EndpointConfiguration("StrategicReceiver")
                .ApplyDefaultConfiguration();

            var endpointInstance = await Endpoint.Start(endpointConfiguration);

            Console.WriteLine("Press a key to quit...");
            Console.ReadKey(true);

            await endpointInstance.Stop();
        }

        private static void DisplayHeader()
        {
            Console.Title = "StrategicReceiver - Priority Queues";

            var backgroundColor = Console.BackgroundColor;
            var foregroundColor = Console.ForegroundColor;
            var windowWith = Console.WindowWidth;

            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("StrategicReceiver".PadRight(windowWith - 1));

            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
        }

    }
}
