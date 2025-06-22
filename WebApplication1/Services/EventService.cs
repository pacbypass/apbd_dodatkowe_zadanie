using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class EventService : IEventService
{
    private readonly EventDbContext _context;

    public EventService(EventDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EventSummaryDto>> GetUpcomingEventsAsync()
    {
        var upcomingEvents = await _context.Events
            .Where(e => e.EventDate > DateTime.Now)
            .Include(e => e.EventSpeakers)
                .ThenInclude(es => es.Speaker)
            .Include(e => e.Participations.Where(p => !p.IsCancelled))
            .ToListAsync();

        return upcomingEvents.Select(e => new EventSummaryDto(
            e.Id,
            e.Title,
            e.EventDate,
            e.Participations.Count(p => !p.IsCancelled),
            e.MaxParticipants - e.Participations.Count(p => !p.IsCancelled),
            e.EventSpeakers.Select(es => es.Speaker.FullName).ToList()
        ));
    }

    public async Task<EventDto?> GetEventByIdAsync(int id)
    {
        var eventEntity = await _context.Events
            .Include(e => e.EventSpeakers)
                .ThenInclude(es => es.Speaker)
            .Include(e => e.Participations.Where(p => !p.IsCancelled))
            .FirstOrDefaultAsync(e => e.Id == id);

        if (eventEntity == null) return null;

        var registeredCount = eventEntity.Participations.Count(p => !p.IsCancelled);

        return new EventDto(
            eventEntity.Id,
            eventEntity.Title,
            eventEntity.Description,
            eventEntity.EventDate,
            eventEntity.MaxParticipants,
            registeredCount,
            eventEntity.MaxParticipants - registeredCount,
            eventEntity.EventSpeakers.Select(es => es.Speaker.FullName).ToList(),
            eventEntity.CreatedAt
        );
    }

    public async Task<EventDto> CreateEventAsync(CreateEventDto dto)
    {
        if (dto.EventDate <= DateTime.Now)
            throw new ArgumentException("Event date cannot be in the past");

        var eventEntity = new Event
        {
            Title = dto.Title,
            Description = dto.Description,
            EventDate = dto.EventDate,
            MaxParticipants = dto.MaxParticipants,
            CreatedAt = DateTime.Now
        };

        _context.Events.Add(eventEntity);
        await _context.SaveChangesAsync();

        return new EventDto(
            eventEntity.Id,
            eventEntity.Title,
            eventEntity.Description,
            eventEntity.EventDate,
            eventEntity.MaxParticipants,
            0,
            eventEntity.MaxParticipants,
            new List<string>(),
            eventEntity.CreatedAt
        );
    }

    public async Task<EventDto?> UpdateEventAsync(int id, UpdateEventDto dto)
    {
        var eventEntity = await _context.Events.FindAsync(id);
        if (eventEntity == null) return null;

        if (dto.EventDate <= DateTime.Now)
            throw new ArgumentException("Event date cannot be in the past");

        eventEntity.Title = dto.Title;
        eventEntity.Description = dto.Description;
        eventEntity.EventDate = dto.EventDate;
        eventEntity.MaxParticipants = dto.MaxParticipants;

        await _context.SaveChangesAsync();
        return await GetEventByIdAsync(id);
    }

    public async Task<bool> DeleteEventAsync(int id)
    {
        var eventEntity = await _context.Events.FindAsync(id);
        if (eventEntity == null) return false;

        _context.Events.Remove(eventEntity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AssignSpeakerToEventAsync(int eventId, int speakerId)
    {
        var eventEntity = await _context.Events.FindAsync(eventId);
        var speaker = await _context.Speakers.FindAsync(speakerId);

        if (eventEntity == null || speaker == null) return false;

        var existingAssignment = await _context.EventSpeakers
            .FirstOrDefaultAsync(es => es.EventId == eventId && es.SpeakerId == speakerId);

        if (existingAssignment != null) return false;

        var conflictingEvent = await _context.EventSpeakers
            .Include(es => es.Event)
            .AnyAsync(es => es.SpeakerId == speakerId && 
                           es.Event.EventDate == eventEntity.EventDate);

        if (conflictingEvent)
            throw new InvalidOperationException("Speaker is already assigned to another event at the same time");

        var eventSpeaker = new EventSpeaker
        {
            EventId = eventId,
            SpeakerId = speakerId,
            AssignedAt = DateTime.Now
        };

        _context.EventSpeakers.Add(eventSpeaker);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveSpeakerFromEventAsync(int eventId, int speakerId)
    {
        var eventSpeaker = await _context.EventSpeakers
            .FirstOrDefaultAsync(es => es.EventId == eventId && es.SpeakerId == speakerId);

        if (eventSpeaker == null) return false;

        _context.EventSpeakers.Remove(eventSpeaker);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RegisterParticipantAsync(int eventId, int participantId)
    {
        var eventEntity = await _context.Events
            .Include(e => e.Participations.Where(p => !p.IsCancelled))
            .FirstOrDefaultAsync(e => e.Id == eventId);
            
        var participant = await _context.Participants.FindAsync(participantId);

        if (eventEntity == null || participant == null) return false;

        if (eventEntity.Participations.Count >= eventEntity.MaxParticipants)
            throw new InvalidOperationException("Event is full");

        var existingParticipation = await _context.Participations
            .FirstOrDefaultAsync(p => p.EventId == eventId && 
                                    p.ParticipantId == participantId && 
                                    !p.IsCancelled);

        if (existingParticipation != null)
            throw new InvalidOperationException("Participant is already registered for this event");

        var participation = new Participation
        {
            EventId = eventId,
            ParticipantId = participantId,
            RegistrationDate = DateTime.Now,
            IsCancelled = false
        };

        _context.Participations.Add(participation);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CancelParticipationAsync(int eventId, int participantId)
    {
        var participation = await _context.Participations
            .Include(p => p.Event)
            .FirstOrDefaultAsync(p => p.EventId == eventId && 
                                    p.ParticipantId == participantId && 
                                    !p.IsCancelled);

        if (participation == null) return false;

        var timeDifference = participation.Event.EventDate - DateTime.Now;
        if (timeDifference.TotalHours < 24)
            throw new InvalidOperationException("Cannot cancel participation within 24 hours of event start");

        participation.IsCancelled = true;
        participation.CancellationDate = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }
} 