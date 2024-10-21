using Microsoft.AspNetCore.SignalR;

namespace Revent.ChatHub.Hubs
{
    public class ChatHub:Hub<IChatHub>
    {
        public ChatHub() { }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.RecieveMessage("Azan");
        }
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId,groupName);
            await Clients.Group(groupName).RecieveMessage($"{Context.ConnectionId} has joined the group {groupName}.");
        }

        public async Task SendMessageToGroup(string groupName,string message)
        {
            await Clients.Group(groupName).RecieveMessage(message);
        }
        public async Task SendMessageToClient(string message)
        {
            await Clients.All.RecieveMessage(message);
        }
    }
}
