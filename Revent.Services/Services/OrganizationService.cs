using AutoMapper;
using Revent.Common.CommonDtos;
using Revent.DataAccess.Implementation.IRepositories;
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
    public class OrganizationService : IOrganizationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrganizationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateAsync(OrganizationCreateDto organizationDto)
        {

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var organization = _mapper.Map<Organizations>(organizationDto);
                await _unitOfWork.OrganizationRepository.AddAsync(organization);

                await _unitOfWork.CommitTransactionAsync();
                
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }

        }

        public async Task DeleteAsync(int id)
        {

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var organization = await GetByIdAsync(id);
                organization.IsDeleted = true;
                _unitOfWork.OrganizationRepository.UpdateAsync(organization);

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }

        }

        public async Task<Organizations> GetByIdAsync(int id)
        {
            var organization = await _unitOfWork.OrganizationRepository.GetAsync(id);
            return organization;
        }

        public async Task<Organizations> GetByUserIdAsync(int userId)
        {
            var organization = await _unitOfWork.OrganizationRepository.GetByUserIdAsync(userId);
            return organization;
        }

        public async Task UpdateAsync(OrganizationUpdateDto organizationDto)
        {

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var organization = await _unitOfWork.OrganizationRepository.GetAsync(organizationDto.OrganizationId);

                _mapper.Map(organizationDto,organization);

                _unitOfWork.OrganizationRepository.UpdateAsync(organization);
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
