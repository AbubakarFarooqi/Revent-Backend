using AutoMapper;
using Revent.Common.CommonDtos;
using Revent.DataAccess.Implementation.UnitOfWork;
using Revent.EFCore.DataModel.Models;
using Revent.Services.IServices;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.Services.Services
{
    public class EventService : IEventService
    {
        IMapper _mapper;
        IUnitOfWork _unitOfWork;

        public EventService(IMapper mapper,IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task AddEventAsync(EventCreateDto eventCreateDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var eventEntity = _mapper.Map<Events>(eventCreateDto);
                eventEntity.GroupChat = new GroupChats();

                await _unitOfWork.EventRepository.AddAsync(eventEntity);

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }
        }

        public async Task DeleteEventAsync(int id)
        {
            await _unitOfWork.EventRepository.DeleteEvent(id);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Events> GetEventByIdAsync(int id)
        {
            return await _unitOfWork.EventRepository.GetAsync(id);
        }
    }
}
