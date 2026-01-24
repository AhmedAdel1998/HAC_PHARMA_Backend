namespace HAC_Pharma.Domain.Entities.Core;

/// <summary>
/// Main company entity representing HAC Pharma
/// </summary>
public class Company : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? LegalName { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? TaxId { get; set; }
    public string? Description { get; set; }
    public string? Logo { get; set; }
    public string? Website { get; set; }
    public DateTime? FoundedDate { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<CompanyLocation> Locations { get; set; } = new List<CompanyLocation>();
    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
}

/// <summary>
/// Company locations including headquarters and virtual offices
/// </summary>
public class CompanyLocation : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public LocationType LocationType { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool IsHeadquarters { get; set; }
    public bool IsVirtualOffice { get; set; }
    public bool IsActive { get; set; } = true;

    // Foreign keys
    public int CompanyId { get; set; }
    public virtual Company Company { get; set; } = null!;
}

public enum LocationType
{
    Headquarters,
    VirtualOffice,
    RegionalOffice,
    Warehouse,
    ManufacturingPlant
}

/// <summary>
/// Company departments
/// </summary>
public class Department : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    // Foreign keys
    public int CompanyId { get; set; }
    public int? ParentDepartmentId { get; set; }

    // Navigation properties
    public virtual Company Company { get; set; } = null!;
    public virtual Department? ParentDepartment { get; set; }
    public virtual ICollection<Department> SubDepartments { get; set; } = new List<Department>();
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public virtual ICollection<InternalTeam> Teams { get; set; } = new List<InternalTeam>();
}

/// <summary>
/// Employee entity
/// </summary>
public class Employee : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? EmployeeCode { get; set; }
    public string? JobTitle { get; set; }
    public DateTime? HireDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;
    public string? ProfileImage { get; set; }

    // Foreign keys
    public int DepartmentId { get; set; }
    public int RoleId { get; set; }
    public int? ManagerId { get; set; }
    public string? UserId { get; set; } // Link to Identity User

    // Navigation properties
    public virtual Department Department { get; set; } = null!;
    public virtual Role Role { get; set; } = null!;
    public virtual Employee? Manager { get; set; }
    public virtual ICollection<Employee> DirectReports { get; set; } = new List<Employee>();
    public virtual ICollection<InternalTeamMember> TeamMemberships { get; set; } = new List<InternalTeamMember>();
}

public enum EmployeeStatus
{
    Active,
    OnLeave,
    Suspended,
    Terminated
}

/// <summary>
/// System roles for employees
/// </summary>
public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public RoleCategory Category { get; set; }
    public bool IsSystemRole { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public virtual ICollection<RolePermission> Permissions { get; set; } = new List<RolePermission>();
}

public enum RoleCategory
{
    Regulatory,
    Sales,
    Medical,
    Operations,
    Management,
    Technical,
    Administrative
}

/// <summary>
/// Role permissions
/// </summary>
public class RolePermission : BaseEntity
{
    public string PermissionName { get; set; } = string.Empty;
    public string? Resource { get; set; }
    public bool CanCreate { get; set; }
    public bool CanRead { get; set; }
    public bool CanUpdate { get; set; }
    public bool CanDelete { get; set; }

    // Foreign keys
    public int RoleId { get; set; }
    public virtual Role Role { get; set; } = null!;
}

/// <summary>
/// Internal teams within departments
/// </summary>
public class InternalTeam : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public TeamType TeamType { get; set; }
    public bool IsActive { get; set; } = true;

    // Foreign keys
    public int DepartmentId { get; set; }
    public int? TeamLeadId { get; set; }

    // Navigation properties
    public virtual Department Department { get; set; } = null!;
    public virtual Employee? TeamLead { get; set; }
    public virtual ICollection<InternalTeamMember> Members { get; set; } = new List<InternalTeamMember>();
}

public enum TeamType
{
    Regulatory,
    Sales,
    MedicalRepresentatives,
    Research,
    Quality,
    Logistics,
    Marketing
}

/// <summary>
/// Team membership junction table
/// </summary>
public class InternalTeamMember : BaseEntity
{
    public int TeamId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime JoinDate { get; set; } = DateTime.UtcNow;
    public DateTime? LeaveDate { get; set; }
    public string? TeamRole { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual InternalTeam Team { get; set; } = null!;
    public virtual Employee Employee { get; set; } = null!;
}
