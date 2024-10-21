using Revent.DataAccess.Implementation.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.DataAccess.Implementation.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        ILookupRepository LookupRepository { get; }
        IEventRepository EventRepository { get; }
        IGroupChatRepository GroupChatRepository { get; }
        IGroupMessageRepository GroupMessageRepository { get; }

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task SaveChangesAsync();
        Task<int> CompleteAsync();
    }
}
