using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

namespace Publisher
{
    internal class Program
    {
        public static string HostName { get; private set; }

        static void Main(string[] args)
        {
           
            var factory = new ConnectionFactory();
            factory.HostName = "http://localhost:15672/";
            factory.UserName = "yyy";
            factory.Password = "hello!";

            using var connection = factory.CreateConnection(HostName = "http://localhost:15672/");
             using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            const string message = "Hello World!";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "hello",
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine($" [x] Sent {message}");

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
            //{
            //    using (var channel = connection.CreateModel())
            //    {
            //        channel.QueueDeclare("hello", false, false, false, null);
            //        string message = "Hello World";
            //        var body = Encoding.UTF8.GetBytes(message);
            //        channel.BasicPublish("", "hello", null, body);
            //        Console.WriteLine(" set {0}", message);
            //    }
            //}
        }
    }
}