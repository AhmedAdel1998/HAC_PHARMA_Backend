namespace HAC_Pharma.Domain.Entities.Regulatory;

/// <summary>
/// Regulatory authorities (SFDA, WHO, GCC, etc.)
/// </summary>
public class RegulatoryAuthority : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Acronym { get; set; }
    public string? Country { get; set; }
    public string? Region { get; set; }
    public string? Website { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<RegulatoryStandard> Standards { get; set; } = new List<RegulatoryStandard>();
    public virtual ICollection<ProductRegistration> ProductRegistrations { get; set; } = new List<ProductRegistration>();
}

/// <summary>
/// Regulatory standards and requirements
/// </summary>
public class RegulatoryStandard : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Version { get; set; }
    public string? Description { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public StandardCategory Category { get; set; }
    public bool IsMandatory { get; set; }
    public bool IsActive { get; set; } = true;

    // Foreign keys
    public int RegulatoryAuthorityId { get; set; }
    public virtual RegulatoryAuthority RegulatoryAuthority { get; set; } = null!;

    // Navigation properties
    public virtual ICollection<ComplianceDocument> ComplianceDocuments { get; set; } = new List<ComplianceDocument>();
}

public enum StandardCategory
{
    Quality,
    Safety,
    Manufacturing,
    Distribution,
    Storage,
    Pharmacovigilance,
    ClinicalTrial
}

/// <summary>
/// Compliance documents and certifications
/// </summary>
public class ComplianceDocument : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? DocumentNumber { get; set; }
    public string? Version { get; set; }
    public DocumentType DocumentType { get; set; }
    public DocumentStatus Status { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string? FilePath { get; set; }
    public string? Description { get; set; }

    // Foreign keys
    public int? RegulatoryStandardId { get; set; }

    // Navigation properties
    public virtual RegulatoryStandard? RegulatoryStandard { get; set; }
}

public enum DocumentType
{
    Certificate,
    License,
    Permit,
    Approval,
    Report,
    Policy,
    Procedure
}

public enum DocumentStatus
{
    Draft,
    UnderReview,
    Approved,
    Active,
    Expired,
    Revoked,
    Archived
}

/// <summary>
/// Standard Operating Procedures
/// </summary>
public class SOP : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string SOPNumber { get; set; } = string.Empty;
    public string? Version { get; set; }
    public string? Department { get; set; }
    public string? Purpose { get; set; }
    public string? Scope { get; set; }
    public string? Procedure { get; set; }
    public string? Responsibilities { get; set; }
    public SOPStatus Status { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ReviewDate { get; set; }
    public DateTime? RetirementDate { get; set; }
    public string? ApprovedBy { get; set; }
    public string? FilePath { get; set; }

    // Navigation properties
    public virtual ICollection<SOPRevision> Revisions { get; set; } = new List<SOPRevision>();
}

public enum SOPStatus
{
    Draft,
    UnderReview,
    Approved,
    Active,
    UnderRevision,
    Retired
}

/// <summary>
/// SOP revision history
/// </summary>
public class SOPRevision : BaseEntity
{
    public int SOPId { get; set; }
    public string Version { get; set; } = string.Empty;
    public string? ChangeDescription { get; set; }
    public DateTime RevisionDate { get; set; }
    public string? RevisedBy { get; set; }
    public string? ApprovedBy { get; set; }

    public virtual SOP SOP { get; set; } = null!;
}

/// <summary>
/// Audit records
/// </summary>
public class Audit : BaseEntity
{
    public string AuditNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public AuditType AuditType { get; set; }
    public AuditStatus Status { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Auditor { get; set; }
    public string? AuditedDepartment { get; set; }
    public string? Scope { get; set; }
    public string? Findings { get; set; }
    public string? Recommendations { get; set; }
    public string? CorrectiveActions { get; set; }
    public int? OverallScore { get; set; }

    // Navigation properties
    public virtual ICollection<AuditFinding> AuditFindings { get; set; } = new List<AuditFinding>();
}

public enum AuditType
{
    Internal,
    External,
    Regulatory,
    Supplier,
    Customer,
    Certification
}

public enum AuditStatus
{
    Planned,
    InProgress,
    Completed,
    FollowUpRequired,
    Closed
}

/// <summary>
/// Individual audit findings
/// </summary>
public class AuditFinding : BaseEntity
{
    public int AuditId { get; set; }
    public string FindingNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public FindingSeverity Severity { get; set; }
    public string? RootCause { get; set; }
    public string? CorrectiveAction { get; set; }
    public string? PreventiveAction { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    public FindingStatus Status { get; set; }

    public virtual Audit Audit { get; set; } = null!;
}

public enum FindingSeverity
{
    Critical,
    Major,
    Minor,
    Observation
}

public enum FindingStatus
{
    Open,
    InProgress,
    PendingVerification,
    Closed
}

/// <summary>
/// Product registration with regulatory authorities
/// </summary>
public class ProductRegistration : BaseEntity
{
    public string RegistrationNumber { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public int RegulatoryAuthorityId { get; set; }
    public RegistrationStatus Status { get; set; }
    public DateTime? SubmissionDate { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public DateTime? RenewalDate { get; set; }
    public string? MarketingAuthorizationNumber { get; set; }
    public string? Notes { get; set; }

    // Navigation properties
    public virtual RegulatoryAuthority RegulatoryAuthority { get; set; } = null!;
    public virtual ICollection<Dossier> Dossiers { get; set; } = new List<Dossier>();
}

public enum RegistrationStatus
{
    Pending,
    UnderReview,
    Approved,
    Rejected,
    Withdrawn,
    Expired,
    Renewed
}

/// <summary>
/// Registration dossiers
/// </summary>
public class Dossier : BaseEntity
{
    public string DossierNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DossierType DossierType { get; set; }
    public string? Version { get; set; }
    public DossierStatus Status { get; set; }
    public DateTime? SubmissionDate { get; set; }
    public string? FilePath { get; set; }
    public string? Description { get; set; }

    // Foreign keys
    public int ProductRegistrationId { get; set; }
    public virtual ProductRegistration ProductRegistration { get; set; } = null!;

    // Navigation properties
    public virtual ICollection<DossierDocument> Documents { get; set; } = new List<DossierDocument>();
}

public enum DossierType
{
    CTD,  // Common Technical Document
    ACTD, // ASEAN CTD
    NDA,  // New Drug Application
    ANDA, // Abbreviated NDA
    Variation,
    Renewal
}

public enum DossierStatus
{
    Draft,
    UnderPreparation,
    Submitted,
    UnderReview,
    QueryRaised,
    Approved,
    Rejected
}

/// <summary>
/// Dossier documents
/// </summary>
public class DossierDocument : BaseEntity
{
    public int DossierId { get; set; }
    public string DocumentName { get; set; } = string.Empty;
    public string? Module { get; set; } // CTD Module (M1-M5)
    public string? Section { get; set; }
    public string? FilePath { get; set; }
    public string? Version { get; set; }
    public DateTime? UploadDate { get; set; }

    public virtual Dossier Dossier { get; set; } = null!;
}

/// <summary>
/// Product lifecycle monitoring
/// </summary>
public class LifecycleMonitoringRecord : BaseEntity
{
    public int ProductId { get; set; }
    public LifecyclePhase Phase { get; set; }
    public DateTime RecordDate { get; set; }
    public string? Summary { get; set; }
    public string? SafetySignals { get; set; }
    public string? EfficacyData { get; set; }
    public string? MarketFeedback { get; set; }
    public string? RecommendedActions { get; set; }
    public string? RecordedBy { get; set; }
}

public enum LifecyclePhase
{
    PreLaunch,
    Launch,
    Growth,
    Maturity,
    Decline,
    Discontinuation
}
