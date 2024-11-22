using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Revent.Common.CommonModels;
using Revent.Common.Constants;
using Revent.Services.IServices;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Revent.Services.Services
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly RabbitMQSetting _rabbitMQSetting;
        private readonly IConnection _connection;
        private readonly IChannel _channel;

        public RabbitMQPublisher(IOptions<RabbitMQSetting> rabbitMQSettins)
        {
            _rabbitMQSetting = rabbitMQSettins.Value;
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMQSetting.HostName ?? "localhost",
                UserName = _rabbitMQSetting.UserName ?? "guest",
                Password = _rabbitMQSetting.Password ?? "guest"
            };

            try
            {
                _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
                _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

                //declare exchage
                _channel.ExchangeDeclareAsync(Revent.Common.Constants.Constants.MESSAGE_BUS_EMAIL_EXCHANGE_NAME, type: ExchangeType.Topic).GetAwaiter().GetResult();

                //declare queues
                _channel.QueueDeclareAsync(queue: Revent.Common.Constants.Constants.EMAIL_OTP_QUEUE, durable: true, exclusive: false, autoDelete: false).GetAwaiter().GetResult();
                _channel.QueueBindAsync(queue: Revent.Common.Constants.Constants.EMAIL_OTP_QUEUE, exchange: Revent.Common.Constants.Constants.MESSAGE_BUS_EMAIL_EXCHANGE_NAME, routingKey: Revent.Common.Constants.Constants.EMAIL_OTP_QUEUE).GetAwaiter().GetResult();


                
                _connection.ConnectionShutdownAsync += RabbitMQConnectionShutdown;

                Console.WriteLine("--> Connected to message Bus");

            }
            catch (Exception ex)
            {
                Console.WriteLine("--> Could not connect to Message Bus");
                throw ex;
            }
        }

        private async Task RabbitMQConnectionShutdown(object sender, ShutdownEventArgs @event)
        {
            Console.WriteLine("--> Message Bus Connection Shutdown");
        }
        
        async Task IRabbitMQPublisher.PublishMessage(string routingKey, object message)
        {
            try
            {
                if (_channel.IsOpen)
                {
                    var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                    await _channel.BasicPublishAsync(
                    routingKey: routingKey,
                    exchange: Revent.Common.Constants.Constants.MESSAGE_BUS_EMAIL_EXCHANGE_NAME,
                    body: body
                            );

                    Console.WriteLine($" [x] Sent message to {routingKey}");
                }
                else
                {
                    Console.WriteLine($" Connection is not open");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Exception in Publish Message MEthod ${ex.Message}");
                throw ex;
            }
        }
    }
}
