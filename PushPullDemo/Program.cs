using System;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;
using System.Threading;

namespace PushPullDemo
{
    class Program
    {
        static IConnection connection;
        static IModel channel;
        static string queueOne = "my.queue.1";

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

            //ReadMessageWithPushModel();
            ReadMessageWithPullModel();

            channel.Close();
            connection.Close();
        }

        private static void ReadMessageWithPushModel()
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                string message = Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine($" this message : {message}");
            };

            string consumerTag = channel.BasicConsume(queueOne, true, consumer);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            channel.BasicCancel(consumerTag);
        }

        private static void ReadMessageWithPullModel()
        {
            Console.WriteLine("'pull model' Press any key to exit");

            while (true)
            {
                Console.WriteLine("waiting for a new message....");

                BasicGetResult result = channel.BasicGet(queueOne, true);
                if(result != null)
                {
                    string message = Encoding.UTF8.GetString(result.Body.ToArray());
                    Console.WriteLine($" this message : {message}");
                }

                if (Console.KeyAvailable)
                {
                    var keyInput = Console.ReadKey();
                    if (keyInput.KeyChar == 'e' || keyInput.KeyChar == 'E')
                        return;
                }

                Thread.Sleep(2000);
            }
        }
    }
}
