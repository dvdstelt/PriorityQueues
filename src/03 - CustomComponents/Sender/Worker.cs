using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Notifications;

namespace Sender
{
    public class Worker : BackgroundService
    {
        const int BatchSize = 250;

        private readonly ILogger<Worker> log;
        private readonly IMediator mediator;
        private readonly Random random = new Random();
        private readonly Guid[] customers = Customers.GetAllCustomers().ToArray();

        public Worker(ILogger<Worker> log, IMediator mediator)
        {
            this.log = log;
            this.mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            log.LogInformation("Mwahahaha");
            DisplayHeader();

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

        private async Task SendBatch()
        {
            var tasks = new List<Task>();

            for (int i = 0; i < BatchSize; i++)
            {
                tasks.Add(SendMessage());
            }
            await Task.WhenAll(tasks);
        }

        private async Task SendMessage()
        {
            var notification = new OrderSubmitted
            {
                CustomerIdentifier = customers[random.Next(customers.Length)]
            };

            await mediator.Publish(notification);
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
    }
}
