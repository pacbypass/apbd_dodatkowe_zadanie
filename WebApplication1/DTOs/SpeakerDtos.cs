namespace WebApplication1.DTOs;

public record CreateSpeakerDto(
    string FirstName,
    string LastName,
    string Email,
    string? Bio
);

public record UpdateSpeakerDto(
    string FirstName,
    string LastName,
    string Email,
    string? Bio
);

public record SpeakerDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string? Bio,
    string FullName
);

public record AssignSpeakerToEventDto(
    int EventId,
    int SpeakerId
); 