using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQDemo.Consumer
{
    public class Program
    {
        static void Main(string[] args)
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "Repasse.RabbitMQ",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (sender, eventArgs) =>
                {
                    byte[] body = eventArgs.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    //* var message = Encoding.UTF8.GetString(eventArgs.Body);

                    Console.WriteLine(Environment.NewLine + "[New message received] " + message);
                };

                channel.BasicConsume(queue: "Repasse.RabbitMQ",
                     autoAck: true,
                     consumer: consumer);

                Console.WriteLine("Aguardando mensagens para processar");
                Console.WriteLine("Enter para sair");
                Console.ReadKey();
            }

        }
    }
}
