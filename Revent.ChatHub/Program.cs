using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Revent.ChatHub.Hubs;
using Revent.DataAccess.Implementation.DbContexts;
using Revent.DataAccess.Implementation.UnitOfWork;
using Revent.Services.IServices;
using Revent.Services.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

// Adding signalR in DI container
services.AddSignalR(options => { options.HandshakeTimeout = TimeSpan.MaxValue; });

//Add logger
builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

// Configure database contexts
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(config.GetConnectionString("DefaultDbConnectionString"))
);

builder.Services.AddDbContext<ReventDbContext>(options =>
    options.UseNpgsql(config.GetConnectionString("DefaultDbConnectionString"))
);

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

services.AddScoped<IUnitOfWork,UnitOfWork>();
services.AddScoped<IGroupChatService,GroupChatService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

//chat hub endpoint
app.MapHub<ChatHub>("chat-hub");

app.Run();
