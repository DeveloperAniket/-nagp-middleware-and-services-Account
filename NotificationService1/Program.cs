using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace NotificationService1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Start NotificationService1");
            await ListenPDFCreated();
            Console.ReadLine();
        }

        private static async Task ListenPDFCreated()
        {
            var connectionFactory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await connectionFactory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: "PDFCreationQueue1", durable: true, exclusive: false, autoDelete: false, arguments: null);

            await channel.ExchangeDeclareAsync(exchange: "FanoutExchange", type: ExchangeType.Fanout, durable: true, autoDelete: false);

            await channel.QueueBindAsync(queue: "PDFCreationQueue1", exchange: "FanoutExchange", routingKey: "Pdf-Completed");

            Console.WriteLine("Waiting for notification 1.");


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

            await channel.BasicConsumeAsync("PDFCreationQueue1", autoAck: true, consumer: AccCreateEventConsumer);
        }
    }
}
