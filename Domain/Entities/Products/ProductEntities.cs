namespace HAC_Pharma.Domain.Entities.Products;

/// <summary>
/// Core product entity
/// </summary>
public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? GenericName { get; set; }
    public string? BrandName { get; set; }
    public string? SKU { get; set; }
    public string? Barcode { get; set; }
    public string? Description { get; set; }
    public string? Manufacturer { get; set; }
    public string? CountryOfOrigin { get; set; }
    public decimal? UnitPrice { get; set; }
    public string? Currency { get; set; }
    public string? DosageForm { get; set; }
    public string? Strength { get; set; }
    public string? PackSize { get; set; }
    public int? ShelfLifeMonths { get; set; }
    public bool RequiresPrescription { get; set; }
    public bool IsControlled { get; set; }
    public bool IsActive { get; set; } = true;
    public string? ImageUrl { get; set; }

    // Foreign keys
    public int ProductCategoryId { get; set; }
    public int? ProductTypeId { get; set; }
    public int? TherapeuticCategoryId { get; set; }
    public int? TemperatureRequirementId { get; set; }

    // Navigation properties
    public virtual ProductCategory ProductCategory { get; set; } = null!;
    public virtual ProductType? ProductType { get; set; }
    public virtual TherapeuticCategory? TherapeuticCategory { get; set; }
    public virtual TemperatureRequirement? TemperatureRequirement { get; set; }
    public virtual ICollection<StabilityProfile> StabilityProfiles { get; set; } = new List<StabilityProfile>();
    public virtual ICollection<ProductDocument> Documents { get; set; } = new List<ProductDocument>();
}

/// <summary>
/// Product categories (Pharmaceutical, Medical Device, etc.)
/// </summary>
public class ProductCategory : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public ProductCategoryType CategoryType { get; set; }
    public int? ParentCategoryId { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ProductCategory? ParentCategory { get; set; }
    public virtual ICollection<ProductCategory> SubCategories { get; set; } = new List<ProductCategory>();
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

public enum ProductCategoryType
{
    Pharmaceutical,
    MedicalDevice,
    MedicalEquipment,
    MedicalSolution,
    FoodSupplement,
    Cosmeceutical
}

/// <summary>
/// Product types (Generic, Innovator)
/// </summary>
public class ProductType : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public ProductTypeCategory Type { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

public enum ProductTypeCategory
{
    Generic,
    Innovator,
    Biosimilar,
    OTC,
    Branded
}

/// <summary>
/// Therapeutic categories
/// </summary>
public class TherapeuticCategory : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? ATCCode { get; set; } // Anatomical Therapeutic Chemical
    public string? Description { get; set; }
    public int? ParentCategoryId { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual TherapeuticCategory? ParentCategory { get; set; }
    public virtual ICollection<TherapeuticCategory> SubCategories { get; set; } = new List<TherapeuticCategory>();
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

/// <summary>
/// Product stability profiles
/// </summary>
public class StabilityProfile : BaseEntity
{
    public int ProductId { get; set; }
    public string StudyNumber { get; set; } = string.Empty;
    public StabilityStudyType StudyType { get; set; }
    public string? StorageCondition { get; set; }
    public decimal? Temperature { get; set; }
    public decimal? Humidity { get; set; }
    public int? DurationMonths { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public StabilityStatus Status { get; set; }
    public string? Conclusion { get; set; }
    public string? ReportPath { get; set; }

    // Navigation properties
    public virtual Product Product { get; set; } = null!;
    public virtual ICollection<StabilityTestResult> TestResults { get; set; } = new List<StabilityTestResult>();
}

public enum StabilityStudyType
{
    LongTerm,
    Accelerated,
    Intermediate,
    PhotoStability,
    InUse
}

public enum StabilityStatus
{
    Planned,
    Ongoing,
    Completed,
    Cancelled
}

/// <summary>
/// Stability test results
/// </summary>
public class StabilityTestResult : BaseEntity
{
    public int StabilityProfileId { get; set; }
    public int TimePointMonths { get; set; }
    public DateTime TestDate { get; set; }
    public string? TestParameter { get; set; }
    public string? Result { get; set; }
    public string? Specification { get; set; }
    public bool PassedSpecification { get; set; }
    public string? TestedBy { get; set; }

    public virtual StabilityProfile StabilityProfile { get; set; } = null!;
}

/// <summary>
/// Temperature requirements for products
/// </summary>
public class TemperatureRequirement : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public TemperatureZone Zone { get; set; }
    public decimal? MinTemperature { get; set; }
    public decimal? MaxTemperature { get; set; }
    public string? Description { get; set; }
    public string? HandlingInstructions { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

public enum TemperatureZone
{
    Ambient,      // 15-25°C
    Chilled,      // 2-8°C
    Frozen,       // -20°C or below
    DeepFrozen,   // -70°C or below
    Controlled    // Specific range
}

/// <summary>
/// Product documentation
/// </summary>
public class ProductDocument : BaseEntity
{
    public int ProductId { get; set; }
    public string DocumentName { get; set; } = string.Empty;
    public ProductDocumentType DocumentType { get; set; }
    public string? Version { get; set; }
    public string? Language { get; set; }
    public string? FilePath { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual Product Product { get; set; } = null!;
}

public enum ProductDocumentType
{
    PIL,        // Patient Information Leaflet
    SPC,        // Summary of Product Characteristics
    MSDS,       // Material Safety Data Sheet
    CoA,        // Certificate of Analysis
    CoO,        // Certificate of Origin
    Artwork,
    Specification,
    Other
}
