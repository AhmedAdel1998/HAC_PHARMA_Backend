namespace HAC_Pharma.Domain.Entities.Regional;

/// <summary>
/// Country entity
/// </summary>
public class Country : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string ISOCode2 { get; set; } = string.Empty; // 2-letter ISO code
    public string? ISOCode3 { get; set; } // 3-letter ISO code
    public string? Capital { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyCode { get; set; }
    public string? Language { get; set; }
    public string? Timezone { get; set; }
    public int? RegionId { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Region? Region { get; set; }
    public virtual ICollection<Market> Markets { get; set; } = new List<Market>();
}

/// <summary>
/// Region entity (GCC, MENA, etc.)
/// </summary>
public class Region : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Country> Countries { get; set; } = new List<Country>();
    public virtual ICollection<GCCExpansionPlan> ExpansionPlans { get; set; } = new List<GCCExpansionPlan>();
}

/// <summary>
/// Market entity
/// </summary>
public class Market : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public int CountryId { get; set; }
    public MarketStatus Status { get; set; }
    public string? MarketSize { get; set; }
    public string? GrowthRate { get; set; }
    public string? KeyPlayers { get; set; }
    public string? RegulatoryEnvironment { get; set; }
    public string? PricingDynamics { get; set; }
    public string? DistributionChannels { get; set; }
    public string? StrategicPriority { get; set; }
    public DateTime? EntryDate { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual Country Country { get; set; } = null!;
}

public enum MarketStatus
{
    Exploring,
    Planning,
    Entering,
    Established,
    Growing,
    Mature,
    Exiting
}

/// <summary>
/// GCC expansion plans
/// </summary>
public class GCCExpansionPlan : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public int? RegionId { get; set; }
    public int? Year { get; set; }
    public string? TargetCountries { get; set; }
    public string? Objectives { get; set; }
    public string? Strategy { get; set; }
    public string? ProductPortfolio { get; set; }
    public string? DistributionStrategy { get; set; }
    public string? RegulatoryStrategy { get; set; }
    public decimal? InvestmentRequired { get; set; }
    public string? ExpectedReturns { get; set; }
    public string? Timeline { get; set; }
    public string? Risks { get; set; }
    public ExpansionPlanStatus Status { get; set; }

    public virtual Region? Region { get; set; }
}

public enum ExpansionPlanStatus
{
    Draft,
    Review,
    Approved,
    InProgress,
    Completed,
    OnHold,
    Cancelled
}

/// <summary>
/// Local partnerships for regional expansion
/// </summary>
public class LocalPartnership : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int? CountryId { get; set; }
    public string? PartnerName { get; set; }
    public LocalPartnerType PartnerType { get; set; }
    public string? PartnerRegistration { get; set; }
    public string? PartnerAddress { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Scope { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Terms { get; set; }
    public LocalPartnershipStatus Status { get; set; }

    public virtual Country? Country { get; set; }
}

public enum LocalPartnerType
{
    Distributor,
    Agent,
    JointVenture,
    Subsidiary,
    Representative
}

public enum LocalPartnershipStatus
{
    Negotiating,
    Active,
    Suspended,
    Terminated
}

/// <summary>
/// Supply chain resilience planning
/// </summary>
public class SupplyChainResiliencePlan : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public int? Year { get; set; }
    public string? Scope { get; set; }
    public string? RiskAssessment { get; set; }
    public string? VulnerabilityAnalysis { get; set; }
    public string? CriticalSuppliers { get; set; }
    public string? AlternativeSuppliers { get; set; }
    public string? InventoryStrategy { get; set; }
    public string? ContingencyPlans { get; set; }
    public string? RecoveryProcedures { get; set; }
    public string? CommunicationPlan { get; set; }
    public ResiliencePlanStatus Status { get; set; }
    public DateTime? LastReviewDate { get; set; }
    public DateTime? NextReviewDate { get; set; }
}

public enum ResiliencePlanStatus
{
    Draft,
    Review,
    Approved,
    Active,
    NeedsUpdate
}
