using System;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;
using System.Threading;

namespace WorkerDemo
{
    class Program
    {

        static IConnection connection;
        static IModel channel;
        static string queueOne = "my.queue.1";
        static string workerName = "";


        static void Main(string[] args)
        {
            Console.Write("Worker Name : ");
            workerName = Console.ReadLine();

            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.VirtualHost = "/";
            factory.Port = 5672;
            factory.UserName = "guest";
            factory.Password = "guest";

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) => 
            {
                string message = Encoding.UTF8.GetString(e.Body.ToArray());
                int durationInSeconds = Int32.Parse(message);

                Console.WriteLine($"[{workerName}] Duration : {durationInSeconds}");

                Thread.Sleep(durationInSeconds * 1000);

                Console.WriteLine("finished");

                channel.BasicAck(e.DeliveryTag, false);
            };

            var consumerTag = channel.BasicConsume(queueOne, false, consumer);

            Console.WriteLine("Waiting for messages...");
            Console.ReadKey();


            channel.Close();
            connection.Close();
        }
    }
}
