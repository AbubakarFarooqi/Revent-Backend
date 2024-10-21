using Microsoft.AspNetCore.SignalR;
using Revent.Services.IServices;

namespace Revent.ChatHub.Hubs
{
    public class ChatHub:Hub<IChatHub>
    {
        private readonly ILogger<ChatHub> _logger;
        private readonly IGroupChatService _groupChatService;
        public ChatHub(ILogger<ChatHub> logger, IGroupChatService groupChatService)
        {
            _logger = logger;
            _groupChatService = groupChatService;
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation($"userId has been connected");
            await base.OnConnectedAsync();
        }
        public async Task JoinGroup(string groupId,string userId)
        {
            try
            {
                var groupChat = _groupChatService.GetGroupChatById(int.Parse(groupId));

                if (groupChat == null) return; // here shouyld be some ack to client

                var participant = groupChat.Participants.Where(x => x.UserId == int.Parse(userId)).FirstOrDefault();

                if (participant == null) return; // not a particpant of the group

                string groupName = groupChat.Name ?? "_" + groupChat.Id;
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
              //  await Clients.Group(groupName).RecieveMessage($"{Context.ConnectionId} has joined the group {groupName}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return; // here shouyld be some ack to client
            }
            
        }

        public async Task SendMessageToGroup(string groupId,string userId,string message)
        {
            // Will Implement Redis Cache Here, And Also Search for more optimized way later on

            try
            {
                var groupChat = _groupChatService.GetGroupChatById(int.Parse(groupId));

                if (groupChat == null) return; // here shouyld be some ack to client

                var participant = groupChat.Participants.Where(x => x.UserId == int.Parse(userId)).FirstOrDefault();

                if (participant == null) return; // not a particpant of the group

                string groupName = groupChat.Name ?? "" + "_" + groupChat.Id;

                await Clients.Group(groupName).RecieveMessage(message,participant.User.Firstname +" "+ participant.User.Lastname);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return; // here shouyld be some ack to client
            }
            ;
        }

        
    }
}
