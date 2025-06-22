namespace WebApplication1.DTOs;

public record CreateParticipantDto(
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber
);

public record UpdateParticipantDto(
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber
);

public record ParticipantDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    string FullName,
    DateTime CreatedAt
);

public record RegisterParticipantDto(
    int EventId,
    int ParticipantId
);

public record ParticipantReportDto(
    int ParticipantId,
    string ParticipantName,
    List<ParticipantEventDto> Events
);

public record ParticipantEventDto(
    int EventId,
    string EventTitle,
    DateTime EventDate,
    List<string> SpeakerNames,
    DateTime RegistrationDate,
    bool IsCancelled,
    DateTime? CancellationDate
); 