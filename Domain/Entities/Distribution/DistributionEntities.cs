using HAC_Pharma.Domain.Entities.Customers;

namespace HAC_Pharma.Domain.Entities.Distribution;

/// <summary>
/// Distributor entity
/// </summary>
public class Distributor : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? LegalName { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? TaxId { get; set; }
    public DistributorType Type { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? LicenseNumber { get; set; }
    public DateTime? LicenseExpiryDate { get; set; }
    public decimal? CreditLimit { get; set; }
    public int? PaymentTermDays { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<SalesOrder> SalesOrders { get; set; } = new List<SalesOrder>();
    public virtual ICollection<AccountManagement> AccountManagements { get; set; } = new List<AccountManagement>();
}

public enum DistributorType
{
    Exclusive,
    NonExclusive,
    Wholesale,
    Retail
}

/// <summary>
/// Wholesaler entity
/// </summary>
public class Wholesaler : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? DrugLicenseNumber { get; set; }
    public DateTime? LicenseExpiryDate { get; set; }
    public decimal? CreditLimit { get; set; }
    public int? PaymentTermDays { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<SalesOrder> SalesOrders { get; set; } = new List<SalesOrder>();
}

/// <summary>
/// Pharmacy entity
/// </summary>
public class Pharmacy : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? LicenseNumber { get; set; }
    public DateTime? LicenseExpiryDate { get; set; }
    public PharmacyType Type { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? PharmacistInCharge { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? OperatingHours { get; set; }
    public bool Is24Hours { get; set; }
    public bool AcceptsInsurance { get; set; }
    public decimal? CreditLimit { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<SalesOrder> SalesOrders { get; set; } = new List<SalesOrder>();
}

public enum PharmacyType
{
    Community,
    Hospital,
    Clinical,
    Chain,
    Independent
}

/// <summary>
/// Hospital entity
/// </summary>
public class Hospital : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? RegistrationNumber { get; set; }
    public HospitalType Type { get; set; }
    public int? BedCapacity { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public bool IsGovernment { get; set; }
    public decimal? CreditLimit { get; set; }
    public int? PaymentTermDays { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<SalesOrder> SalesOrders { get; set; } = new List<SalesOrder>();
    public virtual ICollection<Practitioner> Practitioners { get; set; } = new List<Practitioner>();
}

public enum HospitalType
{
    General,
    Specialty,
    Teaching,
    Private,
    Government
}

/// <summary>
/// Clinic entity
/// </summary>
public class Clinic : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? LicenseNumber { get; set; }
    public ClinicType Type { get; set; }
    public string? Specialty { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<SalesOrder> SalesOrders { get; set; } = new List<SalesOrder>();
    public virtual ICollection<Practitioner> Practitioners { get; set; } = new List<Practitioner>();
}

public enum ClinicType
{
    GeneralPractice,
    Specialty,
    Polyclinic,
    Diagnostic
}

/// <summary>
/// Customer entity (generic)
/// </summary>
public class Customer : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public CustomerType Type { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? TaxId { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public decimal? CreditLimit { get; set; }
    public decimal? CurrentBalance { get; set; }
    public int? PaymentTermDays { get; set; }
    public CustomerStatus Status { get; set; }

    // Navigation properties
    public virtual ICollection<SalesOrder> SalesOrders { get; set; } = new List<SalesOrder>();
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

public enum CustomerType
{
    Distributor,
    Wholesaler,
    Pharmacy,
    Hospital,
    Clinic,
    Government,
    Retail,
    Other
}

public enum CustomerStatus
{
    Active,
    Inactive,
    Suspended,
    Blocked
}

/// <summary>
/// Sales order entity
/// </summary>
public class SalesOrder : BaseEntity
{
    public string OrderNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public int? DistributorId { get; set; }
    public int? WholesalerId { get; set; }
    public int? PharmacyId { get; set; }
    public int? HospitalId { get; set; }
    public int? ClinicId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? RequiredDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public OrderStatus Status { get; set; }
    public string? ShippingAddress { get; set; }
    public string? ShippingCity { get; set; }
    public string? ShippingCountry { get; set; }
    public decimal SubTotal { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? DiscountPercent { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal? ShippingCost { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Currency { get; set; }
    public string? Notes { get; set; }
    public int? SalesRepId { get; set; }

    // Navigation properties
    public virtual Customer Customer { get; set; } = null!;
    public virtual Distributor? Distributor { get; set; }
    public virtual Wholesaler? Wholesaler { get; set; }
    public virtual Pharmacy? Pharmacy { get; set; }
    public virtual Hospital? Hospital { get; set; }
    public virtual Clinic? Clinic { get; set; }
    public virtual ICollection<SalesOrderItem> Items { get; set; } = new List<SalesOrderItem>();
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}

public enum OrderStatus
{
    Draft,
    Pending,
    Confirmed,
    Processing,
    Shipped,
    Delivered,
    Cancelled,
    Returned
}

/// <summary>
/// Sales order line items
/// </summary>
public class SalesOrderItem : BaseEntity
{
    public int SalesOrderId { get; set; }
    public int ProductId { get; set; }
    public string? BatchNumber { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal? DiscountPercent { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal TotalPrice { get; set; }
    public string? Notes { get; set; }

    public virtual SalesOrder SalesOrder { get; set; } = null!;
}

/// <summary>
/// Invoice entity
/// </summary>
public class Invoice : BaseEntity
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public int? SalesOrderId { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime DueDate { get; set; }
    public InvoiceStatus Status { get; set; }
    public decimal SubTotal { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal? PaidAmount { get; set; }
    public decimal? BalanceDue { get; set; }
    public string? Currency { get; set; }
    public string? Notes { get; set; }
    public string? BillingAddress { get; set; }

    // Navigation properties
    public virtual Customer Customer { get; set; } = null!;
    public virtual SalesOrder? SalesOrder { get; set; }
    public virtual ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

public enum InvoiceStatus
{
    Draft,
    Sent,
    Paid,
    PartiallyPaid,
    Overdue,
    Cancelled,
    Disputed
}

/// <summary>
/// Invoice line items
/// </summary>
public class InvoiceItem : BaseEntity
{
    public int InvoiceId { get; set; }
    public int ProductId { get; set; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal TotalPrice { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;
}

/// <summary>
/// Payment records
/// </summary>
public class Payment : BaseEntity
{
    public string PaymentNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public int? InvoiceId { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal Amount { get; set; }
    public string? Currency { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? BankName { get; set; }
    public string? CheckNumber { get; set; }
    public string? TransactionId { get; set; }
    public string? Notes { get; set; }

    // Navigation properties
    public virtual Customer Customer { get; set; } = null!;
    public virtual Invoice? Invoice { get; set; }
}

public enum PaymentMethod
{
    Cash,
    BankTransfer,
    Check,
    CreditCard,
    OnlinePayment,
    LetterOfCredit
}

public enum PaymentStatus
{
    Pending,
    Completed,
    Failed,
    Refunded,
    Cancelled
}

/// <summary>
/// Collection tracking for overdue payments
/// </summary>
public class Collection : BaseEntity
{
    public int CustomerId { get; set; }
    public int? InvoiceId { get; set; }
    public decimal AmountDue { get; set; }
    public int DaysOverdue { get; set; }
    public CollectionStatus Status { get; set; }
    public DateTime? LastContactDate { get; set; }
    public string? LastContactMethod { get; set; }
    public string? Notes { get; set; }
    public string? AssignedTo { get; set; }
    public DateTime? NextFollowUpDate { get; set; }
    public string? PromisedPaymentDate { get; set; }
}

public enum CollectionStatus
{
    Open,
    InProgress,
    PromiseToPay,
    Disputed,
    Escalated,
    Collected,
    WrittenOff
}

/// <summary>
/// Account management for key customers
/// </summary>
public class AccountManagement : BaseEntity
{
    public int CustomerId { get; set; }
    public int? DistributorId { get; set; }
    public string? AccountManagerId { get; set; }
    public AccountTier Tier { get; set; }
    public decimal? AnnualRevenue { get; set; }
    public decimal? GrowthTarget { get; set; }
    public string? StrategicNotes { get; set; }
    public DateTime? LastReviewDate { get; set; }
    public DateTime? NextReviewDate { get; set; }
    public decimal? CustomerSatisfactionScore { get; set; }
    public bool IsStrategicAccount { get; set; }

    // Navigation properties
    public virtual Distributor? Distributor { get; set; }
    public virtual ICollection<AccountActivity> Activities { get; set; } = new List<AccountActivity>();
}

public enum AccountTier
{
    Bronze,
    Silver,
    Gold,
    Platinum,
    Strategic
}

/// <summary>
/// Account activity log
/// </summary>
public class AccountActivity : BaseEntity
{
    public int AccountManagementId { get; set; }
    public ActivityType Type { get; set; }
    public DateTime ActivityDate { get; set; }
    public string? Subject { get; set; }
    public string? Description { get; set; }
    public string? Outcome { get; set; }
    public string? PerformedBy { get; set; }
    public DateTime? FollowUpDate { get; set; }

    public virtual AccountManagement AccountManagement { get; set; } = null!;
}

public enum ActivityType
{
    Call,
    Meeting,
    Email,
    Visit,
    Presentation,
    Negotiation,
    Other
}
