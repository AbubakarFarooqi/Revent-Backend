using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Revent.Common.CommonModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Revent.EmailConsumer.Consumers
{
    public class OtpQueueConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly RabbitMQSetting _rabbitMQSetting;
        private readonly IServiceProvider _serviceProvider;
        private IConnection _connection;
        private IChannel _channel;
        private string _queueName;

        public OtpQueueConsumer(IConfiguration configuration, IOptions<RabbitMQSetting> rabbitMQSettings, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _rabbitMQSetting = rabbitMQSettings.Value;
            _serviceProvider = serviceProvider;
            _queueName = Revent.Common.Constants.Constants.EMAIL_OTP_QUEUE;

            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
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

        public override void Dispose()
        {
            base.Dispose();
            if (_channel.IsOpen)
            {
                _connection.CloseAsync();
                _channel.CloseAsync();
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($" [x] Received message from {_queueName}: {message}");

                //using var scope = _serviceProvider.CreateScope();
                //var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();
                //await emailService.ProcessMessageAsync(message);
                Console.WriteLine($"--> Message Recieved ${message}");
                //channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsumeAsync(queue: _queueName, autoAck: true, consumer: consumer);
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
            //return Task.CompletedTask;
        }
    }
}
