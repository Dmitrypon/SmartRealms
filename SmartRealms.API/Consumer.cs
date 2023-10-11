using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Text;

namespace SmartRealms.API
{
    
public class Consumer : BackgroundService
    {
        private readonly ILogger<Consumer> _logger;
        private readonly IConnection _connection;
        private readonly IModel _channel;


        public Consumer(ILogger<Consumer> logger)
        {
            _logger = logger;

            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "mqtt_queue", durable: false, exclusive: false, arguments: null);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                Debug.WriteLine($"Получено сообщение: {content}");

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume("mqtt_queue", false, consumer);


            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);

            }
        }

    }

 }
