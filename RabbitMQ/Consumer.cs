using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RabbitMQ
{
    public class Consumer
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

        public static string ReceiveMessage()
        {
            string message = "";
            string exchangeName = "wytExchange";
            string queueName = "wytQueue";
            string routeKeyName = "wytRouteKey";
            using (IConnection connection = CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: exchangeName, type: "direct", durable: true, autoDelete: false, arguments: null);

                    channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routeKeyName, arguments: null);

                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        Byte[] body = ea.Body;
                        message = Encoding.UTF8.GetString(body);

                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    };

                    channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

                    Thread.Sleep(10000);
                }
            }
            return message;
        }
    }
}
