namespace HAC_Pharma.Domain.Entities.Warehouse;

/// <summary>
/// Warehouse entity
/// </summary>
public class Warehouse : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public WarehouseType Type { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public decimal? TotalArea { get; set; } // in square meters
    public decimal? UsableArea { get; set; }
    public int? Capacity { get; set; }
    public string? ContactPerson { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
    public bool HasColdStorage { get; set; }
    public bool IsGDPCertified { get; set; }
    public string? LicenseNumber { get; set; }
    public DateTime? LicenseExpiryDate { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<StorageZone> StorageZones { get; set; } = new List<StorageZone>();
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}

public enum WarehouseType
{
    Central,
    Regional,
    Distribution,
    ColdChain,
    Quarantine
}

/// <summary>
/// Storage zones within warehouses
/// </summary>
public class StorageZone : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public int WarehouseId { get; set; }
    public StorageZoneType ZoneType { get; set; }
    public decimal? MinTemperature { get; set; }
    public decimal? MaxTemperature { get; set; }
    public decimal? MinHumidity { get; set; }
    public decimal? MaxHumidity { get; set; }
    public decimal? Area { get; set; }
    public int? Capacity { get; set; }
    public bool IsControlled { get; set; }
    public bool RequiresMonitoring { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Warehouse Warehouse { get; set; } = null!;
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
    public virtual ICollection<TemperatureLog> TemperatureLogs { get; set; } = new List<TemperatureLog>();
}

public enum StorageZoneType
{
    Ambient,
    Chilled,
    Frozen,
    Quarantine,
    Controlled,
    Hazardous
}

/// <summary>
/// Inventory records
/// </summary>
public class Inventory : BaseEntity
{
    public int ProductId { get; set; }
    public int WarehouseId { get; set; }
    public int? StorageZoneId { get; set; }
    public int QuantityOnHand { get; set; }
    public int QuantityReserved { get; set; }
    public int QuantityAvailable => QuantityOnHand - QuantityReserved;
    public int? ReorderLevel { get; set; }
    public int? MaximumLevel { get; set; }
    public string? Location { get; set; } // Bin/Shelf location
    public DateTime? LastStockCheck { get; set; }
    public InventoryStatus Status { get; set; }

    // Navigation properties
    public virtual Warehouse Warehouse { get; set; } = null!;
    public virtual StorageZone? StorageZone { get; set; }
    public virtual ICollection<Batch> Batches { get; set; } = new List<Batch>();
}

public enum InventoryStatus
{
    Available,
    Reserved,
    InTransit,
    OnHold,
    Quarantine,
    Damaged
}

/// <summary>
/// Batch/Lot records
/// </summary>
public class Batch : BaseEntity
{
    public int InventoryId { get; set; }
    public int ProductId { get; set; }
    public string BatchNumber { get; set; } = string.Empty;
    public string? LotNumber { get; set; }
    public int Quantity { get; set; }
    public int? QuantityRemaining { get; set; }
    public DateTime ManufactureDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public DateTime? ReceivedDate { get; set; }
    public string? SupplierBatchNumber { get; set; }
    public string? CoANumber { get; set; } // Certificate of Analysis
    public BatchStatus Status { get; set; }
    public string? Notes { get; set; }

    // Navigation properties
    public virtual Inventory Inventory { get; set; } = null!;
    public virtual ICollection<ExpirationRecord> ExpirationRecords { get; set; } = new List<ExpirationRecord>();
}

public enum BatchStatus
{
    Received,
    Quarantine,
    Approved,
    Released,
    Rejected,
    Recalled,
    Expired
}

/// <summary>
/// Lot tracking (alias for Batch)
/// </summary>
public class Lot : BaseEntity
{
    public string LotNumber { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public string? BatchNumber { get; set; }
    public int Quantity { get; set; }
    public DateTime ManufactureDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string? SourceLocation { get; set; }
    public LotStatus Status { get; set; }
}

public enum LotStatus
{
    Active,
    Depleted,
    Recalled,
    Expired
}

/// <summary>
/// Expiration tracking records
/// </summary>
public class ExpirationRecord : BaseEntity
{
    public int BatchId { get; set; }
    public DateTime ExpiryDate { get; set; }
    public int DaysUntilExpiry { get; set; }
    public ExpirationAlertLevel AlertLevel { get; set; }
    public bool AlertSent { get; set; }
    public DateTime? AlertSentDate { get; set; }
    public string? ActionTaken { get; set; }
    public string? Notes { get; set; }

    public virtual Batch Batch { get; set; } = null!;
}

public enum ExpirationAlertLevel
{
    None,
    Warning90Days,
    Warning60Days,
    Warning30Days,
    Critical14Days,
    Expired
}

/// <summary>
/// Monitoring systems for warehouses
/// </summary>
public class MonitoringSystem : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? DeviceId { get; set; }
    public string? SerialNumber { get; set; }
    public MonitoringDeviceType DeviceType { get; set; }
    public int? WarehouseId { get; set; }
    public int? StorageZoneId { get; set; }
    public string? Location { get; set; }
    public DateTime? LastCalibrationDate { get; set; }
    public DateTime? NextCalibrationDate { get; set; }
    public decimal? AlertMinTemperature { get; set; }
    public decimal? AlertMaxTemperature { get; set; }
    public decimal? AlertMinHumidity { get; set; }
    public decimal? AlertMaxHumidity { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsOnline { get; set; }
    public DateTime? LastReading { get; set; }

    // Navigation properties
    public virtual ICollection<TemperatureLog> TemperatureLogs { get; set; } = new List<TemperatureLog>();
    public virtual ICollection<Alert> Alerts { get; set; } = new List<Alert>();
}

public enum MonitoringDeviceType
{
    TemperatureSensor,
    HumiditySensor,
    Combined,
    DataLogger,
    Gateway
}

/// <summary>
/// Temperature and humidity logs
/// </summary>
public class TemperatureLog : BaseEntity
{
    public int? MonitoringSystemId { get; set; }
    public int? StorageZoneId { get; set; }
    public DateTime ReadingTime { get; set; }
    public decimal Temperature { get; set; }
    public decimal? Humidity { get; set; }
    public decimal? CO2Level { get; set; }
    public bool IsWithinRange { get; set; }
    public string? Source { get; set; }

    // Navigation properties
    public virtual MonitoringSystem? MonitoringSystem { get; set; }
    public virtual StorageZone? StorageZone { get; set; }
}

/// <summary>
/// System alerts
/// </summary>
public class Alert : BaseEntity
{
    public int? MonitoringSystemId { get; set; }
    public AlertType AlertType { get; set; }
    public AlertSeverity Severity { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }
    public decimal? ReadingValue { get; set; }
    public decimal? ThresholdValue { get; set; }
    public DateTime AlertTime { get; set; }
    public bool IsAcknowledged { get; set; }
    public DateTime? AcknowledgedTime { get; set; }
    public string? AcknowledgedBy { get; set; }
    public bool IsResolved { get; set; }
    public DateTime? ResolvedTime { get; set; }
    public string? Resolution { get; set; }

    // Navigation properties
    public virtual MonitoringSystem? MonitoringSystem { get; set; }
}

public enum AlertType
{
    TemperatureHigh,
    TemperatureLow,
    HumidityHigh,
    HumidityLow,
    DeviceOffline,
    BatteryLow,
    CalibrationDue,
    SystemError
}

public enum AlertSeverity
{
    Info,
    Warning,
    Critical,
    Emergency
}

/// <summary>
/// Validation records for equipment and systems
/// </summary>
public class ValidationRecord : BaseEntity
{
    public string ValidationNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public ValidationType Type { get; set; }
    public int? EquipmentId { get; set; }
    public int? SystemId { get; set; }
    public DateTime? ValidationDate { get; set; }
    public DateTime? NextValidationDate { get; set; }
    public ValidationStatus Status { get; set; }
    public string? Protocol { get; set; }
    public string? Results { get; set; }
    public string? Conclusion { get; set; }
    public string? ValidatedBy { get; set; }
    public string? ApprovedBy { get; set; }
    public string? ReportPath { get; set; }
}

public enum ValidationType
{
    IQ,  // Installation Qualification
    OQ,  // Operational Qualification
    PQ,  // Performance Qualification
    CSV, // Computer System Validation
    Revalidation
}

public enum ValidationStatus
{
    Planned,
    InProgress,
    Completed,
    Failed,
    Overdue
}
