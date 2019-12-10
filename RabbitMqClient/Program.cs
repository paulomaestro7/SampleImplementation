using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMqClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest", Port = 5672 };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var queue = "task_queue";

            channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);

            Console.WriteLine("Start send message");

            while (true)
            {
                Console.WriteLine("");
                Console.WriteLine("Write the message");

                var text = Console.ReadLine();
                Console.Clear();

                var body = Encoding.UTF8.GetBytes(text);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "",
                                     routingKey: queue,
                                     basicProperties: properties,
                                     body: body);

                Console.WriteLine("Send: " + text);
            }
        }
    }
}
