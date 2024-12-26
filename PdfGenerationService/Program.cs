
using Grpc.Net.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;

namespace PdfGenerationService
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            Console.WriteLine("Start PdfGenerationService");


            await SubscribePdrRequestEvent();
       
        }

        private static async Task SubscribePdrRequestEvent()
        {
            var connectionFactory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await connectionFactory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            var queue = await channel.QueueDeclareAsync(queue: "Pdf-Request", durable: true, exclusive: false, autoDelete: false, arguments: null);

            Console.WriteLine("Waiting for PDF Request.");


            var AccCreateEventConsumer = new AsyncEventingBasicConsumer(channel);
            AccCreateEventConsumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var jsonObj = JsonConvert.DeserializeObject<CreatePdfGenerateEventRequest>(message);
                Console.WriteLine($" New PDF Generate Request Recived => {message}");

                if (jsonObj != null)
                {
                    StatementResponse channel = GetStatementData(jsonObj.AccountNumber);
                }
              
                Console.WriteLine($"-------------------------------------------------------------------------------------------------------------");

                Console.WriteLine($"-------------------------------------------------------------------------------------------------------------");

                return Task.CompletedTask;
            };

            await channel.BasicConsumeAsync("Pdf-Request", autoAck: true, consumer: AccCreateEventConsumer);
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
    }
}
