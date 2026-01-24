using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HAC_Pharma.Application.DTOs;
using HAC_Pharma.Domain.Interfaces;

namespace HAC_Pharma.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SettingsController : ControllerBase
{
    private readonly ISettingsService _settingsService;

    public SettingsController(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    /// <summary>
    /// Get site settings (Admin only)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SiteSettingsDTO>> GetSettings()
    {
        var settings = await _settingsService.GetSettingsAsync();
        return Ok(settings);
    }

    /// <summary>
    /// Update site settings (Admin only)
    /// </summary>
    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SiteSettingsDTO>> UpdateSettings([FromBody] UpdateSettingsDTO dto)
    {
        var settings = await _settingsService.UpdateSettingsAsync(dto);
        return Ok(settings);
    }

    /// <summary>
    /// Get public settings (for frontend initialization)
    /// </summary>
    [HttpGet("public")]
    [AllowAnonymous]
    public async Task<ActionResult<object>> GetPublicSettings()
    {
        var settings = await _settingsService.GetSettingsAsync();
        
        // Return only public-facing settings
        return Ok(new
        {
            siteName = settings.SiteName,
            contactEmail = settings.ContactEmail,
            contactPhone = settings.ContactPhone,
            whatsappNumber = settings.WhatsappNumber,
            socialLinks = settings.SocialLinks,
            defaultLanguage = settings.DefaultLanguage,
            maintenanceMode = settings.MaintenanceMode,
            maintenanceMessage = settings.MaintenanceMessage,
            logoUrl = settings.LogoUrl,
            faviconUrl = settings.FaviconUrl,
            address = settings.Address,
            addressAr = settings.AddressAr
        });
    }
}
