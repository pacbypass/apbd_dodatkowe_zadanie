using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(EventDbContext context)
    {
        if (await context.Events.AnyAsync() || 
            await context.Speakers.AnyAsync() || 
            await context.Participants.AnyAsync())
        {
            return;
        }

        var speakers = new List<Speaker>
        {
            new Speaker
            {
                FirstName = "Anna",
                LastName = "Nowak",
                Email = "anna.nowak@pjwstk.edu.pl",
                Bio = "Specjalista ds. cyberbezpieczeństwa"
            },
            new Speaker
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jan.kowalski@pjwstk.edu.pl",
                Bio = "Specjalista ds. .NET"
            }
        };

        context.Speakers.AddRange(speakers);
        await context.SaveChangesAsync();

        var participants = new List<Participant>
        {
            new Participant
            {
                FirstName = "Dominik",
                LastName = "Chyliński",
                Email = "dominik.chylinski@pjwstk.edu.pl",
                PhoneNumber = "+48123456789",
                CreatedAt = DateTime.Now
            },
            new Participant
            {
                FirstName = "Kacper",
                LastName = "Gajdamowicz",
                Email = "kacper.gajdamowicz@pjwstk.edu.pl",
                PhoneNumber = "+48987654321",
                CreatedAt = DateTime.Now
            }
        };

        context.Participants.AddRange(participants);
        await context.SaveChangesAsync();

        var baseDate = DateTime.Now.AddDays(30);
        var events = new List<Event>
        {
            new Event
            {
                Title = "Konferencja .NET 2024",
                Description = "Coroczna konferencja dotycząca najnowszych technologii .NET i najlepszych praktyk",
                EventDate = baseDate.AddDays(15),
                MaxParticipants = 5,
                CreatedAt = DateTime.Now
            },
            new Event
            {
                Title = "Warsztat Architektury Chmurowej",
                Description = "Praktyczny warsztat projektowania skalowalnych rozwiązań chmurowych",
                EventDate = baseDate.AddDays(20),
                MaxParticipants = 3,
                CreatedAt = DateTime.Now
            }
        };

        context.Events.AddRange(events);
        await context.SaveChangesAsync();

        var eventSpeakers = new List<EventSpeaker>
        {
            new EventSpeaker
            {
                EventId = events[0].Id,
                SpeakerId = speakers[0].Id,
                AssignedAt = DateTime.Now
            },
            new EventSpeaker
            {
                EventId = events[0].Id,
                SpeakerId = speakers[1].Id,
                AssignedAt = DateTime.Now
            },
            new EventSpeaker
            {
                EventId = events[1].Id,
                SpeakerId = speakers[1].Id,
                AssignedAt = DateTime.Now
            }
        };

        context.EventSpeakers.AddRange(eventSpeakers);
        await context.SaveChangesAsync();

        var participations = new List<Participation>
        {
            new Participation
            {
                ParticipantId = participants[0].Id,
                EventId = events[0].Id,
                RegistrationDate = DateTime.Now,
                IsCancelled = false
            },
            new Participation
            {
                ParticipantId = participants[1].Id,
                EventId = events[0].Id,
                RegistrationDate = DateTime.Now,
                IsCancelled = false
            },
            new Participation
            {
                ParticipantId = participants[0].Id,
                EventId = events[1].Id,
                RegistrationDate = DateTime.Now,
                IsCancelled = false
            }
        };

        context.Participations.AddRange(participations);
        await context.SaveChangesAsync();

        Console.WriteLine("Database seeded successfully with sample data.");
    }
} 