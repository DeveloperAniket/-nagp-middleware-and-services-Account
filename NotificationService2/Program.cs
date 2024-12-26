﻿using RabbitMQ.Client;


using RabbitMQ.Client.Events;
using System.Text;

namespace NotificationService2
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Start NotificationService2");
            await ListenAccountCreated();
            await ListenPDFCreated();

            Console.ReadLine();
        }

        private static async Task ListenAccountCreated()
        {
            var connectionFactory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await connectionFactory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: "AccountCreatedQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            await channel.ExchangeDeclareAsync(exchange: "TopicExchange", type: ExchangeType.Topic, durable: true, autoDelete: false);

            await channel.QueueBindAsync(queue: "AccountCreatedQueue", exchange: "TopicExchange", routingKey: "event_account_created");

            Console.WriteLine("Waiting for notification.");


            var AccCreateEventConsumer = new AsyncEventingBasicConsumer(channel);
            AccCreateEventConsumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" New Account Created => {message}");

                Console.WriteLine($"-------------------------------------------------------------------------------------------------------------");

                Console.WriteLine($"-------------------------------------------------------------------------------------------------------------");

                return Task.CompletedTask;
            };

            await channel.BasicConsumeAsync("AccountCreatedQueue", autoAck: true, consumer: AccCreateEventConsumer);
        }

        private static async Task ListenPDFCreated()
        {
            var connectionFactory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await connectionFactory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: "PDFCreationQueue2", durable: true, exclusive: false, autoDelete: false, arguments: null);

            await channel.ExchangeDeclareAsync(exchange: "FanoutExchange", type: ExchangeType.Fanout, durable: true, autoDelete: false);

            await channel.QueueBindAsync(queue: "PDFCreationQueue2", exchange: "FanoutExchange", routingKey: "Pdf-Completed");

            Console.WriteLine("Waiting for notification.");


            var AccCreateEventConsumer = new AsyncEventingBasicConsumer(channel);
            AccCreateEventConsumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" New PDF Created => {message}");

                Console.WriteLine($"-------------------------------------------------------------------------------------------------------------");

                Console.WriteLine($"-------------------------------------------------------------------------------------------------------------");

                return Task.CompletedTask;
            };

            await channel.BasicConsumeAsync("PDFCreationQueue2", autoAck: true, consumer: AccCreateEventConsumer);
        }
    }
}
