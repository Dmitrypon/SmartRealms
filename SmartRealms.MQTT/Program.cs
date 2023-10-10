using Microsoft.AspNetCore.Connections;
using MQTTnet.AspNetCore;
using RabbitMQ.Client;
using System.Text;

namespace SmartRealms.MQTT
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.ConfigureKestrel(options =>
            {
                // This will allow MQTT connections based on TCP port 1883.
                options.ListenAnyIP(1883, l => l.UseMqtt());

                // This will allow MQTT connections based on HTTP WebSockets with URI "localhost:5000/mqtt"
                // See code below for URI configuration.
                options.ListenAnyIP(5000); // Default HTTP pipeline
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.WebHost.ConfigureKestrel(options =>
            {
                // This will allow MQTT connections based on TCP port 1883.
                options.ListenAnyIP(1883, l => l.UseMqtt());

                // This will allow MQTT connections based on HTTP WebSockets with URI "localhost:5000/mqtt"
                // See code below for URI configuration.
                options.ListenAnyIP(5000); // Default HTTP pipeline
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHostedMqttServer(
                 optionsBuilder =>
                 {
                     optionsBuilder.WithDefaultEndpoint();
                 });

            builder.Services.AddMqttConnectionHandler();
            builder.Services.AddConnections();

            builder.Services.AddSingleton<MqttController>();

            builder.Services.AddHostedService<RabbitMQConsumer>();
            builder.Services.AddHostedService<RabbitMQProducer>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            //app.UseAuthorization();


            //app.MapControllers();

            app.UseRouting();

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapConnectionHandler<MqttConnectionHandler>(
                        "/mqtt",
                        httpConnectionDispatcherOptions => httpConnectionDispatcherOptions.WebSockets.SubProtocolSelector =
                            protocolList => protocolList.FirstOrDefault() ?? string.Empty);
                });

            app.UseMqttServer(
                server =>
                {
                    var mqttController = app.Services.GetRequiredService<MqttController>();
                    /*
                     * Attach event handlers etc. if required.
                     */
                    server.ValidatingConnectionAsync += mqttController.ValidateConnection;
                    server.ClientConnectedAsync += mqttController.OnClientConnected;
                });

            app.Run();

        }

        //specify the MQ server we're connecting to
        //     in our case its localhost since we're running
        //     in a local docker container


            //var factory = new ConnectionFactory() { HostName = "localhost" };

        //// 1. create connection
        //using (var connection = factory.CreateConnection())

        //// 2. create channel
        //using (var channel = connection.CreateModel())
        //{
        //    // 3. connect to the queue
        //    channel.QueueDeclare(queue: "queue",
        //                         durable: false,
        //                         exclusive: false,
        //                         autoDelete: false,
        //                         arguments: null);

        //    int index = 1;
        //    while (index <= 999)
        //    {
        //        // we need to write data in the form of bytes
        //        string message = $"{index}|SuperRabbit{1000 + index}|1Carrot,2Carrot,3Carrot,4Carrot|1|{DateTime.UtcNow.ToLongDateString()}|0|0";
        //        var body = Encoding.UTF8.GetBytes(message);

        //        // push content into the queue 
        //        channel.BasicPublish(exchange: "", routingKey: "queue", basicProperties: null, body: body);
        //        Console.WriteLine(" [x] Sent {0}", message); index++; Thread.Sleep(1000);
        //    }
        //}


        }

    }


