using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace SmartRealms.MQTT
{
    public class RabbitMQProducer  : BackgroundService
    {
        private readonly ILogger<RabbitMQProducer> _logger;

        public RabbitMQProducer(ILogger<RabbitMQProducer> logger)
        {
            _logger = logger;
        }

        

        public void SendMessage(object obj)
        {
            var message = JsonSerializer.Serialize(obj);
            SendMessage(message);
        }

        public void SendMessage(string message)
        {
            // Не забудьте вынести значения "localhost" и "MyQueue"
            // в файл конфигурации
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "mqtt_queue",
                               durable: false,
                               exclusive: false,
                               autoDelete: false,
                               arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                               routingKey: "mqtt_queue",
                               basicProperties: null,
                               body: body);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                SendMessage("MQTT test message" + Guid.NewGuid());
                await Task.Delay(5000, stoppingToken);

            }
        }
        
    }

}
