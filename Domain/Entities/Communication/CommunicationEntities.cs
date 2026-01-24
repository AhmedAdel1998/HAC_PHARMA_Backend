namespace HAC_Pharma.Domain.Entities.Communication;

/// <summary>
/// Contact information
/// </summary>
public class ContactInformation : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public ContactType Type { get; set; }
    public string? EntityType { get; set; } // Company, Partner, Customer, etc.
    public int? EntityId { get; set; }
    public string? PrimaryPhone { get; set; }
    public string? SecondaryPhone { get; set; }
    public string? PrimaryEmail { get; set; }
    public string? SecondaryEmail { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? Website { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? WhatsApp { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; } = true;
}

public enum ContactType
{
    General,
    Sales,
    Support,
    Technical,
    Billing,
    Emergency,
    Executive
}

/// <summary>
/// Communication channels
/// </summary>
public class CommunicationChannel : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public ChannelType Type { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
    public string? Handle { get; set; } // Username, account name, etc.
    public string? AccessCredentials { get; set; } // Encrypted
    public bool IsActive { get; set; } = true;
    public string? Purpose { get; set; }
    public string? Manager { get; set; }
}

public enum ChannelType
{
    Website,
    Email,
    LinkedIn,
    WhatsApp,
    Twitter,
    Facebook,
    Instagram,
    Phone,
    Fax,
    Chat
}

/// <summary>
/// Customer inquiries
/// </summary>
public class Inquiry : BaseEntity
{
    public string InquiryNumber { get; set; } = string.Empty;
    public InquiryType Type { get; set; }
    public InquirySource Source { get; set; }
    public string? ContactName { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public string? Company { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public InquiryPriority Priority { get; set; }
    public InquiryStatus Status { get; set; }
    public string? AssignedTo { get; set; }
    public DateTime? ResponseDate { get; set; }
    public string? Response { get; set; }
    public DateTime? ClosedDate { get; set; }
    public string? Resolution { get; set; }
}

public enum InquiryType
{
    ProductInfo,
    Pricing,
    Partnership,
    Career,
    Media,
    General
}

public enum InquirySource
{
    Website,
    Email,
    Phone,
    LinkedIn,
    WhatsApp,
    Referral,
    Event
}

public enum InquiryPriority
{
    Low,
    Medium,
    High,
    Urgent
}

public enum InquiryStatus
{
    New,
    InProgress,
    Pending,
    Responded,
    Closed
}

/// <summary>
/// Support requests
/// </summary>
public class SupportRequest : BaseEntity
{
    public string TicketNumber { get; set; } = string.Empty;
    public SupportCategory Category { get; set; }
    public string? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public SupportPriority Priority { get; set; }
    public SupportStatus Status { get; set; }
    public string? AssignedTo { get; set; }
    public string? AssignedTeam { get; set; }
    public DateTime? FirstResponseTime { get; set; }
    public DateTime? ResolvedTime { get; set; }
    public string? Resolution { get; set; }
    public int? SatisfactionRating { get; set; }
    public string? FeedbackComments { get; set; }

    // Navigation properties
    public virtual ICollection<SupportTicketComment> Comments { get; set; } = new List<SupportTicketComment>();
}

public enum SupportCategory
{
    Technical,
    Billing,
    Product,
    Delivery,
    Quality,
    Documentation,
    Other
}

public enum SupportPriority
{
    Low,
    Medium,
    High,
    Critical
}

public enum SupportStatus
{
    New,
    Assigned,
    InProgress,
    WaitingOnCustomer,
    WaitingOnVendor,
    Resolved,
    Closed,
    Reopened
}

/// <summary>
/// Support ticket comments/updates
/// </summary>
public class SupportTicketComment : BaseEntity
{
    public int SupportRequestId { get; set; }
    public string Comment { get; set; } = string.Empty;
    public bool IsInternal { get; set; }
    public string? Author { get; set; }
    public string? AttachmentPath { get; set; }

    public virtual SupportRequest SupportRequest { get; set; } = null!;
}
