using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HAC_Pharma.Application.DTOs;
using HAC_Pharma.Domain.Interfaces;

namespace HAC_Pharma.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContentController : ControllerBase
{
    private readonly IContentService _contentService;

    public ContentController(IContentService contentService)
    {
        _contentService = contentService;
    }

    /// <summary>
    /// Get content for a specific page in a language
    /// </summary>
    [HttpGet("{pageKey}/{lang}")]
    public async Task<ActionResult<PageContentDTO>> GetContent(string pageKey, string lang)
    {
        var content = await _contentService.GetContentAsync(pageKey, lang);
        if (content == null)
            return NotFound(new { message = $"Content for page '{pageKey}' in language '{lang}' not found" });

        return Ok(content);
    }

    /// <summary>
    /// Update content for a specific page in a language
    /// </summary>
    [HttpPut("{pageKey}/{lang}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<PageContentDTO>> UpdateContent(string pageKey, string lang, [FromBody] UpdateContentDTO dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var content = await _contentService.UpdateContentAsync(pageKey, lang, dto.Content, userId);
        return Ok(content);
    }

    /// <summary>
    /// List all page keys
    /// </summary>
    [HttpGet("pages")]
    public async Task<ActionResult<List<string>>> GetAllPageKeys()
    {
        var pages = await _contentService.GetAllPageKeysAsync();
        return Ok(pages);
    }

    /// <summary>
    /// Delete content for a specific page in a language
    /// </summary>
    [HttpDelete("{pageKey}/{lang}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteContent(string pageKey, string lang)
    {
        var result = await _contentService.DeleteContentAsync(pageKey, lang);
        if (!result)
            return NotFound(new { message = "Content not found" });

        return NoContent();
    }
}
