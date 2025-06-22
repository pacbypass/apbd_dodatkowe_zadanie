namespace WebApplication1.Models;

public class Participant
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public ICollection<Participation> Participations { get; set; } = new List<Participation>();
    
    public string FullName => $"{FirstName} {LastName}";
} 