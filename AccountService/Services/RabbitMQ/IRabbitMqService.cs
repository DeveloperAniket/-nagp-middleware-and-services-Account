using AccountService.Dtos;
using RabbitMQ.Client;

namespace SharedProject
{
    public interface IRabbitMqService
    {
        Task<bool> RaiseCreateAccount(CreateAccountEventRequest accountRequestDto);
    }
}
