namespace Revent.ChatHub.Hubs
{
    public interface IChatHub
    {
        Task RecieveMessage(string message,string username);
    }
}
