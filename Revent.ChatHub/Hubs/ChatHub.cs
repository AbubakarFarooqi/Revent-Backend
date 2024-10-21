using Microsoft.AspNetCore.SignalR;

namespace Revent.ChatHub.Hubs
{
    public class ChatHub:Hub<IChatHub>
    {
        ILogger<ChatHub> _logger;
        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation($"userId has been connected");
            await base.OnConnectedAsync();
        }
        public async Task JoinGroup(string groupId,)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId,groupName);
            await Clients.Group(groupName).RecieveMessage($"{Context.ConnectionId} has joined the group {groupName}.");
        }

        public async Task SendMessageToGroup(string groupName,string message)
        {
            await Clients.Group(groupName).RecieveMessage(message);
        }

        
    }
}
