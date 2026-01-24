using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HAC_Pharma.Application.DTOs;
using HAC_Pharma.Domain.Interfaces;

namespace HAC_Pharma.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    /// <summary>
    /// Get analytics overview for dashboard
    /// </summary>
    [HttpGet("overview")]
    [Authorize(Roles = "Admin,Editor,Viewer")]
    public async Task<ActionResult<AnalyticsOverviewDTO>> GetOverview()
    {
        var overview = await _analyticsService.GetOverviewAsync();
        return Ok(overview);
    }

    /// <summary>
    /// Get recent activity timeline
    /// </summary>
    [HttpGet("activity")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<List<ActivityItemDTO>>> GetActivity()
    {
        var activity = await _analyticsService.GetRecentActivityAsync();
        return Ok(activity);
    }
}
