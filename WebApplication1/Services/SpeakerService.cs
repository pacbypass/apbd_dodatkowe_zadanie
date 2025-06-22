using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class SpeakerService : ISpeakerService
{
    private readonly EventDbContext _context;

    public SpeakerService(EventDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SpeakerDto>> GetAllSpeakersAsync()
    {
        var speakers = await _context.Speakers.ToListAsync();
        return speakers.Select(s => new SpeakerDto(
            s.Id,
            s.FirstName,
            s.LastName,
            s.Email,
            s.Bio,
            s.FullName
        ));
    }

    public async Task<SpeakerDto?> GetSpeakerByIdAsync(int id)
    {
        var speaker = await _context.Speakers.FindAsync(id);
        if (speaker == null) return null;

        return new SpeakerDto(
            speaker.Id,
            speaker.FirstName,
            speaker.LastName,
            speaker.Email,
            speaker.Bio,
            speaker.FullName
        );
    }

    public async Task<SpeakerDto> CreateSpeakerAsync(CreateSpeakerDto dto)
    {
        var speaker = new Speaker
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Bio = dto.Bio
        };

        _context.Speakers.Add(speaker);
        await _context.SaveChangesAsync();

        return new SpeakerDto(
            speaker.Id,
            speaker.FirstName,
            speaker.LastName,
            speaker.Email,
            speaker.Bio,
            speaker.FullName
        );
    }

    public async Task<SpeakerDto?> UpdateSpeakerAsync(int id, UpdateSpeakerDto dto)
    {
        var speaker = await _context.Speakers.FindAsync(id);
        if (speaker == null) return null;

        speaker.FirstName = dto.FirstName;
        speaker.LastName = dto.LastName;
        speaker.Email = dto.Email;
        speaker.Bio = dto.Bio;

        await _context.SaveChangesAsync();

        return new SpeakerDto(
            speaker.Id,
            speaker.FirstName,
            speaker.LastName,
            speaker.Email,
            speaker.Bio,
            speaker.FullName
        );
    }

    public async Task<bool> DeleteSpeakerAsync(int id)
    {
        var speaker = await _context.Speakers.FindAsync(id);
        if (speaker == null) return false;

        _context.Speakers.Remove(speaker);
        await _context.SaveChangesAsync();
        return true;
    }
} 