namespace WebApplication1.Models;

public class Speaker
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Bio { get; set; }
    
    public ICollection<EventSpeaker> EventSpeakers { get; set; } = new List<EventSpeaker>();
    
    public string FullName => $"{FirstName} {LastName}";
} 