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
    }

}


