namespace WebApplication1.DTOs;

public record CreateEventDto(
    string Title,
    string Description,
    DateTime EventDate,
    int MaxParticipants
);

public record UpdateEventDto(
    string Title,
    string Description,
    DateTime EventDate,
    int MaxParticipants
);

public record EventDto(
    int Id,
    string Title,
    string Description,
    DateTime EventDate,
    int MaxParticipants,
    int RegisteredParticipants,
    int AvailableSpots,
    List<string> SpeakerNames,
    DateTime CreatedAt
);

public record EventSummaryDto(
    int Id,
    string Title,
    DateTime EventDate,
    int RegisteredParticipants,
    int AvailableSpots,
    List<string> SpeakerNames
); 