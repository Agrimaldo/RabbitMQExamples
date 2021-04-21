using System;
using RabbitMQ.Client;
using System.Text;
using System.Collections.Generic;

namespace HeadersDemo
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

            string _exchangeName = "ex.headers.net";
            string _queueOne = "headers.queue.net.1";
            string _queueTwo = "headers.queue.net.2";

            channel.ExchangeDeclare(_exchangeName, "headers", true, false, null);
            channel.QueueDeclare(_queueOne, true, false, false, null);
            channel.QueueDeclare(_queueTwo, true, false, false, null);

            channel.QueueBind(_queueOne, _exchangeName, "", new Dictionary<string, object>()
            {
                { "x-match", "all" },
                { "job", "convert" },
                { "format", "jpeg" }
            });

            channel.QueueBind(_queueTwo, _exchangeName, "", new Dictionary<string, object>()
            {
                { "x-match", "any" },
                { "job", "convert" },
                { "format", "jpeg" }
            });

            IBasicProperties props = channel.CreateBasicProperties();

            props.Headers = new Dictionary<string, object>();

            props.Headers.Add("job", "convert");
            props.Headers.Add("format", "jpeg");

            channel.BasicPublish(_exchangeName, "", props, Encoding.UTF8.GetBytes("First Message"));


            props = channel.CreateBasicProperties();
            props.Headers = new Dictionary<string, object>();

            props.Headers.Add("job", "convert");
            props.Headers.Add("format", "bmp");
            channel.BasicPublish(_exchangeName, "", props, Encoding.UTF8.GetBytes("Second Message"));

            Console.WriteLine("Hello World!");
        }
    }
}
