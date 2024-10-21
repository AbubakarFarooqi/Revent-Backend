using Revent.DataAccess.Implementation.DbContexts;
using Revent.DataAccess.Implementation.Repositories;
using Revent.DataAccess.Implementation.UnitOfWork;
using Revent.EFCore.DataModel.Models;
using Revent.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.Services.Services
{
    public class GroupChatService :IGroupChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        public GroupChatService( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public GroupChats? GetGroupChatById(int id)
        {
            return _unitOfWork.GroupChatRepository.GetAsync(id);
        }
    }
}
