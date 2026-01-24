namespace HAC_Pharma.Domain.Entities.Customers;

/// <summary>
/// Government entities (ministries, agencies)
/// </summary>
public class GovernmentEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public GovernmentEntityType Type { get; set; }
    public string? Ministry { get; set; }
    public string? Department { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public int? PaymentTermDays { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<GovernmentTender> Tenders { get; set; } = new List<GovernmentTender>();
}

public enum GovernmentEntityType
{
    Ministry,
    HealthAuthority,
    HospitalAuthority,
    ProcurementAgency,
    RegulatoryBody
}

/// <summary>
/// Government tenders
/// </summary>
public class GovernmentTender : BaseEntity
{
    public string TenderNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int GovernmentEntityId { get; set; }
    public TenderType Type { get; set; }
    public string? Description { get; set; }
    public DateTime? PublishDate { get; set; }
    public DateTime? SubmissionDeadline { get; set; }
    public DateTime? OpeningDate { get; set; }
    public decimal? EstimatedValue { get; set; }
    public string? Currency { get; set; }
    public TenderStatus Status { get; set; }
    public string? RequiredDocuments { get; set; }
    public bool IsParticipating { get; set; }
    public string? SubmissionStatus { get; set; }
    public string? Result { get; set; }

    public virtual GovernmentEntity GovernmentEntity { get; set; } = null!;
}

public enum TenderType
{
    Open,
    Restricted,
    Framework,
    Emergency,
    DirectPurchase
}

public enum TenderStatus
{
    Published,
    Preparation,
    Submitted,
    UnderEvaluation,
    Awarded,
    Lost,
    Cancelled
}

/// <summary>
/// Institutional buyers (group purchasing organizations)
/// </summary>
public class InstitutionalBuyer : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public InstitutionalBuyerType Type { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int? MemberCount { get; set; }
    public decimal? AnnualPurchaseVolume { get; set; }
    public string? ContractTerms { get; set; }
    public bool IsActive { get; set; } = true;
}

public enum InstitutionalBuyerType
{
    GPO,  // Group Purchasing Organization
    HMO,  // Health Maintenance Organization
    NGO,
    Foundation,
    Cooperative
}

/// <summary>
/// Healthcare providers
/// </summary>
public class HealthcareProvider : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public HealthcareProviderType Type { get; set; }
    public string? LicenseNumber { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Specialties { get; set; }
    public int? BedCount { get; set; }
    public int? PhysicianCount { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Practitioner> Practitioners { get; set; } = new List<Practitioner>();
}

public enum HealthcareProviderType
{
    Hospital,
    Clinic,
    Pharmacy,
    DiagnosticCenter,
    Nursing,
    HomeHealthcare
}

/// <summary>
/// Medical practitioners
/// </summary>
public class Practitioner : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? LicenseNumber { get; set; }
    public PractitionerType Type { get; set; }
    public string? Specialty { get; set; }
    public string? SubSpecialty { get; set; }
    public string? Qualification { get; set; }
    public string? Institution { get; set; }
    public int? HospitalId { get; set; }
    public int? ClinicId { get; set; }
    public int? HealthcareProviderId { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int? YearsOfExperience { get; set; }
    public bool IsKOL { get; set; } // Key Opinion Leader
    public string? Interests { get; set; }
    public PractitionerTier Tier { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual HealthcareProvider? HealthcareProvider { get; set; }
    public virtual ICollection<MedicalRepresentativeInteraction> Interactions { get; set; } = new List<MedicalRepresentativeInteraction>();
}

public enum PractitionerType
{
    Physician,
    Pharmacist,
    Nurse,
    Specialist,
    Consultant
}

public enum PractitionerTier
{
    A,
    B,
    C,
    D
}

/// <summary>
/// Medical representative interactions with practitioners
/// </summary>
public class MedicalRepresentativeInteraction : BaseEntity
{
    public int PractitionerId { get; set; }
    public int? MedicalRepId { get; set; } // Employee ID
    public InteractionType Type { get; set; }
    public DateTime InteractionDate { get; set; }
    public int? DurationMinutes { get; set; }
    public string? Location { get; set; }
    public string? ProductsDiscussed { get; set; }
    public string? KeyMessages { get; set; }
    public string? Feedback { get; set; }
    public string? SamplesProvided { get; set; }
    public string? LiteratureProvided { get; set; }
    public string? FollowUpActions { get; set; }
    public DateTime? NextVisitDate { get; set; }
    public InteractionOutcome Outcome { get; set; }
    public string? Notes { get; set; }

    public virtual Practitioner Practitioner { get; set; } = null!;
}

public enum InteractionType
{
    DetailingVisit,
    GroupMeeting,
    Conference,
    Webinar,
    Phone,
    Email,
    Sample
}

public enum InteractionOutcome
{
    Positive,
    Neutral,
    Negative,
    FollowUpRequired
}
