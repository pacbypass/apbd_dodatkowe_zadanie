namespace WebApplication1.Models;

public class Participation
{
    public int Id { get; set; }
    public int ParticipantId { get; set; }
    public int EventId { get; set; }
    public DateTime RegistrationDate { get; set; }
    public bool IsCancelled { get; set; }
    public DateTime? CancellationDate { get; set; }
    
    public Participant Participant { get; set; } = null!;
    public Event Event { get; set; } = null!;
} 