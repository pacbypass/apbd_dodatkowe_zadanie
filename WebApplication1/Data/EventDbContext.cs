using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class EventDbContext : DbContext
{
    public EventDbContext(DbContextOptions<EventDbContext> options) : base(options) { }

    public DbSet<Event> Events { get; set; }
    public DbSet<Speaker> Speakers { get; set; }
    public DbSet<Participant> Participants { get; set; }
    public DbSet<EventSpeaker> EventSpeakers { get; set; }
    public DbSet<Participation> Participations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.EventDate).IsRequired();
            entity.Property(e => e.MaxParticipants).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<Speaker>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(s => s.LastName).IsRequired().HasMaxLength(100);
            entity.Property(s => s.Email).IsRequired().HasMaxLength(200);
            entity.Property(s => s.Bio).HasMaxLength(1000);
            entity.HasIndex(s => s.Email).IsUnique();
            entity.Ignore(s => s.FullName);
        });

        modelBuilder.Entity<Participant>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(p => p.LastName).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Email).IsRequired().HasMaxLength(200);
            entity.Property(p => p.PhoneNumber).HasMaxLength(20);
            entity.Property(p => p.CreatedAt).IsRequired();
            entity.HasIndex(p => p.Email).IsUnique();
            entity.Ignore(p => p.FullName);
        });

        modelBuilder.Entity<EventSpeaker>(entity =>
        {
            entity.HasKey(es => new { es.EventId, es.SpeakerId });
            entity.Property(es => es.AssignedAt).IsRequired();
            
            entity.HasOne(es => es.Event)
                .WithMany(e => e.EventSpeakers)
                .HasForeignKey(es => es.EventId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(es => es.Speaker)
                .WithMany(s => s.EventSpeakers)
                .HasForeignKey(es => es.SpeakerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Participation>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.RegistrationDate).IsRequired();
            entity.Property(p => p.IsCancelled).IsRequired();
            
            entity.HasOne(p => p.Participant)
                .WithMany(pt => pt.Participations)
                .HasForeignKey(p => p.ParticipantId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(p => p.Event)
                .WithMany(e => e.Participations)
                .HasForeignKey(p => p.EventId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasIndex(p => new { p.ParticipantId, p.EventId }).IsUnique();
        });

        base.OnModelCreating(modelBuilder);
    }
} 