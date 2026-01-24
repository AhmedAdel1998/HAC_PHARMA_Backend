using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HAC_Pharma.Application.DTOs;
using HAC_Pharma.Domain.Interfaces;

namespace HAC_Pharma.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    /// <summary>
    /// Get all events (public)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<EventListResponseDTO>> GetEvents([FromQuery] bool upcomingOnly = false)
    {
        var result = await _eventService.GetEventsAsync(upcomingOnly);
        return Ok(result);
    }

    /// <summary>
    /// Get a single event by ID (public)
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<EventDTO>> GetEvent(int id)
    {
        var evt = await _eventService.GetEventByIdAsync(id);
        if (evt == null)
            return NotFound(new { message = "Event not found" });

        return Ok(evt);
    }

    /// <summary>
    /// Create a new event (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EventDTO>> CreateEvent([FromBody] CreateEventDTO dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        var evt = await _eventService.CreateEventAsync(dto, userId);
        return CreatedAtAction(nameof(GetEvent), new { id = evt.Id }, evt);
    }

    /// <summary>
    /// Update an event (Admin only)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EventDTO>> UpdateEvent(int id, [FromBody] UpdateEventDTO dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        var evt = await _eventService.UpdateEventAsync(id, dto, userId);
        if (evt == null)
            return NotFound(new { message = "Event not found" });

        return Ok(evt);
    }

    /// <summary>
    /// Delete an event (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteEvent(int id)
    {
        var result = await _eventService.DeleteEventAsync(id);
        if (!result)
            return NotFound(new { message = "Event not found" });

        return NoContent();
    }

    /// <summary>
    /// Register for an event (public)
    /// </summary>
    [HttpPost("{id}/register")]
    public async Task<ActionResult<EventRegistrationResponseDTO>> Register(int id, [FromBody] EventRegistrationDTO dto)
    {
        try
        {
            var result = await _eventService.RegisterForEventAsync(id, dto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get event registrations (Admin only)
    /// </summary>
    [HttpGet("{id}/registrations")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<EventRegistrationResponseDTO>>> GetRegistrations(int id)
    {
        var registrations = await _eventService.GetEventRegistrationsAsync(id);
        return Ok(registrations);
    }
}
