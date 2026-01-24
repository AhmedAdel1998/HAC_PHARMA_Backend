namespace HAC_Pharma.Domain.Entities.CMS;

/// <summary>
/// CMS page content with multilingual support
/// </summary>
public class Content : BaseEntity
{
    public string PageKey { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string Language { get; set; } = "en";
    public string? ContentJson { get; set; }
    public bool IsPublished { get; set; } = true;
    public string? UpdatedByUserId { get; set; }

    // Navigation
    public virtual ApplicationUser? UpdatedByUser { get; set; }
}

/// <summary>
/// Media library for images, videos, and documents
/// </summary>
public class Media : BaseEntity
{
    public string Filename { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? OriginalFilename { get; set; }
    public string Type { get; set; } = "image"; // image, video, document
    public string? MimeType { get; set; }
    public long Size { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public string? Alt { get; set; }
    public string? UploadedByUserId { get; set; }

    // Navigation
    public virtual ApplicationUser? UploadedByUser { get; set; }
}



/// <summary>
/// Career job postings
/// </summary>
public class Job : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? TitleAr { get; set; }
    public string? Department { get; set; }
    public string? Location { get; set; }
    public string EmploymentType { get; set; } = "Full-time"; // Full-time, Part-time, Contract
    public string? Description { get; set; }
    public string? DescriptionAr { get; set; }
    public string? RequirementsJson { get; set; } // JSON array of requirements
    public string? BenefitsJson { get; set; } // JSON array of benefits
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    public string? SalaryCurrency { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? ExpiresAt { get; set; }

    // Navigation
    public virtual ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
}

/// <summary>
/// Job applications from candidates
/// </summary>
public class JobApplication : BaseEntity
{
    public int JobId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? CoverLetter { get; set; }
    public string? ResumeUrl { get; set; }
    public string? ResumeFilename { get; set; }
    public string? LinkedInUrl { get; set; }
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
    public string? AdminNotes { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? ReviewedByUserId { get; set; }

    // Navigation
    public virtual Job Job { get; set; } = null!;
    public virtual ApplicationUser? ReviewedByUser { get; set; }
}

public enum ApplicationStatus
{
    Pending,
    Reviewing,
    Shortlisted,
    Interviewed,
    Offered,
    Hired,
    Rejected
}

/// <summary>
/// Events, webinars, and conferences
/// </summary>
public class Event : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? TitleAr { get; set; }
    public string EventType { get; set; } = "webinar"; // webinar, conference, workshop, seminar
    public string? Description { get; set; }
    public string? DescriptionAr { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Location { get; set; }
    public string? VirtualUrl { get; set; } // For webinars
    public string? RegistrationUrl { get; set; }
    public string? ImageUrl { get; set; }
    public int? MaxAttendees { get; set; }
    public bool IsActive { get; set; } = true;
    public bool RequiresRegistration { get; set; } = true;

    // Navigation
    public virtual ICollection<EventRegistration> Registrations { get; set; } = new List<EventRegistration>();

    // Computed property
    public bool IsUpcoming => StartDate > DateTime.UtcNow;
}

/// <summary>
/// Event registrations from attendees
/// </summary>
public class EventRegistration : BaseEntity
{
    public int EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Company { get; set; }
    public string? JobTitle { get; set; }
    public bool Attended { get; set; } = false;
    public DateTime? AttendedAt { get; set; }

    // Navigation
    public virtual Event Event { get; set; } = null!;
}

/// <summary>
/// Site settings as key-value pairs
/// </summary>
public class Setting : BaseEntity
{
    public string Key { get; set; } = string.Empty;
    public string? Value { get; set; }
    public string? Description { get; set; }
    public string ValueType { get; set; } = "string"; // string, number, boolean, json
    public bool IsPublic { get; set; } = false; // Whether accessible without auth
}

/// <summary>
/// Page view analytics (basic tracking)
/// </summary>
public class PageView : BaseEntity
{
    public string PagePath { get; set; } = string.Empty;
    public string? PageTitle { get; set; }
    public string? UserAgent { get; set; }
    public string? IpAddress { get; set; }
    public string? Referrer { get; set; }
    public string? SessionId { get; set; }
    public string? UserId { get; set; }
}

/// <summary>
/// Contact/inquiry submissions
/// </summary>
public class ContactInquiry : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Subject { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Company { get; set; }
    public InquiryStatus Status { get; set; } = InquiryStatus.New;
    public string? AdminNotes { get; set; }
    public DateTime? RespondedAt { get; set; }
    public string? RespondedByUserId { get; set; }

    // Navigation
    public virtual ApplicationUser? RespondedByUser { get; set; }
}

public enum InquiryStatus
{
    New,
    Read,
    Responded,
    Archived
}
