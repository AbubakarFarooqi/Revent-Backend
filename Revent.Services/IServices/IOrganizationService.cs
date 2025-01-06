using Revent.Common.CommonDtos;
using Revent.EFCore.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.Services.IServices
{
    public interface IOrganizationService
    {
        //Task<List<OrganizationDto>> GetAllAsync();
        Task<Organizations> GetByIdAsync(int id);
        Task<Organizations> GetByUserIdAsync(int userId);
        Task CreateAsync(OrganizationCreateDto organizationDto);
        Task UpdateAsync(OrganizationUpdateDto organizationDto);
        //Task DeleteAsync(int id);
    }
}
