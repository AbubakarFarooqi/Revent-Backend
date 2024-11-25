using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Revent.Common.CommonModels;
using Revent.Services.IServices;
using Revent.Services.Services;
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
        private readonly IServiceScopeFactory _serviceScopeProvider;
        private IConnection _connection;
        private IChannel _channel;
        private string _queueName;

       

        public OtpQueueConsumer(IConfiguration configuration, IOptions<RabbitMQSetting> rabbitMQSettings, IServiceScopeFactory serviceScopeProvider)
        {
            _configuration = configuration;
            _rabbitMQSetting = rabbitMQSettings.Value;
            _serviceScopeProvider = serviceScopeProvider;
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

            int retryCount = 0;
            const int maxRetryAttempts = 10; // Maximum number of retries
            const int baseDelayMs = 2000; // Base delay in milliseconds

            while (retryCount < maxRetryAttempts)
            {
                try
                {
                    Console.WriteLine($"Attempting to connect to RabbitMQ (Attempt {retryCount + 1})...");
                    _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
                    _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

                    // Declare exchange
                    _channel.ExchangeDeclareAsync(Revent.Common.Constants.Constants.MESSAGE_BUS_EMAIL_EXCHANGE_NAME, type: ExchangeType.Topic).GetAwaiter().GetResult();

                    // Declare queues
                    _channel.QueueDeclareAsync(queue: Revent.Common.Constants.Constants.EMAIL_OTP_QUEUE, durable: true, exclusive: false, autoDelete: false).GetAwaiter().GetResult();
                    _channel.QueueBindAsync(queue: Revent.Common.Constants.Constants.EMAIL_OTP_QUEUE, exchange: Revent.Common.Constants.Constants.MESSAGE_BUS_EMAIL_EXCHANGE_NAME, routingKey: Revent.Common.Constants.Constants.EMAIL_OTP_QUEUE).GetAwaiter().GetResult();

                    _connection.ConnectionShutdownAsync += RabbitMQConnectionShutdown;

                    Console.WriteLine("--> Connected to message bus");
                    break; // Exit the retry loop on success
                }
                catch (Exception ex)
                {
                    retryCount++;
                    Console.WriteLine($"--> Failed to connect to RabbitMQ: {ex.Message}");

                    if (retryCount >= maxRetryAttempts)
                    {
                        Console.WriteLine("--> Maximum retry attempts reached. Application will continue running without RabbitMQ.");
                        break;
                    }

                    int delay = baseDelayMs * (int)Math.Pow(2, retryCount - 1); // Exponential backoff
                    Console.WriteLine($"Retrying in {delay / 1000} seconds...");
                    Task.Delay(delay).GetAwaiter().GetResult();
                }
            }
        }

        private async Task RabbitMQConnectionShutdown(object sender, ShutdownEventArgs @event)
        {
            Console.WriteLine("--> Message Bus Connection Shutdown");
        }

        public override void Dispose()
        {
            
            if (_channel.IsOpen)
            {
                _connection.CloseAsync();
                _channel.CloseAsync();
            }
            base.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"-> Received message from {_queueName}: {message}");

                using (var scope = _serviceScopeProvider.CreateScope())
                {

                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                    emailService.SendMail(subject: "THis is subject", body: "This is body", to: "muhammadabubakarsiddiquefarooq@gmail.com");
                }
                Console.WriteLine($"--> Message has been processed at ${DateTime.Now}");
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
