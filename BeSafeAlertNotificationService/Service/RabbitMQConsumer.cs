using System.Text;
using System.Text.Json;
using BeSafe.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AlertNotificationService.Service;


    public class RabbitMQConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly string _queueName = "disaster_alerts";
        private IConnection _connection;
        private IModel _channel;
        
        public RabbitMQConsumer(IConfiguration configuration)
        {
            _configuration = configuration;
            
            InitializeRabbitMQ();
        }
        
        private void InitializeRabbitMQ()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
                    UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
                    Password = _configuration["RabbitMQ:Password"] ?? "guest"
                };
                
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                
                _channel.QueueDeclare(
                    queue: _queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                    
                _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                
                Console.WriteLine("RabbitMQ connection established");
                Console.WriteLine($"Listening for messages on queue: {_queueName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing RabbitMQ: {ex.Message}");
                throw;
            }
        }
        
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                string message = null;
                
                try
                {
                    message = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var alertMessage = JsonSerializer.Deserialize<AlertMessageDto>(message);
                    
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                    
                    // Se a mensagem puder ser lida, tentar novamente
                    if (message != null)
                    {
                        _channel.BasicNack(ea.DeliveryTag, false, true);
                    }
                    else
                    {
                        // Se não puder ler a mensagem, descartá-la (não recolocar na fila)
                        _channel.BasicNack(ea.DeliveryTag, false, false);
                    }
                }
            };
            
            _channel.BasicConsume(
                queue: _queueName,
                autoAck: false,
                consumer: consumer);
                
            return Task.CompletedTask;
        }
        
        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }