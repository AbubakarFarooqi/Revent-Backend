using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Tnef;
using Revent.Common.CommonModels;
using Revent.Services.IServices;
using Revent.Services.Services;
using Revent.Common.Extensions;
using Revent.Common.Constants;
using Revent.EFCore.DataModel.Models;
using Revent.Common.CommonDtos;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Revent.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly ILogger<TicketController> _logger;
        private readonly ILookupService _lookupService;
        private readonly IEventService _eventService;
        public TicketController(ITicketService ticketService, ILogger<TicketController> logger, ILookupService lookupService,IEventService eventService)
        {
            _ticketService = ticketService;
            _logger = logger;
            _lookupService = lookupService;
            _eventService = eventService;
        }
        [HttpGet]
        public IActionResult GetTicketByEventId(int id)
        {
            try
            {
                _logger.LogInformation($"Starting getting tickets of event with id {id}");
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiError { Message = ModelState.GetValidationErrorsAsCsv(), StatusCode = Constants.BAD_REQUEST_STATUS_CODE });
                }

                var tickets = _ticketService.GetAllByEventIdAsync(id);

                _logger.LogInformation($"Tickets Has Been Fetched for event id {id}");

                return Ok(new ApiResponse<List<Tickets>> { Data = tickets, StatusCode = Constants.OK_STATUS_CODE });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(Constants.INTERNAL_SERVER_ERROR, "Internal Server Error" + ex.ToString());
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateTicket(TicketDto ticketCreateDto)
        {
            try
            {
                _logger.LogInformation($"Starting creating tickets for event with id {ticketCreateDto.EventId}");
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiError { Message = ModelState.GetValidationErrorsAsCsv(), StatusCode = Constants.BAD_REQUEST_STATUS_CODE });
                }

                var event_ = await _eventService.GetEventByIdAsync(ticketCreateDto.EventId);

                if(event_ == null || event_.IsDeleted == true) return NotFound(new ApiError { Message = $"Event with id {ticketCreateDto.EventId} is not found", StatusCode = Constants.NOT_FOUND});

                var ticketTypeLkp = await _lookupService.GetLookupById(ticketCreateDto.TicketTypeId);

                if (ticketTypeLkp == null || !Constants.TICKET_TYPE_LOOKUP.Contains(ticketTypeLkp.Data ?? "")) return NotFound(new ApiError { Message = "Ticket Type not found in database ", StatusCode = Constants.BAD_REQUEST_STATUS_CODE });

                await _ticketService.CreateTicketAsync(ticketCreateDto);
                _logger.LogInformation($"Tickets Has Been Created for event with id {ticketCreateDto.EventId}");

                return Ok(new ApiResponse<List<Tickets>> { Message = "Ticket Has Been Created", StatusCode = Constants.OK_STATUS_CODE });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(Constants.INTERNAL_SERVER_ERROR, "Internal Server Error" + ex.ToString());
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateTicket(int id,TicketDto ticketUpdateDto)
        {
            try
            {
                _logger.LogInformation($"Starting updating tickets with id {id}");
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiError { Message = ModelState.GetValidationErrorsAsCsv(), StatusCode = Constants.BAD_REQUEST_STATUS_CODE });
                }

                var event_ = await _eventService.GetEventByIdAsync(ticketUpdateDto.EventId);

                if (event_ == null || event_.IsDeleted == true) return NotFound(new ApiError { Message = $"Event with id {ticketUpdateDto.EventId} is not found", StatusCode = Constants.NOT_FOUND });

                var ticketTypeLkp = await _lookupService.GetLookupById(ticketUpdateDto.TicketTypeId);

                if (ticketTypeLkp == null || !Constants.TICKET_TYPE_LOOKUP.Contains(ticketTypeLkp.Data ?? "")) return NotFound(new ApiError { Message = "Ticket Type not found in database ", StatusCode = Constants.BAD_REQUEST_STATUS_CODE });

                await _ticketService.UpdateTicketAsync(id,ticketUpdateDto);
                _logger.LogInformation($"Ticket Has Been updated for with id {id}");

                return Ok(new ApiResponse<List<Tickets>> { Message = "Ticket Has Been Updated", StatusCode = Constants.OK_STATUS_CODE });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(Constants.INTERNAL_SERVER_ERROR, "Internal Server Error" + ex.ToString());
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting ticket with ID: {id}");

                // Attempt to find the event by its ID
                var eventEntity = await _ticketService.GetByIdAsync(id);
                if (eventEntity == null)
                    return NotFound(new ApiError
                    {
                        Message = "Ticket not found in database",
                        StatusCode = Constants.NOT_FOUND
                    });

                // Delete the event
                await _ticketService.DeleteAsync(id);

                _logger.LogInformation($"Ticket with ID: {id} has been deleted");

                //return Ok(new ApiResponse<string>
                //{
                //    Message = $"Event with ID: {eventId} has been deleted",
                //    StatusCode = Constants.OK_STATUS_CODE
                //});

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(Constants.INTERNAL_SERVER_ERROR, "Internal Server Error: " + ex.ToString());
            }
        }
    }
}