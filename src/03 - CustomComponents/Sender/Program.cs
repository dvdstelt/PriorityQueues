using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using Shared;
using Shared.Configuration;
using Shared.Messages;

namespace Sender
{
    class Program
    {
        const int BatchSize = 250;

        private static IEndpointInstance endpointInstance;
        private static readonly Random random = new Random();
        private static readonly Guid[] customers = Customers.GetAllCustomers().ToArray();


        static async Task Main(string[] args)
        {
            DisplayHeader();

            var endpointConfiguration = new EndpointConfiguration("Sender")
                .ApplyDefaultConfiguration(routing =>
                {
                    routing.RouteToEndpoint(typeof(SubmitOrder), "RegularReceiver");
                });

            endpointInstance = await Endpoint.Start(endpointConfiguration);
            Console.ForegroundColor = ConsoleColor.White;

            while (true)
            {
                var key = Console.ReadKey(true);
                Console.WriteLine();

                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        await SendMessage();
                        Console.WriteLine($"Messages sent");
                        break;
                    case ConsoleKey.D2:
                        await SendBatch();
                        Console.WriteLine($"{BatchSize} messages sent");
                        break;
                    case ConsoleKey.Q:
                        Environment.Exit(0);
                        break;
                }
            }
        }

        private static void DisplayHeader()
        {
            Console.Title = "Sender - Priority Queues";

            var backgroundColor = Console.BackgroundColor;
            var foregroundColor = Console.ForegroundColor;
            var windowWith = Console.WindowWidth;

            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Priority Queues via Publish/Subscribe".PadRight(windowWith - 1));

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("  [1] Publish a random customer message".PadRight(windowWith - 1));
            Console.WriteLine($"  [2] Publish {BatchSize} random customer messages".PadRight(windowWith - 1));
            Console.WriteLine("  [q] To quit".PadRight(windowWith - 1));

            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
        }

        static async Task SendBatch()
        {
            var tasks = new List<Task>();

            for (int i = 0; i < BatchSize; i++)
            {
                tasks.Add(SendMessage());
            }
            await Task.WhenAll(tasks);
        }

        private static async Task SendMessage()
        {
            var message = new SubmitOrder
            {
                CustomerId = customers[random.Next(customers.Length)]
            };

            await endpointInstance.Send(message).ConfigureAwait(false);
        }
    }
}
