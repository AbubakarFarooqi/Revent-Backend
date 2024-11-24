
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Revent.EmailConsumer.Consumers;
using Revent.Services.IServices;
using Revent.Services.Services;

IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.Configure<Revent.Common.CommonModels.RabbitMQSetting>(context.Configuration.GetSection("RabbitMQ"));
                    services.Configure<Revent.Common.CommonModels.EmailSettings>(context.Configuration.GetSection("EmailSettings"));
                    services.AddScoped<IEmailService, EmailService>();
                    // Register services
                    // Resgiter db
                    // register logging
                    services.AddHostedService<OtpQueueConsumer>();
                })
                .Build();

await host.RunAsync();