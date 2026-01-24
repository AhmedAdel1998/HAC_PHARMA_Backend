namespace HAC_Pharma.Domain.Entities.Digital;

/// <summary>
/// Digital health tools and applications
/// </summary>
public class DigitalHealthTool : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public DigitalToolType Type { get; set; }
    public string? Description { get; set; }
    public string? Purpose { get; set; }
    public string? TargetUsers { get; set; }
    public string? Platform { get; set; } // Web, Mobile, Both
    public string? Version { get; set; }
    public string? Vendor { get; set; }
    public DateTime? LaunchDate { get; set; }
    public string? Features { get; set; }
    public string? Technologies { get; set; }
    public decimal? LicenseCost { get; set; }
    public DigitalToolStatus Status { get; set; }
    public string? SupportContact { get; set; }
}

public enum DigitalToolType
{
    PatientApp,
    HCPPortal,
    EDetailing,
    CRM,
    Analytics,
    Telemedicine,
    Compliance,
    Monitoring
}

public enum DigitalToolStatus
{
    Development,
    Testing,
    Active,
    Deprecated,
    Retired
}

/// <summary>
/// Data traceability systems
/// </summary>
public class DataTraceabilitySystem : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public TraceabilityType Type { get; set; }
    public string? Scope { get; set; }
    public string? DataSources { get; set; }
    public string? IntegrationPoints { get; set; }
    public string? ComplianceStandards { get; set; }
    public bool IsRealTime { get; set; }
    public string? RetentionPeriod { get; set; }
    public DataTraceabilityStatus Status { get; set; }
    public string? Administrator { get; set; }
}

public enum TraceabilityType
{
    ProductTracking,
    BatchTracking,
    Serialization,
    SupplyChain,
    Quality,
    Compliance
}

public enum DataTraceabilityStatus
{
    Planning,
    Implementation,
    Active,
    Maintenance,
    Decommissioned
}

/// <summary>
/// Monitoring dashboards
/// </summary>
public class MonitoringDashboard : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public DashboardType Type { get; set; }
    public string? Description { get; set; }
    public string? Purpose { get; set; }
    public string? DataSources { get; set; }
    public string? KeyMetrics { get; set; }
    public string? RefreshFrequency { get; set; }
    public string? AccessRoles { get; set; }
    public string? Url { get; set; }
    public DashboardStatus Status { get; set; }
    public string? Owner { get; set; }
}

public enum DashboardType
{
    Executive,
    Operations,
    Sales,
    Compliance,
    Quality,
    ColdChain,
    Inventory
}

public enum DashboardStatus
{
    Development,
    Active,
    Archived
}

/// <summary>
/// Innovation pipeline tracking
/// </summary>
public class InnovationPipeline : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? TherapeuticArea { get; set; }
    public string? Description { get; set; }
    public PipelineStage Stage { get; set; }
    public string? SourceOfInnovation { get; set; }
    public DateTime? IdentifiedDate { get; set; }
    public string? PotentialValue { get; set; }
    public string? RequiredInvestment { get; set; }
    public string? Timeline { get; set; }
    public string? Risks { get; set; }
    public string? Champion { get; set; }
    public PipelineStatus Status { get; set; }
    public int? Priority { get; set; }
}

public enum PipelineStage
{
    Ideation,
    Screening,
    Feasibility,
    Development,
    Validation,
    Launch,
    PostLaunch
}

public enum PipelineStatus
{
    Active,
    OnHold,
    Approved,
    Rejected,
    Completed
}

/// <summary>
/// R&D Projects
/// </summary>
public class RDProject : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? ProjectCode { get; set; }
    public RDProjectType Type { get; set; }
    public string? TherapeuticArea { get; set; }
    public string? Indication { get; set; }
    public RDPhase Phase { get; set; }
    public string? Objective { get; set; }
    public string? Scope { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Budget { get; set; }
    public decimal? ActualSpend { get; set; }
    public string? Currency { get; set; }
    public string? ProjectLead { get; set; }
    public string? Team { get; set; }
    public RDProjectStatus Status { get; set; }
    public string? Milestones { get; set; }
    public string? Deliverables { get; set; }
    public int? CompletionPercentage { get; set; }
}

public enum RDProjectType
{
    NewProductDevelopment,
    FormulationDevelopment,
    ProcessImprovement,
    LifecycleManagement,
    Biosimilar,
    Generic
}

public enum RDPhase
{
    Discovery,
    Preclinical,
    PhaseI,
    PhaseII,
    PhaseIII,
    Registration,
    PostMarketing
}

public enum RDProjectStatus
{
    Planning,
    Active,
    OnHold,
    Completed,
    Terminated
}
