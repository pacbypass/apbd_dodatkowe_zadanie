using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParticipantsController : ControllerBase
{
    private readonly IParticipantService _participantService;
    private readonly ILogger<ParticipantsController> _logger;

    public ParticipantsController(IParticipantService participantService, ILogger<ParticipantsController> logger)
    {
        _participantService = participantService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParticipantDto>>> GetAllParticipants()
    {
        try
        {
            var participants = await _participantService.GetAllParticipantsAsync();
            return Ok(participants);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving participants");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ParticipantDto>> GetParticipant(int id)
    {
        try
        {
            var participant = await _participantService.GetParticipantByIdAsync(id);
            if (participant == null)
                return NotFound($"Participant with ID {id} not found");

            return Ok(participant);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving participant with ID {ParticipantId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<ParticipantDto>> CreateParticipant(CreateParticipantDto dto)
    {
        try
        {
            var participant = await _participantService.CreateParticipantAsync(dto);
            return CreatedAtAction(nameof(GetParticipant), new { id = participant.Id }, participant);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating participant");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ParticipantDto>> UpdateParticipant(int id, UpdateParticipantDto dto)
    {
        try
        {
            var participant = await _participantService.UpdateParticipantAsync(id, dto);
            if (participant == null)
                return NotFound($"Participant with ID {id} not found");

            return Ok(participant);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating participant with ID {ParticipantId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteParticipant(int id)
    {
        try
        {
            var result = await _participantService.DeleteParticipantAsync(id);
            if (!result)
                return NotFound($"Participant with ID {id} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting participant with ID {ParticipantId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}/report")]
    public async Task<ActionResult<ParticipantReportDto>> GetParticipantReport(int id)
    {
        try
        {
            var report = await _participantService.GetParticipantReportAsync(id);
            if (report == null)
                return NotFound($"Participant with ID {id} not found");

            return Ok(report);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating report for participant with ID {ParticipantId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
} 