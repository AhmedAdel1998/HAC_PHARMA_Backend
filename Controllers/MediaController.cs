using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HAC_Pharma.Application.DTOs;
using HAC_Pharma.Domain.Interfaces;

namespace HAC_Pharma.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaController : ControllerBase
{
    private readonly IMediaService _mediaService;

    public MediaController(IMediaService mediaService)
    {
        _mediaService = mediaService;
    }

    /// <summary>
    /// Get media files with pagination
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<MediaListResponseDTO>> GetMedia(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 20,
        [FromQuery] string? type = null)
    {
        var result = await _mediaService.GetMediaAsync(page, limit, type);
        return Ok(result);
    }

    /// <summary>
    /// Get a single media item by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<MediaDTO>> GetMediaById(int id)
    {
        var media = await _mediaService.GetMediaByIdAsync(id);
        if (media == null)
            return NotFound(new { message = "Media not found" });

        return Ok(media);
    }

    /// <summary>
    /// Upload a file
    /// </summary>
    [HttpPost("upload")]
    [Authorize]
    [RequestSizeLimit(50 * 1024 * 1024)] // 50MB limit
    public async Task<ActionResult<MediaUploadResultDTO>> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "No file provided" });

        var userId = User.FindFirst("id")?.Value ?? "";

        using var stream = file.OpenReadStream();
        var result = await _mediaService.UploadAsync(stream, file.FileName, file.ContentType, userId);

        return CreatedAtAction(nameof(GetMediaById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Upload multiple files
    /// </summary>
    [HttpPost("upload-multiple")]
    [Authorize]
    [RequestSizeLimit(100 * 1024 * 1024)] // 100MB limit for multiple
    public async Task<ActionResult<List<MediaUploadResultDTO>>> UploadMultiple(List<IFormFile> files)
    {
        if (files == null || files.Count == 0)
            return BadRequest(new { message = "No files provided" });

        var userId = User.FindFirst("id")?.Value ?? "";
        var results = new List<MediaUploadResultDTO>();

        foreach (var file in files)
        {
            using var stream = file.OpenReadStream();
            var result = await _mediaService.UploadAsync(stream, file.FileName, file.ContentType, userId);
            results.Add(result);
        }

        return Ok(results);
    }

    /// <summary>
    /// Delete a media file
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _mediaService.DeleteAsync(id);
        if (!result)
            return NotFound(new { message = "Media not found" });

        return NoContent();
    }
}
