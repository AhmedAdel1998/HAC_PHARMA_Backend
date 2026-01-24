using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using HAC_Pharma.Application.DTOs;
using HAC_Pharma.Domain.Entities.CMS;
using HAC_Pharma.Domain.Interfaces;
using HAC_Pharma.Infrastructure.Data;

namespace HAC_Pharma.Application.Services;

public class JobService : IJobService
{
    private readonly ApplicationDbContext _context;
    private readonly IMediaService _mediaService;
    private readonly IWebHostEnvironment _environment;

    public JobService(ApplicationDbContext context, IMediaService mediaService, IWebHostEnvironment environment)
    {
        _context = context;
        _mediaService = mediaService;
        _environment = environment;
    }

    public async Task<JobListResponseDTO> GetJobsAsync(bool activeOnly = true)
    {
        var query = _context.Set<Job>()
            .Where(j => !j.IsDeleted);

        if (activeOnly)
        {
            query = query.Where(j => j.IsActive && (j.ExpiresAt == null || j.ExpiresAt > DateTime.UtcNow));
        }

        var jobs = await query
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync();

        return new JobListResponseDTO
        {
            Items = jobs.Select(MapToJobDTO).ToList(),
            Total = jobs.Count
        };
    }

    public async Task<JobDTO?> GetJobByIdAsync(int id)
    {
        var job = await _context.Set<Job>()
            .FirstOrDefaultAsync(j => j.Id == id && !j.IsDeleted);

        return job == null ? null : MapToJobDTO(job);
    }

    public async Task<JobDTO> CreateJobAsync(CreateJobDTO dto, string userId)
    {
        var job = new Job
        {
            Title = dto.Title,
            TitleAr = dto.TitleAr,
            Department = dto.Department,
            Location = dto.Location,
            EmploymentType = dto.Type,
            Description = dto.Description,
            DescriptionAr = dto.DescriptionAr,
            RequirementsJson = dto.Requirements != null ? JsonSerializer.Serialize(dto.Requirements) : null,
            BenefitsJson = dto.Benefits != null ? JsonSerializer.Serialize(dto.Benefits) : null,
            IsActive = dto.IsActive,
            ExpiresAt = dto.ExpiresAt,
            CreatedBy = userId
        };

        _context.Set<Job>().Add(job);
        await _context.SaveChangesAsync();

        return MapToJobDTO(job);
    }

    public async Task<JobDTO?> UpdateJobAsync(int id, UpdateJobDTO dto, string userId)
    {
        var job = await _context.Set<Job>()
            .FirstOrDefaultAsync(j => j.Id == id && !j.IsDeleted);

        if (job == null)
            return null;

        job.Title = dto.Title;
        job.TitleAr = dto.TitleAr;
        job.Department = dto.Department;
        job.Location = dto.Location;
        job.EmploymentType = dto.Type;
        job.Description = dto.Description;
        job.DescriptionAr = dto.DescriptionAr;
        job.RequirementsJson = dto.Requirements != null ? JsonSerializer.Serialize(dto.Requirements) : null;
        job.BenefitsJson = dto.Benefits != null ? JsonSerializer.Serialize(dto.Benefits) : null;
        job.IsActive = dto.IsActive;
        job.ExpiresAt = dto.ExpiresAt;
        job.UpdatedBy = userId;
        job.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToJobDTO(job);
    }

    public async Task<bool> DeleteJobAsync(int id)
    {
        var job = await _context.Set<Job>()
            .FirstOrDefaultAsync(j => j.Id == id && !j.IsDeleted);

        if (job == null)
            return false;

        job.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<ApplicationResponseDTO> SubmitApplicationAsync(int jobId, JobApplicationDTO dto, Stream? resumeStream, string? resumeFilename)
    {
        var job = await _context.Set<Job>()
            .FirstOrDefaultAsync(j => j.Id == jobId && !j.IsDeleted);

        if (job == null)
            throw new InvalidOperationException("Job not found");

        string? resumeUrl = null;
        if (resumeStream != null && !string.IsNullOrEmpty(resumeFilename))
        {
            // Save resume file
            var uploadsPath = Path.Combine(_environment.WebRootPath ?? _environment.ContentRootPath, "uploads", "resumes");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var extension = Path.GetExtension(resumeFilename);
            var uniqueFilename = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsPath, uniqueFilename);

            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                await resumeStream.CopyToAsync(fs);
            }

            resumeUrl = $"/uploads/resumes/{uniqueFilename}";
        }

        var application = new JobApplication
        {
            JobId = jobId,
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            CoverLetter = dto.CoverLetter,
            LinkedInUrl = dto.LinkedInUrl,
            ResumeUrl = resumeUrl,
            ResumeFilename = resumeFilename,
            Status = ApplicationStatus.Pending
        };

        _context.Set<JobApplication>().Add(application);
        await _context.SaveChangesAsync();

        return MapToApplicationResponseDTO(application, job.Title);
    }

    public async Task<ApplicationListResponseDTO> GetApplicationsAsync(int page, int limit, int? jobId, string? status)
    {
        var query = _context.Set<JobApplication>()
            .Include(a => a.Job)
            .Where(a => !a.IsDeleted);

        if (jobId.HasValue)
        {
            query = query.Where(a => a.JobId == jobId.Value);
        }

        if (!string.IsNullOrEmpty(status) && Enum.TryParse<ApplicationStatus>(status, true, out var appStatus))
        {
            query = query.Where(a => a.Status == appStatus);
        }

        var total = await query.CountAsync();

        var applications = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        return new ApplicationListResponseDTO
        {
            Items = applications.Select(a => MapToApplicationResponseDTO(a, a.Job.Title)).ToList(),
            Total = total,
            Page = page,
            Limit = limit
        };
    }

    public async Task<ApplicationResponseDTO?> GetApplicationByIdAsync(int id)
    {
        var application = await _context.Set<JobApplication>()
            .Include(a => a.Job)
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);

        return application == null ? null : MapToApplicationResponseDTO(application, application.Job.Title);
    }

    public async Task<ApplicationResponseDTO?> UpdateApplicationStatusAsync(int id, UpdateApplicationStatusDTO dto, string userId)
    {
        var application = await _context.Set<JobApplication>()
            .Include(a => a.Job)
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);

        if (application == null)
            return null;

        if (Enum.TryParse<ApplicationStatus>(dto.Status, true, out var status))
        {
            application.Status = status;
        }

        application.AdminNotes = dto.AdminNotes;
        application.ReviewedAt = DateTime.UtcNow;
        application.ReviewedByUserId = userId;
        application.UpdatedAt = DateTime.UtcNow;
        application.UpdatedBy = userId;

        await _context.SaveChangesAsync();

        return MapToApplicationResponseDTO(application, application.Job.Title);
    }

    private static JobDTO MapToJobDTO(Job job)
    {
        var requirements = new List<string>();
        var benefits = new List<string>();

        if (!string.IsNullOrEmpty(job.RequirementsJson))
        {
            try { requirements = JsonSerializer.Deserialize<List<string>>(job.RequirementsJson) ?? new List<string>(); }
            catch { }
        }

        if (!string.IsNullOrEmpty(job.BenefitsJson))
        {
            try { benefits = JsonSerializer.Deserialize<List<string>>(job.BenefitsJson) ?? new List<string>(); }
            catch { }
        }

        return new JobDTO
        {
            Id = job.Id,
            Title = job.Title,
            TitleAr = job.TitleAr,
            Department = job.Department,
            Location = job.Location,
            Type = job.EmploymentType,
            Description = job.Description,
            DescriptionAr = job.DescriptionAr,
            Requirements = requirements,
            Benefits = benefits,
            IsActive = job.IsActive,
            CreatedAt = job.CreatedAt,
            ExpiresAt = job.ExpiresAt
        };
    }

    private static ApplicationResponseDTO MapToApplicationResponseDTO(JobApplication app, string jobTitle)
    {
        return new ApplicationResponseDTO
        {
            Id = app.Id,
            JobId = app.JobId,
            JobTitle = jobTitle,
            Name = app.Name,
            Email = app.Email,
            Phone = app.Phone,
            CoverLetter = app.CoverLetter,
            ResumeUrl = app.ResumeUrl,
            LinkedInUrl = app.LinkedInUrl,
            Status = app.Status.ToString().ToLower(),
            AdminNotes = app.AdminNotes,
            CreatedAt = app.CreatedAt,
            ReviewedAt = app.ReviewedAt
        };
    }
}
