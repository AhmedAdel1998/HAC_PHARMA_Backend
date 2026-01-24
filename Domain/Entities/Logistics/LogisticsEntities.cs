namespace HAC_Pharma.Domain.Entities.Logistics;

/// <summary>
/// Supply chain management
/// </summary>
public class SupplyChain : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public SupplyChainType Type { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<ColdChainProcess> ColdChainProcesses { get; set; } = new List<ColdChainProcess>();
    public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
}

public enum SupplyChainType
{
    Standard,
    ColdChain,
    Controlled,
    Express
}

/// <summary>
/// Cold chain process definitions
/// </summary>
public class ColdChainProcess : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public int? SupplyChainId { get; set; }
    public decimal RequiredMinTemperature { get; set; }
    public decimal RequiredMaxTemperature { get; set; }
    public string? PackagingRequirements { get; set; }
    public string? HandlingInstructions { get; set; }
    public int? MaxTransitTimeHours { get; set; }
    public bool RequiresContinuousMonitoring { get; set; }
    public bool RequiresTemperatureLogging { get; set; }
    public string? ValidationProtocol { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual SupplyChain? SupplyChain { get; set; }
}

/// <summary>
/// Transport vehicles
/// </summary>
public class TransportVehicle : BaseEntity
{
    public string VehicleNumber { get; set; } = string.Empty;
    public string? PlateNumber { get; set; }
    public VehicleType Type { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public int? Year { get; set; }
    public decimal? Capacity { get; set; } // in cubic meters or kg
    public bool HasRefrigeration { get; set; }
    public decimal? MinTemperature { get; set; }
    public decimal? MaxTemperature { get; set; }
    public bool HasGPS { get; set; }
    public bool HasTemperatureMonitoring { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public DateTime? NextMaintenanceDate { get; set; }
    public DateTime? InsuranceExpiryDate { get; set; }
    public VehicleStatus Status { get; set; }
    public int? LogisticsPartnerId { get; set; }

    // Navigation properties
    public virtual LogisticsPartner? LogisticsPartner { get; set; }
    public virtual ICollection<TransportTrip> Trips { get; set; } = new List<TransportTrip>();
    public virtual ICollection<GPSDevice> GPSDevices { get; set; } = new List<GPSDevice>();
}

public enum VehicleType
{
    Van,
    Truck,
    RefrigeratedVan,
    RefrigeratedTruck,
    Container
}

public enum VehicleStatus
{
    Available,
    InTransit,
    Maintenance,
    OutOfService
}

/// <summary>
/// Transport trips
/// </summary>
public class TransportTrip : BaseEntity
{
    public string TripNumber { get; set; } = string.Empty;
    public int VehicleId { get; set; }
    public string? DriverName { get; set; }
    public string? DriverPhone { get; set; }
    public string OriginLocation { get; set; } = string.Empty;
    public string DestinationLocation { get; set; } = string.Empty;
    public DateTime? ScheduledDepartureTime { get; set; }
    public DateTime? ActualDepartureTime { get; set; }
    public DateTime? ScheduledArrivalTime { get; set; }
    public DateTime? ActualArrivalTime { get; set; }
    public decimal? DistanceKm { get; set; }
    public TripStatus Status { get; set; }
    public string? Notes { get; set; }

    // Navigation properties
    public virtual TransportVehicle Vehicle { get; set; } = null!;
    public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    public virtual ICollection<TripTemperatureLog> TemperatureLogs { get; set; } = new List<TripTemperatureLog>();
}

public enum TripStatus
{
    Scheduled,
    InProgress,
    Completed,
    Cancelled,
    Delayed
}

/// <summary>
/// Temperature logs during transport
/// </summary>
public class TripTemperatureLog : BaseEntity
{
    public int TripId { get; set; }
    public DateTime ReadingTime { get; set; }
    public decimal Temperature { get; set; }
    public decimal? Humidity { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public bool IsWithinRange { get; set; }
    public string? DeviceId { get; set; }

    public virtual TransportTrip Trip { get; set; } = null!;
}

/// <summary>
/// GPS tracking devices
/// </summary>
public class GPSDevice : BaseEntity
{
    public string DeviceId { get; set; } = string.Empty;
    public string? SerialNumber { get; set; }
    public string? IMEI { get; set; }
    public int? VehicleId { get; set; }
    public decimal? CurrentLatitude { get; set; }
    public decimal? CurrentLongitude { get; set; }
    public decimal? Speed { get; set; }
    public DateTime? LastUpdateTime { get; set; }
    public GPSDeviceStatus Status { get; set; }
    public decimal? BatteryLevel { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual TransportVehicle? Vehicle { get; set; }
    public virtual ICollection<GPSTrackingHistory> TrackingHistory { get; set; } = new List<GPSTrackingHistory>();
}

public enum GPSDeviceStatus
{
    Active,
    Inactive,
    LowBattery,
    Offline
}

/// <summary>
/// GPS tracking history
/// </summary>
public class GPSTrackingHistory : BaseEntity
{
    public int GPSDeviceId { get; set; }
    public DateTime Timestamp { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public decimal? Speed { get; set; }
    public decimal? Heading { get; set; }
    public string? Address { get; set; }

    public virtual GPSDevice GPSDevice { get; set; } = null!;
}

/// <summary>
/// Thermal packaging specifications
/// </summary>
public class ThermalPackaging : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public PackagingType Type { get; set; }
    public decimal? MinTemperature { get; set; }
    public decimal? MaxTemperature { get; set; }
    public int? DurationHours { get; set; } // How long it maintains temperature
    public string? Dimensions { get; set; }
    public decimal? Weight { get; set; }
    public decimal? Capacity { get; set; }
    public string? Material { get; set; }
    public bool IsReusable { get; set; }
    public decimal? UnitCost { get; set; }
    public string? Supplier { get; set; }
    public bool IsActive { get; set; } = true;
}

public enum PackagingType
{
    InsulatedBox,
    CoolBox,
    PassiveShipper,
    ActiveShipper,
    GelPack,
    DryIce
}

/// <summary>
/// Shipments
/// </summary>
public class Shipment : BaseEntity
{
    public string ShipmentNumber { get; set; } = string.Empty;
    public int? SupplyChainId { get; set; }
    public int? TripId { get; set; }
    public string? TrackingNumber { get; set; }
    public string? OriginWarehouse { get; set; }
    public string? DestinationAddress { get; set; }
    public string? RecipientName { get; set; }
    public string? RecipientPhone { get; set; }
    public ShipmentStatus Status { get; set; }
    public DateTime? ShipmentDate { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public DateTime? ActualDeliveryDate { get; set; }
    public decimal? TotalWeight { get; set; }
    public int? TotalPackages { get; set; }
    public decimal? DeclaredValue { get; set; }
    public string? Currency { get; set; }
    public bool RequiresColdChain { get; set; }
    public int? ThermalPackagingId { get; set; }
    public string? SpecialInstructions { get; set; }

    // Navigation properties
    public virtual SupplyChain? SupplyChain { get; set; }
    public virtual TransportTrip? Trip { get; set; }
    public virtual ThermalPackaging? ThermalPackaging { get; set; }
    public virtual ICollection<ShipmentItem> Items { get; set; } = new List<ShipmentItem>();
    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
    public virtual ICollection<TraceabilityRecord> TraceabilityRecords { get; set; } = new List<TraceabilityRecord>();
}

public enum ShipmentStatus
{
    Pending,
    Processing,
    Shipped,
    InTransit,
    OutForDelivery,
    Delivered,
    Failed,
    Returned,
    Cancelled
}

/// <summary>
/// Shipment items
/// </summary>
public class ShipmentItem : BaseEntity
{
    public int ShipmentId { get; set; }
    public int ProductId { get; set; }
    public string? BatchNumber { get; set; }
    public int Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? TotalPrice { get; set; }

    public virtual Shipment Shipment { get; set; } = null!;
}

/// <summary>
/// Delivery records
/// </summary>
public class Delivery : BaseEntity
{
    public string DeliveryNumber { get; set; } = string.Empty;
    public int ShipmentId { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? ReceivedBy { get; set; }
    public string? Signature { get; set; } // Path to signature image
    public DeliveryStatus Status { get; set; }
    public bool ConditionAcceptable { get; set; }
    public decimal? ReceivedTemperature { get; set; }
    public string? Notes { get; set; }
    public string? ProofOfDeliveryPath { get; set; }

    public virtual Shipment Shipment { get; set; } = null!;
}

public enum DeliveryStatus
{
    Pending,
    Delivered,
    PartiallyDelivered,
    Refused,
    Failed
}

/// <summary>
/// Traceability records for products
/// </summary>
public class TraceabilityRecord : BaseEntity
{
    public int? ShipmentId { get; set; }
    public int? ProductId { get; set; }
    public string? BatchNumber { get; set; }
    public TraceabilityEventType EventType { get; set; }
    public DateTime EventTime { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public string? PerformedBy { get; set; }
    public string? AdditionalData { get; set; } // JSON for extra fields

    public virtual Shipment? Shipment { get; set; }
}

public enum TraceabilityEventType
{
    Manufactured,
    Received,
    Stored,
    Picked,
    Packed,
    Shipped,
    InTransit,
    Delivered,
    Returned,
    Recalled,
    Disposed
}

/// <summary>
/// Logistics partners (3PL, carriers)
/// </summary>
public class LogisticsPartner : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public LogisticsPartnerType Type { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? ContactPerson { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
    public bool HasColdChainCapability { get; set; }
    public bool IsGDPCertified { get; set; }
    public string? CertificateNumber { get; set; }
    public DateTime? CertificateExpiryDate { get; set; }
    public decimal? Rating { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<TransportVehicle> Vehicles { get; set; } = new List<TransportVehicle>();
    public virtual ICollection<LogisticsContract> Contracts { get; set; } = new List<LogisticsContract>();
}

public enum LogisticsPartnerType
{
    Carrier,
    ThirdPartyLogistics,
    Courier,
    FreightForwarder
}

/// <summary>
/// Logistics contracts
/// </summary>
public class LogisticsContract : BaseEntity
{
    public int LogisticsPartnerId { get; set; }
    public string ContractNumber { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? ContractValue { get; set; }
    public string? Currency { get; set; }
    public string? Terms { get; set; }
    public string? SLADetails { get; set; }
    public ContractStatus Status { get; set; }

    public virtual LogisticsPartner LogisticsPartner { get; set; } = null!;
}

public enum ContractStatus
{
    Draft,
    Active,
    Expired,
    Terminated,
    Renewal
}
