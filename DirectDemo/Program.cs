using System;
using System.Text;
using RabbitMQ.Client;

namespace DirectDemo
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

            string _exName = "ex.direct.net";

            string _queueInfo = "my.infos.net";
            string _queueWarning = "my.warnings.net";
            string _queueError = "my.errors.net";

            channel.ExchangeDeclare(_exName, "direct", true, false, null);

            channel.QueueDeclare(_queueInfo, true, false, false, null);
            channel.QueueDeclare(_queueWarning, true, false, false, null);
            channel.QueueDeclare(_queueError, true, false, false, null);

            channel.QueueBind(_queueInfo, _exName, "info");
            channel.QueueBind(_queueWarning, _exName, "warning");
            channel.QueueBind(_queueError, _exName, "error");

            channel.BasicPublish(_exName, "info", null, Encoding.UTF8.GetBytes("Message with routing key 'info' from .net Core "));
            channel.BasicPublish(_exName, "warning", null, Encoding.UTF8.GetBytes("Message with routing key 'warning' from .net Core "));
            channel.BasicPublish(_exName, "error", null, Encoding.UTF8.GetBytes("Message with routing key 'error' from .net Core "));

            Console.WriteLine("Direct Demo");
            Console.ReadKey();
        }
    }
}
