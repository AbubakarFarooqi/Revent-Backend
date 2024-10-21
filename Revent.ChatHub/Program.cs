using Revent.ChatHub.Hubs;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Adding signalR in DI container
services.AddSignalR(options => { options.HandshakeTimeout = TimeSpan.MaxValue; });

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

//chat hub endpoint
app.MapHub<ChatHub>("chat-hub");

app.Run();
