using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly ILogger<EventsController> _logger;

    public EventsController(IEventService eventService, ILogger<EventsController> logger)
    {
        _eventService = eventService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventSummaryDto>>> GetUpcomingEvents()
    {
        try
        {
            var events = await _eventService.GetUpcomingEventsAsync();
            return Ok(events);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving upcoming events");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDto>> GetEvent(int id)
    {
        try
        {
            var eventDto = await _eventService.GetEventByIdAsync(id);
            if (eventDto == null)
                return NotFound($"Event with ID {id} not found");

            return Ok(eventDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving event with ID {EventId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<EventDto>> CreateEvent(CreateEventDto dto)
    {
        try
        {
            var eventDto = await _eventService.CreateEventAsync(dto);
            return CreatedAtAction(nameof(GetEvent), new { id = eventDto.Id }, eventDto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating event");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EventDto>> UpdateEvent(int id, UpdateEventDto dto)
    {
        try
        {
            var eventDto = await _eventService.UpdateEventAsync(id, dto);
            if (eventDto == null)
                return NotFound($"Event with ID {id} not found");

            return Ok(eventDto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating event with ID {EventId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        try
        {
            var result = await _eventService.DeleteEventAsync(id);
            if (!result)
                return NotFound($"Event with ID {id} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting event with ID {EventId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("{eventId}/speakers/{speakerId}")]
    public async Task<IActionResult> AssignSpeaker(int eventId, int speakerId)
    {
        try
        {
            var result = await _eventService.AssignSpeakerToEventAsync(eventId, speakerId);
            if (!result)
                return NotFound("Event or speaker not found");

            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning speaker {SpeakerId} to event {EventId}", speakerId, eventId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{eventId}/speakers/{speakerId}")]
    public async Task<IActionResult> RemoveSpeaker(int eventId, int speakerId)
    {
        try
        {
            var result = await _eventService.RemoveSpeakerFromEventAsync(eventId, speakerId);
            if (!result)
                return NotFound("Speaker assignment not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing speaker {SpeakerId} from event {EventId}", speakerId, eventId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("{eventId}/participants/{participantId}")]
    public async Task<IActionResult> RegisterParticipant(int eventId, int participantId)
    {
        try
        {
            var result = await _eventService.RegisterParticipantAsync(eventId, participantId);
            if (!result)
                return NotFound("Event or participant not found");

            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering participant {ParticipantId} for event {EventId}", participantId, eventId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{eventId}/participants/{participantId}")]
    public async Task<IActionResult> CancelParticipation(int eventId, int participantId)
    {
        try
        {
            var result = await _eventService.CancelParticipationAsync(eventId, participantId);
            if (!result)
                return NotFound("Participation not found");

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling participation for participant {ParticipantId} in event {EventId}", participantId, eventId);
            return StatusCode(500, "Internal server error");
        }
    }
} 