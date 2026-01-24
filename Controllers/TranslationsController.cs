using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HAC_Pharma.Domain.Interfaces;

namespace HAC_Pharma.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TranslationsController : ControllerBase
{
    private readonly ITranslationService _translationService;

    public TranslationsController(ITranslationService translationService)
    {
        _translationService = translationService;
    }

    /// <summary>
    /// Get all translations for a language (for frontend use)
    /// </summary>
    [HttpGet("{lang}")]
    [AllowAnonymous]
    public async Task<ActionResult<Dictionary<string, object>>> GetAllTranslations(string lang)
    {
        var translations = await _translationService.GetAllTranslationsAsync(lang);
        return Ok(translations);
    }

    /// <summary>
    /// Get translations for a specific page
    /// </summary>
    [HttpGet("{pageKey}/{lang}")]
    [AllowAnonymous]
    public async Task<ActionResult<Dictionary<string, object>>> GetPageTranslations(string pageKey, string lang)
    {
        var translations = await _translationService.GetPageTranslationsAsync(pageKey, lang);
        if (translations == null)
            return NotFound(new { message = $"Translations for '{pageKey}' in '{lang}' not found" });

        return Ok(translations);
    }

    /// <summary>
    /// Update translations for a specific page (Admin only)
    /// </summary>
    [HttpPut("{pageKey}/{lang}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult> UpdatePageTranslations(
        string pageKey, 
        string lang, 
        [FromBody] Dictionary<string, object> translations)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        var result = await _translationService.UpdatePageTranslationsAsync(pageKey, lang, translations, userId);
        
        if (!result)
            return BadRequest(new { message = "Failed to update translations" });

        return Ok(new { message = "Translations updated successfully" });
    }

    /// <summary>
    /// Get list of all editable pages
    /// </summary>
    [HttpGet("pages")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<List<PageInfoDTO>>> GetAllPages()
    {
        var pages = await _translationService.GetAllPagesAsync();
        return Ok(pages);
    }

    /// <summary>
    /// Seed translations from request body (Admin only, for initial setup)
    /// </summary>
    [HttpPost("seed/{lang}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> SeedTranslations(string lang, [FromBody] Dictionary<string, object> translations)
    {
        await _translationService.SeedTranslationsAsync(lang, translations);
        return Ok(new { message = $"Translations seeded for language '{lang}'" });
    }

    /// <summary>
    /// Check if translations exist in database
    /// </summary>
    [HttpGet("status")]
    [AllowAnonymous]
    public async Task<ActionResult<object>> GetStatus()
    {
        var hasTranslations = await _translationService.HasTranslationsAsync();
        return Ok(new { 
            initialized = hasTranslations,
            message = hasTranslations ? "Translations are available" : "Translations not initialized"
        });
    }
}
