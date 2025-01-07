using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Revent.Common.CommonDtos;
using Revent.Common.CommonModels;
using Revent.Common.Constants;
using Revent.Services.IServices;
using Revent.Common.Extensions;
using Revent.Services.Services;
using Revent.EFCore.DataModel.Models;


namespace Revent.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;
        private readonly ILogger<OrganizationController> _logger;
        private readonly IUserService _userService;
        private readonly ILookupService _lookupService;

        public OrganizationController( ILookupService lookupService ,IUserService userService, ILogger<OrganizationController> logger, IOrganizationService organizationService)
        {
            _organizationService = organizationService;
            _logger = logger;
            _userService = userService;
            _lookupService = lookupService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrganization(OrganizationCreateDto organizationDto)
        {
            try
            {
                _logger.LogInformation("Starting creating an organization");
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiError { Message = ModelState.GetValidationErrorsAsCsv(), StatusCode = Constants.BAD_REQUEST_STATUS_CODE });
                }

                // Validating Organizer
                var organizaer = await _userService.FindUserById(organizationDto.UserId);
                if (organizaer == null) return BadRequest(new ApiError { Message = "Organizer with given Id not found in database", StatusCode = Constants.BAD_REQUEST_STATUS_CODE });
                
                organizationDto.ContactNumber = organizaer.PhoneNumber;
                
                var isUserAlreadyAnOrganization = await _organizationService.GetByUserIdAsync(organizationDto.UserId);
                if(isUserAlreadyAnOrganization != null) return BadRequest(new ApiError { Message = "This User is already an organization", StatusCode = Constants.BAD_REQUEST_STATUS_CODE });
                
                //Validating OrganizationTye
                var organizationTypeLkp = await _lookupService.GetLookupById(organizationDto.OrganizationType);
                if (organizationTypeLkp == null || organizationTypeLkp.Value != Constants.ORGANIZATION_TYPE_LOOKUP) return BadRequest(new ApiError { Message = "Organization Type not found in database", StatusCode = Constants.BAD_REQUEST_STATUS_CODE });

                //eventCreateDto.Organizer = organizaer;
                //eventCreateDto.EventType = eventTypeLkp;

                await _organizationService.CreateAsync(organizationDto);

                _logger.LogInformation("Organization Has Been Created");

                return Ok(new ApiResponse<object> { Message = "Organization Has been Created", StatusCode = Constants.OK_STATUS_CODE });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(Constants.INTERNAL_SERVER_ERROR, "Internal Server Error" + ex.ToString());
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrganization(int id, OrganizationUpdateDto organizationDto)
        {


            try
            {

                _logger.LogInformation("Starting updating an organization");


                if (id != organizationDto.OrganizationId)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiError { Message = ModelState.GetValidationErrorsAsCsv(), StatusCode = Constants.BAD_REQUEST_STATUS_CODE });
                }

                //Validating OrganizationTye
                var organizationTypeLkp = await _lookupService.GetLookupById(organizationDto.OrganizationType);
                if (organizationTypeLkp == null || organizationTypeLkp.Value != Constants.ORGANIZATION_TYPE_LOOKUP) return BadRequest(new ApiError { Message = "Organization Type not found in database", StatusCode = Constants.BAD_REQUEST_STATUS_CODE });
                
                await _organizationService.UpdateAsync(organizationDto);

                _logger.LogInformation("Organization Has Been Updated");

                return Ok(new ApiResponse<object> { Message = "Organization Has been Updated", StatusCode = Constants.OK_STATUS_CODE });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(Constants.INTERNAL_SERVER_ERROR, "Internal Server Error" + ex.ToString());
            }

            


        }


        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetOrganizationById(int id)
        {


            try
            {

                _logger.LogInformation($"Starting Getting  organization with Id {id}");

                var organization = await _organizationService.GetByIdAsync(id);

                if(organization == null) return NotFound(new ApiResponse<string> { Message = $"organization with Id {id} not found in Database" ,StatusCode = Constants.NOT_FOUND});

                _logger.LogInformation("Organization Has Been Fetched");

                return Ok(new ApiResponse<Organizations> { Data = organization, Message = "Organization Has been fetched", StatusCode = Constants.OK_STATUS_CODE });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(Constants.INTERNAL_SERVER_ERROR, "Internal Server Error" + ex.ToString());
            }

        }

        [HttpGet("GetByUserId/{id}")]
        public async Task<IActionResult> GetOrganizationByUserId(int id)
        {


            try
            {

                _logger.LogInformation($"Starting Getting  organization with UserId {id}");

                var organization = await _organizationService.GetByUserIdAsync(id);

                if (organization == null) return NotFound(new ApiResponse<string> { Message = $"organization with UserId {id} not found in Database", StatusCode = Constants.NOT_FOUND });

                _logger.LogInformation("Organization Has Been Fetched");

                return Ok(new ApiResponse<Organizations> { Data = organization, Message = "Organization Has been fetched", StatusCode = Constants.OK_STATUS_CODE });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(Constants.INTERNAL_SERVER_ERROR, "Internal Server Error" + ex.ToString());
            }

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOrganization(int id)
        {
            try
            {

                _logger.LogInformation($"Starting Deleteing organization with Id {id}");

                var organization = await _organizationService.GetByIdAsync(id);

                if (organization == null) return NotFound(new ApiResponse<string> { Message = $"organization with Id {id} not found in Database", StatusCode = Constants.NOT_FOUND });

                await _organizationService.DeleteAsync(id); ;

                _logger.LogInformation("Organization Has Been Deleted");

                return Ok(new ApiResponse<Organizations> { Message = "Organization Has been deletd", StatusCode = Constants.OK_STATUS_CODE });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(Constants.INTERNAL_SERVER_ERROR, "Internal Server Error" + ex.ToString());
            }

        }
    }
}
