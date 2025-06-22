using WebApplication1.DTOs;

namespace WebApplication1.Services;

public interface ISpeakerService
{
    Task<IEnumerable<SpeakerDto>> GetAllSpeakersAsync();
    Task<SpeakerDto?> GetSpeakerByIdAsync(int id);
    Task<SpeakerDto> CreateSpeakerAsync(CreateSpeakerDto dto);
    Task<SpeakerDto?> UpdateSpeakerAsync(int id, UpdateSpeakerDto dto);
    Task<bool> DeleteSpeakerAsync(int id);
} 