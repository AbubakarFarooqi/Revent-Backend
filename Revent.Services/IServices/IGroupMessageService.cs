using Revent.Common.CommonDtos;

namespace Revent.Services.IServices
{
    public interface IGroupMessageService
    {
        Task AddGroupMessageAsync(GroupMessageDto groupMessageDto);
    }
}
