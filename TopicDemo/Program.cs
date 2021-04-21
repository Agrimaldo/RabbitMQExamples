using System;
using RabbitMQ.Client;
using System.Text;

namespace TopicDemo
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

            string _exName = "ex.topic.net";
            string _queueOne = "my.queue.one";
            string _queueTwo = "my.queue.two";
            string _queueThree = "my.queue.three";

            channel.ExchangeDeclare(_exName, "topic", true, false, null);

            channel.QueueDeclare(_queueOne, true, false, false, null);
            channel.QueueDeclare(_queueTwo, true, false, false, null);
            channel.QueueDeclare(_queueThree, true, false, false, null);

            channel.QueueBind(_queueOne, _exName, "*.image.*");
            channel.QueueBind(_queueTwo, _exName, "#.image");
            channel.QueueBind(_queueThree, _exName, "image.#");


            channel.BasicPublish(_exName, "convert.image.bmp", null, Encoding.UTF8.GetBytes("message with routing key 'convert.image.bmp'"));
            channel.BasicPublish(_exName, "convert.bitmap.image", null, Encoding.UTF8.GetBytes("message with routing key 'convert.bitmap.image'"));
            channel.BasicPublish(_exName, "image.bitmap.32bit", null, Encoding.UTF8.GetBytes("message with routing key 'image.bitmap.32bit'"));

            Console.WriteLine("Hello World!");

        }
    }
}
