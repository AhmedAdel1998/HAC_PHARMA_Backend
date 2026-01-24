namespace HAC_Pharma.Domain.Entities.Marketing;

/// <summary>
/// Market analysis records
/// </summary>
public class MarketAnalysis : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? AnalysisNumber { get; set; }
    public AnalysisType Type { get; set; }
    public string? Market { get; set; }
    public string? Region { get; set; }
    public string? TherapeuticArea { get; set; }
    public DateTime? AnalysisDate { get; set; }
    public string? MarketSize { get; set; }
    public string? GrowthRate { get; set; }
    public string? KeyFindings { get; set; }
    public string? Recommendations { get; set; }
    public string? Competitors { get; set; }
    public string? Opportunities { get; set; }
    public string? Threats { get; set; }
    public string? PreparedBy { get; set; }
    public string? ReportPath { get; set; }

    // Navigation properties
    public virtual ICollection<UnmetMedicalNeed> UnmetNeeds { get; set; } = new List<UnmetMedicalNeed>();
}

public enum AnalysisType
{
    MarketEntry,
    Competitive,
    ProductLaunch,
    Expansion,
    Quarterly,
    Annual
}

/// <summary>
/// Unmet medical needs tracking
/// </summary>
public class UnmetMedicalNeed : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? TherapeuticArea { get; set; }
    public string? Disease { get; set; }
    public string? CurrentTreatmentGaps { get; set; }
    public string? PatientPopulation { get; set; }
    public NeedPriority Priority { get; set; }
    public string? PotentialSolutions { get; set; }
    public string? MarketPotential { get; set; }
    public string? Status { get; set; }
    public int? MarketAnalysisId { get; set; }

    public virtual MarketAnalysis? MarketAnalysis { get; set; }
}

public enum NeedPriority
{
    Low,
    Medium,
    High,
    Critical
}

/// <summary>
/// Marketing campaigns
/// </summary>
public class Campaign : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public CampaignType Type { get; set; }
    public string? Objective { get; set; }
    public string? TargetAudience { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Budget { get; set; }
    public decimal? ActualSpend { get; set; }
    public string? Currency { get; set; }
    public CampaignStatus Status { get; set; }
    public string? Channels { get; set; } // JSON array
    public string? KeyMessages { get; set; }
    public int? BrandId { get; set; }

    // Navigation properties
    public virtual Brand? Brand { get; set; }
    public virtual ICollection<CampaignResult> Results { get; set; } = new List<CampaignResult>();
}

public enum CampaignType
{
    ProductLaunch,
    BrandAwareness,
    Educational,
    Promotional,
    Digital,
    Event
}

public enum CampaignStatus
{
    Planning,
    Active,
    Completed,
    Cancelled,
    Paused
}

/// <summary>
/// Campaign results tracking
/// </summary>
public class CampaignResult : BaseEntity
{
    public int CampaignId { get; set; }
    public string Metric { get; set; } = string.Empty;
    public string? TargetValue { get; set; }
    public string? ActualValue { get; set; }
    public DateTime? MeasuredDate { get; set; }
    public string? Notes { get; set; }

    public virtual Campaign Campaign { get; set; } = null!;
}

/// <summary>
/// Marketing strategy documents
/// </summary>
public class MarketingStrategy : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public int? Year { get; set; }
    public string? Market { get; set; }
    public string? Vision { get; set; }
    public string? Mission { get; set; }
    public string? Objectives { get; set; }
    public string? TargetSegments { get; set; }
    public string? Positioning { get; set; }
    public string? CompetitiveStrategy { get; set; }
    public string? MarketingMix { get; set; }
    public string? Budget { get; set; }
    public string? KPIs { get; set; }
    public StrategyStatus Status { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovalDate { get; set; }
}

public enum StrategyStatus
{
    Draft,
    UnderReview,
    Approved,
    Active,
    Archived
}

/// <summary>
/// Brand management
/// </summary>
public class Brand : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public string? Logo { get; set; }
    public string? Tagline { get; set; }
    public string? Positioning { get; set; }
    public string? TargetAudience { get; set; }
    public string? BrandValues { get; set; }
    public string? ColorPalette { get; set; }
    public string? Typography { get; set; }
    public BrandStatus Status { get; set; }
    public DateTime? LaunchDate { get; set; }

    // Navigation properties
    public virtual ICollection<Campaign> Campaigns { get; set; } = new List<Campaign>();
}

public enum BrandStatus
{
    Development,
    Active,
    Discontinued
}

/// <summary>
/// Portfolio expansion plans
/// </summary>
public class PortfolioExpansionPlan : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public int? Year { get; set; }
    public string? TherapeuticArea { get; set; }
    public string? Market { get; set; }
    public string? Rationale { get; set; }
    public string? TargetProducts { get; set; }
    public string? SourceOptions { get; set; } // Licensing, Acquisition, Development
    public decimal? InvestmentRequired { get; set; }
    public string? ExpectedRevenue { get; set; }
    public string? Timeline { get; set; }
    public string? Risks { get; set; }
    public ExpansionStatus Status { get; set; }
}

public enum ExpansionStatus
{
    Ideation,
    Evaluation,
    Approved,
    InProgress,
    Completed,
    Cancelled
}

/// <summary>
/// Exclusivity agreements
/// </summary>
public class ExclusivityAgreement : BaseEntity
{
    public string AgreementNumber { get; set; } = string.Empty;
    public string? PartnerName { get; set; }
    public int? ProductId { get; set; }
    public string? Territory { get; set; }
    public ExclusivityType Type { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Terms { get; set; }
    public string? MinimumCommitments { get; set; }
    public decimal? RoyaltyRate { get; set; }
    public AgreementStatus Status { get; set; }
    public string? DocumentPath { get; set; }
}

public enum ExclusivityType
{
    Exclusive,
    SemiExclusive,
    NonExclusive
}

public enum AgreementStatus
{
    Draft,
    Negotiation,
    Signed,
    Active,
    Expired,
    Terminated
}
