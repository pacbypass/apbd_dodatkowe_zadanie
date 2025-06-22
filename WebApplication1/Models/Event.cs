namespace WebApplication1.Models;

public class Event
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public int MaxParticipants { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public ICollection<EventSpeaker> EventSpeakers { get; set; } = new List<EventSpeaker>();
    public ICollection<Participation> Participations { get; set; } = new List<Participation>();
} 