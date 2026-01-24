using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HAC_Pharma.Application.DTOs;
using HAC_Pharma.Domain.Interfaces;

namespace HAC_Pharma.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    /// <summary>
    /// Submit a Contact Inquiry (public)
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ContactInquiryResponseDTO>> SubmitContact([FromBody] SubmitContactDTO dto)
    {
        var result = await _contactService.SubmitContactAsync(dto);
        return CreatedAtAction(nameof(GetContactById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Get all Contact Inquiries (Admin only)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ContactListResponseDTO>> GetContactList(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 20,
        [FromQuery] string? status = null,
        [FromQuery] string? search = null)
    {
        var result = await _contactService.GetContactListAsync(page, limit, status, search);
        return Ok(result);
    }

    /// <summary>
    /// Get a single Contact Inquiry by ID (Admin only)
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ContactInquiryResponseDTO>> GetContactById(int id)
    {
        var inquiry = await _contactService.GetContactByIdAsync(id);
        if (inquiry == null)
            return NotFound(new { message = "Inquiry not found" });

        return Ok(inquiry);
    }

    /// <summary>
    /// Update Contact Inquiry status (Admin only)
    /// </summary>
    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ContactInquiryResponseDTO>> UpdateContactStatus(int id, [FromBody] UpdateContactStatusDTO dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        var inquiry = await _contactService.UpdateContactStatusAsync(id, dto, userId);
        if (inquiry == null)
            return NotFound(new { message = "Inquiry not found" });

        return Ok(inquiry);
    }

    /// <summary>
    /// Delete Contact Inquiry (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        var result = await _contactService.DeleteContactAsync(id);
        if (!result)
            return NotFound(new { message = "Inquiry not found" });

        return NoContent();
    }
}
