using System.ComponentModel.DataAnnotations;

namespace HAC_Pharma.Application.DTOs;

#region Auth DTOs

public class LoginRequestDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; } = false;
}

public class LoginResponseDTO
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public UserInfoDTO User { get; set; } = null!;
    public int ExpiresIn { get; set; } // seconds
}

public class RefreshTokenRequestDTO
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}

public class UserInfoDTO
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Role { get; set; } = "Viewer";
    public string? ProfileImage { get; set; }
    public DateTime? LastLoginAt { get; set; }
}

#endregion

#region Content DTOs

public class PageContentDTO
{
    public int Id { get; set; }
    public string PageKey { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public object? Content { get; set; } // Deserialized JSON
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}

public class UpdateContentDTO
{
    [Required]
    public object Content { get; set; } = null!; // JSON object
}

public class PageListDTO
{
    public List<string> Pages { get; set; } = new();
}

#endregion

#region Product DTOs

public class ProductDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? NameAr { get; set; }
    public int? CategoryId { get; set; }
    public string? Category { get; set; }
    public string? CategoryCode { get; set; }
    public string? Description { get; set; }
    public string? DescriptionAr { get; set; }
    public string? Dosage { get; set; }
    public string? DosageForm { get; set; }
    public string? Strength { get; set; }
    public string StockStatus { get; set; } = "available";
    public decimal? PriceSar { get; set; }
    public decimal? PriceUsd { get; set; }
    public string? Image { get; set; }
    public string? Manufacturer { get; set; }
    public bool RequiresPrescription { get; set; }
    public bool IsActive { get; set; }
}

public class ProductListResponseDTO
{
    public List<ProductDTO> Items { get; set; } = new();
    public int Total { get; set; }
    public int Page { get; set; }
    public int Limit { get; set; }
}

public class CreateProductDTO
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? NameAr { get; set; }

    public int CategoryId { get; set; }

    public string? Description { get; set; }
    public string? DescriptionAr { get; set; }

    [MaxLength(200)]
    public string? Dosage { get; set; }

    [MaxLength(100)]
    public string? DosageForm { get; set; }

    [MaxLength(100)]
    public string? Strength { get; set; }

    public string StockStatus { get; set; } = "available";

    [Range(0, double.MaxValue)]
    public decimal? PriceSar { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? PriceUsd { get; set; }

    public string? Image { get; set; }

    [MaxLength(200)]
    public string? Manufacturer { get; set; }

    public bool RequiresPrescription { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateProductDTO : CreateProductDTO
{
}

public class ProductCategoryDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public int ProductCount { get; set; }
}

public class CreateCategoryDTO
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Code { get; set; }

    public string? Description { get; set; }
}

public class DrugInteractionDTO
{
    public int DrugId { get; set; }
    public string DrugName { get; set; } = string.Empty;
    public string Severity { get; set; } = "minor"; // minor, moderate, major
    public string? Description { get; set; }
}

#endregion

#region Media DTOs

public class MediaDTO
{
    public int Id { get; set; }
    public string Filename { get; set; } = string.Empty;
    public string? OriginalFilename { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? MimeType { get; set; }
    public long Size { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public string? Alt { get; set; }
    public DateTime UploadedAt { get; set; }
    public string? UploadedBy { get; set; }
}

public class MediaListResponseDTO
{
    public List<MediaDTO> Items { get; set; } = new();
    public int Total { get; set; }
    public int Page { get; set; }
    public int Limit { get; set; }
}

public class MediaUploadResultDTO
{
    public int Id { get; set; }
    public string Filename { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public long Size { get; set; }
}

#endregion

#region User Management DTOs

public class UserDTO
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Role { get; set; } = "Viewer";
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}

public class UserListResponseDTO
{
    public List<UserDTO> Items { get; set; } = new();
    public int Total { get; set; }
}

public class CreateUserDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    [Required]
    public string Role { get; set; } = "Viewer"; // Admin, Editor, Viewer
}

public class UpdateUserDTO
{
    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    public string? Role { get; set; }
    public bool? IsActive { get; set; }
}

public class ChangePasswordDTO
{
    [Required]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string NewPassword { get; set; } = string.Empty;
}

#endregion

#region Contact DTOs

public class SubmitContactDTO
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Phone { get; set; }

    [MaxLength(100)]
    public string? Subject { get; set; }

    [Required]
    public string Message { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Company { get; set; }
}

public class ContactInquiryResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Subject { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Company { get; set; }
    public string Status { get; set; } = string.Empty; // New, Read, Responded, Archived
    public string? AdminNotes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? RespondedAt { get; set; }
    public string? RespondedBy { get; set; }
}

public class ContactListResponseDTO
{
    public List<ContactInquiryResponseDTO> Items { get; set; } = new();
    public int Total { get; set; }
    public int Page { get; set; }
    public int Limit { get; set; }
}

public class UpdateContactStatusDTO
{
    [Required]
    public string Status { get; set; } = string.Empty;

    public string? AdminNotes { get; set; }
}

#endregion

#region Job DTOs

public class JobDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? TitleAr { get; set; }
    public string? Department { get; set; }
    public string? Location { get; set; }
    public string Type { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DescriptionAr { get; set; }
    public List<string> Requirements { get; set; } = new();
    public List<string> Benefits { get; set; } = new();
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

public class JobListResponseDTO
{
    public List<JobDTO> Items { get; set; } = new();
    public int Total { get; set; }
}

public class CreateJobDTO
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? TitleAr { get; set; }

    [MaxLength(100)]
    public string? Department { get; set; }

    [MaxLength(100)]
    public string? Location { get; set; }

    [Required]
    public string Type { get; set; } = "Full-time";

    public string? Description { get; set; }
    public string? DescriptionAr { get; set; }
    public List<string>? Requirements { get; set; }
    public List<string>? Benefits { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? ExpiresAt { get; set; }
}

public class UpdateJobDTO : CreateJobDTO
{
}

public class JobApplicationDTO
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Phone { get; set; }

    public string? CoverLetter { get; set; }
    public string? LinkedInUrl { get; set; }
    // Resume is handled via file upload
}

public class ApplicationResponseDTO
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? CoverLetter { get; set; }
    public string? ResumeUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? AdminNotes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
}

public class ApplicationListResponseDTO
{
    public List<ApplicationResponseDTO> Items { get; set; } = new();
    public int Total { get; set; }
    public int Page { get; set; }
    public int Limit { get; set; }
}

public class UpdateApplicationStatusDTO
{
    [Required]
    public string Status { get; set; } = string.Empty;

    public string? AdminNotes { get; set; }
}

#endregion

#region Event DTOs

public class EventDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? TitleAr { get; set; }
    public string Type { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DescriptionAr { get; set; }
    public DateTime Date { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Location { get; set; }
    public string? VirtualUrl { get; set; }
    public string? RegistrationUrl { get; set; }
    public string? ImageUrl { get; set; }
    public int? MaxAttendees { get; set; }
    public int CurrentRegistrations { get; set; }
    public bool IsUpcoming { get; set; }
    public bool IsActive { get; set; }
}

public class EventListResponseDTO
{
    public List<EventDTO> Items { get; set; } = new();
    public int Total { get; set; }
}

public class CreateEventDTO
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? TitleAr { get; set; }

    [Required]
    public string Type { get; set; } = "webinar"; // webinar, conference, workshop, seminar

    public string? Description { get; set; }
    public string? DescriptionAr { get; set; }

    [Required]
    public DateTime Date { get; set; }

    public DateTime? EndDate { get; set; }

    [MaxLength(200)]
    public string? Location { get; set; }

    public string? VirtualUrl { get; set; }
    public string? RegistrationUrl { get; set; }
    public string? ImageUrl { get; set; }
    public int? MaxAttendees { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateEventDTO : CreateEventDTO
{
}

public class EventRegistrationDTO
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Phone { get; set; }

    [MaxLength(200)]
    public string? Company { get; set; }

    [MaxLength(100)]
    public string? JobTitle { get; set; }
}

public class EventRegistrationResponseDTO
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Company { get; set; }
    public DateTime RegisteredAt { get; set; }
    public bool Attended { get; set; }
}

#endregion

#region Analytics DTOs

public class AnalyticsOverviewDTO
{
    public VisitorStatsDTO Visitors { get; set; } = new();
    public PageViewStatsDTO PageViews { get; set; } = new();
    public List<TopPageDTO> TopPages { get; set; } = new();
    public ContactStatsDTO ContactStats { get; set; } = new();
    public int ApplicationCount { get; set; }
    public int TotalProducts { get; set; }
    public int ActiveJobs { get; set; }
    public int UpcomingEvents { get; set; }
}

public class VisitorStatsDTO
{
    public int Today { get; set; }
    public int Week { get; set; }
    public int Month { get; set; }
}

public class PageViewStatsDTO
{
    public int Today { get; set; }
    public int Week { get; set; }
    public int Month { get; set; }
}

public class TopPageDTO
{
    public string Page { get; set; } = string.Empty;
    public int Views { get; set; }
}

public class ContactStatsDTO
{
    public int New { get; set; }
    public int Total { get; set; }
}

#endregion

#region Settings DTOs

public class SiteSettingsDTO
{
    public string? SiteName { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public SocialLinksDTO SocialLinks { get; set; } = new();
    public string? WhatsappNumber { get; set; }
    public string? DefaultLanguage { get; set; }
    public bool MaintenanceMode { get; set; }
    public string? MaintenanceMessage { get; set; }
    public string? LogoUrl { get; set; }
    public string? FaviconUrl { get; set; }
    public string? Address { get; set; }
    public string? AddressAr { get; set; }
}

public class SocialLinksDTO
{
    public string? LinkedIn { get; set; }
    public string? Twitter { get; set; }
    public string? Facebook { get; set; }
    public string? Instagram { get; set; }
    public string? Youtube { get; set; }
}


public class UpdateSettingsDTO : SiteSettingsDTO
{
}

public class ActivityItemDTO
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}

#endregion

#region Common DTOs

public class ApiResponseDTO<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
}

public class PaginationQueryDTO
{
    public int Page { get; set; } = 1;
    public int Limit { get; set; } = 20;
    public string? Search { get; set; }
    public string? SortBy { get; set; }
    public bool Descending { get; set; } = false;
}

#endregion
