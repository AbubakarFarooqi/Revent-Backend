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
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TicketService(IUnitOfWork unitOfwork,IMapper mapper)
        {
            _unitOfWork = unitOfwork;
            _mapper = mapper;
        }

        public async Task CreateTicketAsync(TicketDto ticketDto)
        {

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var ticket =  _mapper.Map<Tickets>(ticketDto);

                await _unitOfWork.TicketRepository.AddAsync(ticket);

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }

        }

        public List<Tickets> GetAllByEventIdAsync(int id)
        {
            return  _unitOfWork.TicketRepository.GetByEventIdAsync(id);

        }

        public async Task UpdateTicketAsync(int id, TicketDto ticketUpdateDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var entity = await _unitOfWork.TicketRepository.GetAsync(id);

                _mapper.Map(ticketUpdateDto, entity);

                _unitOfWork.TicketRepository.UpdateAsync(entity);

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }
        }
    }
}
