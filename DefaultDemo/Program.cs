using System;
using RabbitMQ.Client;
using System.Text;

namespace DefaultDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            IConnection connection;
            IModel channel;

            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.VirtualHost = "/";
            factory.Port = 5672;
            factory.UserName = "guest";
            factory.Password = "guest";

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            string _queueOne = "default.queue.net.1";
            string _queueTwo = "default.queue.net.2";

            channel.QueueDeclare(_queueOne, true, false, false, null);
            channel.QueueDeclare(_queueTwo, true, false, false, null);

            channel.BasicPublish("", _queueOne, null, Encoding.UTF8.GetBytes($"message with routing key {_queueOne}"));
            channel.BasicPublish("", _queueTwo, null, Encoding.UTF8.GetBytes($"message with routing key {_queueTwo}"));


            Console.WriteLine("Hello World!");
        }
    }
}
