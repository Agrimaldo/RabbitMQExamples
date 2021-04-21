using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FanoutConsumer.ConsoleApp
{
    class Program
    {
        static IConnection connection;
        static IModel channel;
        static void Main(string[] args)
        {


            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.VirtualHost = "/";
            factory.Port = 5672;
            factory.UserName = "guest";
            factory.Password = "guest";

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += ConsumerReceived;

            string _queueOne = "my.queue.one.net";

            var consumerTag = channel.BasicConsume(_queueOne, false, consumer);

            Console.WriteLine("FanoutConsumer waiting...");
            Console.ReadKey();
        }

        private static void ConsumerReceived(object sender, BasicDeliverEventArgs e)
        {
            string message = Encoding.UTF8.GetString(e.Body.ToArray());

            Console.WriteLine($"Message : {message}");

            channel.BasicNack(e.DeliveryTag, false, false);
        }
    }
}
