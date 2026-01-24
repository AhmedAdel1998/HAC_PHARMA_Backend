using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HAC_Pharma.Application.DTOs;
using HAC_Pharma.Domain.Interfaces;

namespace HAC_Pharma.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobsController(IJobService jobService)
    {
        _jobService = jobService;
    }

    /// <summary>
    /// Get all active jobs (public)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<JobListResponseDTO>> GetJobs([FromQuery] bool activeOnly = true)
    {
        var result = await _jobService.GetJobsAsync(activeOnly);
        return Ok(result);
    }

    /// <summary>
    /// Get a single job by ID (public)
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<JobDTO>> GetJob(int id)
    {
        var job = await _jobService.GetJobByIdAsync(id);
        if (job == null)
            return NotFound(new { message = "Job not found" });

        return Ok(job);
    }

    /// <summary>
    /// Create a new job posting (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<JobDTO>> CreateJob([FromBody] CreateJobDTO dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        var job = await _jobService.CreateJobAsync(dto, userId);
        return CreatedAtAction(nameof(GetJob), new { id = job.Id }, job);
    }

    /// <summary>
    /// Update a job posting (Admin only)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<JobDTO>> UpdateJob(int id, [FromBody] UpdateJobDTO dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        var job = await _jobService.UpdateJobAsync(id, dto, userId);
        if (job == null)
            return NotFound(new { message = "Job not found" });

        return Ok(job);
    }

    /// <summary>
    /// Delete a job posting (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteJob(int id)
    {
        var result = await _jobService.DeleteJobAsync(id);
        if (!result)
            return NotFound(new { message = "Job not found" });

        return NoContent();
    }

    /// <summary>
    /// Apply for a job (public)
    /// </summary>
    [HttpPost("{id}/apply")]
    [RequestSizeLimit(10 * 1024 * 1024)] // 10MB for resume
    public async Task<ActionResult<ApplicationResponseDTO>> Apply(int id, [FromForm] JobApplicationDTO dto, IFormFile? resume)
    {
        try
        {
            Stream? resumeStream = null;
            string? resumeFilename = null;

            if (resume != null && resume.Length > 0)
            {
                resumeStream = resume.OpenReadStream();
                resumeFilename = resume.FileName;
            }

            var result = await _jobService.SubmitApplicationAsync(id, dto, resumeStream, resumeFilename);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ApplicationsController : ControllerBase
{
    private readonly IJobService _jobService;

    public ApplicationsController(IJobService jobService)
    {
        _jobService = jobService;
    }

    /// <summary>
    /// Get all job applications (Admin only)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApplicationListResponseDTO>> GetApplications(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 20,
        [FromQuery] int? jobId = null,
        [FromQuery] string? status = null)
    {
        var result = await _jobService.GetApplicationsAsync(page, limit, jobId, status);
        return Ok(result);
    }

    /// <summary>
    /// Get a single application by ID (Admin only)
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApplicationResponseDTO>> GetApplication(int id)
    {
        var application = await _jobService.GetApplicationByIdAsync(id);
        if (application == null)
            return NotFound(new { message = "Application not found" });

        return Ok(application);
    }

    /// <summary>
    /// Update application status (Admin only)
    /// </summary>
    [HttpPut("{id}/status")]
    public async Task<ActionResult<ApplicationResponseDTO>> UpdateApplicationStatus(int id, [FromBody] UpdateApplicationStatusDTO dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        var application = await _jobService.UpdateApplicationStatusAsync(id, dto, userId);
        if (application == null)
            return NotFound(new { message = "Application not found" });

        return Ok(application);
    }
}
