using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace BeSafe.Services;

public class RabbitMQService : IDisposable
{
      private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<RabbitMQService> _logger;
    
    public static readonly string AlertQueueName = "disaster_alerts";
    public static readonly string AlertExchangeName = "alert_exchange";
    public static readonly string NewAlertRoutingKey = "alert.new";
    public static readonly string UpdatedAlertRoutingKey = "alert.updated";

    public RabbitMQService(ILogger<RabbitMQService> logger)
    {
        _logger = logger;
        try
        {
            var factory = new ConnectionFactory 
            { 
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            
            _channel.ExchangeDeclare(
                exchange: AlertExchangeName,
                type: "topic",
                durable: true,
                autoDelete: false);
                
            _channel.QueueDeclare(
                queue: AlertQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false);
            
            _channel.QueueBind(
                queue: AlertQueueName, 
                exchange: AlertExchangeName, 
                routingKey: NewAlertRoutingKey);
                
            _channel.QueueBind(
                queue: AlertQueueName, 
                exchange: AlertExchangeName, 
                routingKey: UpdatedAlertRoutingKey);
                
            _logger.LogInformation("RabbitMQ connection established successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to establish RabbitMQ connection");
            throw;
        }
    }

    public void PublishAlert<T>(T message, string routingKey)
    {
        try
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);
            
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            
            _channel.BasicPublish(
                exchange: AlertExchangeName,
                routingKey: routingKey,
                basicProperties: properties,
                body: body);
                
            _logger.LogInformation($"Alert published to RabbitMQ with routing key: {routingKey}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish alert to RabbitMQ");
            throw;
        }
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}