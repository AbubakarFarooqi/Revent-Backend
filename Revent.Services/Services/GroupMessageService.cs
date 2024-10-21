using AutoMapper;
using Revent.Common.CommonDtos;
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
    public class GroupMessageService : IGroupMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GroupMessageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddGroupMessageAsync(GroupMessageDto groupMessageDto)
        {
                
            var groupMessage = _mapper.Map<GroupMessages>(groupMessageDto);
            groupMessage.SentAt = DateTime.Now;
            await _unitOfWork.GroupMessageRepository.AddAsync(groupMessage);

            await _unitOfWork.SaveChangesAsync();
            
           
        }
    }
}
