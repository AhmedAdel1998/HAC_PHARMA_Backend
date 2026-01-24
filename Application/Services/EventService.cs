using Microsoft.EntityFrameworkCore;
using HAC_Pharma.Application.DTOs;
using HAC_Pharma.Domain.Entities.CMS;
using HAC_Pharma.Domain.Interfaces;
using HAC_Pharma.Infrastructure.Data;

namespace HAC_Pharma.Application.Services;

public class EventService : IEventService
{
    private readonly ApplicationDbContext _context;

    public EventService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EventListResponseDTO> GetEventsAsync(bool upcomingOnly = false)
    {
        var query = _context.Set<Event>()
            .Include(e => e.Registrations)
            .Where(e => !e.IsDeleted && e.IsActive);

        if (upcomingOnly)
        {
            query = query.Where(e => e.StartDate > DateTime.UtcNow);
        }

        var events = await query
            .OrderBy(e => e.StartDate)
            .ToListAsync();

        return new EventListResponseDTO
        {
            Items = events.Select(MapToEventDTO).ToList(),
            Total = events.Count
        };
    }

    public async Task<EventDTO?> GetEventByIdAsync(int id)
    {
        var evt = await _context.Set<Event>()
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

        return evt == null ? null : MapToEventDTO(evt);
    }

    public async Task<EventDTO> CreateEventAsync(CreateEventDTO dto, string userId)
    {
        var evt = new Event
        {
            Title = dto.Title,
            TitleAr = dto.TitleAr,
            EventType = dto.Type,
            Description = dto.Description,
            DescriptionAr = dto.DescriptionAr,
            StartDate = dto.Date,
            EndDate = dto.EndDate,
            Location = dto.Location,
            VirtualUrl = dto.VirtualUrl,
            RegistrationUrl = dto.RegistrationUrl,
            ImageUrl = dto.ImageUrl,
            MaxAttendees = dto.MaxAttendees,
            IsActive = dto.IsActive,
            CreatedBy = userId
        };

        _context.Set<Event>().Add(evt);
        await _context.SaveChangesAsync();

        return MapToEventDTO(evt);
    }

    public async Task<EventDTO?> UpdateEventAsync(int id, UpdateEventDTO dto, string userId)
    {
        var evt = await _context.Set<Event>()
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

        if (evt == null)
            return null;

        evt.Title = dto.Title;
        evt.TitleAr = dto.TitleAr;
        evt.EventType = dto.Type;
        evt.Description = dto.Description;
        evt.DescriptionAr = dto.DescriptionAr;
        evt.StartDate = dto.Date;
        evt.EndDate = dto.EndDate;
        evt.Location = dto.Location;
        evt.VirtualUrl = dto.VirtualUrl;
        evt.RegistrationUrl = dto.RegistrationUrl;
        evt.ImageUrl = dto.ImageUrl;
        evt.MaxAttendees = dto.MaxAttendees;
        evt.IsActive = dto.IsActive;
        evt.UpdatedBy = userId;
        evt.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToEventDTO(evt);
    }

    public async Task<bool> DeleteEventAsync(int id)
    {
        var evt = await _context.Set<Event>()
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

        if (evt == null)
            return false;

        evt.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<EventRegistrationResponseDTO> RegisterForEventAsync(int eventId, EventRegistrationDTO dto)
    {
        var evt = await _context.Set<Event>()
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(e => e.Id == eventId && !e.IsDeleted && e.IsActive);

        if (evt == null)
            throw new InvalidOperationException("Event not found");

        // Check capacity
        if (evt.MaxAttendees.HasValue && evt.Registrations.Count >= evt.MaxAttendees.Value)
            throw new InvalidOperationException("Event is at full capacity");

        // Check for duplicate registration
        var existingReg = await _context.Set<EventRegistration>()
            .FirstOrDefaultAsync(r => r.EventId == eventId && r.Email == dto.Email && !r.IsDeleted);

        if (existingReg != null)
            throw new InvalidOperationException("Already registered for this event");

        var registration = new EventRegistration
        {
            EventId = eventId,
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            Company = dto.Company,
            JobTitle = dto.JobTitle
        };

        _context.Set<EventRegistration>().Add(registration);
        await _context.SaveChangesAsync();

        return new EventRegistrationResponseDTO
        {
            Id = registration.Id,
            EventId = eventId,
            EventTitle = evt.Title,
            Name = registration.Name,
            Email = registration.Email,
            Company = registration.Company,
            RegisteredAt = registration.CreatedAt,
            Attended = registration.Attended
        };
    }

    public async Task<List<EventRegistrationResponseDTO>> GetEventRegistrationsAsync(int eventId)
    {
        var evt = await _context.Set<Event>()
            .FirstOrDefaultAsync(e => e.Id == eventId && !e.IsDeleted);

        if (evt == null)
            return new List<EventRegistrationResponseDTO>();

        var registrations = await _context.Set<EventRegistration>()
            .Where(r => r.EventId == eventId && !r.IsDeleted)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return registrations.Select(r => new EventRegistrationResponseDTO
        {
            Id = r.Id,
            EventId = eventId,
            EventTitle = evt.Title,
            Name = r.Name,
            Email = r.Email,
            Company = r.Company,
            RegisteredAt = r.CreatedAt,
            Attended = r.Attended
        }).ToList();
    }

    private static EventDTO MapToEventDTO(Event evt)
    {
        return new EventDTO
        {
            Id = evt.Id,
            Title = evt.Title,
            TitleAr = evt.TitleAr,
            Type = evt.EventType,
            Description = evt.Description,
            DescriptionAr = evt.DescriptionAr,
            Date = evt.StartDate,
            EndDate = evt.EndDate,
            Location = evt.Location,
            VirtualUrl = evt.VirtualUrl,
            RegistrationUrl = evt.RegistrationUrl,
            ImageUrl = evt.ImageUrl,
            MaxAttendees = evt.MaxAttendees,
            CurrentRegistrations = evt.Registrations?.Count ?? 0,
            IsUpcoming = evt.IsUpcoming,
            IsActive = evt.IsActive
        };
    }
}
