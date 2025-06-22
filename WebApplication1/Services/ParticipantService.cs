using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class ParticipantService : IParticipantService
{
    private readonly EventDbContext _context;

    public ParticipantService(EventDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ParticipantDto>> GetAllParticipantsAsync()
    {
        var participants = await _context.Participants.ToListAsync();
        return participants.Select(p => new ParticipantDto(
            p.Id,
            p.FirstName,
            p.LastName,
            p.Email,
            p.PhoneNumber,
            p.FullName,
            p.CreatedAt
        ));
    }

    public async Task<ParticipantDto?> GetParticipantByIdAsync(int id)
    {
        var participant = await _context.Participants.FindAsync(id);
        if (participant == null) return null;

        return new ParticipantDto(
            participant.Id,
            participant.FirstName,
            participant.LastName,
            participant.Email,
            participant.PhoneNumber,
            participant.FullName,
            participant.CreatedAt
        );
    }

    public async Task<ParticipantDto> CreateParticipantAsync(CreateParticipantDto dto)
    {
        var participant = new Participant
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            CreatedAt = DateTime.Now
        };

        _context.Participants.Add(participant);
        await _context.SaveChangesAsync();

        return new ParticipantDto(
            participant.Id,
            participant.FirstName,
            participant.LastName,
            participant.Email,
            participant.PhoneNumber,
            participant.FullName,
            participant.CreatedAt
        );
    }

    public async Task<ParticipantDto?> UpdateParticipantAsync(int id, UpdateParticipantDto dto)
    {
        var participant = await _context.Participants.FindAsync(id);
        if (participant == null) return null;

        participant.FirstName = dto.FirstName;
        participant.LastName = dto.LastName;
        participant.Email = dto.Email;
        participant.PhoneNumber = dto.PhoneNumber;

        await _context.SaveChangesAsync();

        return new ParticipantDto(
            participant.Id,
            participant.FirstName,
            participant.LastName,
            participant.Email,
            participant.PhoneNumber,
            participant.FullName,
            participant.CreatedAt
        );
    }

    public async Task<bool> DeleteParticipantAsync(int id)
    {
        var participant = await _context.Participants.FindAsync(id);
        if (participant == null) return false;

        _context.Participants.Remove(participant);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<ParticipantReportDto?> GetParticipantReportAsync(int participantId)
    {
        var participant = await _context.Participants
            .Include(p => p.Participations)
                .ThenInclude(pt => pt.Event)
                    .ThenInclude(e => e.EventSpeakers)
                        .ThenInclude(es => es.Speaker)
            .FirstOrDefaultAsync(p => p.Id == participantId);

        if (participant == null) return null;

        var participantEvents = participant.Participations
            .Select(pt => new ParticipantEventDto(
                pt.Event.Id,
                pt.Event.Title,
                pt.Event.EventDate,
                pt.Event.EventSpeakers.Select(es => es.Speaker.FullName).ToList(),
                pt.RegistrationDate,
                pt.IsCancelled,
                pt.CancellationDate
            ))
            .OrderBy(pe => pe.EventDate)
            .ToList();

        return new ParticipantReportDto(
            participant.Id,
            participant.FullName,
            participantEvents
        );
    }
}