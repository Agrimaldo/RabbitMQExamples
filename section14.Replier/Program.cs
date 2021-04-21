using System;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

namespace section14.Replier
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

            string queueResponse = "my.response";
            string queueRequest = "my.request";

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                string request = Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine($" request : {request}");

                string response = $"Response for {request}";

                channel.BasicPublish("", queueResponse, null, Encoding.UTF8.GetBytes(response));
            };

            channel.BasicConsume(queueRequest, true, consumer);

            Console.WriteLine("press any key for exit");
            Console.ReadKey();

            channel.Close();
            connection.Close();
        }
    }
}
