
using AccountService.Contexts;
using AccountService.Services;
using System;
using Microsoft.EntityFrameworkCore;
using SharedProject;

namespace AccountService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Add services to the container.
            builder.Services.AddGrpc();

            builder.Services.AddSingleton<IAccountDataBaseService, AccountDataBaseService>();
            builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();             
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapGrpcService<AccountDataBaseService>();
            //app.MapGet("/grpc", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.MapControllers();

            app.Run();
        }
    }
}
