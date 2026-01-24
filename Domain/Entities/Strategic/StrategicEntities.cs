namespace HAC_Pharma.Domain.Entities.Strategic;

/// <summary>
/// Company mission statement
/// </summary>
public class Mission : BaseEntity
{
    public string Statement { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? Year { get; set; }
    public string? Version { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public string? ApprovedBy { get; set; }
    public bool IsCurrent { get; set; }
}

/// <summary>
/// Company vision statement
/// </summary>
public class Vision : BaseEntity
{
    public string Statement { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? TimeHorizon { get; set; }
    public int? Year { get; set; }
    public string? Version { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public string? ApprovedBy { get; set; }
    public bool IsCurrent { get; set; }
}

/// <summary>
/// Strategic roadmap
/// </summary>
public class RoadMap : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Code { get; set; }
    public RoadmapType Type { get; set; }
    public int? StartYear { get; set; }
    public int? EndYear { get; set; }
    public string? Objectives { get; set; }
    public string? KeyInitiatives { get; set; }
    public string? StrategicPillars { get; set; }
    public string? SuccessCriteria { get; set; }
    public RoadmapStatus Status { get; set; }
    public string? Owner { get; set; }
    public DateTime? LastReviewDate { get; set; }

    // Navigation properties
    public virtual ICollection<Milestone> Milestones { get; set; } = new List<Milestone>();
}

public enum RoadmapType
{
    Corporate,
    Product,
    Technology,
    Market,
    Operational
}

public enum RoadmapStatus
{
    Draft,
    Review,
    Approved,
    Active,
    Completed,
    Archived
}

/// <summary>
/// Strategic milestones
/// </summary>
public class Milestone : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public int? RoadMapId { get; set; }
    public string? Description { get; set; }
    public MilestoneType Type { get; set; }
    public DateTime? TargetDate { get; set; }
    public DateTime? ActualDate { get; set; }
    public string? Owner { get; set; }
    public string? Dependencies { get; set; }
    public string? Deliverables { get; set; }
    public StrategicMilestoneStatus Status { get; set; }
    public int? Progress { get; set; }
    public string? Notes { get; set; }

    public virtual RoadMap? RoadMap { get; set; }
}

public enum MilestoneType
{
    Strategic,
    Operational,
    Regulatory,
    Commercial,
    Technical
}

public enum StrategicMilestoneStatus
{
    NotStarted,
    InProgress,
    Achieved,
    Delayed,
    AtRisk,
    Cancelled
}

/// <summary>
/// Impact metrics
/// </summary>
public class ImpactMetric : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public ImpactCategory Category { get; set; }
    public string? Description { get; set; }
    public string? Unit { get; set; }
    public string? BaselineValue { get; set; }
    public string? TargetValue { get; set; }
    public string? CurrentValue { get; set; }
    public string? MeasurementMethod { get; set; }
    public string? DataSource { get; set; }
    public string? Frequency { get; set; }
    public string? Owner { get; set; }
    public DateTime? LastMeasuredDate { get; set; }
    public bool IsActive { get; set; } = true;
}

public enum ImpactCategory
{
    PatientImpact,
    SocialImpact,
    EnvironmentalImpact,
    EconomicImpact,
    OperationalImpact
}

/// <summary>
/// Performance indicators (KPIs)
/// </summary>
public class PerformanceIndicator : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public KPICategory Category { get; set; }
    public string? Description { get; set; }
    public string? Formula { get; set; }
    public string? Unit { get; set; }
    public decimal? TargetValue { get; set; }
    public decimal? CurrentValue { get; set; }
    public decimal? PreviousValue { get; set; }
    public KPITrend Trend { get; set; }
    public KPIStatus Status { get; set; }
    public string? DataSource { get; set; }
    public string? UpdateFrequency { get; set; }
    public string? Owner { get; set; }
    public string? Department { get; set; }
    public DateTime? LastUpdated { get; set; }
    public bool IsStrategic { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<PerformanceIndicatorHistory> History { get; set; } = new List<PerformanceIndicatorHistory>();
}

public enum KPICategory
{
    Financial,
    Operational,
    Customer,
    Quality,
    Compliance,
    Innovation,
    People
}

public enum KPITrend
{
    Improving,
    Stable,
    Declining,
    Unknown
}

public enum KPIStatus
{
    OnTarget,
    AtRisk,
    BelowTarget,
    AboveTarget
}

/// <summary>
/// Performance indicator history
/// </summary>
public class PerformanceIndicatorHistory : BaseEntity
{
    public int PerformanceIndicatorId { get; set; }
    public DateTime MeasurementDate { get; set; }
    public decimal Value { get; set; }
    public string? Notes { get; set; }
    public string? RecordedBy { get; set; }

    public virtual PerformanceIndicator PerformanceIndicator { get; set; } = null!;
}
