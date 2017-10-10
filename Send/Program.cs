using System;
using RabbitMQ.Client;
using Serilog;
using System.Text;

namespace Send
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.ColoredConsole()
                .WriteTo.RollingFile(@"E:\C#\Приват\RabbitMQ\Send\Logs\Log-{Date}.txt")
                .CreateLogger();

            logger.Debug("The Send.cs was started");
            
            var factory = new ConnectionFactory() { HostName = "localhost" };
            logger.Debug("Created new Connectionfactory with name= {HostName}", factory.HostName);
            using (var connection = factory.CreateConnection())
            {
                logger.Debug("Created new connection {connection}", connection.ToString());
                using(var channel = connection.CreateModel())
                {
                    string queueName = "first";
                    channel.QueueDeclare(
                        queue: queueName,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                    logger.Debug("Created queye {name} ", queueName);

                    string message = "Hello RabbitMQ!!";
                    var data = Encoding.UTF8.GetBytes(message);
                    logger.Debug("Encoded message: {message} to bytes", message);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: "first",
                        basicProperties: null,
                        body: data);
                    logger.Information("Published data");
                }
                Console.WriteLine("Press Enter to exit");
                Console.ReadLine();
            }
        }
    }
}
