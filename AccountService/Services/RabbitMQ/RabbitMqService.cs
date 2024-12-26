using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using AccountService.Contexts.Entities;
using AccountService.Dtos;

namespace SharedProject
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly ConnectionFactory _connectionFactory;

        public RabbitMqService()
        {
            _connectionFactory = new ConnectionFactory { HostName = "localhost" };

        }

        public async Task<bool> RaiseCreateAccount(CreateAccountEventRequest accountRequestDto)
        {
            try
            {
                using var connection = await _connectionFactory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(queue: "AccountCreatedQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                await channel.ExchangeDeclareAsync(exchange: "TopicExchange", type: ExchangeType.Topic, durable: true, autoDelete: false);

                await channel.QueueBindAsync(queue: "AccountCreatedQueue", exchange: "TopicExchange", routingKey: "event_account_created");

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(accountRequestDto));

                await channel.BasicPublishAsync(exchange: "TopicExchange", routingKey: "event_account_created", body: body);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
