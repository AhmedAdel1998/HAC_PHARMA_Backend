using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using HAC_Pharma.Application.DTOs;
using HAC_Pharma.Domain.Entities.CMS;
using HAC_Pharma.Domain.Interfaces;
using HAC_Pharma.Infrastructure.Data;

namespace HAC_Pharma.Application.Services;

public class ContentService : IContentService
{
    private readonly ApplicationDbContext _context;

    public ContentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PageContentDTO?> GetContentAsync(string pageKey, string language)
    {
        var content = await _context.Set<Content>()
            .Include(c => c.UpdatedByUser)
            .FirstOrDefaultAsync(c => c.PageKey == pageKey && c.Language == language && !c.IsDeleted);

        if (content == null)
            return null;

        return new PageContentDTO
        {
            Id = content.Id,
            PageKey = content.PageKey,
            Language = content.Language,
            Content = string.IsNullOrEmpty(content.ContentJson) 
                ? null 
                : JsonSerializer.Deserialize<object>(content.ContentJson),
            UpdatedAt = content.UpdatedAt ?? content.CreatedAt,
            UpdatedBy = content.UpdatedByUser?.Email
        };
    }

    public async Task<PageContentDTO?> UpdateContentAsync(string pageKey, string language, object contentObj, string userId)
    {
        var content = await _context.Set<Content>()
            .FirstOrDefaultAsync(c => c.PageKey == pageKey && c.Language == language && !c.IsDeleted);

        var contentJson = JsonSerializer.Serialize(contentObj);

        if (content == null)
        {
            // Create new content
            content = new Content
            {
                PageKey = pageKey,
                Language = language,
                ContentJson = contentJson,
                UpdatedByUserId = userId,
                CreatedBy = userId
            };
            _context.Set<Content>().Add(content);
        }
        else
        {
            // Update existing
            content.ContentJson = contentJson;
            content.UpdatedByUserId = userId;
            content.UpdatedBy = userId;
            content.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        return await GetContentAsync(pageKey, language);
    }

    public async Task<List<string>> GetAllPageKeysAsync()
    {
        return await _context.Set<Content>()
            .Where(c => !c.IsDeleted)
            .Select(c => c.PageKey)
            .Distinct()
            .OrderBy(k => k)
            .ToListAsync();
    }

    public async Task<bool> DeleteContentAsync(string pageKey, string language)
    {
        var content = await _context.Set<Content>()
            .FirstOrDefaultAsync(c => c.PageKey == pageKey && c.Language == language && !c.IsDeleted);

        if (content == null)
            return false;

        content.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }
}
