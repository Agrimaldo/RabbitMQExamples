using System;
using System.Text;
using RabbitMQ.Client;

namespace FanoutPublisher.ConsoleApp
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

            string _exchangeName = "ex.fanout.net";
            string _queueOne = "my.queue.one.net";
            string _queueTwo = "my.queue.two.net";



            channel.ExchangeDeclare(_exchangeName, "fanout", true, false, null);

            channel.QueueDeclare(_queueOne, true, false, false, null);
            channel.QueueDeclare(_queueTwo, true, false, false, null);

            channel.QueueBind(_queueOne, _exchangeName, "");
            channel.QueueBind(_queueTwo, _exchangeName, "");

            channel.BasicPublish(_exchangeName, "", null, Encoding.UTF8.GetBytes("First message from .netcore! "));
            channel.BasicPublish(_exchangeName, "", null, Encoding.UTF8.GetBytes("Second message from .netcore! "));


            Console.WriteLine("Press a key to exit.");
            Console.ReadKey();

            channel.QueueDelete(_queueOne);
            channel.QueueDelete(_queueTwo);
            channel.ExchangeDelete(_exchangeName);

            channel.Close();

            connection.Close();

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
