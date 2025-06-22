using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpeakersController : ControllerBase
{
    private readonly ISpeakerService _speakerService;
    private readonly ILogger<SpeakersController> _logger;

    public SpeakersController(ISpeakerService speakerService, ILogger<SpeakersController> logger)
    {
        _speakerService = speakerService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSpeakers()
    {
        try
        {
            var speakers = await _speakerService.GetAllSpeakersAsync();
            return Ok(speakers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving speakers");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSpeaker(int id)
    {
        try
        {
            var speaker = await _speakerService.GetSpeakerByIdAsync(id);
            if (speaker == null)
                return NotFound($"Speaker with ID {id} not found");

            return Ok(speaker);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving speaker with ID {SpeakerId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateSpeaker(CreateSpeakerDto dto)
    {
        try
        {
            var speaker = await _speakerService.CreateSpeakerAsync(dto);
            return CreatedAtAction(nameof(GetSpeaker), new { id = speaker.Id }, speaker);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating speaker");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSpeaker(int id, UpdateSpeakerDto dto)
    {
        try
        {
            var speaker = await _speakerService.UpdateSpeakerAsync(id, dto);
            if (speaker == null)
                return NotFound($"Speaker with ID {id} not found");

            return Ok(speaker);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating speaker with ID {SpeakerId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSpeaker(int id)
    {
        try
        {
            var result = await _speakerService.DeleteSpeakerAsync(id);
            if (!result)
                return NotFound($"Speaker with ID {id} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting speaker with ID {SpeakerId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
} 