using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Revent.Common.CommonDtos;
using Revent.Common.CommonModels;
using Revent.Common.Constants;
using Revent.Common.Extensions;
using Revent.EFCore.DataModel.Models;
using Revent.Services.IServices;

namespace Revent.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        ILogger<EventController> _logger;
        IUserService _userService;
        ILookupService _lookupService;
        IEventService _eventService;
        //private readonly IRabbitMQPublisher _rabbitMQPublisher;

        //public EventController(ILogger<EventController> logger,IUserService userService, ILookupService lookupService, IEventService eventService, IRabbitMQPublisher rabbitMQPublisher)
        //{
        //    _logger = logger;
        //    _userService = userService;
        //    _lookupService = lookupService;
        //    _eventService = eventService;
        //    _rabbitMQPublisher = rabbitMQPublisher;
        //}


        public EventController(ILogger<EventController> logger, IUserService userService, ILookupService lookupService, IEventService eventService)
        {
            _logger = logger;
            _userService = userService;
            _lookupService = lookupService;
            _eventService = eventService;
        }

        [HttpPost("CreateEvent")]
        public async Task<IActionResult> CreateEvent(EventCreateDto eventCreateDto)
        {
            try
            {
                _logger.LogInformation("Starting creating an event");
                if(!ModelState.IsValid)
                {
                    return BadRequest(new ApiError { Message = ModelState.GetValidationErrorsAsCsv(), StatusCode = Constants.BAD_REQUEST_STATUS_CODE});
                }

                // Validating Organizer
                var organizaer = await _userService.FindUserById(eventCreateDto.OrganizerId);
                if (organizaer == null) return BadRequest(new ApiError { Message = "Organizer with given Id not found in database", StatusCode = Constants.BAD_REQUEST_STATUS_CODE });

                //Validating EventType
                var eventTypeLkp = await _lookupService.GetLookupById(eventCreateDto.EventTypeId);
                if (eventTypeLkp == null) return BadRequest(new ApiError { Message = "Event Type not found in database", StatusCode = Constants.BAD_REQUEST_STATUS_CODE });

                eventCreateDto.Organizer = organizaer;
                eventCreateDto.EventType = eventTypeLkp;

                await _eventService.AddEventAsync(eventCreateDto);
                
                _logger.LogInformation("Event Has Been Created");

                return Ok(new ApiResponse<object> {Message="Event Has been Created",StatusCode = Constants.OK_STATUS_CODE });
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(Constants.INTERNAL_SERVER_ERROR, "Internal Server Error" + ex.ToString());
            }
        }

        [HttpGet("GetEventById")]
        public async Task<IActionResult> CreateEvent(int eventId)
        {
            try
            {
                _logger.LogInformation("Getting an event");
               
                //Validating EventType
                var eventEntity = await _eventService.GetEventByIdAsync(eventId);
                if (eventEntity == null) return BadRequest(new ApiError { Message = "Event not found in database", StatusCode = Constants.BAD_REQUEST_STATUS_CODE });

                _logger.LogInformation("Event Has Been found");

                return Ok(new ApiResponse<Events> { Data = eventEntity, Message = "Event Has been Found", StatusCode = Constants.OK_STATUS_CODE });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(Constants.INTERNAL_SERVER_ERROR, "Internal Server Error" + ex.ToString());
            }
        }

        [HttpDelete("DeleteEvent/{eventId}")]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            try
            {
                _logger.LogInformation($"Deleting event with ID: {eventId}");

                // Attempt to find the event by its ID
                var eventEntity = await _eventService.GetEventByIdAsync(eventId);
                if (eventEntity == null)
                    return NotFound(new ApiError
                    {
                        Message = "Event not found in database",
                        StatusCode = Constants.NOT_FOUND
                    });

                // Delete the event
                await _eventService.DeleteEventAsync(eventId);

                _logger.LogInformation($"Event with ID: {eventId} has been deleted");

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

        //[HttpPost("testMessageBus")]
        //public async Task<IActionResult> TestMessageBus()
        //{
        //    try
        //    {
        //        await _rabbitMQPublisher.PublishMessage(Revent.Common.Constants.Constants.EMAIL_OTP_QUEUE, new[] { "Azan", "Usman" });
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
                
        //        return StatusCode(Constants.INTERNAL_SERVER_ERROR, "Internal Server Error: " + ex.ToString());
        //    }
        //}
    }
}
