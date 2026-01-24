using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using HAC_Pharma.Application.DTOs;
using HAC_Pharma.Domain.Entities.CMS;
using HAC_Pharma.Domain.Interfaces;
using HAC_Pharma.Infrastructure.Data;

namespace HAC_Pharma.Application.Services;

public class SettingsService : ISettingsService
{
    private readonly ApplicationDbContext _context;

    // Keys for site settings
    private static class SettingKeys
    {
        public const string SiteName = "site_name";
        public const string ContactEmail = "contact_email";
        public const string ContactPhone = "contact_phone";
        public const string SocialLinks = "social_links";
        public const string WhatsappNumber = "whatsapp_number";
        public const string DefaultLanguage = "default_language";
        public const string MaintenanceMode = "maintenance_mode";
        public const string MaintenanceMessage = "maintenance_message";
        public const string LogoUrl = "logo_url";
        public const string FaviconUrl = "favicon_url";
        public const string Address = "address";
        public const string AddressAr = "address_ar";
    }

    public SettingsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SiteSettingsDTO> GetSettingsAsync()
    {
        var settings = await _context.Set<Setting>()
            .Where(s => !s.IsDeleted)
            .ToDictionaryAsync(s => s.Key, s => s.Value);

        var socialLinks = new SocialLinksDTO();
        if (settings.TryGetValue(SettingKeys.SocialLinks, out var socialJson) && !string.IsNullOrEmpty(socialJson))
        {
            try
            {
                socialLinks = JsonSerializer.Deserialize<SocialLinksDTO>(socialJson) ?? new SocialLinksDTO();
            }
            catch { }
        }

        return new SiteSettingsDTO
        {
            SiteName = settings.GetValueOrDefault(SettingKeys.SiteName),
            ContactEmail = settings.GetValueOrDefault(SettingKeys.ContactEmail),
            ContactPhone = settings.GetValueOrDefault(SettingKeys.ContactPhone),
            SocialLinks = socialLinks,
            WhatsappNumber = settings.GetValueOrDefault(SettingKeys.WhatsappNumber),
            DefaultLanguage = settings.GetValueOrDefault(SettingKeys.DefaultLanguage) ?? "en",
            MaintenanceMode = settings.GetValueOrDefault(SettingKeys.MaintenanceMode) == "true",
            MaintenanceMessage = settings.GetValueOrDefault(SettingKeys.MaintenanceMessage),
            LogoUrl = settings.GetValueOrDefault(SettingKeys.LogoUrl),
            FaviconUrl = settings.GetValueOrDefault(SettingKeys.FaviconUrl),
            Address = settings.GetValueOrDefault(SettingKeys.Address),
            AddressAr = settings.GetValueOrDefault(SettingKeys.AddressAr)
        };
    }

    public async Task<SiteSettingsDTO> UpdateSettingsAsync(UpdateSettingsDTO dto)
    {
        await SetSettingValueAsync(SettingKeys.SiteName, dto.SiteName ?? "");
        await SetSettingValueAsync(SettingKeys.ContactEmail, dto.ContactEmail ?? "");
        await SetSettingValueAsync(SettingKeys.ContactPhone, dto.ContactPhone ?? "");
        await SetSettingValueAsync(SettingKeys.SocialLinks, JsonSerializer.Serialize(dto.SocialLinks));
        await SetSettingValueAsync(SettingKeys.WhatsappNumber, dto.WhatsappNumber ?? "");
        await SetSettingValueAsync(SettingKeys.DefaultLanguage, dto.DefaultLanguage ?? "en");
        await SetSettingValueAsync(SettingKeys.MaintenanceMode, dto.MaintenanceMode.ToString().ToLower());
        await SetSettingValueAsync(SettingKeys.MaintenanceMessage, dto.MaintenanceMessage ?? "");
        await SetSettingValueAsync(SettingKeys.LogoUrl, dto.LogoUrl ?? "");
        await SetSettingValueAsync(SettingKeys.FaviconUrl, dto.FaviconUrl ?? "");
        await SetSettingValueAsync(SettingKeys.Address, dto.Address ?? "");
        await SetSettingValueAsync(SettingKeys.AddressAr, dto.AddressAr ?? "");

        return await GetSettingsAsync();
    }

    public async Task<string?> GetSettingValueAsync(string key)
    {
        var setting = await _context.Set<Setting>()
            .FirstOrDefaultAsync(s => s.Key == key && !s.IsDeleted);

        return setting?.Value;
    }

    public async Task SetSettingValueAsync(string key, string value)
    {
        var setting = await _context.Set<Setting>()
            .FirstOrDefaultAsync(s => s.Key == key && !s.IsDeleted);

        if (setting == null)
        {
            setting = new Setting
            {
                Key = key,
                Value = value
            };
            _context.Set<Setting>().Add(setting);
        }
        else
        {
            setting.Value = value;
            setting.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }
}
