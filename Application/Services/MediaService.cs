using Microsoft.EntityFrameworkCore;
using HAC_Pharma.Application.DTOs;
using HAC_Pharma.Domain.Entities.CMS;
using HAC_Pharma.Domain.Interfaces;
using HAC_Pharma.Infrastructure.Data;

namespace HAC_Pharma.Application.Services;

public class MediaService : IMediaService
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly string _uploadsPath;

    public MediaService(ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
        _uploadsPath = Path.Combine(_environment.WebRootPath ?? _environment.ContentRootPath, "uploads");
        
        // Ensure uploads directory exists
        if (!Directory.Exists(_uploadsPath))
            Directory.CreateDirectory(_uploadsPath);
    }

    public async Task<MediaListResponseDTO> GetMediaAsync(int page, int limit, string? type)
    {
        var query = _context.Set<Media>()
            .Where(m => !m.IsDeleted);

        if (!string.IsNullOrEmpty(type))
        {
            query = query.Where(m => m.Type == type);
        }

        var total = await query.CountAsync();

        var items = await query
            .OrderByDescending(m => m.CreatedAt)
            .Skip((page - 1) * limit)
            .Take(limit)
            .Include(m => m.UploadedByUser)
            .Select(m => new MediaDTO
            {
                Id = m.Id,
                Filename = m.Filename,
                OriginalFilename = m.OriginalFilename,
                Url = m.Url,
                Type = m.Type,
                MimeType = m.MimeType,
                Size = m.Size,
                Width = m.Width,
                Height = m.Height,
                Alt = m.Alt,
                UploadedAt = m.CreatedAt,
                UploadedBy = m.UploadedByUser != null ? m.UploadedByUser.Email : null
            })
            .ToListAsync();

        return new MediaListResponseDTO
        {
            Items = items,
            Total = total,
            Page = page,
            Limit = limit
        };
    }

    public async Task<MediaDTO?> GetMediaByIdAsync(int id)
    {
        var media = await _context.Set<Media>()
            .Include(m => m.UploadedByUser)
            .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

        if (media == null)
            return null;

        return new MediaDTO
        {
            Id = media.Id,
            Filename = media.Filename,
            OriginalFilename = media.OriginalFilename,
            Url = media.Url,
            Type = media.Type,
            MimeType = media.MimeType,
            Size = media.Size,
            Width = media.Width,
            Height = media.Height,
            Alt = media.Alt,
            UploadedAt = media.CreatedAt,
            UploadedBy = media.UploadedByUser?.Email
        };
    }

    public async Task<MediaUploadResultDTO> UploadAsync(Stream fileStream, string filename, string contentType, string userId)
    {
        // Generate unique filename
        var extension = Path.GetExtension(filename);
        var uniqueFilename = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(_uploadsPath, uniqueFilename);

        // Save file
        using (var fs = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.CopyToAsync(fs);
        }

        var fileInfo = new FileInfo(filePath);

        // Determine media type
        var mediaType = GetMediaType(contentType);

        var media = new Media
        {
            Filename = uniqueFilename,
            OriginalFilename = filename,
            Url = $"/uploads/{uniqueFilename}",
            Type = mediaType,
            MimeType = contentType,
            Size = fileInfo.Length,
            UploadedByUserId = userId,
            CreatedBy = userId
        };

        _context.Set<Media>().Add(media);
        await _context.SaveChangesAsync();

        return new MediaUploadResultDTO
        {
            Id = media.Id,
            Filename = media.Filename,
            Url = media.Url,
            Type = media.Type,
            Size = media.Size
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var media = await _context.Set<Media>()
            .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

        if (media == null)
            return false;

        // Delete physical file
        var filePath = Path.Combine(_uploadsPath, media.Filename);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        // Soft delete record
        media.IsDeleted = true;
        await _context.SaveChangesAsync();

        return true;
    }

    public string GetMediaUrl(string filename)
    {
        return $"/uploads/{filename}";
    }

    private static string GetMediaType(string contentType)
    {
        if (contentType.StartsWith("image/"))
            return "image";
        if (contentType.StartsWith("video/"))
            return "video";
        if (contentType.StartsWith("audio/"))
            return "audio";
        if (contentType == "application/pdf")
            return "document";
        if (contentType.Contains("document") || contentType.Contains("spreadsheet") || contentType.Contains("presentation"))
            return "document";
        return "other";
    }
}
