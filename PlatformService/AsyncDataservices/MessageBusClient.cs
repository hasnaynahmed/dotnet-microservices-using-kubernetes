using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory(){HostName = _configuration["RabitMQHost"], 
                Port = int.Parse(_configuration["RabbitMQPort"])};

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutDown;
                Console.WriteLine($"--> RabbitMQ - Connected to message bus");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not connect to Message bug: {ex.Message}");
            }
        }
        public void PublishNewPlatform(PlatformPublishDto platformPublishDto)
        {
            var message = JsonSerializer.Serialize(platformPublishDto);
            if(_connection.IsOpen)
            {
                Console.WriteLine($"--> RabbitMQ connection open, sending message.");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine($"--> RabbitMQ connection closed, not sending message.");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "trigger",
                routingKey: "",
                basicProperties: null,
                body
            );
            Console.WriteLine($"--> We have sent {message}.");
        }

        public void Dispose()
        {
            Console.WriteLine($"--> RabbitMQ - Messagebus dispossed!");
            if(_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        private void RabbitMQ_ConnectionShutDown(object sender, EventArgs arg)
        {
            Console.WriteLine($"--> RabbitMQ connection shutted down!");
        }
    }
}