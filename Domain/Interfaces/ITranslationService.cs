using HAC_Pharma.Application.DTOs;

namespace HAC_Pharma.Domain.Interfaces;

/// <summary>
/// Service for managing website translations
/// </summary>
public interface ITranslationService
{
    /// <summary>
    /// Get all translations for a language (returns full JSON object)
    /// </summary>
    Task<Dictionary<string, object>> GetAllTranslationsAsync(string language);
    
    /// <summary>
    /// Get translations for a specific page in a language
    /// </summary>
    Task<Dictionary<string, object>?> GetPageTranslationsAsync(string pageKey, string language);
    
    /// <summary>
    /// Update translations for a specific page
    /// </summary>
    Task<bool> UpdatePageTranslationsAsync(string pageKey, string language, Dictionary<string, object> translations, string userId);
    
    /// <summary>
    /// Get list of all editable page keys
    /// </summary>
    Task<List<PageInfoDTO>> GetAllPagesAsync();
    
    /// <summary>
    /// Seed translations from JSON files into the database
    /// </summary>
    Task SeedTranslationsAsync(string language, Dictionary<string, object> translations);
    
    /// <summary>
    /// Check if translations exist in database
    /// </summary>
    Task<bool> HasTranslationsAsync();
}

/// <summary>
/// DTO for page information
/// </summary>
public class PageInfoDTO
{
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool HasEnglish { get; set; }
    public bool HasArabic { get; set; }
    public DateTime? LastUpdated { get; set; }
}
