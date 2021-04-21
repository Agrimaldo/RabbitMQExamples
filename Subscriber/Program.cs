using System;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

namespace Subscriber
{
    class Program
    {
        static IConnection connection;
        static IModel channel;
        static string queueOne = "my.queue.1";
        static void Main()
        {
            Console.Write("Queue name:");
            string queueName = Console.ReadLine();

            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.VirtualHost = "/";
            factory.Port = 5672;
            factory.UserName = "guest";
            factory.Password = "guest";

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, e) =>
            {
                string message = Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine($"Queue [{queueName}] Message : {message}");

            };

            var consumerTag = channel.BasicConsume(queueName, true, consumer);

            Console.WriteLine($"using the queue {queueName}");
            Console.ReadKey();

            channel.Close();
            connection.Close();
        }
    }
}
