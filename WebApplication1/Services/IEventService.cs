using WebApplication1.DTOs;

namespace WebApplication1.Services;

public interface IEventService
{
    Task<IEnumerable<EventSummaryDto>> GetUpcomingEventsAsync();
    Task<EventDto?> GetEventByIdAsync(int id);
    Task<EventDto> CreateEventAsync(CreateEventDto dto);
    Task<EventDto?> UpdateEventAsync(int id, UpdateEventDto dto);
    Task<bool> DeleteEventAsync(int id);
    Task<bool> AssignSpeakerToEventAsync(int eventId, int speakerId);
    Task<bool> RemoveSpeakerFromEventAsync(int eventId, int speakerId);
    Task<bool> RegisterParticipantAsync(int eventId, int participantId);
    Task<bool> CancelParticipationAsync(int eventId, int participantId);
} 