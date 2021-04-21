using System;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

namespace section14.Requestor
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
                string message = Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine($" response : {message}");
            };

            channel.BasicConsume(queueResponse, true, consumer);


            while (true)
            {
                Console.Write("Enter a Request:");
                string request = Console.ReadLine();

                if (request == "exit")
                    break;

                channel.BasicPublish("", queueRequest, null, Encoding.UTF8.GetBytes(request));
            }

            channel.Close();
            connection.Close();
        }
    }
}
