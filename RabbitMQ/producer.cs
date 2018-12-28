using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ
{
    public class producer
    {
        public static IConnection CreateConnection()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.Port = 5672;
            factory.VirtualHost = "/";
            factory.UserName = "guest";
            factory.Password = "guest";

            return factory.CreateConnection();
        }

        public static void SendMessage(string message)
        {
            string exchangeName = "wytExchange";
            string queueName = "wytQueue";
            string routeKeyName = "wytRouteKey";
            using (IConnection connection = CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: exchangeName, type: "direct", durable: true, autoDelete: false, arguments: null);
                    channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    IBasicProperties properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    Byte[] body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: exchangeName, routingKey: routeKeyName, basicProperties: properties, body: body);
                }
            }
        }
    }
}
