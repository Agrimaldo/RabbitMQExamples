using System;
using RabbitMQ.Client;
using System.Text;

namespace Publisher
{
    class Program
    {
        static IConnection connection;
        static IModel channel;
        static string queueOne = "my.queue.1";
        static string exName = "ex.fanout";

        static void Main()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.VirtualHost = "/";
            factory.Port = 5672;
            factory.UserName = "guest";
            factory.Password = "guest";

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            while (true)
            {
                Console.Write("Enter Message :");
                string message = Console.ReadLine();

                if (message == "exit")
                {
                    break;
                }

                channel.BasicPublish(exName, "", null, Encoding.UTF8.GetBytes(message));
            }

            channel.Close();
            connection.Close();
        }
    }
}
