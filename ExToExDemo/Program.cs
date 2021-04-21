using System;
using RabbitMQ.Client;
using System.Text;

namespace ExToExDemo
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

            string exNameOne = "ex.net.1";
            string exNameTwo = "ex.net.2";

            string queueOne = "queue.net.1";
            string queueTwo = "queue.net.2";

            channel.ExchangeDeclare(exNameOne, "direct", true, false, null);
            channel.ExchangeDeclare(exNameTwo, "direct", true, false, null);

            channel.QueueDeclare(queueOne, true, false, false, null);
            channel.QueueDeclare(queueTwo, true, false, false, null);


            channel.QueueBind(queueOne, exNameOne, "key1");
            channel.QueueBind(queueTwo, exNameTwo, "key2");

            channel.ExchangeBind(exNameTwo, exNameOne, "key2");

            channel.BasicPublish(exNameOne, "key1", null, Encoding.UTF8.GetBytes("Message with routing key 'key1' from .net Core(ExToEx.exe)"));
            channel.BasicPublish(exNameOne, "key2", null, Encoding.UTF8.GetBytes("Message with routing key 'key2' from .net Core(ExToEx.exe)"));


            Console.WriteLine("Exchange to Exchange Demo");
            Console.ReadKey();
        }
    }
}
