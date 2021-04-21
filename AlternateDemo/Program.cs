using System;
using RabbitMQ.Client;
using System.Text;
using System.Collections.Generic;

namespace AlternateDemo
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

            string exFanout = "ex.net.fanout";
            string exDirect = "ex.net.direct";

            string queueOne = "queue.net.1";
            string queueTwo = "queue.net.2";
            string queueThree = "queue.net.3";

            channel.ExchangeDeclare(exFanout, "fanout", true, false, null);
            channel.ExchangeDeclare(exDirect, "direct", true, false, new Dictionary<string, object>() 
            {
                { "alternate-exchange", exFanout }
            });

            channel.QueueDeclare(queueOne, true, false, false, null);
            channel.QueueDeclare(queueTwo, true, false, false, null);
            channel.QueueDeclare(queueThree, true, false, false, null);

            channel.QueueBind(queueOne, exDirect, "video");
            channel.QueueBind(queueTwo, exDirect, "image");
            channel.QueueBind(queueThree, exFanout, "");

            channel.BasicPublish(exDirect, "video", null, Encoding.UTF8.GetBytes("Message with routing key 'video' from .net Core(AlternateDemo.exe)"));
            channel.BasicPublish(exDirect, "image", null, Encoding.UTF8.GetBytes("Message with routing key 'image' from .net Core(AlternateDemo.exe)"));
            channel.BasicPublish(exDirect, "text", null, Encoding.UTF8.GetBytes("Message with routing key 'text' from .net Core(AlternateDemo.exe)"));





        }
    }
}
