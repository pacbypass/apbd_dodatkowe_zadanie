using WebApplication1.DTOs;

namespace WebApplication1.Services;

public interface IParticipantService
{
    Task<IEnumerable<ParticipantDto>> GetAllParticipantsAsync();
    Task<ParticipantDto?> GetParticipantByIdAsync(int id);
    Task<ParticipantDto> CreateParticipantAsync(CreateParticipantDto dto);
    Task<ParticipantDto?> UpdateParticipantAsync(int id, UpdateParticipantDto dto);
    Task<bool> DeleteParticipantAsync(int id);
    Task<ParticipantReportDto?> GetParticipantReportAsync(int participantId);
} 