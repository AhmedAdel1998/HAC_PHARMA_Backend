using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using HAC_Pharma.Domain.Entities.CMS;
using HAC_Pharma.Domain.Interfaces;
using HAC_Pharma.Infrastructure.Data;

namespace HAC_Pharma.Application.Services;

public class TranslationService : ITranslationService
{
    private readonly ApplicationDbContext _context;

    // Define all editable pages with their display names
    private static readonly List<PageInfoDTO> PageDefinitions = new()
    {
        new PageInfoDTO { Key = "common", Name = "Common Terms", NameAr = "المصطلحات العامة", Description = "Navigation, buttons, common labels" },
        new PageInfoDTO { Key = "header", Name = "Header", NameAr = "الرأس", Description = "Top navigation and header content" },
        new PageInfoDTO { Key = "hero", Name = "Hero Section", NameAr = "القسم الرئيسي", Description = "Main landing section" },
        new PageInfoDTO { Key = "stats", Name = "Statistics", NameAr = "الإحصائيات", Description = "Statistics section labels" },
        new PageInfoDTO { Key = "about", Name = "About Page", NameAr = "صفحة من نحن", Description = "About us content" },
        new PageInfoDTO { Key = "services", Name = "Services", NameAr = "الخدمات", Description = "Services section content" },
        new PageInfoDTO { Key = "foundation", Name = "Foundation", NameAr = "المؤسسة", Description = "Certifications and trust badges" },
        new PageInfoDTO { Key = "customers", Name = "Customers", NameAr = "العملاء", Description = "Customer section content" },
        new PageInfoDTO { Key = "roadmap", Name = "Roadmap", NameAr = "خارطة الطريق", Description = "Company roadmap section" },
        new PageInfoDTO { Key = "products", Name = "Products", NameAr = "المنتجات", Description = "Products page content" },
        new PageInfoDTO { Key = "contact", Name = "Contact", NameAr = "اتصل بنا", Description = "Contact page and form" },
        new PageInfoDTO { Key = "footer", Name = "Footer", NameAr = "التذييل", Description = "Footer content and links" },
        new PageInfoDTO { Key = "blog", Name = "Blog", NameAr = "المدونة", Description = "Blog page content" },
        new PageInfoDTO { Key = "testimonials", Name = "Testimonials", NameAr = "الشهادات", Description = "Customer testimonials" },
        new PageInfoDTO { Key = "caseStudies", Name = "Case Studies", NameAr = "دراسات الحالة", Description = "Case studies section" },
        new PageInfoDTO { Key = "downloads", Name = "Downloads", NameAr = "التحميلات", Description = "Resources and downloads" },
        new PageInfoDTO { Key = "portal", Name = "Client Portal", NameAr = "بوابة العميل", Description = "Client portal login" },
        new PageInfoDTO { Key = "chat", Name = "Chat Widget", NameAr = "نافذة الدردشة", Description = "Chat widget content" },
        new PageInfoDTO { Key = "scheduler", Name = "Scheduler", NameAr = "جدولة المواعيد", Description = "Meeting scheduler" },
        new PageInfoDTO { Key = "news", Name = "News", NameAr = "الأخبار", Description = "News articles and updates" },
        new PageInfoDTO { Key = "team", Name = "Team", NameAr = "الفريق", Description = "Team members and leadership" },
        new PageInfoDTO { Key = "partners", Name = "Partners", NameAr = "الشركاء", Description = "Strategic partners and clients" },
        new PageInfoDTO { Key = "careers", Name = "Careers", NameAr = "الوظائف", Description = "Job listings and careers page" },
        new PageInfoDTO { Key = "events", Name = "Events", NameAr = "الفعاليات", Description = "Upcoming events and webinars" },
        new PageInfoDTO { Key = "theme", Name = "Theme", NameAr = "المظهر", Description = "Theme toggle labels" },
        new PageInfoDTO { Key = "accessibility", Name = "Accessibility", NameAr = "إمكانية الوصول", Description = "Accessibility labels" }
    };

    public TranslationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Dictionary<string, object>> GetAllTranslationsAsync(string language)
    {
        var result = new Dictionary<string, object>();

        // Get all content for this language
        var contents = await _context.CmsContents
            .Where(c => c.Language == language && !c.IsDeleted)
            .ToListAsync();

        foreach (var content in contents)
        {
            if (!string.IsNullOrEmpty(content.ContentJson))
            {
                try
                {
                    var pageTranslations = JsonSerializer.Deserialize<Dictionary<string, object>>(content.ContentJson);
                    if (pageTranslations != null)
                    {
                        result[content.PageKey] = pageTranslations;
                    }
                }
                catch
                {
                    // Skip invalid JSON
                }
            }
        }

        return result;
    }

    public async Task<Dictionary<string, object>?> GetPageTranslationsAsync(string pageKey, string language)
    {
        var content = await _context.CmsContents
            .FirstOrDefaultAsync(c => c.PageKey == pageKey && c.Language == language && !c.IsDeleted);

        if (content == null || string.IsNullOrEmpty(content.ContentJson))
            return null;

        try
        {
            return JsonSerializer.Deserialize<Dictionary<string, object>>(content.ContentJson);
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> UpdatePageTranslationsAsync(string pageKey, string language, Dictionary<string, object> translations, string userId)
    {
        var content = await _context.CmsContents
            .FirstOrDefaultAsync(c => c.PageKey == pageKey && c.Language == language && !c.IsDeleted);

        var json = JsonSerializer.Serialize(translations);

        if (content == null)
        {
            content = new Content
            {
                PageKey = pageKey,
                Language = language,
                Title = pageKey,
                ContentJson = json,
                IsPublished = true,
                CreatedBy = userId
            };
            _context.CmsContents.Add(content);
        }
        else
        {
            content.ContentJson = json;
            content.UpdatedAt = DateTime.UtcNow;
            content.UpdatedBy = userId;
            
            // Only set foreign key if it's a valid GUID (skip for "System")
            if (Guid.TryParse(userId, out _))
            {
                content.UpdatedByUserId = userId;
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<PageInfoDTO>> GetAllPagesAsync()
    {
        // Get existing content to check which languages exist
        var contents = await _context.CmsContents
            .Where(c => !c.IsDeleted)
            .Select(c => new { c.PageKey, c.Language, c.UpdatedAt })
            .ToListAsync();

        var pages = PageDefinitions.Select(p => new PageInfoDTO
        {
            Key = p.Key,
            Name = p.Name,
            NameAr = p.NameAr,
            Description = p.Description,
            HasEnglish = contents.Any(c => c.PageKey == p.Key && c.Language == "en"),
            HasArabic = contents.Any(c => c.PageKey == p.Key && c.Language == "ar"),
            LastUpdated = contents
                .Where(c => c.PageKey == p.Key)
                .OrderByDescending(c => c.UpdatedAt)
                .Select(c => c.UpdatedAt)
                .FirstOrDefault()
        }).ToList();

        return pages;
    }

    public async Task SeedTranslationsAsync(string language, Dictionary<string, object> translations)
    {
        foreach (var kvp in translations)
        {
            var pageKey = kvp.Key;
            var pageContent = kvp.Value;

            // Check if already exists
            var exists = await _context.CmsContents
                .AnyAsync(c => c.PageKey == pageKey && c.Language == language && !c.IsDeleted);

            if (!exists)
            {
                var json = JsonSerializer.Serialize(pageContent);
                var content = new Content
                {
                    PageKey = pageKey,
                    Language = language,
                    Title = pageKey,
                    ContentJson = json,
                    IsPublished = true,
                    CreatedBy = "System"
                };
                _context.CmsContents.Add(content);
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasTranslationsAsync()
    {
        return await _context.CmsContents.AnyAsync(c => !c.IsDeleted);
    }
}
