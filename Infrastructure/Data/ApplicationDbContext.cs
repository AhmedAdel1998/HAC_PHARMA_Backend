using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HAC_Pharma.Domain.Entities;
using HAC_Pharma.Domain.Entities.Core;
using HAC_Pharma.Domain.Entities.Regulatory;
using HAC_Pharma.Domain.Entities.Products;
using HAC_Pharma.Domain.Entities.Warehouse;
using HAC_Pharma.Domain.Entities.Logistics;
using HAC_Pharma.Domain.Entities.Distribution;
using HAC_Pharma.Domain.Entities.Marketing;
using HAC_Pharma.Domain.Entities.Partnership;
using HAC_Pharma.Domain.Entities.Customers;
using HAC_Pharma.Domain.Entities.Regional;
using HAC_Pharma.Domain.Entities.Digital;
using HAC_Pharma.Domain.Entities.Communication;
using HAC_Pharma.Domain.Entities.Strategic;

namespace HAC_Pharma.Infrastructure.Data;

/// <summary>
/// Application Database Context with Identity support
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Notification> Notifications { get; set; }

    #region Core Organization Entities
    public DbSet<Company> Companies { get; set; }
    public DbSet<CompanyLocation> CompanyLocations { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Role> SystemRoles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<InternalTeam> InternalTeams { get; set; }
    public DbSet<InternalTeamMember> InternalTeamMembers { get; set; }
    #endregion

    #region Regulatory & Compliance Entities
    public DbSet<RegulatoryAuthority> RegulatoryAuthorities { get; set; }
    public DbSet<RegulatoryStandard> RegulatoryStandards { get; set; }
    public DbSet<ComplianceDocument> ComplianceDocuments { get; set; }
    public DbSet<SOP> SOPs { get; set; }
    public DbSet<SOPRevision> SOPRevisions { get; set; }
    public DbSet<Audit> Audits { get; set; }
    public DbSet<AuditFinding> AuditFindings { get; set; }
    public DbSet<ProductRegistration> ProductRegistrations { get; set; }
    public DbSet<Dossier> Dossiers { get; set; }
    public DbSet<DossierDocument> DossierDocuments { get; set; }
    public DbSet<LifecycleMonitoringRecord> LifecycleMonitoringRecords { get; set; }
    #endregion

    #region Product & Portfolio Entities
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<TherapeuticCategory> TherapeuticCategories { get; set; }
    public DbSet<StabilityProfile> StabilityProfiles { get; set; }
    public DbSet<StabilityTestResult> StabilityTestResults { get; set; }
    public DbSet<TemperatureRequirement> TemperatureRequirements { get; set; }
    public DbSet<ProductDocument> ProductDocuments { get; set; }
    #endregion

    #region Warehouse & Storage Entities
    public DbSet<HAC_Pharma.Domain.Entities.Warehouse.Warehouse> Warehouses { get; set; }
    public DbSet<StorageZone> StorageZones { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Batch> Batches { get; set; }
    public DbSet<Lot> Lots { get; set; }
    public DbSet<ExpirationRecord> ExpirationRecords { get; set; }
    public DbSet<MonitoringSystem> MonitoringSystems { get; set; }
    public DbSet<TemperatureLog> TemperatureLogs { get; set; }
    public DbSet<Alert> Alerts { get; set; }
    public DbSet<ValidationRecord> ValidationRecords { get; set; }
    #endregion

    #region Cold Chain & Logistics Entities
    public DbSet<SupplyChain> SupplyChains { get; set; }
    public DbSet<ColdChainProcess> ColdChainProcesses { get; set; }
    public DbSet<TransportVehicle> TransportVehicles { get; set; }
    public DbSet<TransportTrip> TransportTrips { get; set; }
    public DbSet<TripTemperatureLog> TripTemperatureLogs { get; set; }
    public DbSet<GPSDevice> GPSDevices { get; set; }
    public DbSet<GPSTrackingHistory> GPSTrackingHistories { get; set; }
    public DbSet<ThermalPackaging> ThermalPackagings { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<ShipmentItem> ShipmentItems { get; set; }
    public DbSet<Delivery> Deliveries { get; set; }
    public DbSet<TraceabilityRecord> TraceabilityRecords { get; set; }
    public DbSet<LogisticsPartner> LogisticsPartners { get; set; }
    public DbSet<LogisticsContract> LogisticsContracts { get; set; }
    #endregion

    #region Distribution & Sales Entities
    public DbSet<Distributor> Distributors { get; set; }
    public DbSet<Wholesaler> Wholesalers { get; set; }
    public DbSet<Pharmacy> Pharmacies { get; set; }
    public DbSet<Hospital> Hospitals { get; set; }
    public DbSet<Clinic> Clinics { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<SalesOrder> SalesOrders { get; set; }
    public DbSet<SalesOrderItem> SalesOrderItems { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<AccountManagement> AccountManagements { get; set; }
    public DbSet<AccountActivity> AccountActivities { get; set; }
    #endregion

    #region Business Development & Marketing Entities
    public DbSet<MarketAnalysis> MarketAnalyses { get; set; }
    public DbSet<UnmetMedicalNeed> UnmetMedicalNeeds { get; set; }
    public DbSet<Campaign> Campaigns { get; set; }
    public DbSet<CampaignResult> CampaignResults { get; set; }
    public DbSet<MarketingStrategy> MarketingStrategies { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<PortfolioExpansionPlan> PortfolioExpansionPlans { get; set; }
    public DbSet<ExclusivityAgreement> ExclusivityAgreements { get; set; }
    #endregion

    #region Partnership & Collaboration Entities
    public DbSet<Partner> Partners { get; set; }
    public DbSet<PartnershipAgreement> PartnershipAgreements { get; set; }
    public DbSet<CollaborationProject> CollaborationProjects { get; set; }
    public DbSet<ProjectMilestone> ProjectMilestones { get; set; }
    public DbSet<ResearchProject> ResearchProjects { get; set; }
    public DbSet<InnovationProgram> InnovationPrograms { get; set; }
    #endregion

    #region Customer & Institutional Entities
    public DbSet<GovernmentEntity> GovernmentEntities { get; set; }
    public DbSet<GovernmentTender> GovernmentTenders { get; set; }
    public DbSet<InstitutionalBuyer> InstitutionalBuyers { get; set; }
    public DbSet<HealthcareProvider> HealthcareProviders { get; set; }
    public DbSet<Practitioner> Practitioners { get; set; }
    public DbSet<MedicalRepresentativeInteraction> MedicalRepresentativeInteractions { get; set; }
    #endregion

    #region Regional Operations Entities
    public DbSet<Country> Countries { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<Market> Markets { get; set; }
    public DbSet<GCCExpansionPlan> GCCExpansionPlans { get; set; }
    public DbSet<LocalPartnership> LocalPartnerships { get; set; }
    public DbSet<SupplyChainResiliencePlan> SupplyChainResiliencePlans { get; set; }
    #endregion

    #region Digital & Innovation Entities
    public DbSet<DigitalHealthTool> DigitalHealthTools { get; set; }
    public DbSet<DataTraceabilitySystem> DataTraceabilitySystems { get; set; }
    public DbSet<MonitoringDashboard> MonitoringDashboards { get; set; }
    public DbSet<InnovationPipeline> InnovationPipelines { get; set; }
    public DbSet<RDProject> RDProjects { get; set; }
    #endregion

    #region Communication Entities
    public DbSet<ContactInformation> ContactInformations { get; set; }
    public DbSet<CommunicationChannel> CommunicationChannels { get; set; }
    public DbSet<Inquiry> Inquiries { get; set; }
    public DbSet<SupportRequest> SupportRequests { get; set; }
    public DbSet<SupportTicketComment> SupportTicketComments { get; set; }
    #endregion

    #region Strategic & Planning Entities
    public DbSet<Mission> Missions { get; set; }
    public DbSet<Vision> Visions { get; set; }
    public DbSet<RoadMap> RoadMaps { get; set; }
    public DbSet<Milestone> Milestones { get; set; }
    public DbSet<ImpactMetric> ImpactMetrics { get; set; }
    public DbSet<PerformanceIndicator> PerformanceIndicators { get; set; }
    public DbSet<PerformanceIndicatorHistory> PerformanceIndicatorHistories { get; set; }
    #endregion

    #region CMS Entities
    public DbSet<HAC_Pharma.Domain.Entities.CMS.Content> CmsContents { get; set; }
    public DbSet<HAC_Pharma.Domain.Entities.CMS.Media> CmsMedia { get; set; }

    public DbSet<HAC_Pharma.Domain.Entities.CMS.Job> Jobs { get; set; }
    public DbSet<HAC_Pharma.Domain.Entities.CMS.JobApplication> JobApplications { get; set; }
    public DbSet<HAC_Pharma.Domain.Entities.CMS.Event> CmsEvents { get; set; }
    public DbSet<HAC_Pharma.Domain.Entities.CMS.EventRegistration> EventRegistrations { get; set; }
    public DbSet<HAC_Pharma.Domain.Entities.CMS.Setting> CmsSettings { get; set; }
    public DbSet<HAC_Pharma.Domain.Entities.CMS.PageView> PageViews { get; set; }
    public DbSet<HAC_Pharma.Domain.Entities.CMS.ContactInquiry> ContactInquiries { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Configure table names with prefix for organization
        ConfigureTableNames(modelBuilder);

        // Configure relationships
        ConfigureRelationships(modelBuilder);

        // Configure soft delete query filter
        ConfigureSoftDeleteFilter(modelBuilder);
    }

    private void ConfigureTableNames(ModelBuilder modelBuilder)
    {
        // Identity tables
        modelBuilder.Entity<ApplicationUser>().ToTable("Users");
        modelBuilder.Entity<ApplicationRole>().ToTable("Roles");
    }

    private void ConfigureRelationships(ModelBuilder modelBuilder)
    {
        // Set ALL foreign keys to NoAction to avoid cascade delete issues
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var foreignKey in entityType.GetForeignKeys())
            {
                foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
            }
        }

        // Configure decimal precision
        foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetPrecision(18);
            property.SetScale(2);
        }
    }

    private void ConfigureSoftDeleteFilter(ModelBuilder modelBuilder)
    {
        // Apply soft delete filter to all entities inheriting from BaseEntity
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
                var property = System.Linq.Expressions.Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                var falseConstant = System.Linq.Expressions.Expression.Constant(false);
                var comparison = System.Linq.Expressions.Expression.Equal(property, falseConstant);
                var lambda = System.Linq.Expressions.Expression.Lambda(comparison, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (BaseEntity)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }
            else
            {
                entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
