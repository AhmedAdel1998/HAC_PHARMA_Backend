using System.Text.Json;
using HAC_Pharma.Domain.Interfaces;

namespace HAC_Pharma.Infrastructure.Data;

/// <summary>
/// Seeds translation data from JSON files into the database
/// </summary>
public static class TranslationSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider, IWebHostEnvironment environment)
    {
        using var scope = serviceProvider.CreateScope();
        var translationService = scope.ServiceProvider.GetRequiredService<ITranslationService>();

        // Check if already seeded
        if (await translationService.HasTranslationsAsync())
        {
            Console.WriteLine("✓ Translations already exist in database");
            return;
        }

        Console.WriteLine("Seeding translations from JSON files...");

        // Try to load and seed English translations
        var enPath = Path.Combine(environment.ContentRootPath, "wwwroot", "i18n", "en.json");
        if (!File.Exists(enPath))
        {
            // Try alternate path
            enPath = Path.Combine(environment.ContentRootPath, "ClientApp", "src", "assets", "i18n", "en.json");
        }

        if (File.Exists(enPath))
        {
            await SeedFromFileAsync(translationService, enPath, "en");
        }
        else
        {
            Console.WriteLine($"⚠ English translations file not found. Will use default from API endpoint.");
            // Seed with empty/default structure
            await SeedDefaultTranslationsAsync(translationService, "en");
        }

        // Try to load and seed Arabic translations
        var arPath = Path.Combine(environment.ContentRootPath, "wwwroot", "i18n", "ar.json");
        if (!File.Exists(arPath))
        {
            arPath = Path.Combine(environment.ContentRootPath, "ClientApp", "src", "assets", "i18n", "ar.json");
        }

        if (File.Exists(arPath))
        {
            await SeedFromFileAsync(translationService, arPath, "ar");
        }
        else
        {
            Console.WriteLine($"⚠ Arabic translations file not found. Will use default from API endpoint.");
            await SeedDefaultTranslationsAsync(translationService, "ar");
        }

        Console.WriteLine("✓ Translation seeding complete");
    }

    private static async Task SeedFromFileAsync(ITranslationService service, string filePath, string language)
    {
        try
        {
            var json = await File.ReadAllTextAsync(filePath);
            var translations = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
            
            if (translations != null)
            {
                await service.SeedTranslationsAsync(language, translations);
                Console.WriteLine($"✓ Seeded {translations.Count} translation sections for '{language}'");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error seeding {language} translations: {ex.Message}");
        }
    }

    private static async Task SeedDefaultTranslationsAsync(ITranslationService service, string language)
    {
        // Create minimal default structure
        var defaults = new Dictionary<string, object>
        {
            ["common"] = new Dictionary<string, string>
            {
                ["home"] = language == "ar" ? "الرئيسية" : "Home",
                ["about"] = language == "ar" ? "من نحن" : "About",
                ["services"] = language == "ar" ? "الخدمات" : "Services",
                ["products"] = language == "ar" ? "المنتجات" : "Products",
                ["contact"] = language == "ar" ? "اتصل بنا" : "Contact"
            },
            ["hero"] = new Dictionary<string, string>
            {
                ["badge"] = language == "ar" ? "ابتكار الرعاية الصحية" : "Innovating Healthcare",
                ["subtitle"] = language == "ar" ? "ربط الابتكار الدوائي العالمي باحتياجات الرعاية الصحية المحلية" : "Connecting global pharmaceutical innovation with local healthcare needs"
            }
        };

        await service.SeedTranslationsAsync(language, defaults);
        Console.WriteLine($"✓ Seeded default translations for '{language}'");
    }
}
