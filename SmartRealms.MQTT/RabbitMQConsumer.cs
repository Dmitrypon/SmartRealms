using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Diagnostics;

namespace SmartRealms.MQTT
{
    public class RabbitMQConsumer : BackgroundService
    {
        
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQConsumer()
        {

            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "api_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                // Как то обрабатываем полученное сообщение))
                Debug.WriteLine($"Получено сообщение: {content}");

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume("api_queue", false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }


    }
}