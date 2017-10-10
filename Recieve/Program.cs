using System.Text;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Serilog;

namespace Recieve
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.RollingFile(@"E:\C#\Приват\RabbitMQ\Send\Logs\Log-{Date}.txt").WriteTo.ColoredConsole().CreateLogger();
            var factory = new ConnectionFactory() { HostName = "localhost" };
            logger.Debug("Created new factory {factory}", factory.HostName);
            using (var connection = factory.CreateConnection())
            {
                logger.Debug("Created connection {connection}", connection.ToString());
                using (var channel = connection.CreateModel())
                {
                    string queyeName = "first";
                    channel.QueueDeclare(queyeName, false, false, false, null);
                    logger.Debug("Created queye {queye}", queyeName);

             
                    var consumer = new EventingBasicConsumer(channel);
                    logger.Debug("Created new consumer {consumer}", consumer.ToString());
                    consumer.Received += (model, ea) =>
                    {
                        var data = ea.Body;
                       
                        var message = Encoding.UTF8.GetString(data);
                        logger.Debug("Get messagge from ea.Body {message}", message);
                        System.Console.WriteLine(message);
                    };

                    channel.BasicConsume("first", true, consumer);
                    System.Console.WriteLine("Type [enter] to exit");
                    System.Console.ReadLine();
                    ;
                }
            }
       
        }


    }
}

