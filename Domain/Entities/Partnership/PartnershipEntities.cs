namespace HAC_Pharma.Domain.Entities.Partnership;

/// <summary>
/// Partner entity
/// </summary>
public class Partner : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public PartnerType Type { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? Description { get; set; }
    public string? Capabilities { get; set; }
    public string? Certifications { get; set; }
    public PartnerStatus Status { get; set; }
    public DateTime? PartnershipStartDate { get; set; }
    public decimal? Rating { get; set; }

    // Navigation properties
    public virtual ICollection<PartnershipAgreement> Agreements { get; set; } = new List<PartnershipAgreement>();
    public virtual ICollection<CollaborationProject> Projects { get; set; } = new List<CollaborationProject>();
}

public enum PartnerType
{
    Manufacturer,
    LicensedAgent,
    CMO,      // Contract Manufacturing Organization
    CDMO,     // Contract Development and Manufacturing Organization
    AcademicInstitution,
    GovernmentEntity,
    Research,
    Distributor,
    Supplier
}

public enum PartnerStatus
{
    Prospect,
    Active,
    OnHold,
    Inactive,
    Terminated
}

/// <summary>
/// Partnership agreements
/// </summary>
public class PartnershipAgreement : BaseEntity
{
    public string AgreementNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int PartnerId { get; set; }
    public AgreementType AgreementType { get; set; }
    public string? Scope { get; set; }
    public string? Territory { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Value { get; set; }
    public string? Currency { get; set; }
    public string? Terms { get; set; }
    public string? KeyObligations { get; set; }
    public string? Milestones { get; set; }
    public PartnerAgreementStatus Status { get; set; }
    public string? DocumentPath { get; set; }

    public virtual Partner Partner { get; set; } = null!;
}

public enum AgreementType
{
    Manufacturing,
    Licensing,
    Distribution,
    CoMarketing,
    CoDevelopment,
    Research,
    Service,
    Supply
}

public enum PartnerAgreementStatus
{
    Draft,
    Negotiation,
    Pending,
    Signed,
    Active,
    Expired,
    Terminated
}

/// <summary>
/// Collaboration projects
/// </summary>
public class CollaborationProject : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? ProjectCode { get; set; }
    public int? PartnerId { get; set; }
    public ProjectType Type { get; set; }
    public string? Objective { get; set; }
    public string? Scope { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Budget { get; set; }
    public string? Currency { get; set; }
    public ProjectStatus Status { get; set; }
    public string? ProjectManager { get; set; }
    public string? KeyDeliverables { get; set; }
    public string? CurrentProgress { get; set; }
    public int? CompletionPercentage { get; set; }

    // Navigation properties
    public virtual Partner? Partner { get; set; }
    public virtual ICollection<ProjectMilestone> Milestones { get; set; } = new List<ProjectMilestone>();
}

public enum ProjectType
{
    Research,
    Development,
    Manufacturing,
    Marketing,
    Training,
    Technology
}

public enum ProjectStatus
{
    Planning,
    InProgress,
    OnHold,
    Completed,
    Cancelled
}

/// <summary>
/// Project milestones
/// </summary>
public class ProjectMilestone : BaseEntity
{
    public int CollaborationProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? PlannedDate { get; set; }
    public DateTime? ActualDate { get; set; }
    public MilestoneStatus Status { get; set; }
    public string? Deliverables { get; set; }
    public string? Notes { get; set; }

    public virtual CollaborationProject Project { get; set; } = null!;
}

public enum MilestoneStatus
{
    Pending,
    InProgress,
    Completed,
    Delayed,
    Cancelled
}

/// <summary>
/// Research projects
/// </summary>
public class ResearchProject : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? ProjectCode { get; set; }
    public ResearchType Type { get; set; }
    public string? TherapeuticArea { get; set; }
    public string? Indication { get; set; }
    public string? Objective { get; set; }
    public string? Methodology { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Budget { get; set; }
    public ResearchStatus Status { get; set; }
    public string? PrincipalInvestigator { get; set; }
    public int? PartnerId { get; set; }
    public string? Publications { get; set; }
    public string? EthicsApproval { get; set; }

    public virtual Partner? Partner { get; set; }
}

public enum ResearchType
{
    BasicResearch,
    Preclinical,
    ClinicalTrial,
    Observational,
    Epidemiological,
    Outcomes
}

public enum ResearchStatus
{
    Proposed,
    Approved,
    Ongoing,
    Completed,
    Published,
    Terminated
}

/// <summary>
/// Innovation programs
/// </summary>
public class InnovationProgram : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public InnovationType Type { get; set; }
    public string? Focus { get; set; }
    public string? Description { get; set; }
    public string? Objectives { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Budget { get; set; }
    public string? KeyInitiatives { get; set; }
    public string? ExpectedOutcomes { get; set; }
    public InnovationStatus Status { get; set; }
    public string? Lead { get; set; }
}

public enum InnovationType
{
    ProductInnovation,
    ProcessInnovation,
    DigitalInnovation,
    OpenInnovation,
    Incubator
}

public enum InnovationStatus
{
    Ideation,
    Evaluation,
    Active,
    Scaling,
    Completed,
    Archived
}
