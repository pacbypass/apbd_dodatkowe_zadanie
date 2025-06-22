namespace WebApplication1.Models;

public class EventSpeaker
{
    public int EventId { get; set; }
    public int SpeakerId { get; set; }
    public DateTime AssignedAt { get; set; }
    
    public Event Event { get; set; } = null!;
    public Speaker Speaker { get; set; } = null!;
} 