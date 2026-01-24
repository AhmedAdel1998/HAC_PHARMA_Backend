using Microsoft.EntityFrameworkCore;
using HAC_Pharma.Application.DTOs;
using HAC_Pharma.Domain.Entities.CMS;
using HAC_Pharma.Domain.Interfaces;
using HAC_Pharma.Infrastructure.Data;

namespace HAC_Pharma.Application.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly ApplicationDbContext _context;

    public AnalyticsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AnalyticsOverviewDTO> GetOverviewAsync()
    {
        var today = DateTime.UtcNow.Date;
        var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
        var startOfMonth = new DateTime(today.Year, today.Month, 1);

        // 1. Visitor Stats (Unique IPs)
        var visitorsToday = await _context.PageViews
            .Where(p => p.CreatedAt >= today)
            .Select(p => p.IpAddress)
            .Distinct()
            .CountAsync();

        var visitorsWeek = await _context.PageViews
            .Where(p => p.CreatedAt >= startOfWeek)
            .Select(p => p.IpAddress)
            .Distinct()
            .CountAsync();

        var visitorsMonth = await _context.PageViews
            .Where(p => p.CreatedAt >= startOfMonth)
            .Select(p => p.IpAddress)
            .Distinct()
            .CountAsync();

        // 2. Page Views
        var viewsToday = await _context.PageViews.CountAsync(p => p.CreatedAt >= today);
        var viewsWeek = await _context.PageViews.CountAsync(p => p.CreatedAt >= startOfWeek);
        var viewsMonth = await _context.PageViews.CountAsync(p => p.CreatedAt >= startOfMonth);

        // 3. Top Pages (Last 30 days)
        var topPages = await _context.PageViews
            .Where(p => p.CreatedAt >= today.AddDays(-30) && !p.PagePath.Contains("/admin") && !p.PagePath.Contains("/api"))
            .GroupBy(p => p.PagePath)
            .Select(g => new TopPageDTO 
            { 
                Page = g.Key, 
                Views = g.Count() 
            })
            .OrderByDescending(x => x.Views)
            .Take(5)
            .ToListAsync();

        // 4. Counts
        var contactNew = await _context.ContactInquiries.CountAsync(r => r.Status == InquiryStatus.New);
        var contactTotal = await _context.ContactInquiries.CountAsync();
        var appsPending = await _context.JobApplications.CountAsync(a => a.Status == ApplicationStatus.Pending);
        
        // Extra counts for dashboard
        var totalProducts = await _context.Products.CountAsync(p => p.IsActive);
        var activeJobs = await _context.Jobs.CountAsync(j => j.IsActive);
        var upcomingEvents = await _context.CmsEvents.CountAsync(e => e.StartDate > DateTime.UtcNow);

        return new AnalyticsOverviewDTO
        {
            Visitors = new VisitorStatsDTO 
            { 
                Today = visitorsToday, 
                Week = visitorsWeek, 
                Month = visitorsMonth 
            },
            PageViews = new PageViewStatsDTO 
            { 
                Today = viewsToday, 
                Week = viewsWeek, 
                Month = viewsMonth 
            },
            TopPages = topPages,
            ContactStats = new ContactStatsDTO 
            { 
                New = contactNew, 
                Total = contactTotal 
            },
            ApplicationCount = appsPending,
            TotalProducts = totalProducts,
            ActiveJobs = activeJobs,
            UpcomingEvents = upcomingEvents
        };
    }

    public async Task<List<ActivityItemDTO>> GetRecentActivityAsync()
    {
        var activities = new List<ActivityItemDTO>();
        
        // 1. Content Updates (Pages)
        var recentContent = await _context.CmsContents
            .Where(c => c.UpdatedAt != null || c.CreatedAt > DateTime.UtcNow.AddDays(-30))
            .OrderByDescending(c => c.UpdatedAt ?? c.CreatedAt)
            .Take(5)
            .Select(c => new ActivityItemDTO
            {
                Id = c.Id,
                Type = "page_update",
                Title = "Page Updated",
                Description = c.PageKey,
                User = c.UpdatedByUserId ?? c.CreatedBy ?? "System",
                Timestamp = c.UpdatedAt ?? c.CreatedAt
            })
            .ToListAsync();
        activities.AddRange(recentContent);

        // 2. New Products
        var recentProducts = await _context.Products
            .OrderByDescending(p => p.CreatedAt)
            .Take(5)
            .Select(p => new ActivityItemDTO
            {
                Id = p.Id,
                Type = "product_add",
                Title = "Product Added",
                Description = p.Name,
                User = p.CreatedBy ?? "System",
                Timestamp = p.CreatedAt
            })
            .ToListAsync();
        activities.AddRange(recentProducts);

        // 3. New Contact Messages
        var recentContacts = await _context.ContactInquiries
            .OrderByDescending(r => r.CreatedAt)
            .Take(5)
            .Select(r => new ActivityItemDTO
            {
                Id = r.Id,
                Type = "contact_received",
                Title = "New Message",
                Description = r.Subject ?? "No Subject",
                User = r.Name, 
                Timestamp = r.CreatedAt
            })
            .ToListAsync();
        activities.AddRange(recentContacts);
        
        // 4. Job Applications
        var recentApps = await _context.JobApplications
            .OrderByDescending(a => a.CreatedAt)
            .Take(5)
            .Select(a => new ActivityItemDTO
            {
                Id = a.Id,
                Type = "job_application_received", // Changed type to match conceptual event
                Title = "Job Application",
                Description = a.Name,
                User = "Candidate",
                Timestamp = a.CreatedAt
            })
            .ToListAsync();
        activities.AddRange(recentApps);

        return activities.OrderByDescending(a => a.Timestamp).Take(10).ToList();
    }

    public async Task TrackPageViewAsync(string pagePath, string? pageTitle, string? userAgent, string? ipAddress, string? referrer, string? sessionId, string? userId)
    {
        // Don't track API calls or static assets
        if (pagePath.StartsWith("/api", StringComparison.OrdinalIgnoreCase) || 
            pagePath.Contains(".") || 
            pagePath.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase))
            return;

        try
        {
            // Truncate fields to prevent DB errors
            var safePath = pagePath?.Length > 2000 ? pagePath.Substring(0, 2000) : pagePath;
            var safeTitle = pageTitle?.Length > 500 ? pageTitle.Substring(0, 500) : pageTitle;
            var safeUserAgent = userAgent?.Length > 500 ? userAgent.Substring(0, 500) : userAgent;
            var safeReferrer = referrer?.Length > 2000 ? referrer.Substring(0, 2000) : referrer;

            var pageView = new PageView
            {
                PagePath = safePath ?? "",
                PageTitle = safeTitle,
                UserAgent = safeUserAgent,
                IpAddress = ipAddress,
                Referrer = safeReferrer, 
                SessionId = sessionId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.PageViews.Add(pageView);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Log specific error to help debugging
            Console.WriteLine($"[Analytics Error] {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"[Analytics Inner Error] {ex.InnerException.Message}");
            }
        }
    }
}
