
using Grpc.Net.Client;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data.Common;
using System.Text;

namespace PdfGenerationService
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            Console.WriteLine("Start PdfGenerationService");
            
            var program = new Program();

            await program.SubscribePdrRequestEvent();

            Console.ReadLine();
        }

        private async Task SubscribePdrRequestEvent()
        {
            var connectionFactory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await connectionFactory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            var queue = await channel.QueueDeclareAsync(queue: "Pdf-Request", durable: true, exclusive: false, autoDelete: false, arguments: null);

            Console.WriteLine("Waiting for PDF Request.");


            var AccCreateEventConsumer = new AsyncEventingBasicConsumer(channel);
            AccCreateEventConsumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var jsonObj = JsonConvert.DeserializeObject<CreatePdfGenerateEventRequest>(message);
                Console.WriteLine($" New PDF Generate Request Recived => {message}");

                if (jsonObj != null)
                {
                    StatementResponse channel = GetStatementData(jsonObj.AccountNumber);

                    if (channel != null && channel.Statementdetail != null && channel.Statementdetail.TransactionDetails.Count != 0)
                    {
                        Console.WriteLine($" PDF In Progress => {message}");
                        Thread.Sleep(2000);
                        await RaiseOrderCreate(channel);
                    }
                }

                Console.WriteLine($"-------------------------------------------------------------------------------------------------------------");

                Console.WriteLine($"-------------------------------------------------------------------------------------------------------------");
             };

            await channel.BasicConsumeAsync("Pdf-Request", autoAck: true, consumer: AccCreateEventConsumer);

            Console.ReadLine();
        }

        private static StatementResponse GetStatementData(int accountNumber)
        {

            // The port number must match the port of the gRPC server.
            var channel = GrpcChannel.ForAddress("http://localhost:11837");
            var client = new Statement.StatementClient(channel);
            var reply = client.GetStatement(new StatementRequest { AccountNumber = accountNumber });
            Console.WriteLine("Greeting: " + reply.Statementdetail.Name);
            return reply;
        }

        private async Task RaiseOrderCreate(StatementResponse deatils)
        {
            var connectionFactory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await connectionFactory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync("PDFCreationQueue1", durable: true, exclusive: false, autoDelete: false);
            await channel.QueueDeclareAsync("PDFCreationQueue2", durable: true, exclusive: false, autoDelete: false);
            await channel.ExchangeDeclareAsync("FanoutExchange", ExchangeType.Fanout, durable: true, autoDelete: false);
            await channel.QueueBindAsync("PDFCreationQueue1", "FanoutExchange", string.Empty);
            await channel.QueueBindAsync("PDFCreationQueue2", "FanoutExchange", string.Empty);

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(deatils));

            await channel.BasicPublishAsync(exchange: "FanoutExchange", routingKey: "Pdf-Completed", body: body);

            await Task.CompletedTask;
        }
    }
}
