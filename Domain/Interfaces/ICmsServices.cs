using HAC_Pharma.Application.DTOs;
using HAC_Pharma.Domain.Entities.CMS;

namespace HAC_Pharma.Domain.Interfaces;

#region Auth Service

public interface IAuthService
{
    Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO request);
    Task<LoginResponseDTO?> RefreshTokenAsync(string refreshToken);
    Task<bool> LogoutAsync(string userId);
    Task<UserInfoDTO?> GetCurrentUserAsync(string userId);
    Task<bool> ValidateTokenAsync(string token);
}

#endregion

#region Content Service

public interface IContentService
{
    Task<PageContentDTO?> GetContentAsync(string pageKey, string language);
    Task<PageContentDTO?> UpdateContentAsync(string pageKey, string language, object content, string userId);
    Task<List<string>> GetAllPageKeysAsync();
    Task<bool> DeleteContentAsync(string pageKey, string language);
}

#endregion

#region Product Service

public interface IProductService
{
    Task<ProductListResponseDTO> GetProductsAsync(int page, int limit, string? category, string? search, bool includeInactive = false);
    Task<ProductDTO?> GetProductByIdAsync(int id);
    Task<ProductDTO> CreateProductAsync(CreateProductDTO dto, string userId);
    Task<ProductDTO?> UpdateProductAsync(int id, UpdateProductDTO dto, string userId);
    Task<bool> DeleteProductAsync(int id);
    Task<List<ProductCategoryDTO>> GetCategoriesAsync();
    Task<ProductCategoryDTO> CreateCategoryAsync(CreateCategoryDTO dto, string userId);
    Task<bool> DeleteCategoryAsync(int id);
    Task<List<DrugInteractionDTO>> GetInteractionsAsync(int productId);
}

#endregion

#region Media Service

public interface IMediaService
{
    Task<MediaListResponseDTO> GetMediaAsync(int page, int limit, string? type);
    Task<MediaDTO?> GetMediaByIdAsync(int id);
    Task<MediaUploadResultDTO> UploadAsync(Stream fileStream, string filename, string contentType, string userId);
    Task<bool> DeleteAsync(int id);
    string GetMediaUrl(string filename);
}

#endregion

#region User Service

public interface IUserService
{
    Task<UserListResponseDTO> GetUsersAsync(int page, int limit, string? search);
    Task<UserDTO?> GetUserByIdAsync(string id);
    Task<UserDTO> CreateUserAsync(CreateUserDTO dto);
    Task<UserDTO?> UpdateUserAsync(string id, UpdateUserDTO dto);
    Task<bool> DeleteUserAsync(string id);
    Task<bool> ChangePasswordAsync(string userId, ChangePasswordDTO dto);
}

#endregion

#region Contact Service

public interface IContactService
{
    Task<ContactInquiryResponseDTO> SubmitContactAsync(SubmitContactDTO dto);
    Task<ContactListResponseDTO> GetContactListAsync(int page, int limit, string? status, string? search);
    Task<ContactInquiryResponseDTO?> GetContactByIdAsync(int id);
    Task<ContactInquiryResponseDTO?> UpdateContactStatusAsync(int id, UpdateContactStatusDTO dto, string userId);
    Task<bool> DeleteContactAsync(int id);
}

#endregion

#region Job Service

public interface IJobService
{
    Task<JobListResponseDTO> GetJobsAsync(bool activeOnly = true);
    Task<JobDTO?> GetJobByIdAsync(int id);
    Task<JobDTO> CreateJobAsync(CreateJobDTO dto, string userId);
    Task<JobDTO?> UpdateJobAsync(int id, UpdateJobDTO dto, string userId);
    Task<bool> DeleteJobAsync(int id);
    
    // Applications
    Task<ApplicationResponseDTO> SubmitApplicationAsync(int jobId, JobApplicationDTO dto, Stream? resumeStream, string? resumeFilename);
    Task<ApplicationListResponseDTO> GetApplicationsAsync(int page, int limit, int? jobId, string? status);
    Task<ApplicationResponseDTO?> GetApplicationByIdAsync(int id);
    Task<ApplicationResponseDTO?> UpdateApplicationStatusAsync(int id, UpdateApplicationStatusDTO dto, string userId);
}

#endregion

#region Event Service

public interface IEventService
{
    Task<EventListResponseDTO> GetEventsAsync(bool upcomingOnly = false);
    Task<EventDTO?> GetEventByIdAsync(int id);
    Task<EventDTO> CreateEventAsync(CreateEventDTO dto, string userId);
    Task<EventDTO?> UpdateEventAsync(int id, UpdateEventDTO dto, string userId);
    Task<bool> DeleteEventAsync(int id);
    Task<EventRegistrationResponseDTO> RegisterForEventAsync(int eventId, EventRegistrationDTO dto);
    Task<List<EventRegistrationResponseDTO>> GetEventRegistrationsAsync(int eventId);
}

#endregion

#region Analytics Service

public interface IAnalyticsService
{
    Task<AnalyticsOverviewDTO> GetOverviewAsync();
    Task<List<ActivityItemDTO>> GetRecentActivityAsync();
    Task TrackPageViewAsync(string pagePath, string? pageTitle, string? userAgent, string? ipAddress, string? referrer, string? sessionId, string? userId);
}

#endregion

#region Settings Service

public interface ISettingsService
{
    Task<SiteSettingsDTO> GetSettingsAsync();
    Task<SiteSettingsDTO> UpdateSettingsAsync(UpdateSettingsDTO dto);
    Task<string?> GetSettingValueAsync(string key);
    Task SetSettingValueAsync(string key, string value);
}

#endregion
