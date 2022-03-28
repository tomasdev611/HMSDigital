using Microsoft.EntityFrameworkCore;
using HMS2SDK.Data.Models;

namespace HMS2SDK.Data
{
    public partial class HMSContext : DbContext
    {
        public HMSContext()
        {
        }

        public HMSContext(DbContextOptions<HMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AssetTagMaster> AssetTagMaster { get; set; }
        public virtual DbSet<DataTranslation> DataTranslation { get; set; }
        public virtual DbSet<DataTranslationCopy1> DataTranslationCopy1 { get; set; }
        public virtual DbSet<Division> Division { get; set; }
        public virtual DbSet<Hospice> Hospice { get; set; }
        public virtual DbSet<HospiceBranchMap> HospiceBranchMap { get; set; }
        public virtual DbSet<Inventory> Inventory { get; set; }
        public virtual DbSet<InventoryMap> InventoryMap { get; set; }
        public virtual DbSet<InventoryMapCopy1> InventoryMapCopy1 { get; set; }
        public virtual DbSet<InventoryPmcs> InventoryPmcs { get; set; }
        public virtual DbSet<InventoryPmcsLog> InventoryPmcsLog { get; set; }
        public virtual DbSet<Inventoryserialnumbers> Inventoryserialnumbers { get; set; }
        public virtual DbSet<LinkPatientProducts> LinkPatientProducts { get; set; }
        public virtual DbSet<LinkPatientProductsCopy1> LinkPatientProductsCopy1 { get; set; }
        public virtual DbSet<Logging> Logging { get; set; }
        public virtual DbSet<MailingListEmails> MailingListEmails { get; set; }
        public virtual DbSet<MailingLists> MailingLists { get; set; }
        public virtual DbSet<Markers> Markers { get; set; }
        public virtual DbSet<MarkersDispatch> MarkersDispatch { get; set; }
        public virtual DbSet<MarkersPassages> MarkersPassages { get; set; }
        public virtual DbSet<MarkersUpdate> MarkersUpdate { get; set; }
        public virtual DbSet<Messages> Messages { get; set; }
        public virtual DbSet<OrderMap> OrderMap { get; set; }
        public virtual DbSet<PassagesFacilities> PassagesFacilities { get; set; }
        public virtual DbSet<PassagesPatients> PassagesPatients { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<PatientHms> PatientHms { get; set; }
        public virtual DbSet<PatientMap> PatientMap { get; set; }
        public virtual DbSet<PatientMapCopy1> PatientMapCopy1 { get; set; }
        public virtual DbSet<Patientlineitems> Patientlineitems { get; set; }
        public virtual DbSet<Patientorders> Patientorders { get; set; }
        public virtual DbSet<PermissionsRestrictedPages> PermissionsRestrictedPages { get; set; }
        public virtual DbSet<PermissionsUsers> PermissionsUsers { get; set; }
        public virtual DbSet<PickupStaging> PickupStaging { get; set; }
        public virtual DbSet<PoTags> PoTags { get; set; }
        public virtual DbSet<Semaphore> Semaphore { get; set; }
        public virtual DbSet<TblAccumulatedDepreciation> TblAccumulatedDepreciation { get; set; }
        public virtual DbSet<TblAfterHours> TblAfterHours { get; set; }
        public virtual DbSet<TblBillingAdjReview> TblBillingAdjReview { get; set; }
        public virtual DbSet<TblBillingFiles> TblBillingFiles { get; set; }
        public virtual DbSet<TblBillingFilesH2h> TblBillingFilesH2h { get; set; }
        public virtual DbSet<TblBillingInvoiceCombine> TblBillingInvoiceCombine { get; set; }
        public virtual DbSet<TblBillingPatientDays> TblBillingPatientDays { get; set; }
        public virtual DbSet<TblBillingProcess> TblBillingProcess { get; set; }
        public virtual DbSet<TblBillingProcessSim> TblBillingProcessSim { get; set; }
        public virtual DbSet<TblBillingRevenueOnly> TblBillingRevenueOnly { get; set; }
        public virtual DbSet<TblBrowser> TblBrowser { get; set; }
        public virtual DbSet<TblCapex> TblCapex { get; set; }
        public virtual DbSet<TblCapexCopy> TblCapexCopy { get; set; }
        public virtual DbSet<TblCategories> TblCategories { get; set; }
        public virtual DbSet<TblCensusFiles> TblCensusFiles { get; set; }
        public virtual DbSet<TblContractInventory> TblContractInventory { get; set; }
        public virtual DbSet<TblContractInventoryCopy> TblContractInventoryCopy { get; set; }
        public virtual DbSet<TblContracts> TblContracts { get; set; }
        public virtual DbSet<TblCreditRequests> TblCreditRequests { get; set; }
        public virtual DbSet<TblCsFiles> TblCsFiles { get; set; }
        public virtual DbSet<TblCustomerEmails> TblCustomerEmails { get; set; }
        public virtual DbSet<TblCustomers> TblCustomers { get; set; }
        public virtual DbSet<TblDcHistory> TblDcHistory { get; set; }
        public virtual DbSet<TblDeclinedOffCap> TblDeclinedOffCap { get; set; }
        public virtual DbSet<TblDepreciationMaster> TblDepreciationMaster { get; set; }
        public virtual DbSet<TblDepreciationMaster61015> TblDepreciationMaster61015 { get; set; }
        public virtual DbSet<TblDispatchHistory> TblDispatchHistory { get; set; }
        public virtual DbSet<TblDispatchNotes> TblDispatchNotes { get; set; }
        public virtual DbSet<TblDmeGroups> TblDmeGroups { get; set; }
        public virtual DbSet<TblDmePartners> TblDmePartners { get; set; }
        public virtual DbSet<TblDrivers> TblDrivers { get; set; }
        public virtual DbSet<TblExcludeSubcatSubcat> TblExcludeSubcatSubcat { get; set; }
        public virtual DbSet<TblFacilities> TblFacilities { get; set; }
        public virtual DbSet<TblFavorites> TblFavorites { get; set; }
        public virtual DbSet<TblGlCodes> TblGlCodes { get; set; }
        public virtual DbSet<TblHospices> TblHospices { get; set; }
        public virtual DbSet<TblInventory> TblInventory { get; set; }
        public virtual DbSet<TblInventoryAdds> TblInventoryAdds { get; set; }
        public virtual DbSet<TblInventoryAdds61015> TblInventoryAdds61015 { get; set; }
        public virtual DbSet<TblInventoryAdds772015> TblInventoryAdds772015 { get; set; }
        public virtual DbSet<TblInventoryBakOld> TblInventoryBakOld { get; set; }
        public virtual DbSet<TblInventoryBundleLinks> TblInventoryBundleLinks { get; set; }
        public virtual DbSet<TblInventoryBundles> TblInventoryBundles { get; set; }
        public virtual DbSet<TblInventoryIssues> TblInventoryIssues { get; set; }
        public virtual DbSet<TblInventoryIssues61015> TblInventoryIssues61015 { get; set; }
        public virtual DbSet<TblInventoryIssues61215> TblInventoryIssues61215 { get; set; }
        public virtual DbSet<TblInventoryIssues772015> TblInventoryIssues772015 { get; set; }
        public virtual DbSet<TblInventoryIssuesCopy> TblInventoryIssuesCopy { get; set; }
        public virtual DbSet<TblInventoryIssuesCopy1> TblInventoryIssuesCopy1 { get; set; }
        public virtual DbSet<TblInventoryIssuesCopy2> TblInventoryIssuesCopy2 { get; set; }
        public virtual DbSet<TblInventoryIssuesCopy3> TblInventoryIssuesCopy3 { get; set; }
        public virtual DbSet<TblInventoryLoadIssues> TblInventoryLoadIssues { get; set; }
        public virtual DbSet<TblInventoryPreList> TblInventoryPreList { get; set; }
        public virtual DbSet<TblInventoryQuantities> TblInventoryQuantities { get; set; }
        public virtual DbSet<TblInventoryQuantities61015> TblInventoryQuantities61015 { get; set; }
        public virtual DbSet<TblInventoryQuantities61115> TblInventoryQuantities61115 { get; set; }
        public virtual DbSet<TblInventoryQuantities61215> TblInventoryQuantities61215 { get; set; }
        public virtual DbSet<TblInventoryQuantitiesCopy> TblInventoryQuantitiesCopy { get; set; }
        public virtual DbSet<TblInventoryStatus> TblInventoryStatus { get; set; }
        public virtual DbSet<TblInventoryTransfers> TblInventoryTransfers { get; set; }
        public virtual DbSet<TblInventoryTransfers6102015> TblInventoryTransfers6102015 { get; set; }
        public virtual DbSet<TblInventoryTransfersIntra> TblInventoryTransfersIntra { get; set; }
        public virtual DbSet<TblInventoryTransfersIntra6102105> TblInventoryTransfersIntra6102105 { get; set; }
        public virtual DbSet<TblInventoryTransfersItems> TblInventoryTransfersItems { get; set; }
        public virtual DbSet<TblInventoryTransfersItems6102105> TblInventoryTransfersItems6102105 { get; set; }
        public virtual DbSet<TblInventoryVendorPricing> TblInventoryVendorPricing { get; set; }
        public virtual DbSet<TblInventoryWriteOffs> TblInventoryWriteOffs { get; set; }
        public virtual DbSet<TblInventoryWriteOffs6115> TblInventoryWriteOffs6115 { get; set; }
        public virtual DbSet<TblInventoryWriteOffsCopy> TblInventoryWriteOffsCopy { get; set; }
        public virtual DbSet<TblInvoiceFiles> TblInvoiceFiles { get; set; }
        public virtual DbSet<TblInvoices> TblInvoices { get; set; }
        public virtual DbSet<TblInvoicesCopy> TblInvoicesCopy { get; set; }
        public virtual DbSet<TblInvoicesCopy1> TblInvoicesCopy1 { get; set; }
        public virtual DbSet<TblInvoicesLocation> TblInvoicesLocation { get; set; }
        public virtual DbSet<TblInvoicesLocationCopy> TblInvoicesLocationCopy { get; set; }
        public virtual DbSet<TblInvoicesLocationCopy1> TblInvoicesLocationCopy1 { get; set; }
        public virtual DbSet<TblInvoicesLocationCopy2> TblInvoicesLocationCopy2 { get; set; }
        public virtual DbSet<TblInvoicesLocationold> TblInvoicesLocationold { get; set; }
        public virtual DbSet<TblInvoicesLocationolder> TblInvoicesLocationolder { get; set; }
        public virtual DbSet<TblItemsFiles> TblItemsFiles { get; set; }
        public virtual DbSet<TblLocationInvQty> TblLocationInvQty { get; set; }
        public virtual DbSet<TblLocationInventory> TblLocationInventory { get; set; }
        public virtual DbSet<TblLocationMapping> TblLocationMapping { get; set; }
        public virtual DbSet<TblLocations> TblLocations { get; set; }
        public virtual DbSet<TblLoginHistory> TblLoginHistory { get; set; }
        public virtual DbSet<TblMaintenanceNotes> TblMaintenanceNotes { get; set; }
        public virtual DbSet<TblMessageViews> TblMessageViews { get; set; }
        public virtual DbSet<TblMessages> TblMessages { get; set; }
        public virtual DbSet<TblMessagesCopy> TblMessagesCopy { get; set; }
        public virtual DbSet<TblNurses> TblNurses { get; set; }
        public virtual DbSet<TblOffCapSummary> TblOffCapSummary { get; set; }
        public virtual DbSet<TblOrderStats> TblOrderStats { get; set; }
        public virtual DbSet<TblOrderUpdates> TblOrderUpdates { get; set; }
        public virtual DbSet<TblOrders> TblOrders { get; set; }
        public virtual DbSet<TblOrdersCopy1> TblOrdersCopy1 { get; set; }
        public virtual DbSet<TblOrdersVersions> TblOrdersVersions { get; set; }
        public virtual DbSet<TblPatientBilling> TblPatientBilling { get; set; }
        public virtual DbSet<TblPatientBillingGaps> TblPatientBillingGaps { get; set; }
        public virtual DbSet<TblPatientDeliveryCharges> TblPatientDeliveryCharges { get; set; }
        public virtual DbSet<TblPatientMoves> TblPatientMoves { get; set; }
        public virtual DbSet<TblPatientRespite> TblPatientRespite { get; set; }
        public virtual DbSet<TblPatientUpdates> TblPatientUpdates { get; set; }
        public virtual DbSet<TblPatients> TblPatients { get; set; }
        public virtual DbSet<TblPhysicalInventory> TblPhysicalInventory { get; set; }
        public virtual DbSet<TblPhysicalInventory71015> TblPhysicalInventory71015 { get; set; }
        public virtual DbSet<TblPhysicalInventoryCompleted> TblPhysicalInventoryCompleted { get; set; }
        public virtual DbSet<TblPhysicalInventoryCompletedCopy> TblPhysicalInventoryCompletedCopy { get; set; }
        public virtual DbSet<TblPhysicalInventoryCompletedCopy1> TblPhysicalInventoryCompletedCopy1 { get; set; }
        public virtual DbSet<TblPhysicalInventoryCompletedCopy2> TblPhysicalInventoryCompletedCopy2 { get; set; }
        public virtual DbSet<TblPhysicalInventoryCompletedCopy3> TblPhysicalInventoryCompletedCopy3 { get; set; }
        public virtual DbSet<TblPhysicalInventoryCopy> TblPhysicalInventoryCopy { get; set; }
        public virtual DbSet<TblPhysicalInventoryCopy1> TblPhysicalInventoryCopy1 { get; set; }
        public virtual DbSet<TblPhysicalInventoryCopy2> TblPhysicalInventoryCopy2 { get; set; }
        public virtual DbSet<TblPhysicalInventoryCopy3> TblPhysicalInventoryCopy3 { get; set; }
        public virtual DbSet<TblPhysicalInventoryVern20160922> TblPhysicalInventoryVern20160922 { get; set; }
        public virtual DbSet<TblPiIssues> TblPiIssues { get; set; }
        public virtual DbSet<TblPoDeleteItems> TblPoDeleteItems { get; set; }
        public virtual DbSet<TblProducts> TblProducts { get; set; }
        public virtual DbSet<TblPurchaseOrderDocuments> TblPurchaseOrderDocuments { get; set; }
        public virtual DbSet<TblPurchaseOrderItems> TblPurchaseOrderItems { get; set; }
        public virtual DbSet<TblPurchaseOrderItemsReceived> TblPurchaseOrderItemsReceived { get; set; }
        public virtual DbSet<TblPurchaseOrderItemsReceivedCopy> TblPurchaseOrderItemsReceivedCopy { get; set; }
        public virtual DbSet<TblPurchaseOrderNotes> TblPurchaseOrderNotes { get; set; }
        public virtual DbSet<TblPurchaseOrders> TblPurchaseOrders { get; set; }
        public virtual DbSet<TblQaCalls> TblQaCalls { get; set; }
        public virtual DbSet<TblResponses> TblResponses { get; set; }
        public virtual DbSet<TblResponsesOld> TblResponsesOld { get; set; }
        public virtual DbSet<TblSerialNumbers> TblSerialNumbers { get; set; }
        public virtual DbSet<TblSerialNumbers61015> TblSerialNumbers61015 { get; set; }
        public virtual DbSet<TblSerialNumbers6112015> TblSerialNumbers6112015 { get; set; }
        public virtual DbSet<TblSerialNumbers61215> TblSerialNumbers61215 { get; set; }
        public virtual DbSet<TblSerialNumbers61315> TblSerialNumbers61315 { get; set; }
        public virtual DbSet<TblSubCategories> TblSubCategories { get; set; }
        public virtual DbSet<TblTagExceptions> TblTagExceptions { get; set; }
        public virtual DbSet<TblTicketSettings> TblTicketSettings { get; set; }
        public virtual DbSet<TblTickets> TblTickets { get; set; }
        public virtual DbSet<TblTicketsCopy> TblTicketsCopy { get; set; }
        public virtual DbSet<TblTicketsCopy1> TblTicketsCopy1 { get; set; }
        public virtual DbSet<TblTicketsCopy2> TblTicketsCopy2 { get; set; }
        public virtual DbSet<TblTicketsOld> TblTicketsOld { get; set; }
        public virtual DbSet<TblUnadjusted> TblUnadjusted { get; set; }
        public virtual DbSet<TblUnadjustedForecast> TblUnadjustedForecast { get; set; }
        public virtual DbSet<TblUnadjustedForecastCopy> TblUnadjustedForecastCopy { get; set; }
        public virtual DbSet<TblUsers> TblUsers { get; set; }
        public virtual DbSet<TblUsers2> TblUsers2 { get; set; }
        public virtual DbSet<TblUsersCopy> TblUsersCopy { get; set; }
        public virtual DbSet<TblUsersCopy1> TblUsersCopy1 { get; set; }
        public virtual DbSet<TblVehicles> TblVehicles { get; set; }
        public virtual DbSet<TblVendors> TblVendors { get; set; }
        public virtual DbSet<TblZipToLocation> TblZipToLocation { get; set; }
        public virtual DbSet<TblZipToLocationCopy> TblZipToLocationCopy { get; set; }
        public virtual DbSet<TblZipToLocationCopy1> TblZipToLocationCopy1 { get; set; }
        public virtual DbSet<TblZipToLocationCopy2> TblZipToLocationCopy2 { get; set; }
        public virtual DbSet<TblZipToLocationCopy3> TblZipToLocationCopy3 { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssetTagMaster>(entity =>
            {
                entity.HasKey(e => e.AtagId)
                    .HasName("PRIMARY");

                entity.ToTable("asset_tag_master");

                entity.Property(e => e.AtagId)
                    .HasColumnName("atagID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.ImproperTag)
                    .HasColumnName("improper_tag")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OtherProcessed)
                    .HasColumnName("other_processed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PiProcessed)
                    .HasColumnName("pi_processed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PoProcessed)
                    .HasColumnName("po_processed")
                    .HasColumnType("int(1)");
            });

            modelBuilder.Entity<DataTranslation>(entity =>
            {
                entity.ToTable("data_translation");

                entity.HasIndex(e => e.Name)
                    .HasName("name")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DataSource)
                    .IsRequired()
                    .HasColumnName("data_source")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DataType)
                    .IsRequired()
                    .HasColumnName("data_type")
                    .HasColumnType("varchar(63)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DateUploaded)
                    .HasColumnName("date_uploaded")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<DataTranslationCopy1>(entity =>
            {
                entity.ToTable("data_translation_copy1");

                entity.HasIndex(e => e.Name)
                    .HasName("name")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DataSource)
                    .IsRequired()
                    .HasColumnName("data_source")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DataType)
                    .IsRequired()
                    .HasColumnName("data_type")
                    .HasColumnType("varchar(63)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DateUploaded)
                    .HasColumnName("date_uploaded")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Division>(entity =>
            {
                entity.ToTable("division");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Division1)
                    .HasColumnName("division")
                    .HasColumnType("char(2)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.DivisionName)
                    .HasColumnName("division_name")
                    .HasColumnType("char(60)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<Hospice>(entity =>
            {
                entity.ToTable("hospice");

                entity.HasIndex(e => e.Class)
                    .HasName("class");

                entity.HasIndex(e => e.Division)
                    .HasName("division");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Class)
                    .HasColumnName("class")
                    .HasColumnType("char(2)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.ContactName)
                    .HasColumnName("contactName")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.ContactNumber)
                    .HasColumnName("contactNumber")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Division)
                    .HasColumnName("division")
                    .HasColumnType("char(2)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.RegionId)
                    .HasColumnName("regionID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<HospiceBranchMap>(entity =>
            {
                entity.ToTable("hospice_branch_map");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.HmsId)
                    .HasColumnName("hms_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.HmsName)
                    .IsRequired()
                    .HasColumnName("hms_name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.SourceId)
                    .HasColumnName("source_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SourceName)
                    .IsRequired()
                    .HasColumnName("source_name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.ToTable("inventory");

                entity.HasIndex(e => e.Disposable)
                    .HasName("disposable");

                entity.HasIndex(e => e.Inactive)
                    .HasName("inactive");

                entity.HasIndex(e => e.InvSerial)
                    .HasName("inv_serial");

                entity.HasIndex(e => e.Supply)
                    .HasName("supply");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Concentrator)
                    .HasColumnName("concentrator")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DefaultRentPrice)
                    .HasColumnName("default_rent_price")
                    .HasColumnType("decimal(20,2)");

                entity.Property(e => e.DefaultSalePrice)
                    .HasColumnName("default_sale_price")
                    .HasColumnType("decimal(20,2)");

                entity.Property(e => e.Disposable)
                    .HasColumnName("disposable")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Enteral)
                    .HasColumnName("enteral")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Hidden)
                    .HasColumnName("hidden")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Inactive)
                    .HasColumnName("inactive")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.InvDesc1)
                    .HasColumnName("inv_desc_1")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.InvDesc2)
                    .HasColumnName("inv_desc_2")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.InvSerial)
                    .HasColumnName("inv_serial")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvTank)
                    .HasColumnName("inv_tank")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvTax)
                    .HasColumnName("inv_tax")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MedCode)
                    .HasColumnName("med_code")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Supply)
                    .HasColumnName("supply")
                    .HasColumnType("int(11)");

                entity.Property(e => e.VQuantity)
                    .HasColumnName("v_quantity")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<InventoryMap>(entity =>
            {
                entity.ToTable("inventory_map");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedTimestamp)
                    .HasColumnName("created_timestamp")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.HmsInventoryCode)
                    .IsRequired()
                    .HasColumnName("hms_inventory_code")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnName("source")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.SourceInventoryCode)
                    .IsRequired()
                    .HasColumnName("source_inventory_code")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<InventoryMapCopy1>(entity =>
            {
                entity.ToTable("inventory_map_copy1");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedTimestamp)
                    .HasColumnName("created_timestamp")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.HmsInventoryCode)
                    .IsRequired()
                    .HasColumnName("hms_inventory_code")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnName("source")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.SourceInventoryCode)
                    .IsRequired()
                    .HasColumnName("source_inventory_code")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<InventoryPmcs>(entity =>
            {
                entity.ToTable("inventory_pmcs");

                entity.HasIndex(e => e.SerialNumber)
                    .HasName("serial_number")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LastMaintenance)
                    .HasColumnName("last_maintenance")
                    .HasColumnType("datetime");

                entity.Property(e => e.SerialNumber)
                    .IsRequired()
                    .HasColumnName("serial_number")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<InventoryPmcsLog>(entity =>
            {
                entity.ToTable("inventory_pmcs_log");

                entity.HasIndex(e => e.InventoryPmcsId)
                    .HasName("fk_inventory_pmcs_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InventoryPmcsId)
                    .HasColumnName("inventory_pmcs_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OldLastMaintenance)
                    .HasColumnName("old_last_maintenance")
                    .HasColumnType("datetime");

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.InventoryPmcs)
                    .WithMany(p => p.InventoryPmcsLog)
                    .HasForeignKey(d => d.InventoryPmcsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_inventory_pmcs_id");
            });

            modelBuilder.Entity<Inventoryserialnumbers>(entity =>
            {
                entity.ToTable("inventoryserialnumbers");

                entity.HasIndex(e => e.Division)
                    .HasName("division");

                entity.HasIndex(e => e.InvId)
                    .HasName("inv_ID");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CurAcctNumber)
                    .HasColumnName("cur_acct_number")
                    .HasColumnType("varchar(80)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.CurHours)
                    .HasColumnName("cur_hours")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Da)
                    .HasColumnName("DA")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Division)
                    .HasColumnName("division")
                    .HasColumnType("char(2)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.InvId)
                    .HasColumnName("inv_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LastAcctNumber)
                    .HasColumnName("last_acct_number")
                    .HasColumnType("varchar(80)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.LastEdit)
                    .HasColumnName("last_edit")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.MaintDue)
                    .HasColumnName("maint_due")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Manufacture)
                    .HasColumnName("manufacture")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.MfgSerialNumber)
                    .HasColumnName("mfg_serial_number")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.PreMaintDue)
                    .HasColumnName("pre_maint_due")
                    .HasColumnType("date");

                entity.Property(e => e.SerialNumber)
                    .HasColumnName("serial_number")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("char(1)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<LinkPatientProducts>(entity =>
            {
                entity.HasKey(e => e.LinkId)
                    .HasName("PRIMARY");

                entity.ToTable("link_patient_products");

                entity.HasIndex(e => e.AssetTag)
                    .HasName("ft_asset_tag");

                entity.HasIndex(e => e.Delivered)
                    .HasName("delivered");

                entity.HasIndex(e => e.OrderId)
                    .HasName("orderID");

                entity.HasIndex(e => e.PatientId)
                    .HasName("patientID");

                entity.HasIndex(e => e.ProductId)
                    .HasName("productID");

                entity.Property(e => e.LinkId)
                    .HasColumnName("linkID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetStatus)
                    .HasColumnName("asset_status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.AssetTag)
                    .IsRequired()
                    .HasColumnName("asset_tag")
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.AssetTagExchange)
                    .IsRequired()
                    .HasColumnName("asset_tag_exchange")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.AssetTagPickup)
                    .HasColumnName("asset_tag_pickup")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Delivered)
                    .HasColumnName("delivered")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DeliveredTimestamp)
                    .HasColumnName("delivered_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.DmeServiced)
                    .HasColumnName("dme_serviced")
                    .HasColumnType("int(1)");

                entity.Property(e => e.EmailCheckProcessed)
                    .HasColumnName("email_check_processed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ExchangeId)
                    .HasColumnName("exchangeID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InventoryProcessed)
                    .HasColumnName("inventory_processed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.IssueNotification)
                    .HasColumnName("issue_notification")
                    .HasColumnType("int(1)");

                entity.Property(e => e.LotNum)
                    .HasColumnName("lot_num")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OffCapCost)
                    .HasColumnName("off_cap_cost")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.OffCapItem)
                    .HasColumnName("off_cap_item")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OffCapUser)
                    .HasColumnName("off_cap_user")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Pickedup)
                    .HasColumnName("pickedup")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PickupTimestamp)
                    .HasColumnName("pickup_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.ProductId)
                    .HasColumnName("productID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RespiteId)
                    .HasColumnName("respiteID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<LinkPatientProductsCopy1>(entity =>
            {
                entity.HasKey(e => e.LinkId)
                    .HasName("PRIMARY");

                entity.ToTable("link_patient_products_copy1");

                entity.HasIndex(e => e.AssetTag)
                    .HasName("ft_asset_tag");

                entity.HasIndex(e => e.Delivered)
                    .HasName("delivered");

                entity.HasIndex(e => e.OrderId)
                    .HasName("orderID");

                entity.HasIndex(e => e.PatientId)
                    .HasName("patientID");

                entity.HasIndex(e => e.ProductId)
                    .HasName("productID");

                entity.Property(e => e.LinkId)
                    .HasColumnName("linkID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetStatus)
                    .HasColumnName("asset_status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.AssetTag)
                    .IsRequired()
                    .HasColumnName("asset_tag")
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.AssetTagExchange)
                    .IsRequired()
                    .HasColumnName("asset_tag_exchange")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.AssetTagPickup)
                    .HasColumnName("asset_tag_pickup")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Delivered)
                    .HasColumnName("delivered")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DeliveredTimestamp)
                    .HasColumnName("delivered_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.DmeServiced)
                    .HasColumnName("dme_serviced")
                    .HasColumnType("int(1)");

                entity.Property(e => e.EmailCheckProcessed)
                    .HasColumnName("email_check_processed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ExchangeId)
                    .HasColumnName("exchangeID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InventoryProcessed)
                    .HasColumnName("inventory_processed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.IssueNotification)
                    .HasColumnName("issue_notification")
                    .HasColumnType("int(1)");

                entity.Property(e => e.LotNum)
                    .HasColumnName("lot_num")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OffCapCost)
                    .HasColumnName("off_cap_cost")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.OffCapItem)
                    .HasColumnName("off_cap_item")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OffCapUser)
                    .HasColumnName("off_cap_user")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Pickedup)
                    .HasColumnName("pickedup")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PickupTimestamp)
                    .HasColumnName("pickup_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.ProductId)
                    .HasColumnName("productID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RespiteId)
                    .HasColumnName("respiteID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Logging>(entity =>
            {
                entity.ToTable("logging");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Created)
                    .HasColumnName("created")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Instance)
                    .HasColumnName("instance")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnName("message")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnName("source")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<MailingListEmails>(entity =>
            {
                entity.ToTable("mailing_list_emails");

                entity.HasIndex(e => e.MailingListId)
                    .HasName("mailing_list_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.MailingListId)
                    .HasColumnName("mailing_list_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.MailingList)
                    .WithMany(p => p.MailingListEmails)
                    .HasForeignKey(d => d.MailingListId)
                    .HasConstraintName("mailing_list_emails_ibfk_1");
            });

            modelBuilder.Entity<MailingLists>(entity =>
            {
                entity.ToTable("mailing_lists");

                entity.HasIndex(e => e.Constant)
                    .HasName("constant")
                    .IsUnique();

                entity.HasIndex(e => e.Name)
                    .HasName("name")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Constant)
                    .IsRequired()
                    .HasColumnName("constant")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<Markers>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("markers");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Lat)
                    .HasColumnName("lat")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Long)
                    .HasColumnName("long")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Zip)
                    .HasColumnName("zip")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<MarkersDispatch>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("markers_dispatch");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Lat)
                    .HasColumnName("lat")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Long)
                    .HasColumnName("long")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Zip)
                    .HasColumnName("zip")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<MarkersPassages>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("markers_passages");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Lat)
                    .HasColumnName("lat")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Long)
                    .HasColumnName("long")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Zip)
                    .HasColumnName("zip")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<MarkersUpdate>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("markers_update");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Lat)
                    .HasColumnName("lat")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Long)
                    .HasColumnName("long")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Zip)
                    .HasColumnName("zip")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<Messages>(entity =>
            {
                entity.ToTable("messages");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasColumnName("key")
                    .HasColumnType("char(20)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Text)
                    .HasColumnName("text")
                    .HasColumnType("varchar(300)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<OrderMap>(entity =>
            {
                entity.ToTable("order_map");

                entity.HasIndex(e => e.HmsOrderNumber)
                    .HasName("hms_order_number_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.HmsOrderNumber)
                    .HasColumnName("hms_order_number")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnName("source")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.SourceOrderNumber)
                    .HasColumnName("source_order_number")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<PassagesFacilities>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("passages_facilities");

                entity.Property(e => e.AC)
                    .HasColumnName("A/C")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Address)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Aide)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.City)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Clergy)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.County)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.IdtNurse)
                    .HasColumnName("IDT Nurse")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Md)
                    .HasColumnName("MD")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.MdLia)
                    .HasColumnName("MD Lia")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Name)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Owner)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Phone)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PhyLia)
                    .HasColumnName("Phy Lia")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.R)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Rn)
                    .HasColumnName("RN")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.S)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Sw)
                    .HasColumnName("SW")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Zip)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<PassagesPatients>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("passages_patients");

                entity.Property(e => e.County)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Facility)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.FirstName)
                    .HasColumnName("First Name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.HsArea)
                    .HasColumnName("HS area")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LastName)
                    .HasColumnName("Last Name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Patient)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Section)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Status)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("patient");

                entity.HasIndex(e => e.AccountNumber)
                    .HasName("accountNumberIndex");

                entity.HasIndex(e => e.AcctNumber)
                    .HasName("acct_numberIndex");

                entity.HasIndex(e => e.Class)
                    .HasName("classIndex");

                entity.HasIndex(e => e.Dc)
                    .HasName("dc");

                entity.HasIndex(e => e.Division)
                    .HasName("divisionIndex");

                entity.HasIndex(e => e.Firstname)
                    .HasName("firstname");

                entity.HasIndex(e => e.HospiceId)
                    .HasName("hospiceID");

                entity.HasIndex(e => e.Lastname)
                    .HasName("lastnameIndex");

                entity.HasIndex(e => e.LocationId)
                    .HasName("locationID");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AccountNumber)
                    .HasColumnName("accountNumber")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.AcctNumber)
                    .IsRequired()
                    .HasColumnName("acct_number")
                    .HasColumnType("varchar(40)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasColumnName("address_1")
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Address2)
                    .IsRequired()
                    .HasColumnName("address_2")
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.CTaxCode)
                    .IsRequired()
                    .HasColumnName("c_tax_code")
                    .HasColumnType("varchar(5)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Class)
                    .HasColumnName("class")
                    .HasColumnType("char(5)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.CoTaxCode)
                    .HasColumnName("co_tax_code")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Comment1)
                    .HasColumnName("comment_1")
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Comment2)
                    .HasColumnName("comment_2")
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Comment3)
                    .HasColumnName("comment_3")
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Comment4)
                    .HasColumnName("comment_4")
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Cpap)
                    .HasColumnName("cpap")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("date");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Dc)
                    .HasColumnName("dc")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DcDate)
                    .HasColumnName("dc_date")
                    .HasColumnType("date");

                entity.Property(e => e.DefaultDme)
                    .HasColumnName("default_dme")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Diagnosis)
                    .HasColumnName("diagnosis")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Division)
                    .HasColumnName("division")
                    .HasColumnType("char(5)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.DmeServiced)
                    .HasColumnName("dme_serviced")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DoNotCall)
                    .HasColumnName("do_not_call")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DobDay)
                    .HasColumnName("dob_day")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.DobMonth)
                    .HasColumnName("dob_month")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.DobYear)
                    .HasColumnName("dob_year")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Epap)
                    .HasColumnName("epap")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.FacilityId)
                    .HasColumnName("facilityID")
                    .HasColumnType("int(10)");

                entity.Property(e => e.FacilityName)
                    .HasColumnName("facility_name")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Firstname)
                    .HasColumnName("firstname")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.HeightFeet)
                    .HasColumnName("height_feet")
                    .HasColumnType("int(3)");

                entity.Property(e => e.HeightInches)
                    .HasColumnName("height_inches")
                    .HasColumnType("int(3)");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Inactive)
                    .HasColumnName("inactive")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Indigent)
                    .HasColumnName("indigent")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Ipap)
                    .HasColumnName("ipap")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.IsPediatric).HasColumnName("is_pediatric");

                entity.Property(e => e.LastChanged)
                    .HasColumnName("last_changed")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Lastname)
                    .HasColumnName("lastname")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationType)
                    .HasColumnName("location_type")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.MInitial)
                    .HasColumnName("m_initial")
                    .HasColumnType("char(1)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.MapId)
                    .HasColumnName("map_id")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Nurse)
                    .HasColumnName("nurse")
                    .HasColumnType("varchar(80)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.NursingHome)
                    .HasColumnName("nursing_home")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.O2Order)
                    .HasColumnName("o2_order")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.OtherContact)
                    .HasColumnName("other_contact")
                    .HasColumnType("varchar(250)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.PatientHeight)
                    .HasColumnName("patient_height")
                    .HasColumnType("int(5)");

                entity.Property(e => e.PatientWeight)
                    .HasColumnName("patient_weight")
                    .HasColumnType("int(5)");

                entity.Property(e => e.Phone1)
                    .HasColumnName("phone_1")
                    .HasColumnType("varchar(12)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Phone2)
                    .HasColumnName("phone_2")
                    .HasColumnType("varchar(12)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.PreviousDcDate)
                    .HasColumnName("previous_dc_date")
                    .HasColumnType("date");

                entity.Property(e => e.STaxCode)
                    .HasColumnName("s_tax_code")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Taxable)
                    .HasColumnName("taxable")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Team)
                    .HasColumnName("team")
                    .HasColumnType("varchar(80)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Zip)
                    .HasColumnName("zip")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<PatientHms>(entity =>
            {
                entity.ToTable("patient_hms");

                entity.HasIndex(e => e.AccountNumber)
                    .HasName("accountNumberIndex");

                entity.HasIndex(e => e.AcctNumber)
                    .HasName("acct_numberIndex");

                entity.HasIndex(e => e.Class)
                    .HasName("classIndex");

                entity.HasIndex(e => e.CustomerId)
                    .HasName("customerID");

                entity.HasIndex(e => e.Dc)
                    .HasName("dc");

                entity.HasIndex(e => e.DcDate)
                    .HasName("dc_date");

                entity.HasIndex(e => e.DefaultDme)
                    .HasName("dme");

                entity.HasIndex(e => e.Division)
                    .HasName("divisionIndex");

                entity.HasIndex(e => e.DmeServiced)
                    .HasName("dme_serviced");

                entity.HasIndex(e => e.Firstname)
                    .HasName("ft_firstname");

                entity.HasIndex(e => e.HospiceId)
                    .HasName("hospiceID");

                entity.HasIndex(e => e.Lastname)
                    .HasName("lastnameIndex");

                entity.HasIndex(e => e.LocationId)
                    .HasName("locationID");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AccountNumber)
                    .HasColumnName("accountNumber")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.AcctNumber)
                    .IsRequired()
                    .HasColumnName("acct_number")
                    .HasColumnType("varchar(40)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasColumnName("address_1")
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Address2)
                    .IsRequired()
                    .HasColumnName("address_2")
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.BipapRate)
                    .HasColumnName("bipap_rate")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CTaxCode)
                    .IsRequired()
                    .HasColumnName("c_tax_code")
                    .HasColumnType("varchar(5)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Class)
                    .HasColumnName("class")
                    .HasColumnType("char(5)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.CoTaxCode)
                    .HasColumnName("co_tax_code")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Comment1)
                    .HasColumnName("comment_1")
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Comment2)
                    .HasColumnName("comment_2")
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Comment3)
                    .HasColumnName("comment_3")
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Comment4)
                    .HasColumnName("comment_4")
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Cpap)
                    .HasColumnName("cpap")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("date");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Dc)
                    .HasColumnName("dc")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DcDate)
                    .HasColumnName("dc_date")
                    .HasColumnType("date");

                entity.Property(e => e.DefaultDme)
                    .HasColumnName("default_dme")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Devnotes)
                    .HasColumnName("devnotes")
                    .HasColumnType("text")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Diagnosis)
                    .HasColumnName("diagnosis")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Division)
                    .HasColumnName("division")
                    .HasColumnType("char(5)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.DmeServiced)
                    .HasColumnName("dme_serviced")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DoNotCall)
                    .HasColumnName("do_not_call")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DobDay)
                    .HasColumnName("dob_day")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.DobMonth)
                    .HasColumnName("dob_month")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.DobYear)
                    .HasColumnName("dob_year")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.EmergencyOptout)
                    .HasColumnName("emergency_optout")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Epap)
                    .HasColumnName("epap")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.FacilityId)
                    .HasColumnName("facilityID")
                    .HasColumnType("int(10)");

                entity.Property(e => e.FacilityName)
                    .HasColumnName("facility_name")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Firstname)
                    .HasColumnName("firstname")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.GeoCoded)
                    .HasColumnName("geo_coded")
                    .HasColumnType("int(1)");

                entity.Property(e => e.HeightFeet)
                    .HasColumnName("height_feet")
                    .HasColumnType("int(3)");

                entity.Property(e => e.HeightInches)
                    .HasColumnName("height_inches")
                    .HasColumnType("int(3)");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Inactive)
                    .HasColumnName("inactive")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Indigent)
                    .HasColumnName("indigent")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Ipap)
                    .HasColumnName("ipap")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.IsPediatric).HasColumnName("is_pediatric");

                entity.Property(e => e.LastChanged)
                    .HasColumnName("last_changed")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Lastname)
                    .HasColumnName("lastname")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationType)
                    .HasColumnName("location_type")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.MInitial)
                    .HasColumnName("m_initial")
                    .HasColumnType("char(1)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.MapId)
                    .HasColumnName("map_id")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Nurse)
                    .HasColumnName("nurse")
                    .HasColumnType("varchar(80)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.NursingHome)
                    .HasColumnName("nursing_home")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.O2Order)
                    .HasColumnName("o2_order")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.OtherContact)
                    .HasColumnName("other_contact")
                    .HasColumnType("varchar(250)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.PassagesPatientId)
                    .HasColumnName("passages_patientID")
                    .HasColumnType("int(8)");

                entity.Property(e => e.PatientHeight)
                    .HasColumnName("patient_height")
                    .HasColumnType("int(5)");

                entity.Property(e => e.PatientWeight)
                    .HasColumnName("patient_weight")
                    .HasColumnType("int(5)");

                entity.Property(e => e.Phone1)
                    .HasColumnName("phone_1")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Phone2)
                    .HasColumnName("phone_2")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.PreviousDcDate)
                    .HasColumnName("previous_dc_date")
                    .HasColumnType("date");

                entity.Property(e => e.STaxCode)
                    .HasColumnName("s_tax_code")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Taxable)
                    .HasColumnName("taxable")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Team)
                    .HasColumnName("team")
                    .HasColumnType("varchar(80)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Zip)
                    .HasColumnName("zip")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<PatientMap>(entity =>
            {
                entity.ToTable("patient_map");

                entity.HasIndex(e => e.HmsIdNumber)
                    .HasName("hms_id_number_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.HmsIdNumber)
                    .HasColumnName("hms_id_number")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnName("source")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.SourceIdNumber)
                    .HasColumnName("source_id_number")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<PatientMapCopy1>(entity =>
            {
                entity.ToTable("patient_map_copy1");

                entity.HasIndex(e => e.HmsIdNumber)
                    .HasName("hms_id_number_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.HmsIdNumber)
                    .HasColumnName("hms_id_number")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnName("source")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.SourceIdNumber)
                    .HasColumnName("source_id_number")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Patientlineitems>(entity =>
            {
                entity.ToTable("patientlineitems");

                entity.HasIndex(e => e.CreateDate)
                    .HasName("create_dateIndex");

                entity.HasIndex(e => e.DeliveryTs)
                    .HasName("delivery_tsIndex");

                entity.HasIndex(e => e.InvId)
                    .HasName("inv_IDIndex");

                entity.HasIndex(e => e.PatientId)
                    .HasName("patientIDIndex");

                entity.HasIndex(e => e.PickupReqTs)
                    .HasName("pickup_req_ts");

                entity.HasIndex(e => e.PickupTs)
                    .HasName("pickup_tsIndex");

                entity.HasIndex(e => e.SnId)
                    .HasName("sn_ID");

                entity.HasIndex(e => e.Status)
                    .HasName("status");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("date");

                entity.Property(e => e.CreateTs)
                    .HasColumnName("create_ts")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeliveryTs)
                    .HasColumnName("delivery_ts")
                    .HasColumnType("datetime");

                entity.Property(e => e.HistoryArchive)
                    .HasColumnName("historyArchive")
                    .HasColumnType("char(1)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Instructions)
                    .HasColumnName("instructions")
                    .HasColumnType("varchar(10000)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.InvId)
                    .HasColumnName("inv_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LastChanged)
                    .HasColumnName("last_changed")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.LotNumber)
                    .HasColumnName("lot_number")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Lpm)
                    .HasColumnName("LPM")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OrderedBy)
                    .HasColumnName("ordered_by")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Other)
                    .HasColumnName("other")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PatientOrdersId)
                    .HasColumnName("patientOrdersID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PickupCode)
                    .HasColumnName("pickup_code")
                    .HasColumnType("char(1)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.PickupOrderedBy)
                    .HasColumnName("pickup_ordered_by")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PickupReqTs)
                    .HasColumnName("pickup_req_ts")
                    .HasColumnType("datetime");

                entity.Property(e => e.PickupTs)
                    .HasColumnName("pickup_ts")
                    .HasColumnType("datetime");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("decimal(20,2)");

                entity.Property(e => e.SnId)
                    .HasColumnName("sn_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Patientorders>(entity =>
            {
                entity.ToTable("patientorders");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Completed)
                    .HasColumnName("completed")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("date");

                entity.Property(e => e.Instructions)
                    .HasColumnName("instructions")
                    .HasColumnType("varchar(10000)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.LastChanged)
                    .HasColumnName("last_changed")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.OrderedBy)
                    .HasColumnName("ordered_by")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<PermissionsRestrictedPages>(entity =>
            {
                entity.ToTable("permissions_restricted_pages");

                entity.HasIndex(e => e.PageName)
                    .HasName("page_name")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DefaultAccess)
                    .HasColumnName("default_access")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PageName)
                    .IsRequired()
                    .HasColumnName("page_name")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<PermissionsUsers>(entity =>
            {
                entity.ToTable("permissions_users");

                entity.HasIndex(e => new { e.UserId, e.PermissionType, e.PermissionValue })
                    .HasName("user_type_value_index")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PermissionType)
                    .HasColumnName("permission_type")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.PermissionValue)
                    .HasColumnName("permission_value")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<PickupStaging>(entity =>
            {
                entity.ToTable("pickup_staging");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Address2)
                    .HasColumnName("address2")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DeliveryCompleted)
                    .HasColumnName("delivery_completed")
                    .HasColumnType("timestamp");

                entity.Property(e => e.DeliveryDispatched)
                    .HasColumnName("delivery_dispatched")
                    .HasColumnType("timestamp");

                entity.Property(e => e.DeliveryId)
                    .HasColumnName("deliveryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DeliveryOrdered)
                    .HasColumnName("delivery_ordered")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasColumnName("firstname")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.HmsHospiceId)
                    .HasColumnName("hms_hospice_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.HmsPatientId)
                    .HasColumnName("hms_patient_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.HospiceName)
                    .IsRequired()
                    .HasColumnName("hospice_name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasColumnName("lastname")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.PickupCompleted)
                    .HasColumnName("pickup_completed")
                    .HasColumnType("timestamp");

                entity.Property(e => e.PickupDispatched)
                    .HasColumnName("pickup_dispatched")
                    .HasColumnType("timestamp");

                entity.Property(e => e.PickupId)
                    .HasColumnName("pickupID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PickupOrdered)
                    .HasColumnName("pickup_ordered")
                    .HasColumnType("timestamp");

                entity.Property(e => e.RrInvCode)
                    .IsRequired()
                    .HasColumnName("rr_inv_code")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.SourcePatientId)
                    .HasColumnName("source_patient_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnName("state")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Zip)
                    .HasColumnName("zip")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<PoTags>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("po_tags");

                entity.Property(e => e.Tag)
                    .HasColumnName("tag")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<Semaphore>(entity =>
            {
                entity.ToTable("semaphore");

                entity.HasIndex(e => e.Process)
                    .HasName("process")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Process)
                    .IsRequired()
                    .HasColumnName("process")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TimeStarted)
                    .HasColumnName("time_started")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<TblAccumulatedDepreciation>(entity =>
            {
                entity.HasKey(e => e.AccdepId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_accumulated_depreciation");

                entity.Property(e => e.AccdepId)
                    .HasColumnName("accdepID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.BookValue)
                    .HasColumnName("book_value")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.DepAmount)
                    .HasColumnName("dep_amount")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.DepDate)
                    .HasColumnName("dep_date")
                    .HasColumnType("date");

                entity.Property(e => e.DepType)
                    .HasColumnName("dep_type")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DepmasterId)
                    .HasColumnName("depmasterID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblAfterHours>(entity =>
            {
                entity.HasKey(e => e.AhId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_after_hours");

                entity.Property(e => e.AhId)
                    .HasColumnName("ahID")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblBillingAdjReview>(entity =>
            {
                entity.HasKey(e => e.ReviewId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_billing_adj_review");

                entity.HasIndex(e => e.CustomerId)
                    .HasName("customer_index");

                entity.HasIndex(e => e.HospiceId)
                    .HasName("hospice_index");

                entity.HasIndex(e => e.ReviewId)
                    .HasName("review_index");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_index");

                entity.Property(e => e.ReviewId)
                    .HasColumnName("reviewID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(1)");
            });

            modelBuilder.Entity<TblBillingFiles>(entity =>
            {
                entity.HasKey(e => e.BillingfileId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_billing_files");

                entity.HasIndex(e => e.BillingfileId)
                    .HasName("billingfile_index");

                entity.HasIndex(e => e.Filename)
                    .HasName("filename_index");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_index");

                entity.Property(e => e.BillingfileId)
                    .HasColumnName("billingfileID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.Filename)
                    .HasColumnName("filename")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Submitted)
                    .HasColumnName("submitted")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblBillingFilesH2h>(entity =>
            {
                entity.HasKey(e => e.BillingfileId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_billing_files_h2h");

                entity.HasIndex(e => e.BillingfileId)
                    .HasName("billingfile_index");

                entity.HasIndex(e => e.Filename)
                    .HasName("filename_index");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_index");

                entity.Property(e => e.BillingfileId)
                    .HasColumnName("billingfileID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.Filename)
                    .HasColumnName("filename")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Submitted)
                    .HasColumnName("submitted")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblBillingInvoiceCombine>(entity =>
            {
                entity.HasKey(e => e.CombineId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_billing_invoice_combine");

                entity.HasIndex(e => e.CombineId)
                    .HasName("combine_index");

                entity.HasIndex(e => e.CustomerId)
                    .HasName("customer_index");

                entity.HasIndex(e => e.HospiceId)
                    .HasName("hospice_index");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_index");

                entity.Property(e => e.CombineId)
                    .HasColumnName("combineID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(1)");
            });

            modelBuilder.Entity<TblBillingPatientDays>(entity =>
            {
                entity.HasKey(e => e.PatientdaysId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_billing_patient_days");

                entity.HasIndex(e => e.CustomerId)
                    .HasName("customer_index");

                entity.HasIndex(e => e.HospiceId)
                    .HasName("hospice_index");

                entity.HasIndex(e => e.PatientDays)
                    .HasName("patient_days_index");

                entity.HasIndex(e => e.PatientdaysId)
                    .HasName("patientdays_index");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_index");

                entity.Property(e => e.PatientdaysId)
                    .HasColumnName("patientdaysID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PatientDays)
                    .HasColumnName("patient_days")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(1)");
            });

            modelBuilder.Entity<TblBillingProcess>(entity =>
            {
                entity.HasKey(e => e.ProcessId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_billing_process");

                entity.HasIndex(e => e.CustomerId)
                    .HasName("customer_index");

                entity.HasIndex(e => e.LimitHospiceId)
                    .HasName("limit_hospice_index");

                entity.HasIndex(e => e.ProcessComplete)
                    .HasName("process_complete_index");

                entity.HasIndex(e => e.ProcessId)
                    .HasName("process_index");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_index");

                entity.Property(e => e.ProcessId)
                    .HasColumnName("processID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EmailSent)
                    .HasColumnName("email_sent")
                    .HasColumnType("timestamp");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.LimitHospiceId)
                    .HasColumnName("limit_hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NoBilling)
                    .HasColumnName("no_billing")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ProcessComplete)
                    .HasColumnName("process_complete")
                    .HasColumnType("int(1)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(1)");
            });

            modelBuilder.Entity<TblBillingProcessSim>(entity =>
            {
                entity.HasKey(e => e.ProcessId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_billing_process_sim");

                entity.Property(e => e.ProcessId)
                    .HasColumnName("processID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EmailSent)
                    .HasColumnName("email_sent")
                    .HasColumnType("timestamp");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.LimitHospiceId)
                    .HasColumnName("limit_hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NoBilling)
                    .HasColumnName("no_billing")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ProcessComplete)
                    .HasColumnName("process_complete")
                    .HasColumnType("int(1)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(1)");
            });

            modelBuilder.Entity<TblBillingRevenueOnly>(entity =>
            {
                entity.HasKey(e => e.RevId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_billing_revenue_only");

                entity.Property(e => e.RevId)
                    .HasColumnName("revID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NonPerDiem)
                    .HasColumnName("non_per_diem")
                    .HasColumnType("decimal(9,2)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Utilization)
                    .HasColumnName("utilization")
                    .HasColumnType("decimal(9,2)");
            });

            modelBuilder.Entity<TblBrowser>(entity =>
            {
                entity.HasKey(e => e.WidthId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_browser");

                entity.Property(e => e.WidthId)
                    .HasColumnName("widthID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.BrowserHeight)
                    .HasColumnName("browser_height")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.BrowserWidth)
                    .HasColumnName("browser_width")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Ip)
                    .HasColumnName("IP")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<TblCapex>(entity =>
            {
                entity.ToTable("tbl_capex");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Capexvalue)
                    .HasColumnName("capexvalue")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.Groupnum)
                    .HasColumnName("groupnum")
                    .HasColumnType("int(1)");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Invcount)
                    .HasColumnName("invcount")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblCapexCopy>(entity =>
            {
                entity.ToTable("tbl_capex_copy");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Capexvalue)
                    .HasColumnName("capexvalue")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.Groupnum)
                    .HasColumnName("groupnum")
                    .HasColumnType("int(1)");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Invcount)
                    .HasColumnName("invcount")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblCategories>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_categories");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("categoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.CategoryName)
                    .HasColumnName("category_name")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.ImageName)
                    .HasColumnName("image_name")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.ImageV2)
                    .HasColumnName("imageV2")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Private)
                    .HasColumnName("private")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblCensusFiles>(entity =>
            {
                entity.HasKey(e => e.FileId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_census_files");

                entity.Property(e => e.FileId)
                    .HasColumnName("fileID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Filename)
                    .HasColumnName("filename")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<TblContractInventory>(entity =>
            {
                entity.HasKey(e => e.InvctrId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_contract_inventory");

                entity.Property(e => e.InvctrId)
                    .HasColumnName("invctrID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ContractId)
                    .HasColumnName("contractID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.HzOnly)
                    .HasColumnName("hz_only")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NoApprovalRequired)
                    .HasColumnName("no_approval_required")
                    .HasColumnType("int(1)");

                entity.Property(e => e.NumIncluded)
                    .HasColumnName("num_included")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OrderScreen)
                    .HasColumnName("order_screen")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Perdiem)
                    .HasColumnName("perdiem")
                    .HasColumnType("int(1)");

                entity.Property(e => e.RentalPrice)
                    .HasColumnName("rental_price")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.SalePrice)
                    .HasColumnName("sale_price")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.UpdateTimestamp)
                    .HasColumnName("update_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UpdateUserId)
                    .HasColumnName("update_userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblContractInventoryCopy>(entity =>
            {
                entity.HasKey(e => e.InvctrId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_contract_inventory_copy");

                entity.Property(e => e.InvctrId)
                    .HasColumnName("invctrID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ContractId)
                    .HasColumnName("contractID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NoApprovalRequired)
                    .HasColumnName("no_approval_required")
                    .HasColumnType("int(1)");

                entity.Property(e => e.NumIncluded)
                    .HasColumnName("num_included")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OrderScreen)
                    .HasColumnName("order_screen")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Perdiem)
                    .HasColumnName("perdiem")
                    .HasColumnType("int(1)");

                entity.Property(e => e.RentalPrice)
                    .HasColumnName("rental_price")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.SalePrice)
                    .HasColumnName("sale_price")
                    .HasColumnType("decimal(8,2)");
            });

            modelBuilder.Entity<TblContracts>(entity =>
            {
                entity.HasKey(e => e.ContractId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_contracts");

                entity.Property(e => e.ContractId)
                    .HasColumnName("contractID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Bill2ndConc)
                    .HasColumnName("bill_2nd_conc")
                    .HasColumnType("int(1)");

                entity.Property(e => e.CombineTanks)
                    .HasColumnName("combine_tanks")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.ContractName)
                    .HasColumnName("contract_name")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.ContractNumber)
                    .HasColumnName("contract_number")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DeliveryCharge)
                    .HasColumnName("delivery_charge")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DetailBilling)
                    .HasColumnName("detail_billing")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DmeId)
                    .HasColumnName("dmeID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.Filename)
                    .HasColumnName("filename")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MaxMileage)
                    .HasColumnName("max_mileage")
                    .HasColumnType("int(5)");

                entity.Property(e => e.MileageItem)
                    .HasColumnName("mileage_item")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.OverageFee1)
                    .HasColumnName("overage_fee_1")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.OverageFee2)
                    .HasColumnName("overage_fee_2")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.OverageFee3)
                    .HasColumnName("overage_fee_3")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.OverageFee4)
                    .HasColumnName("overage_fee_4")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.OverageFee5)
                    .HasColumnName("overage_fee_5")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.OverageFee6)
                    .HasColumnName("overage_fee_6")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.OverageFee7)
                    .HasColumnName("overage_fee_7")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.OverageFee8)
                    .HasColumnName("overage_fee_8")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.OverageFee9)
                    .HasColumnName("overage_fee_9")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.PerDiemRate)
                    .HasColumnName("per_diem_rate")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.SalesTaxRate)
                    .HasColumnName("sales_tax_rate")
                    .HasColumnType("decimal(11,5)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.UpdateTimestamp)
                    .HasColumnName("update_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UpdateUserId)
                    .HasColumnName("update_userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UtilizationItem1)
                    .HasColumnName("utilization_item_1")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UtilizationItem2)
                    .HasColumnName("utilization_item_2")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UtilizationItem3)
                    .HasColumnName("utilization_item_3")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UtilizationItem4)
                    .HasColumnName("utilization_item_4")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UtilizationItem5)
                    .HasColumnName("utilization_item_5")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UtilizationItem6)
                    .HasColumnName("utilization_item_6")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UtilizationItem7)
                    .HasColumnName("utilization_item_7")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UtilizationItem8)
                    .HasColumnName("utilization_item_8")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UtilizationItem9)
                    .HasColumnName("utilization_item_9")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UtilizationPctg1)
                    .HasColumnName("utilization_pctg_1")
                    .HasColumnType("decimal(5,3)");

                entity.Property(e => e.UtilizationPctg2)
                    .HasColumnName("utilization_pctg_2")
                    .HasColumnType("decimal(5,3)");

                entity.Property(e => e.UtilizationPctg3)
                    .HasColumnName("utilization_pctg_3")
                    .HasColumnType("decimal(5,3)");

                entity.Property(e => e.UtilizationPctg4)
                    .HasColumnName("utilization_pctg_4")
                    .HasColumnType("decimal(5,3)");

                entity.Property(e => e.UtilizationPctg5)
                    .HasColumnName("utilization_pctg_5")
                    .HasColumnType("decimal(5,3)");

                entity.Property(e => e.UtilizationPctg6)
                    .HasColumnName("utilization_pctg_6")
                    .HasColumnType("decimal(5,3)");

                entity.Property(e => e.UtilizationPctg7)
                    .HasColumnName("utilization_pctg_7")
                    .HasColumnType("decimal(5,3)");

                entity.Property(e => e.UtilizationPctg8)
                    .HasColumnName("utilization_pctg_8")
                    .HasColumnType("decimal(5,3)");

                entity.Property(e => e.UtilizationPctg9)
                    .HasColumnName("utilization_pctg_9")
                    .HasColumnType("decimal(5,3)");

                entity.Property(e => e.UtilizationQty1)
                    .HasColumnName("utilization_qty_1")
                    .HasColumnType("int(5)");

                entity.Property(e => e.UtilizationQty2)
                    .HasColumnName("utilization_qty_2")
                    .HasColumnType("int(5)");

                entity.Property(e => e.UtilizationQty3)
                    .HasColumnName("utilization_qty_3")
                    .HasColumnType("int(5)");

                entity.Property(e => e.UtilizationQty4)
                    .HasColumnName("utilization_qty_4")
                    .HasColumnType("int(5)");

                entity.Property(e => e.UtilizationQty5)
                    .HasColumnName("utilization_qty_5")
                    .HasColumnType("int(5)");

                entity.Property(e => e.UtilizationQty6)
                    .HasColumnName("utilization_qty_6")
                    .HasColumnType("int(5)");

                entity.Property(e => e.UtilizationQty7)
                    .HasColumnName("utilization_qty_7")
                    .HasColumnType("int(5)");

                entity.Property(e => e.UtilizationQty8)
                    .HasColumnName("utilization_qty_8")
                    .HasColumnType("int(5)");

                entity.Property(e => e.UtilizationQty9)
                    .HasColumnName("utilization_qty_9")
                    .HasColumnType("int(5)");
            });

            modelBuilder.Entity<TblCreditRequests>(entity =>
            {
                entity.HasKey(e => e.CreditId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_credit_requests");

                entity.Property(e => e.CreditId)
                    .HasColumnName("creditID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.BillingDate)
                    .HasColumnName("billing_date")
                    .HasColumnType("date");

                entity.Property(e => e.CreditAmount)
                    .HasColumnName("credit_amount")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvoiceNumber)
                    .HasColumnName("invoice_number")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PatientName)
                    .HasColumnName("patient_name")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PostArray)
                    .HasColumnName("post_array")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblCsFiles>(entity =>
            {
                entity.HasKey(e => e.CsfileId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_cs_files");

                entity.Property(e => e.CsfileId)
                    .HasColumnName("csfileID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.Filename)
                    .HasColumnName("filename")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Submitted)
                    .HasColumnName("submitted")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblCustomerEmails>(entity =>
            {
                entity.HasKey(e => e.EmailId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_customer_emails");

                entity.Property(e => e.EmailId)
                    .HasColumnName("emailID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.All)
                    .HasColumnName("all")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Body)
                    .HasColumnName("body")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Footer)
                    .HasColumnName("footer")
                    .HasColumnType("int(1)");

                entity.Property(e => e.From)
                    .HasColumnName("from")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Fromemail)
                    .HasColumnName("fromemail")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Subject)
                    .HasColumnName("subject")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");
            });

            modelBuilder.Entity<TblCustomers>(entity =>
            {
                entity.HasKey(e => e.CustomerId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_customers");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CompanyAddress)
                    .HasColumnName("company_address")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.CompanyCity)
                    .HasColumnName("company_city")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.CompanyInactive)
                    .HasColumnName("company_inactive")
                    .HasColumnType("int(1)");

                entity.Property(e => e.CompanyName)
                    .HasColumnName("company_name")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.CompanyReportingNational)
                    .HasColumnName("company_reporting_national")
                    .HasColumnType("int(1)");

                entity.Property(e => e.CompanyReportingQuality)
                    .HasColumnName("company_reporting_quality")
                    .HasColumnType("int(1)");

                entity.Property(e => e.CompanyReportingRegions)
                    .HasColumnName("company_reporting_regions")
                    .HasColumnType("int(1)");

                entity.Property(e => e.CompanyState)
                    .HasColumnName("company_state")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.CompanyZip)
                    .HasColumnName("company_zip")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UpdateTimestamp)
                    .HasColumnName("update_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UpdateUserId)
                    .HasColumnName("update_userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblDcHistory>(entity =>
            {
                entity.HasKey(e => e.DcId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_dc_history");

                entity.Property(e => e.DcId)
                    .HasColumnName("dcID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DcDate)
                    .HasColumnName("dc_date")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<TblDeclinedOffCap>(entity =>
            {
                entity.HasKey(e => e.DenyId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_declined_off_cap");

                entity.Property(e => e.DenyId)
                    .HasColumnName("denyID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProductId)
                    .HasColumnName("productID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblDepreciationMaster>(entity =>
            {
                entity.HasKey(e => e.DepmasterId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_depreciation_master");

                entity.Property(e => e.DepmasterId)
                    .HasColumnName("depmasterID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetCost)
                    .HasColumnName("asset_cost")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.AssetEntry)
                    .HasColumnName("asset_entry")
                    .HasColumnType("date");

                entity.Property(e => e.AssetLife)
                    .HasColumnName("asset_life")
                    .HasColumnType("int(3)");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvaddId)
                    .HasColumnName("invaddID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationInvId)
                    .HasColumnName("location_invID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PoId)
                    .HasColumnName("poID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblDepreciationMaster61015>(entity =>
            {
                entity.HasKey(e => e.DepmasterId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_depreciation_master_61015");

                entity.Property(e => e.DepmasterId)
                    .HasColumnName("depmasterID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetCost)
                    .HasColumnName("asset_cost")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.AssetEntry)
                    .HasColumnName("asset_entry")
                    .HasColumnType("date");

                entity.Property(e => e.AssetLife)
                    .HasColumnName("asset_life")
                    .HasColumnType("int(3)");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvaddId)
                    .HasColumnName("invaddID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationInvId)
                    .HasColumnName("location_invID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PoId)
                    .HasColumnName("poID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblDispatchHistory>(entity =>
            {
                entity.HasKey(e => e.DhId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_dispatch_history");

                entity.Property(e => e.DhId)
                    .HasColumnName("dhID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DispatchStatus)
                    .HasColumnName("dispatch_status")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DriverId)
                    .HasColumnName("driverID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblDispatchNotes>(entity =>
            {
                entity.HasKey(e => e.NoteId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_dispatch_notes");

                entity.Property(e => e.NoteId)
                    .HasColumnName("noteID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblDmeGroups>(entity =>
            {
                entity.HasKey(e => e.DmegroupId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_dme_groups");

                entity.Property(e => e.DmegroupId)
                    .HasColumnName("dmegroupID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.GroupName)
                    .HasColumnName("group_name")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<TblDmePartners>(entity =>
            {
                entity.HasKey(e => e.DmeId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_dme_partners");

                entity.Property(e => e.DmeId)
                    .HasColumnName("dmeID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CompanyAddress)
                    .HasColumnName("company_address")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.CompanyCity)
                    .HasColumnName("company_city")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.CompanyEmails)
                    .HasColumnName("company_emails")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.CompanyName)
                    .HasColumnName("company_name")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.CompanyState)
                    .HasColumnName("company_state")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.CompanyZip)
                    .HasColumnName("company_zip")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DmegroupId)
                    .HasColumnName("dmegroupID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EmailOnly)
                    .HasColumnName("email_only")
                    .HasColumnType("int(1)");

                entity.Property(e => e.EmailOnlyPassword)
                    .HasColumnName("email_only_password")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.EmailOnlyUsername)
                    .HasColumnName("email_only_username")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvoiceEmail)
                    .HasColumnName("invoice_email")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MultiHospice)
                    .HasColumnName("multi_hospice")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ShareViewDmeId)
                    .HasColumnName("share_view_dmeID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblDrivers>(entity =>
            {
                entity.HasKey(e => e.DriverId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_drivers");

                entity.Property(e => e.DriverId)
                    .HasColumnName("driverID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Division)
                    .HasColumnName("division")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Firstname)
                    .HasColumnName("firstname")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Inactive)
                    .HasColumnName("inactive")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Lastname)
                    .HasColumnName("lastname")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(5)");

                entity.Property(e => e.Mobile)
                    .HasColumnName("mobile")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Network)
                    .HasColumnName("network")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.VehicleId)
                    .HasColumnName("vehicleID")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<TblExcludeSubcatSubcat>(entity =>
            {
                entity.HasKey(e => e.ExcludeId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_exclude_subcat_subcat");

                entity.Property(e => e.ExcludeId)
                    .HasColumnName("excludeID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ExcludeSubCategoryId)
                    .HasColumnName("exclude_sub_categoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SubCategoryId)
                    .HasColumnName("sub_categoryID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblFacilities>(entity =>
            {
                entity.HasKey(e => e.FacilityId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_facilities");

                entity.Property(e => e.FacilityId)
                    .HasColumnName("facilityID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FacilityAddress)
                    .HasColumnName("facility_address")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.FacilityCity)
                    .HasColumnName("facility_city")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.FacilityName)
                    .HasColumnName("facility_name")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.FacilityPhone)
                    .HasColumnName("facility_phone")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.FacilityState)
                    .HasColumnName("facility_state")
                    .HasColumnType("varchar(2)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.FacilityZip)
                    .HasColumnName("facility_zip")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblFavorites>(entity =>
            {
                entity.HasKey(e => e.FavsId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_favorites");

                entity.Property(e => e.FavsId)
                    .HasColumnName("favsID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CategoryArray)
                    .HasColumnName("category_array")
                    .HasColumnType("text")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.FavoriteArray)
                    .HasColumnName("favorite_array")
                    .HasColumnType("text")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblGlCodes>(entity =>
            {
                entity.HasKey(e => e.GlId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_gl_codes");

                entity.Property(e => e.GlId)
                    .HasColumnName("glID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AccountCode)
                    .HasColumnName("account_code")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AccountCodeDescription)
                    .HasColumnName("account_code_description")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<TblHospices>(entity =>
            {
                entity.HasKey(e => e.HospiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_hospices");

                entity.HasIndex(e => e.CustomerId)
                    .HasName("tbl_hospices_customerid_idx");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AutoApproval)
                    .HasColumnName("auto_approval")
                    .HasColumnType("int(3)");

                entity.Property(e => e.BillingContact)
                    .HasColumnName("billing_contact")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.BillingContactPhone)
                    .HasColumnName("billing_contact_phone")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.CallInDays)
                    .HasColumnName("call_in_days")
                    .HasColumnType("int(1)");

                entity.Property(e => e.CostCenter)
                    .HasColumnName("cost_center")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.CpdBudget)
                    .HasColumnName("cpd_budget")
                    .HasColumnType("decimal(5,2)");

                entity.Property(e => e.CreditHold)
                    .HasColumnName("credit_hold")
                    .HasColumnType("int(1)");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DetailBilling)
                    .HasColumnName("detail_billing")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DmeId)
                    .HasColumnName("dmeID")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.EmailType)
                    .HasColumnName("email_type")
                    .HasColumnType("int(1)");

                entity.Property(e => e.HospiceAddress)
                    .HasColumnName("hospice_address")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.HospiceBillingId)
                    .HasColumnName("hospice_billing_ID")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.HospiceCity)
                    .HasColumnName("hospice_city")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.HospiceInactive)
                    .HasColumnName("hospice_inactive")
                    .HasColumnType("int(1)");

                entity.Property(e => e.HospiceName)
                    .HasColumnName("hospice_name")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.HospiceNotifications)
                    .HasColumnName("hospice_notifications")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.HospicePhone)
                    .HasColumnName("hospice_phone")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.HospiceShortName)
                    .HasColumnName("hospice_short_name")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.HospiceState)
                    .HasColumnName("hospice_state")
                    .HasColumnType("varchar(2)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.HospiceZip)
                    .HasColumnName("hospice_zip")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.IncludeCoverSheet)
                    .HasColumnName("include_cover_sheet")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InvoiceEmail)
                    .HasColumnName("invoice_email")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LegacyMitsClass)
                    .HasColumnName("legacy_MITS_class")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LegacyMitsDivision)
                    .HasColumnName("legacy_MITS_division")
                    .HasColumnType("varchar(5)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LocationCode)
                    .HasColumnName("location_code")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.NoPerdiemLines)
                    .HasColumnName("no_perdiem_lines")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OffCapApproval)
                    .HasColumnName("off_cap_approval")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PaymentPlan)
                    .HasColumnName("payment_plan")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PaymentTerms)
                    .HasColumnName("payment_terms")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PccrEmailCopy)
                    .IsRequired()
                    .HasColumnName("pccr_email_copy")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PdfInvoice)
                    .HasColumnName("pdf_invoice")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PerDiemOverride)
                    .HasColumnName("per_diem_override")
                    .HasColumnType("decimal(6,2)");

                entity.Property(e => e.RegionTag)
                    .HasColumnName("region_tag")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.RevenueOnly)
                    .HasColumnName("revenue_only")
                    .HasColumnType("int(1)");

                entity.Property(e => e.SendExcel)
                    .HasColumnName("send_excel")
                    .HasColumnType("int(1)");

                entity.Property(e => e.TestHospice)
                    .HasColumnName("test_hospice")
                    .HasColumnType("int(1)");

                entity.Property(e => e.UpdateTimestamp)
                    .HasColumnName("update_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UpdateUserId)
                    .HasColumnName("update_userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Version2)
                    .HasColumnName("version_2")
                    .HasColumnType("int(1)");
            });

            modelBuilder.Entity<TblInventory>(entity =>
            {
                entity.HasKey(e => e.InventoryId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasColumnType("int(1)");

                entity.Property(e => e.AssetTagRequired)
                    .HasColumnName("asset_tag_required")
                    .HasColumnType("int(1)");

                entity.Property(e => e.BariItem)
                    .HasColumnName("bari_item")
                    .HasColumnType("int(1)");

                entity.Property(e => e.BipapSetting)
                    .HasColumnName("bipap_setting")
                    .HasColumnType("int(1)");

                entity.Property(e => e.CapexItem)
                    .HasColumnName("capex_item")
                    .HasColumnType("int(1)");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("categoryID")
                    .HasColumnType("int(4)");

                entity.Property(e => e.ChooseQuantity)
                    .HasColumnName("choose_quantity")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Concentrator)
                    .HasColumnName("concentrator")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.CpapSetting)
                    .HasColumnName("cpap_setting")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DbmCode)
                    .IsRequired()
                    .HasColumnName("dbm_code")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DefaultRentPrice)
                    .HasColumnName("default_rent_price")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.DefaultSalePrice)
                    .HasColumnName("default_sale_price")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.Depreciation)
                    .HasColumnName("depreciation")
                    .HasColumnType("int(5)");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Disposable)
                    .HasColumnName("disposable")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Donated)
                    .HasColumnName("donated")
                    .HasColumnType("int(1)");

                entity.Property(e => e.GlId)
                    .HasColumnName("glID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IncludedInvCodes)
                    .HasColumnName("included_inv_codes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Inventoried)
                    .HasColumnName("inventoried")
                    .HasColumnType("int(1)");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.LotNumRequired)
                    .HasColumnName("lot_num_required")
                    .HasColumnType("int(1)");

                entity.Property(e => e.MaintInterval)
                    .HasColumnName("maint_interval")
                    .HasColumnType("int(11)")
                    .HasComment("maximum maintenance interval for equipment that requires regular maintenance");

                entity.Property(e => e.OxygenSetting)
                    .HasColumnName("oxygen_setting")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ProductDescription)
                    .HasColumnName("product_description")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.ProductImage)
                    .HasColumnName("product_image")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.QuantityLimit)
                    .HasColumnName("quantity_limit")
                    .HasColumnType("int(2)");

                entity.Property(e => e.RegMaintRequired)
                    .HasColumnName("reg_maint_required")
                    .HasComment("flag for equipment that requires regular maintenance");

                entity.Property(e => e.SerialNumRequired)
                    .HasColumnName("serial_num_required")
                    .HasColumnType("int(1)");

                entity.Property(e => e.SubCategoryId)
                    .HasColumnName("sub_categoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TimeDelivery)
                    .HasColumnName("time_delivery")
                    .HasColumnType("int(4)");

                entity.Property(e => e.TimePickup)
                    .HasColumnName("time_pickup")
                    .HasColumnType("int(4)");
            });

            modelBuilder.Entity<TblInventoryAdds>(entity =>
            {
                entity.HasKey(e => e.InvaddId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_adds");

                entity.Property(e => e.InvaddId)
                    .HasColumnName("invaddID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddStatus)
                    .HasColumnName("add_status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ApprovalNotes)
                    .HasColumnName("approval_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetAge)
                    .HasColumnName("asset_age")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetRejects)
                    .HasColumnName("asset_rejects")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("timestamp");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvStatus)
                    .HasColumnName("inv_status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.ProcessDate)
                    .HasColumnName("process_date")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Processed)
                    .HasColumnName("processed")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(5)");

                entity.Property(e => e.RelatedId)
                    .HasColumnName("relatedID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SerialNumber)
                    .HasColumnName("serial_number")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserIdProcessor)
                    .HasColumnName("userID_processor")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblInventoryAdds61015>(entity =>
            {
                entity.HasKey(e => e.InvaddId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_adds_61015");

                entity.Property(e => e.InvaddId)
                    .HasColumnName("invaddID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddStatus)
                    .HasColumnName("add_status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ApprovalNotes)
                    .HasColumnName("approval_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetAge)
                    .HasColumnName("asset_age")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetRejects)
                    .HasColumnName("asset_rejects")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("timestamp");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvStatus)
                    .HasColumnName("inv_status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.ProcessDate)
                    .HasColumnName("process_date")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Processed)
                    .HasColumnName("processed")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(5)");

                entity.Property(e => e.RelatedId)
                    .HasColumnName("relatedID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SerialNumber)
                    .HasColumnName("serial_number")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserIdProcessor)
                    .HasColumnName("userID_processor")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblInventoryAdds772015>(entity =>
            {
                entity.HasKey(e => e.InvaddId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_adds_772015");

                entity.Property(e => e.InvaddId)
                    .HasColumnName("invaddID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddStatus)
                    .HasColumnName("add_status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ApprovalNotes)
                    .HasColumnName("approval_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetAge)
                    .HasColumnName("asset_age")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetRejects)
                    .HasColumnName("asset_rejects")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("timestamp");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvStatus)
                    .HasColumnName("inv_status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.ProcessDate)
                    .HasColumnName("process_date")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Processed)
                    .HasColumnName("processed")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(5)");

                entity.Property(e => e.RelatedId)
                    .HasColumnName("relatedID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SerialNumber)
                    .HasColumnName("serial_number")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserIdProcessor)
                    .HasColumnName("userID_processor")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblInventoryBakOld>(entity =>
            {
                entity.HasKey(e => e.InventoryId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_bak_old");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasColumnType("int(1)");

                entity.Property(e => e.AssetTagRequired)
                    .HasColumnName("asset_tag_required")
                    .HasColumnType("int(1)");

                entity.Property(e => e.BariItem)
                    .HasColumnName("bari_item")
                    .HasColumnType("int(1)");

                entity.Property(e => e.BipapSetting)
                    .HasColumnName("bipap_setting")
                    .HasColumnType("int(1)");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("categoryID")
                    .HasColumnType("int(4)");

                entity.Property(e => e.ChooseQuantity)
                    .HasColumnName("choose_quantity")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Concentrator)
                    .HasColumnName("concentrator")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.CpapSetting)
                    .HasColumnName("cpap_setting")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DefaultRentPrice)
                    .HasColumnName("default_rent_price")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.DefaultSalePrice)
                    .HasColumnName("default_sale_price")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.Depreciation)
                    .HasColumnName("depreciation")
                    .HasColumnType("int(5)");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Disposable)
                    .HasColumnName("disposable")
                    .HasColumnType("int(1)");

                entity.Property(e => e.GlId)
                    .HasColumnName("glID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IncludedInvCodes)
                    .HasColumnName("included_inv_codes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Inventoried)
                    .HasColumnName("inventoried")
                    .HasColumnType("int(1)");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.LotNumRequired)
                    .HasColumnName("lot_num_required")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OxygenSetting)
                    .HasColumnName("oxygen_setting")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ProductDescription)
                    .HasColumnName("product_description")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.ProductImage)
                    .HasColumnName("product_image")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.SerialNumRequired)
                    .HasColumnName("serial_num_required")
                    .HasColumnType("int(1)");

                entity.Property(e => e.SubCategoryId)
                    .HasColumnName("sub_categoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TimeDelivery)
                    .HasColumnName("time_delivery")
                    .HasColumnType("int(4)");

                entity.Property(e => e.TimePickup)
                    .HasColumnName("time_pickup")
                    .HasColumnType("int(4)");
            });

            modelBuilder.Entity<TblInventoryBundleLinks>(entity =>
            {
                entity.HasKey(e => e.BundleLinkId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_bundle_links");

                entity.Property(e => e.BundleLinkId)
                    .HasColumnName("bundleLinkID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.BundleId)
                    .HasColumnName("bundleID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblInventoryBundles>(entity =>
            {
                entity.HasKey(e => e.BundleId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_bundles");

                entity.Property(e => e.BundleId)
                    .HasColumnName("bundleID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<TblInventoryIssues>(entity =>
            {
                entity.HasKey(e => e.InvissueId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_issues");

                entity.Property(e => e.InvissueId)
                    .HasColumnName("invissueID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DeliveredTimestamp)
                    .HasColumnName("delivered_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Delivery)
                    .HasColumnName("delivery")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DispositionNotes)
                    .HasColumnName("disposition_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DriverId)
                    .HasColumnName("driverID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IssueDescription)
                    .HasColumnName("issue_description")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LinkId)
                    .HasColumnName("linkID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Pickup)
                    .HasColumnName("pickup")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PickupTimestamp)
                    .HasColumnName("pickup_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.UpdateTimestamp)
                    .HasColumnName("update_timestamp")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<TblInventoryIssues61015>(entity =>
            {
                entity.HasKey(e => e.InvissueId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_issues_61015");

                entity.Property(e => e.InvissueId)
                    .HasColumnName("invissueID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DeliveredTimestamp)
                    .HasColumnName("delivered_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Delivery)
                    .HasColumnName("delivery")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DispositionNotes)
                    .HasColumnName("disposition_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DriverId)
                    .HasColumnName("driverID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LinkId)
                    .HasColumnName("linkID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Pickup)
                    .HasColumnName("pickup")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PickupTimestamp)
                    .HasColumnName("pickup_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.UpdateTimestamp)
                    .HasColumnName("update_timestamp")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<TblInventoryIssues61215>(entity =>
            {
                entity.HasKey(e => e.InvissueId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_issues_61215");

                entity.Property(e => e.InvissueId)
                    .HasColumnName("invissueID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DeliveredTimestamp)
                    .HasColumnName("delivered_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Delivery)
                    .HasColumnName("delivery")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DispositionNotes)
                    .HasColumnName("disposition_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DriverId)
                    .HasColumnName("driverID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LinkId)
                    .HasColumnName("linkID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Pickup)
                    .HasColumnName("pickup")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PickupTimestamp)
                    .HasColumnName("pickup_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.UpdateTimestamp)
                    .HasColumnName("update_timestamp")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<TblInventoryIssues772015>(entity =>
            {
                entity.HasKey(e => e.InvissueId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_issues_772015");

                entity.Property(e => e.InvissueId)
                    .HasColumnName("invissueID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DeliveredTimestamp)
                    .HasColumnName("delivered_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Delivery)
                    .HasColumnName("delivery")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DispositionNotes)
                    .HasColumnName("disposition_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DriverId)
                    .HasColumnName("driverID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IssueDescription)
                    .HasColumnName("issue_description")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LinkId)
                    .HasColumnName("linkID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Pickup)
                    .HasColumnName("pickup")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PickupTimestamp)
                    .HasColumnName("pickup_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.UpdateTimestamp)
                    .HasColumnName("update_timestamp")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<TblInventoryIssuesCopy>(entity =>
            {
                entity.HasKey(e => e.InvissueId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_issues_copy");

                entity.Property(e => e.InvissueId)
                    .HasColumnName("invissueID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DeliveredTimestamp)
                    .HasColumnName("delivered_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Delivery)
                    .HasColumnName("delivery")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DispositionNotes)
                    .HasColumnName("disposition_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DriverId)
                    .HasColumnName("driverID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LinkId)
                    .HasColumnName("linkID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Pickup)
                    .HasColumnName("pickup")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PickupTimestamp)
                    .HasColumnName("pickup_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.UpdateTimestamp)
                    .HasColumnName("update_timestamp")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<TblInventoryIssuesCopy1>(entity =>
            {
                entity.HasKey(e => e.InvissueId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_issues_copy1");

                entity.Property(e => e.InvissueId)
                    .HasColumnName("invissueID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DeliveredTimestamp)
                    .HasColumnName("delivered_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Delivery)
                    .HasColumnName("delivery")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DispositionNotes)
                    .HasColumnName("disposition_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DriverId)
                    .HasColumnName("driverID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IssueDescription)
                    .HasColumnName("issue_description")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LinkId)
                    .HasColumnName("linkID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Pickup)
                    .HasColumnName("pickup")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PickupTimestamp)
                    .HasColumnName("pickup_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.UpdateTimestamp)
                    .HasColumnName("update_timestamp")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<TblInventoryIssuesCopy2>(entity =>
            {
                entity.HasKey(e => e.InvissueId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_issues_copy2");

                entity.Property(e => e.InvissueId)
                    .HasColumnName("invissueID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DeliveredTimestamp)
                    .HasColumnName("delivered_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Delivery)
                    .HasColumnName("delivery")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DispositionNotes)
                    .HasColumnName("disposition_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DriverId)
                    .HasColumnName("driverID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IssueDescription)
                    .HasColumnName("issue_description")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LinkId)
                    .HasColumnName("linkID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Pickup)
                    .HasColumnName("pickup")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PickupTimestamp)
                    .HasColumnName("pickup_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.UpdateTimestamp)
                    .HasColumnName("update_timestamp")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<TblInventoryIssuesCopy3>(entity =>
            {
                entity.HasKey(e => e.InvissueId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_issues_copy3");

                entity.Property(e => e.InvissueId)
                    .HasColumnName("invissueID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DeliveredTimestamp)
                    .HasColumnName("delivered_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Delivery)
                    .HasColumnName("delivery")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DispositionNotes)
                    .HasColumnName("disposition_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DriverId)
                    .HasColumnName("driverID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IssueDescription)
                    .HasColumnName("issue_description")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LinkId)
                    .HasColumnName("linkID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Pickup)
                    .HasColumnName("pickup")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PickupTimestamp)
                    .HasColumnName("pickup_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.UpdateTimestamp)
                    .HasColumnName("update_timestamp")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<TblInventoryLoadIssues>(entity =>
            {
                entity.HasKey(e => e.InvloadissueId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_load_issues");

                entity.Property(e => e.InvloadissueId)
                    .HasColumnName("invloadissueID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LinkId)
                    .HasColumnName("linkID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblInventoryPreList>(entity =>
            {
                entity.HasKey(e => e.PrelistId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_pre_list");

                entity.Property(e => e.PrelistId)
                    .HasColumnName("prelistID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PrelistInvCode)
                    .HasColumnName("prelist_inv_code")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PrelistLocator)
                    .HasColumnName("prelist_locator")
                    .HasColumnType("varchar(3)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<TblInventoryQuantities>(entity =>
            {
                entity.HasKey(e => e.QuantityId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_quantities");

                entity.HasIndex(e => e.LocationId)
                    .HasName("locationID");

                entity.Property(e => e.QuantityId)
                    .HasColumnName("quantityID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AdjustmentNotes)
                    .HasColumnName("adjustment_notes")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetTagPickup)
                    .HasColumnName("asset_tag_pickup")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Delivery)
                    .HasColumnName("delivery")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Imported)
                    .HasColumnName("imported")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InvAdjustment)
                    .HasColumnName("inv_adjustment")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InvStatus)
                    .HasColumnName("inv_status")
                    .HasColumnType("int(2)");

                entity.Property(e => e.InvaddId)
                    .HasColumnName("invaddID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvtransId)
                    .HasColumnName("invtransID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvwoId)
                    .HasColumnName("invwoID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.LinkId)
                    .HasColumnName("linkID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationInvId)
                    .HasColumnName("location_invID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PhysicalinvId)
                    .HasColumnName("physicalinvID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PiBalanceClear)
                    .HasColumnName("pi_balance_clear")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PiDate)
                    .HasColumnName("pi_date")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Pickup)
                    .HasColumnName("pickup")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PoitemId)
                    .HasColumnName("poitemID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblInventoryQuantities61015>(entity =>
            {
                entity.HasKey(e => e.QuantityId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_quantities_61015");

                entity.HasIndex(e => e.LocationId)
                    .HasName("locationID");

                entity.Property(e => e.QuantityId)
                    .HasColumnName("quantityID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AdjustmentNotes)
                    .HasColumnName("adjustment_notes")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetTagPickup)
                    .HasColumnName("asset_tag_pickup")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Delivery)
                    .HasColumnName("delivery")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Imported)
                    .HasColumnName("imported")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InvAdjustment)
                    .HasColumnName("inv_adjustment")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InvStatus)
                    .HasColumnName("inv_status")
                    .HasColumnType("int(2)");

                entity.Property(e => e.InvaddId)
                    .HasColumnName("invaddID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvtransId)
                    .HasColumnName("invtransID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvwoId)
                    .HasColumnName("invwoID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.LinkId)
                    .HasColumnName("linkID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationInvId)
                    .HasColumnName("location_invID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PiBalanceClear)
                    .HasColumnName("pi_balance_clear")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PiDate)
                    .HasColumnName("pi_date")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Pickup)
                    .HasColumnName("pickup")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PoitemId)
                    .HasColumnName("poitemID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblInventoryQuantities61115>(entity =>
            {
                entity.HasKey(e => e.QuantityId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_quantities_61115");

                entity.HasIndex(e => e.LocationId)
                    .HasName("locationID");

                entity.Property(e => e.QuantityId)
                    .HasColumnName("quantityID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AdjustmentNotes)
                    .HasColumnName("adjustment_notes")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetTagPickup)
                    .HasColumnName("asset_tag_pickup")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Delivery)
                    .HasColumnName("delivery")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Imported)
                    .HasColumnName("imported")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InvAdjustment)
                    .HasColumnName("inv_adjustment")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InvStatus)
                    .HasColumnName("inv_status")
                    .HasColumnType("int(2)");

                entity.Property(e => e.InvaddId)
                    .HasColumnName("invaddID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvtransId)
                    .HasColumnName("invtransID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvwoId)
                    .HasColumnName("invwoID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.LinkId)
                    .HasColumnName("linkID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationInvId)
                    .HasColumnName("location_invID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PhysicalinvId)
                    .HasColumnName("physicalinvID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PiBalanceClear)
                    .HasColumnName("pi_balance_clear")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PiDate)
                    .HasColumnName("pi_date")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Pickup)
                    .HasColumnName("pickup")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PoitemId)
                    .HasColumnName("poitemID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblInventoryQuantities61215>(entity =>
            {
                entity.HasKey(e => e.QuantityId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_quantities_61215");

                entity.HasIndex(e => e.LocationId)
                    .HasName("locationID");

                entity.Property(e => e.QuantityId)
                    .HasColumnName("quantityID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AdjustmentNotes)
                    .HasColumnName("adjustment_notes")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetTagPickup)
                    .HasColumnName("asset_tag_pickup")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Delivery)
                    .HasColumnName("delivery")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Imported)
                    .HasColumnName("imported")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InvAdjustment)
                    .HasColumnName("inv_adjustment")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InvStatus)
                    .HasColumnName("inv_status")
                    .HasColumnType("int(2)");

                entity.Property(e => e.InvaddId)
                    .HasColumnName("invaddID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvtransId)
                    .HasColumnName("invtransID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvwoId)
                    .HasColumnName("invwoID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.LinkId)
                    .HasColumnName("linkID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationInvId)
                    .HasColumnName("location_invID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PhysicalinvId)
                    .HasColumnName("physicalinvID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PiBalanceClear)
                    .HasColumnName("pi_balance_clear")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PiDate)
                    .HasColumnName("pi_date")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Pickup)
                    .HasColumnName("pickup")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PoitemId)
                    .HasColumnName("poitemID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblInventoryQuantitiesCopy>(entity =>
            {
                entity.HasKey(e => e.QuantityId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_quantities_copy");

                entity.HasIndex(e => e.LocationId)
                    .HasName("locationID");

                entity.Property(e => e.QuantityId)
                    .HasColumnName("quantityID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AdjustmentNotes)
                    .HasColumnName("adjustment_notes")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetTagPickup)
                    .HasColumnName("asset_tag_pickup")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Delivery)
                    .HasColumnName("delivery")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Imported)
                    .HasColumnName("imported")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InvAdjustment)
                    .HasColumnName("inv_adjustment")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InvStatus)
                    .HasColumnName("inv_status")
                    .HasColumnType("int(2)");

                entity.Property(e => e.InvaddId)
                    .HasColumnName("invaddID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvtransId)
                    .HasColumnName("invtransID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvwoId)
                    .HasColumnName("invwoID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.LinkId)
                    .HasColumnName("linkID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationInvId)
                    .HasColumnName("location_invID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PiDate)
                    .HasColumnName("pi_date")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Pickup)
                    .HasColumnName("pickup")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PoitemId)
                    .HasColumnName("poitemID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblInventoryStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_status");

                entity.Property(e => e.StatusId)
                    .HasColumnName("statusID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SerialNumber)
                    .HasColumnName("serial_number")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(1)");
            });

            modelBuilder.Entity<TblInventoryTransfers>(entity =>
            {
                entity.HasKey(e => e.InvtransId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_transfers");

                entity.Property(e => e.InvtransId)
                    .HasColumnName("invtransID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ApprovalNotes)
                    .HasColumnName("approval_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("timestamp");

                entity.Property(e => e.FreightCost)
                    .HasColumnName("freight_cost")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.FreightPkg)
                    .HasColumnName("freight_pkg")
                    .HasColumnType("int(1)");

                entity.Property(e => e.FreightPkgHeight)
                    .HasColumnName("freight_pkg_height")
                    .HasColumnType("int(5)");

                entity.Property(e => e.FreightPkgLength)
                    .HasColumnName("freight_pkg_length")
                    .HasColumnType("int(5)");

                entity.Property(e => e.FreightPkgNum)
                    .HasColumnName("freight_pkg_num")
                    .HasColumnType("int(5)");

                entity.Property(e => e.FreightPkgWidth)
                    .HasColumnName("freight_pkg_width")
                    .HasColumnType("int(5)");

                entity.Property(e => e.InvStatus)
                    .HasColumnName("inv_status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.LocationIdFrom)
                    .HasColumnName("locationID_from")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationIdTo)
                    .HasColumnName("locationID_to")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.ProcessDate)
                    .HasColumnName("process_date")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.TransferModality)
                    .HasColumnName("transfer_modality")
                    .HasColumnType("int(1)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserIdProcessor)
                    .HasColumnName("userID_processor")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblInventoryTransfers6102015>(entity =>
            {
                entity.HasKey(e => e.InvtransId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_transfers_6102015");

                entity.Property(e => e.InvtransId)
                    .HasColumnName("invtransID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ApprovalNotes)
                    .HasColumnName("approval_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("timestamp");

                entity.Property(e => e.FreightCost)
                    .HasColumnName("freight_cost")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.FreightPkg)
                    .HasColumnName("freight_pkg")
                    .HasColumnType("int(1)");

                entity.Property(e => e.FreightPkgHeight)
                    .HasColumnName("freight_pkg_height")
                    .HasColumnType("int(5)");

                entity.Property(e => e.FreightPkgLength)
                    .HasColumnName("freight_pkg_length")
                    .HasColumnType("int(5)");

                entity.Property(e => e.FreightPkgNum)
                    .HasColumnName("freight_pkg_num")
                    .HasColumnType("int(5)");

                entity.Property(e => e.FreightPkgWidth)
                    .HasColumnName("freight_pkg_width")
                    .HasColumnType("int(5)");

                entity.Property(e => e.InvStatus)
                    .HasColumnName("inv_status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.LocationIdFrom)
                    .HasColumnName("locationID_from")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationIdTo)
                    .HasColumnName("locationID_to")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.ProcessDate)
                    .HasColumnName("process_date")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.TransferModality)
                    .HasColumnName("transfer_modality")
                    .HasColumnType("int(1)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserIdProcessor)
                    .HasColumnName("userID_processor")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblInventoryTransfersIntra>(entity =>
            {
                entity.HasKey(e => e.InvtransId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_transfers_intra");

                entity.Property(e => e.InvtransId)
                    .HasColumnName("invtransID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("timestamp");

                entity.Property(e => e.InvStatus)
                    .HasColumnName("inv_status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.LocationIdFrom)
                    .HasColumnName("locationID_from")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationIdTo)
                    .HasColumnName("locationID_to")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Processed)
                    .HasColumnName("processed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(5)");

                entity.Property(e => e.SerialNumber)
                    .HasColumnName("serial_number")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.VehicleId)
                    .HasColumnName("vehicleID")
                    .HasColumnType("int(50)");
            });

            modelBuilder.Entity<TblInventoryTransfersIntra6102105>(entity =>
            {
                entity.HasKey(e => e.InvtransId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_transfers_intra_6102105");

                entity.Property(e => e.InvtransId)
                    .HasColumnName("invtransID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("timestamp");

                entity.Property(e => e.InvStatus)
                    .HasColumnName("inv_status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.LocationIdFrom)
                    .HasColumnName("locationID_from")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationIdTo)
                    .HasColumnName("locationID_to")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Processed)
                    .HasColumnName("processed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(5)");

                entity.Property(e => e.SerialNumber)
                    .HasColumnName("serial_number")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.VehicleId)
                    .HasColumnName("vehicleID")
                    .HasColumnType("int(50)");
            });

            modelBuilder.Entity<TblInventoryTransfersItems>(entity =>
            {
                entity.HasKey(e => e.InvtransitemId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_transfers_items");

                entity.Property(e => e.InvtransitemId)
                    .HasColumnName("invtransitemID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("timestamp");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvtransId)
                    .HasColumnName("invtransID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(5)");

                entity.Property(e => e.QuantityReceived)
                    .HasColumnName("quantity_received")
                    .HasColumnType("int(5)");

                entity.Property(e => e.SerialNumber)
                    .HasColumnName("serial_number")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.TransferConfirmed)
                    .HasColumnName("transfer_confirmed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblInventoryTransfersItems6102105>(entity =>
            {
                entity.HasKey(e => e.InvtransitemId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_transfers_items_6102105");

                entity.Property(e => e.InvtransitemId)
                    .HasColumnName("invtransitemID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("timestamp");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvtransId)
                    .HasColumnName("invtransID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(5)");

                entity.Property(e => e.QuantityReceived)
                    .HasColumnName("quantity_received")
                    .HasColumnType("int(5)");

                entity.Property(e => e.SerialNumber)
                    .HasColumnName("serial_number")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.TransferConfirmed)
                    .HasColumnName("transfer_confirmed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblInventoryVendorPricing>(entity =>
            {
                entity.HasKey(e => e.PricingId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_vendor_pricing");

                entity.Property(e => e.PricingId)
                    .HasColumnName("pricingID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Inactive)
                    .HasColumnName("inactive")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UnitOfMeasure)
                    .HasColumnName("unit_of_measure")
                    .HasColumnType("varchar(2)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.VendorId)
                    .HasColumnName("vendorID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.VendorModel)
                    .HasColumnName("vendor_model")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.VendorPrice)
                    .HasColumnName("vendor_price")
                    .HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<TblInventoryWriteOffs>(entity =>
            {
                entity.HasKey(e => e.InvwoId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_write_offs");

                entity.Property(e => e.InvwoId)
                    .HasColumnName("invwoID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ApprovalNotes)
                    .HasColumnName("approval_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetAge)
                    .HasColumnName("asset_age")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Filename)
                    .HasColumnName("filename")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Hospice)
                    .IsRequired()
                    .HasColumnName("hospice")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvStatus)
                    .HasColumnName("inv_status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PatientName)
                    .IsRequired()
                    .HasColumnName("patient_name")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.ProcessDate)
                    .HasColumnName("process_date")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(5)");

                entity.Property(e => e.Reason)
                    .HasColumnName("reason")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.SerialNumber)
                    .HasColumnName("serial_number")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserIdProcessor)
                    .HasColumnName("userID_processor")
                    .HasColumnType("int(11)");

                entity.Property(e => e.WoStatus)
                    .HasColumnName("wo_status")
                    .HasColumnType("int(1)");
            });

            modelBuilder.Entity<TblInventoryWriteOffs6115>(entity =>
            {
                entity.HasKey(e => e.InvwoId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_write_offs_6115");

                entity.Property(e => e.InvwoId)
                    .HasColumnName("invwoID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ApprovalNotes)
                    .HasColumnName("approval_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetAge)
                    .HasColumnName("asset_age")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Filename)
                    .HasColumnName("filename")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Hospice)
                    .IsRequired()
                    .HasColumnName("hospice")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvStatus)
                    .HasColumnName("inv_status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PatientName)
                    .IsRequired()
                    .HasColumnName("patient_name")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.ProcessDate)
                    .HasColumnName("process_date")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(5)");

                entity.Property(e => e.Reason)
                    .HasColumnName("reason")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.SerialNumber)
                    .HasColumnName("serial_number")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserIdProcessor)
                    .HasColumnName("userID_processor")
                    .HasColumnType("int(11)");

                entity.Property(e => e.WoStatus)
                    .HasColumnName("wo_status")
                    .HasColumnType("int(1)");
            });

            modelBuilder.Entity<TblInventoryWriteOffsCopy>(entity =>
            {
                entity.HasKey(e => e.InvwoId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory_write_offs_copy");

                entity.Property(e => e.InvwoId)
                    .HasColumnName("invwoID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ApprovalNotes)
                    .HasColumnName("approval_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetAge)
                    .HasColumnName("asset_age")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Filename)
                    .HasColumnName("filename")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Hospice)
                    .IsRequired()
                    .HasColumnName("hospice")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvStatus)
                    .HasColumnName("inv_status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PatientName)
                    .IsRequired()
                    .HasColumnName("patient_name")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.ProcessDate)
                    .HasColumnName("process_date")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(5)");

                entity.Property(e => e.Reason)
                    .HasColumnName("reason")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.SerialNumber)
                    .HasColumnName("serial_number")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserIdProcessor)
                    .HasColumnName("userID_processor")
                    .HasColumnType("int(11)");

                entity.Property(e => e.WoStatus)
                    .HasColumnName("wo_status")
                    .HasColumnType("int(1)");
            });

            modelBuilder.Entity<TblInvoiceFiles>(entity =>
            {
                entity.HasKey(e => e.FileId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_invoice_files");

                entity.Property(e => e.FileId)
                    .HasColumnName("fileID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Filename)
                    .HasColumnName("filename")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<TblInvoices>(entity =>
            {
                entity.HasKey(e => e.InvoiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_invoices");

                entity.Property(e => e.InvoiceId)
                    .HasColumnName("invoiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CombinedInv)
                    .HasColumnName("combined_inv")
                    .HasColumnType("varchar(250)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DatemonthPrefix)
                    .HasColumnName("datemonth_prefix")
                    .HasColumnType("varchar(6)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.Filename)
                    .HasColumnName("filename")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NonPerDiem)
                    .HasColumnName("non_per_diem")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.PatientDays)
                    .HasColumnName("patient_days")
                    .HasColumnType("int(10)");

                entity.Property(e => e.PerDiem)
                    .HasColumnName("per_diem")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Utilization)
                    .HasColumnName("utilization")
                    .HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<TblInvoicesCopy>(entity =>
            {
                entity.HasKey(e => e.InvoiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_invoices_copy");

                entity.Property(e => e.InvoiceId)
                    .HasColumnName("invoiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CombinedInv)
                    .HasColumnName("combined_inv")
                    .HasColumnType("varchar(250)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DatemonthPrefix)
                    .HasColumnName("datemonth_prefix")
                    .HasColumnType("varchar(6)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.Filename)
                    .HasColumnName("filename")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NonPerDiem)
                    .HasColumnName("non_per_diem")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.PatientDays)
                    .HasColumnName("patient_days")
                    .HasColumnType("int(10)");

                entity.Property(e => e.PerDiem)
                    .HasColumnName("per_diem")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Utilization)
                    .HasColumnName("utilization")
                    .HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<TblInvoicesCopy1>(entity =>
            {
                entity.HasKey(e => e.InvoiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_invoices_copy1");

                entity.Property(e => e.InvoiceId)
                    .HasColumnName("invoiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CombinedInv)
                    .HasColumnName("combined_inv")
                    .HasColumnType("varchar(250)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DatemonthPrefix)
                    .HasColumnName("datemonth_prefix")
                    .HasColumnType("varchar(6)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.Filename)
                    .HasColumnName("filename")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NonPerDiem)
                    .HasColumnName("non_per_diem")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.PatientDays)
                    .HasColumnName("patient_days")
                    .HasColumnType("int(10)");

                entity.Property(e => e.PerDiem)
                    .HasColumnName("per_diem")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Utilization)
                    .HasColumnName("utilization")
                    .HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<TblInvoicesLocation>(entity =>
            {
                entity.HasKey(e => e.InvoicelocId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_invoices_location");

                entity.HasIndex(e => e.EndDate)
                    .HasName("end_date_index");

                entity.HasIndex(e => e.HospiceId)
                    .HasName("hospice_index");

                entity.HasIndex(e => e.InvoicelocId)
                    .HasName("invoiceloc_index");

                entity.HasIndex(e => e.StartDate)
                    .HasName("start_date_index");

                entity.Property(e => e.InvoicelocId)
                    .HasColumnName("invoicelocID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Credit)
                    .HasColumnName("credit")
                    .HasColumnType("int(1)");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DatemonthPrefix)
                    .HasColumnName("datemonth_prefix")
                    .HasColumnType("varchar(6)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NonPerDiem)
                    .HasColumnName("non_per_diem")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.PatientDays)
                    .HasColumnName("patient_days")
                    .HasColumnType("int(10)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PerDiem)
                    .HasColumnName("per_diem")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Utilization)
                    .HasColumnName("utilization")
                    .HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<TblInvoicesLocationCopy>(entity =>
            {
                entity.HasKey(e => e.InvoicelocId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_invoices_location_copy");

                entity.Property(e => e.InvoicelocId)
                    .HasColumnName("invoicelocID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DatemonthPrefix)
                    .HasColumnName("datemonth_prefix")
                    .HasColumnType("varchar(6)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NonPerDiem)
                    .HasColumnName("non_per_diem")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.PatientDays)
                    .HasColumnName("patient_days")
                    .HasColumnType("int(10)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PerDiem)
                    .HasColumnName("per_diem")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Utilization)
                    .HasColumnName("utilization")
                    .HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<TblInvoicesLocationCopy1>(entity =>
            {
                entity.HasKey(e => e.InvoicelocId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_invoices_location_copy1");

                entity.Property(e => e.InvoicelocId)
                    .HasColumnName("invoicelocID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DatemonthPrefix)
                    .HasColumnName("datemonth_prefix")
                    .HasColumnType("varchar(6)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NonPerDiem)
                    .HasColumnName("non_per_diem")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.PatientDays)
                    .HasColumnName("patient_days")
                    .HasColumnType("int(10)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PerDiem)
                    .HasColumnName("per_diem")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Utilization)
                    .HasColumnName("utilization")
                    .HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<TblInvoicesLocationCopy2>(entity =>
            {
                entity.HasKey(e => e.InvoicelocId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_invoices_location_copy2");

                entity.Property(e => e.InvoicelocId)
                    .HasColumnName("invoicelocID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DatemonthPrefix)
                    .HasColumnName("datemonth_prefix")
                    .HasColumnType("varchar(6)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NonPerDiem)
                    .HasColumnName("non_per_diem")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.PatientDays)
                    .HasColumnName("patient_days")
                    .HasColumnType("int(10)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PerDiem)
                    .HasColumnName("per_diem")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Utilization)
                    .HasColumnName("utilization")
                    .HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<TblInvoicesLocationold>(entity =>
            {
                entity.HasKey(e => e.InvoicelocId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_invoices_locationold");

                entity.Property(e => e.InvoicelocId)
                    .HasColumnName("invoicelocID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DatemonthPrefix)
                    .HasColumnName("datemonth_prefix")
                    .HasColumnType("varchar(6)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NonPerDiem)
                    .HasColumnName("non_per_diem")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.PatientDays)
                    .HasColumnName("patient_days")
                    .HasColumnType("int(10)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PerDiem)
                    .HasColumnName("per_diem")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Utilization)
                    .HasColumnName("utilization")
                    .HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<TblInvoicesLocationolder>(entity =>
            {
                entity.HasKey(e => e.InvoicelocId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_invoices_locationolder");

                entity.Property(e => e.InvoicelocId)
                    .HasColumnName("invoicelocID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DatemonthPrefix)
                    .HasColumnName("datemonth_prefix")
                    .HasColumnType("varchar(6)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NonPerDiem)
                    .HasColumnName("non_per_diem")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.PatientDays)
                    .HasColumnName("patient_days")
                    .HasColumnType("int(10)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PerDiem)
                    .HasColumnName("per_diem")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Utilization)
                    .HasColumnName("utilization")
                    .HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<TblItemsFiles>(entity =>
            {
                entity.HasKey(e => e.FileId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_items_files");

                entity.Property(e => e.FileId)
                    .HasColumnName("fileID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Filename)
                    .HasColumnName("filename")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<TblLocationInvQty>(entity =>
            {
                entity.HasKey(e => e.InvcountId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_location_inv_qty");

                entity.Property(e => e.InvcountId)
                    .HasColumnName("invcountID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.QtyStatus)
                    .HasColumnName("qty_status")
                    .HasColumnType("int(1)");
            });

            modelBuilder.Entity<TblLocationInventory>(entity =>
            {
                entity.HasKey(e => e.LocationInvId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_location_inventory");

                entity.Property(e => e.LocationInvId)
                    .HasColumnName("location_invID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTags)
                    .HasColumnName("asset_tags")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Processed)
                    .HasColumnName("processed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ScanType)
                    .HasColumnName("scan_type")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SerialNumbers)
                    .HasColumnName("serial_numbers")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.TagsProcessed)
                    .HasColumnName("tags_processed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");
            });

            modelBuilder.Entity<TblLocationMapping>(entity =>
            {
                entity.HasKey(e => e.ZipId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_location_mapping");

                entity.Property(e => e.ZipId)
                    .HasColumnName("zipID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Lat)
                    .HasColumnName("lat")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.LocationName)
                    .HasColumnName("location_name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Long)
                    .HasColumnName("long")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.ZipCode)
                    .HasColumnName("zip_code")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<TblLocations>(entity =>
            {
                entity.HasKey(e => e.LocationId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_locations");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ExcludeReports)
                    .HasColumnName("exclude_reports")
                    .HasColumnType("int(1)");

                entity.Property(e => e.H2hRev)
                    .HasColumnName("H2H_rev")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Inactive)
                    .HasColumnName("inactive")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Invemail)
                    .HasColumnName("invemail")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LegacyMitsDivision)
                    .HasColumnName("legacy_MITS_division")
                    .HasColumnType("varchar(5)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LocationAddress)
                    .HasColumnName("location_address")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LocationCity)
                    .HasColumnName("location_city")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LocationCode)
                    .HasColumnName("location_code")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LocationName)
                    .HasColumnName("location_name")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LocationPhone)
                    .HasColumnName("location_phone")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LocationState)
                    .HasColumnName("location_state")
                    .HasColumnType("varchar(2)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LocationZip)
                    .HasColumnName("location_zip")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.NoPi)
                    .HasColumnName("no_pi")
                    .HasColumnType("int(1)");

                entity.Property(e => e.NoShowRevReport)
                    .HasColumnName("no_show_rev_report")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PccrAssignment)
                    .IsRequired()
                    .HasColumnName("pccr_assignment")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PccrEmailNotifications)
                    .IsRequired()
                    .HasColumnName("pccr_email_notifications")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Timezone)
                    .HasColumnName("timezone")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'-6'");

                entity.Property(e => e.Vehicle)
                    .HasColumnName("vehicle")
                    .HasColumnType("int(1)");

                entity.Property(e => e.VehicleLocation)
                    .HasColumnName("vehicle_location")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblLoginHistory>(entity =>
            {
                entity.HasKey(e => e.HistoryId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_login_history");

                entity.HasIndex(e => e.Timestamp)
                    .HasName("timestamp");

                entity.HasIndex(e => e.UserId)
                    .HasName("userID");

                entity.Property(e => e.HistoryId)
                    .HasColumnName("historyID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IpAddress)
                    .HasColumnName("ip_address")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblMaintenanceNotes>(entity =>
            {
                entity.HasKey(e => e.MnoteId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_maintenance_notes");

                entity.Property(e => e.MnoteId)
                    .HasColumnName("mnoteID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MainNote)
                    .HasColumnName("main_note")
                    .HasColumnType("text")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.NoteSubmitted)
                    .HasColumnName("note_submitted")
                    .HasColumnType("timestamp");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblMessageViews>(entity =>
            {
                entity.HasKey(e => e.ViewId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_message_views");

                entity.Property(e => e.ViewId)
                    .HasColumnName("viewID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MessageId)
                    .HasColumnName("messageID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblMessages>(entity =>
            {
                entity.HasKey(e => e.MessageId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_messages");

                entity.Property(e => e.MessageId)
                    .HasColumnName("messageID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MessageBody)
                    .HasColumnName("message_body")
                    .HasColumnType("text")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.MessageEnd)
                    .HasColumnName("message_end")
                    .HasColumnType("date");

                entity.Property(e => e.MessageStatus)
                    .HasColumnName("message_status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.MessageTitle)
                    .HasColumnName("message_title")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<TblMessagesCopy>(entity =>
            {
                entity.HasKey(e => e.MessageId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_messages_copy");

                entity.Property(e => e.MessageId)
                    .HasColumnName("messageID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MessageBody)
                    .HasColumnName("message_body")
                    .HasColumnType("text")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.MessageEnd)
                    .HasColumnName("message_end")
                    .HasColumnType("date");

                entity.Property(e => e.MessageStatus)
                    .HasColumnName("message_status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.MessageTitle)
                    .HasColumnName("message_title")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<TblNurses>(entity =>
            {
                entity.HasKey(e => e.NurseId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_nurses");

                entity.Property(e => e.NurseId)
                    .HasColumnName("nurseID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NurseEmail)
                    .HasColumnName("nurse_email")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.NurseName)
                    .HasColumnName("nurse_name")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.NursePhone)
                    .HasColumnName("nurse_phone")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<TblOffCapSummary>(entity =>
            {
                entity.HasKey(e => e.OffcapId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_off_cap_summary");

                entity.Property(e => e.OffcapId)
                    .HasColumnName("offcapID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AutoApproval)
                    .HasColumnName("auto_approval")
                    .HasColumnType("int(11)");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NumDays)
                    .HasColumnName("num_days")
                    .HasColumnType("int(3)");

                entity.Property(e => e.OffCapApprovalUserId)
                    .HasColumnName("off_cap_approval_userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OffCapCost)
                    .HasColumnName("off_cap_cost")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.OffCapMonth)
                    .HasColumnName("off_cap_month")
                    .HasColumnType("varchar(5)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.OffCapYear)
                    .HasColumnName("off_cap_year")
                    .HasColumnType("varchar(5)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OrderedBy)
                    .HasColumnName("ordered_by")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblOrderStats>(entity =>
            {
                entity.HasKey(e => e.SummaryId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_order_stats");

                entity.Property(e => e.SummaryId)
                    .HasColumnName("summaryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.HospiceQuery)
                    .HasColumnName("hospice_query")
                    .HasColumnType("text")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.ReportGeo)
                    .HasColumnName("report_geo")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.SourceCount)
                    .HasColumnName("source_count")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SourceMonth)
                    .HasColumnName("source_month")
                    .HasColumnType("varchar(5)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.SourceType)
                    .HasColumnName("source_type")
                    .HasColumnType("int(1)");

                entity.Property(e => e.SourceYear)
                    .HasColumnName("source_year")
                    .HasColumnType("varchar(5)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<TblOrderUpdates>(entity =>
            {
                entity.HasKey(e => e.OrderupdateId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_order_updates");

                entity.HasIndex(e => e.Cancel)
                    .HasName("Cancel");

                entity.HasIndex(e => e.OrderId)
                    .HasName("tbl_order_updates_orderid_idx");

                entity.Property(e => e.OrderupdateId)
                    .HasColumnName("orderupdateID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.BopOrder)
                    .HasColumnName("bop_order")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Cancel)
                    .HasColumnName("cancel")
                    .HasColumnType("int(1)");

                entity.Property(e => e.CancelNotes)
                    .HasColumnName("cancel_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Delivery)
                    .HasColumnName("delivery")
                    .HasColumnType("int(1)");

                entity.Property(e => e.IncDelivery)
                    .HasColumnName("inc_delivery")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OffCapApproval)
                    .HasColumnName("off_cap_approval")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OffCapAutoApproval)
                    .HasColumnName("off_cap_auto_approval")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OffCapCancel)
                    .HasColumnName("off_cap_cancel")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OffcapComments)
                    .HasColumnName("offcap_comments")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.OrderDate)
                    .HasColumnName("order_date")
                    .HasColumnType("date");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Pickup)
                    .HasColumnName("pickup")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.TransitionOrder)
                    .HasColumnName("transition_order")
                    .HasColumnType("int(1)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblOrders>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_orders");

                entity.HasIndex(e => e.AckEmailSent)
                    .HasName("ack_email_sent");

                entity.HasIndex(e => e.AutoAckTimestamp)
                    .HasName("auto_ack_timestamp");

                entity.HasIndex(e => e.AutoApprovalTimestamp)
                    .HasName("auto_approval_timestamp");

                entity.HasIndex(e => e.CancelledTimestamp)
                    .HasName("cancelled_timestamp");

                entity.HasIndex(e => e.DeliveredTimestamp)
                    .HasName("delivered_timestamp");

                entity.HasIndex(e => e.DispatchTimestamp)
                    .HasName("dispatch_timestamp");

                entity.HasIndex(e => e.DmeAcknowledgement)
                    .HasName("dme_acknowledgement");

                entity.HasIndex(e => e.DmeId)
                    .HasName("dmeID");

                entity.HasIndex(e => e.DriverId)
                    .HasName("driverID");

                entity.HasIndex(e => e.HospiceId)
                    .HasName("hospiceID");

                entity.HasIndex(e => e.MigrationOrder)
                    .HasName("migration_order");

                entity.HasIndex(e => e.MoveId)
                    .HasName("moveID");

                entity.HasIndex(e => e.OrderDate)
                    .HasName("order_date");

                entity.HasIndex(e => e.PatientId)
                    .HasName("patientID");

                entity.HasIndex(e => e.PickedupTimestamp)
                    .HasName("pickedup_timestamp");

                entity.HasIndex(e => e.Status)
                    .HasName("status");

                entity.HasIndex(e => e.Timestamp)
                    .HasName("timestamp");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AckComments)
                    .HasColumnName("ack_comments")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.AckEmailSent)
                    .HasColumnName("ack_email_sent")
                    .HasColumnType("int(1)");

                entity.Property(e => e.AppComments)
                    .HasColumnName("app_comments")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ApprCustomer)
                    .HasColumnName("appr_customer")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AppuserId)
                    .HasColumnName("appuserID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AutoAckTimestamp)
                    .HasColumnName("auto_ack_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.AutoApprovalTimestamp)
                    .HasColumnName("auto_approval_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.BopOrder)
                    .HasColumnName("bop_order")
                    .HasColumnType("int(1)");

                entity.Property(e => e.BypassApproval)
                    .HasColumnName("bypass_approval")
                    .HasColumnType("int(1)");

                entity.Property(e => e.BypassApprovalNotes)
                    .HasColumnName("bypass_approval_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CancelledTimestamp)
                    .HasColumnName("cancelled_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.DelAddress)
                    .HasColumnName("del_address")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DeliveredTimestamp)
                    .HasColumnName("delivered_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.DeliveryNotes)
                    .HasColumnName("delivery_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DeliveryTiming)
                    .HasColumnName("delivery_timing")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DeliveryType)
                    .HasColumnName("delivery_type")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DispatchTimestamp)
                    .HasColumnName("dispatch_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.DmeAcknowledgement)
                    .HasColumnName("dme_acknowledgement")
                    .HasColumnType("timestamp");

                entity.Property(e => e.DmeId)
                    .HasColumnName("dmeID")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DmeuserId)
                    .HasColumnName("dmeuserID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DriverId)
                    .HasColumnName("driverID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Exchange)
                    .HasColumnName("exchange")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ExchangeOrderId)
                    .HasColumnName("exchange_orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ExchangeReason)
                    .HasColumnName("exchange_reason")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FacilityName)
                    .HasColumnName("facility_name")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FutureDate)
                    .HasColumnName("future_date")
                    .HasColumnType("date");

                entity.Property(e => e.FutureTimeRange)
                    .HasColumnName("future_time_range")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.HospDischarge)
                    .HasColumnName("hosp_discharge")
                    .HasColumnType("int(1)");

                entity.Property(e => e.HospDischargeTime)
                    .HasColumnName("hosp_discharge_time")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.HsApproval1)
                    .HasColumnName("hs_approval")
                    .HasColumnType("timestamp");

                entity.Property(e => e.HsAutoApproval)
                    .HasColumnName("hs_auto_approval")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Hsapproval)
                    .HasColumnName("hsapproval")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Instructions)
                    .HasColumnName("instructions")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.InvProcessed)
                    .HasColumnName("inv_processed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.MailCustomer)
                    .HasColumnName("mail_customer")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MigrationOrder)
                    .HasColumnName("migration_order")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ModAutoApproval)
                    .HasColumnName("mod_auto_approval")
                    .HasColumnType("int(1)");

                entity.Property(e => e.MoveId)
                    .HasColumnName("moveID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MoveReason)
                    .HasColumnName("move_reason")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OffCapApproval)
                    .HasColumnName("off_cap_approval")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OffCapApprovalOrgUserId)
                    .HasColumnName("off_cap_approval_org_userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OffCapApprovalUserId)
                    .HasColumnName("off_cap_approval_userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OffCapArray)
                    .HasColumnName("off_cap_array")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OffCapAutoApproval)
                    .HasColumnName("off_cap_auto_approval")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OffCapOrder)
                    .HasColumnName("off_cap_order")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OrderArray)
                    .IsRequired()
                    .HasColumnName("order_array")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OrderDate)
                    .HasColumnName("order_date")
                    .HasColumnType("date");

                entity.Property(e => e.OrderSource)
                    .HasColumnName("order_source")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OrderingNurse)
                    .HasColumnName("ordering_nurse")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OrderingNursePhone)
                    .HasColumnName("ordering_nurse_phone")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OrgOrderArray)
                    .HasColumnName("org_order_array")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OrgOrderDate)
                    .HasColumnName("org_order_date")
                    .HasColumnType("date");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PickedupTimestamp)
                    .HasColumnName("pickedup_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Pickup)
                    .HasColumnName("pickup")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PickupReason)
                    .HasColumnName("pickup_reason")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.PickupType)
                    .HasColumnName("pickup_type")
                    .HasColumnType("int(1)");

                entity.Property(e => e.RekeyOrder)
                    .HasColumnName("rekey_order")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Respite)
                    .HasColumnName("respite")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.TimingOverride)
                    .HasColumnName("timing_override")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TimingOverrideNotes)
                    .HasColumnName("timing_override_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TransitionOrder)
                    .HasColumnName("transition_order")
                    .HasColumnType("int(1)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblOrdersCopy1>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_orders_copy1");

                entity.HasIndex(e => e.AckEmailSent)
                    .HasName("ack_email_sent");

                entity.HasIndex(e => e.AutoAckTimestamp)
                    .HasName("auto_ack_timestamp");

                entity.HasIndex(e => e.AutoApprovalTimestamp)
                    .HasName("auto_approval_timestamp");

                entity.HasIndex(e => e.CancelledTimestamp)
                    .HasName("cancelled_timestamp");

                entity.HasIndex(e => e.DeliveredTimestamp)
                    .HasName("delivered_timestamp");

                entity.HasIndex(e => e.DispatchTimestamp)
                    .HasName("dispatch_timestamp");

                entity.HasIndex(e => e.DmeAcknowledgement)
                    .HasName("dme_acknowledgement");

                entity.HasIndex(e => e.DmeId)
                    .HasName("dmeID");

                entity.HasIndex(e => e.DriverId)
                    .HasName("driverID");

                entity.HasIndex(e => e.HospiceId)
                    .HasName("hospiceID");

                entity.HasIndex(e => e.MigrationOrder)
                    .HasName("migration_order");

                entity.HasIndex(e => e.MoveId)
                    .HasName("moveID");

                entity.HasIndex(e => e.OrderDate)
                    .HasName("order_date");

                entity.HasIndex(e => e.PatientId)
                    .HasName("patientID");

                entity.HasIndex(e => e.PickedupTimestamp)
                    .HasName("pickedup_timestamp");

                entity.HasIndex(e => e.Status)
                    .HasName("status");

                entity.HasIndex(e => e.Timestamp)
                    .HasName("timestamp");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AckComments)
                    .HasColumnName("ack_comments")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.AckEmailSent)
                    .HasColumnName("ack_email_sent")
                    .HasColumnType("int(1)");

                entity.Property(e => e.AppComments)
                    .HasColumnName("app_comments")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ApprCustomer)
                    .HasColumnName("appr_customer")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AppuserId)
                    .HasColumnName("appuserID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AutoAckTimestamp)
                    .HasColumnName("auto_ack_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.AutoApprovalTimestamp)
                    .HasColumnName("auto_approval_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.BopOrder)
                    .HasColumnName("bop_order")
                    .HasColumnType("int(1)");

                entity.Property(e => e.BypassApproval)
                    .HasColumnName("bypass_approval")
                    .HasColumnType("int(1)");

                entity.Property(e => e.BypassApprovalNotes)
                    .HasColumnName("bypass_approval_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CancelledTimestamp)
                    .HasColumnName("cancelled_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.DelAddress)
                    .HasColumnName("del_address")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DeliveredTimestamp)
                    .HasColumnName("delivered_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.DeliveryNotes)
                    .HasColumnName("delivery_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DeliveryTiming)
                    .HasColumnName("delivery_timing")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DeliveryType)
                    .HasColumnName("delivery_type")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DispatchTimestamp)
                    .HasColumnName("dispatch_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.DmeAcknowledgement)
                    .HasColumnName("dme_acknowledgement")
                    .HasColumnType("timestamp");

                entity.Property(e => e.DmeId)
                    .HasColumnName("dmeID")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DmeuserId)
                    .HasColumnName("dmeuserID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DriverId)
                    .HasColumnName("driverID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Exchange)
                    .HasColumnName("exchange")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ExchangeOrderId)
                    .HasColumnName("exchange_orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ExchangeReason)
                    .HasColumnName("exchange_reason")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FacilityName)
                    .HasColumnName("facility_name")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FutureDate)
                    .HasColumnName("future_date")
                    .HasColumnType("date");

                entity.Property(e => e.FutureTimeRange)
                    .HasColumnName("future_time_range")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.HospDischarge)
                    .HasColumnName("hosp_discharge")
                    .HasColumnType("int(1)");

                entity.Property(e => e.HospDischargeTime)
                    .HasColumnName("hosp_discharge_time")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.HsApproval1)
                    .HasColumnName("hs_approval")
                    .HasColumnType("timestamp");

                entity.Property(e => e.HsAutoApproval)
                    .HasColumnName("hs_auto_approval")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Hsapproval)
                    .HasColumnName("hsapproval")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Instructions)
                    .HasColumnName("instructions")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.InvProcessed)
                    .HasColumnName("inv_processed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.MailCustomer)
                    .HasColumnName("mail_customer")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MigrationOrder)
                    .HasColumnName("migration_order")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ModAutoApproval)
                    .HasColumnName("mod_auto_approval")
                    .HasColumnType("int(1)");

                entity.Property(e => e.MoveId)
                    .HasColumnName("moveID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MoveReason)
                    .HasColumnName("move_reason")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OffCapApproval)
                    .HasColumnName("off_cap_approval")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OffCapApprovalOrgUserId)
                    .HasColumnName("off_cap_approval_org_userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OffCapApprovalUserId)
                    .HasColumnName("off_cap_approval_userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OffCapArray)
                    .HasColumnName("off_cap_array")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OffCapAutoApproval)
                    .HasColumnName("off_cap_auto_approval")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OffCapOrder)
                    .HasColumnName("off_cap_order")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OrderArray)
                    .IsRequired()
                    .HasColumnName("order_array")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OrderDate)
                    .HasColumnName("order_date")
                    .HasColumnType("date");

                entity.Property(e => e.OrderSource)
                    .HasColumnName("order_source")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OrderingNurse)
                    .HasColumnName("ordering_nurse")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OrderingNursePhone)
                    .HasColumnName("ordering_nurse_phone")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OrgOrderArray)
                    .HasColumnName("org_order_array")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OrgOrderDate)
                    .HasColumnName("org_order_date")
                    .HasColumnType("date");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PickedupTimestamp)
                    .HasColumnName("pickedup_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Pickup)
                    .HasColumnName("pickup")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PickupReason)
                    .HasColumnName("pickup_reason")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.PickupType)
                    .HasColumnName("pickup_type")
                    .HasColumnType("int(1)");

                entity.Property(e => e.RekeyOrder)
                    .HasColumnName("rekey_order")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Respite)
                    .HasColumnName("respite")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.TimingOverride)
                    .HasColumnName("timing_override")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TimingOverrideNotes)
                    .HasColumnName("timing_override_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TransitionOrder)
                    .HasColumnName("transition_order")
                    .HasColumnType("int(1)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblOrdersVersions>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tbl_orders_versions");

                entity.HasIndex(e => e.OrderId)
                    .HasName("orderID");

                entity.HasIndex(e => e.PatientId)
                    .HasName("patientID");

                entity.Property(e => e.AckComments)
                    .HasColumnName("ack_comments")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.AckEmailSent)
                    .HasColumnName("ack_email_sent")
                    .HasColumnType("int(1)");

                entity.Property(e => e.AppComments)
                    .HasColumnName("app_comments")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ApprCustomer)
                    .HasColumnName("appr_customer")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AppuserId)
                    .HasColumnName("appuserID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AutoAckTimestamp)
                    .HasColumnName("auto_ack_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.AutoApprovalTimestamp)
                    .HasColumnName("auto_approval_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.BopOrder)
                    .HasColumnName("bop_order")
                    .HasColumnType("int(1)");

                entity.Property(e => e.BypassApproval)
                    .HasColumnName("bypass_approval")
                    .HasColumnType("int(1)");

                entity.Property(e => e.BypassApprovalNotes)
                    .HasColumnName("bypass_approval_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CancelledTimestamp)
                    .HasColumnName("cancelled_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.DelAddress)
                    .HasColumnName("del_address")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DeliveredTimestamp)
                    .HasColumnName("delivered_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.DeliveryNotes)
                    .HasColumnName("delivery_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DeliveryTiming)
                    .HasColumnName("delivery_timing")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DeliveryType)
                    .HasColumnName("delivery_type")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DispatchTimestamp)
                    .HasColumnName("dispatch_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.DmeAcknowledgement)
                    .HasColumnName("dme_acknowledgement")
                    .HasColumnType("timestamp");

                entity.Property(e => e.DmeId)
                    .HasColumnName("dmeID")
                    .HasColumnType("int(1)");

                entity.Property(e => e.DmeuserId)
                    .HasColumnName("dmeuserID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DriverId)
                    .HasColumnName("driverID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Exchange)
                    .HasColumnName("exchange")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ExchangeOrderId)
                    .HasColumnName("exchange_orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ExchangeReason)
                    .HasColumnName("exchange_reason")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FacilityName)
                    .HasColumnName("facility_name")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FutureDate)
                    .HasColumnName("future_date")
                    .HasColumnType("date");

                entity.Property(e => e.FutureTimeRange)
                    .HasColumnName("future_time_range")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.HospDischarge)
                    .HasColumnName("hosp_discharge")
                    .HasColumnType("int(1)");

                entity.Property(e => e.HospDischargeTime)
                    .HasColumnName("hosp_discharge_time")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.HsApproval1)
                    .HasColumnName("hs_approval")
                    .HasColumnType("timestamp");

                entity.Property(e => e.HsAutoApproval)
                    .HasColumnName("hs_auto_approval")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Hsapproval)
                    .HasColumnName("hsapproval")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Instructions)
                    .HasColumnName("instructions")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.InvProcessed)
                    .HasColumnName("inv_processed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.MailCustomer)
                    .HasColumnName("mail_customer")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MigrationOrder)
                    .HasColumnName("migration_order")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ModAutoApproval)
                    .HasColumnName("mod_auto_approval")
                    .HasColumnType("int(1)");

                entity.Property(e => e.MoveId)
                    .HasColumnName("moveID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MoveReason)
                    .HasColumnName("move_reason")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OffCapApproval)
                    .HasColumnName("off_cap_approval")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OffCapApprovalOrgUserId)
                    .HasColumnName("off_cap_approval_org_userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OffCapApprovalUserId)
                    .HasColumnName("off_cap_approval_userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OffCapArray)
                    .HasColumnName("off_cap_array")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OffCapAutoApproval)
                    .HasColumnName("off_cap_auto_approval")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OffCapOrder)
                    .HasColumnName("off_cap_order")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OrderArray)
                    .IsRequired()
                    .HasColumnName("order_array")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OrderDate)
                    .HasColumnName("order_date")
                    .HasColumnType("date");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OrderSource)
                    .HasColumnName("order_source")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OrderingNurse)
                    .HasColumnName("ordering_nurse")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OrderingNursePhone)
                    .HasColumnName("ordering_nurse_phone")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OrgOrderArray)
                    .HasColumnName("org_order_array")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OrgOrderDate)
                    .HasColumnName("org_order_date")
                    .HasColumnType("date");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PickedupTimestamp)
                    .HasColumnName("pickedup_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Pickup)
                    .HasColumnName("pickup")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PickupReason)
                    .HasColumnName("pickup_reason")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.PickupType)
                    .HasColumnName("pickup_type")
                    .HasColumnType("int(1)");

                entity.Property(e => e.RekeyOrder)
                    .HasColumnName("rekey_order")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Respite)
                    .HasColumnName("respite")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.TransitionOrder)
                    .HasColumnName("transition_order")
                    .HasColumnType("int(1)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblPatientBilling>(entity =>
            {
                entity.HasKey(e => e.PatientbillingId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_patient_billing");

                entity.Property(e => e.PatientbillingId)
                    .HasColumnName("patientbillingID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.BillingItem)
                    .HasColumnName("billing_item")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.BillingNote)
                    .HasColumnName("billing_note")
                    .HasColumnType("text")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.BillingPurchaseAmt)
                    .HasColumnName("billing_purchase_amt")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.BillingSaleAmt)
                    .HasColumnName("billing_sale_amt")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.BillingSaleDate)
                    .HasColumnName("billing_sale_date")
                    .HasColumnType("date");

                entity.Property(e => e.BillingSubmitted)
                    .HasColumnName("billing_submitted")
                    .HasColumnType("timestamp");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblPatientBillingGaps>(entity =>
            {
                entity.HasKey(e => e.PatientbillinggapId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_patient_billing_gaps");

                entity.Property(e => e.PatientbillinggapId)
                    .HasColumnName("patientbillinggapID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.BillingGapEndDate)
                    .HasColumnName("billing_gap_end_date")
                    .HasColumnType("date");

                entity.Property(e => e.BillingGapStartDate)
                    .HasColumnName("billing_gap_start_date")
                    .HasColumnType("date");

                entity.Property(e => e.BillingNote)
                    .HasColumnName("billing_note")
                    .HasColumnType("text")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.BillingSubmitted)
                    .HasColumnName("billing_submitted")
                    .HasColumnType("timestamp");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblPatientDeliveryCharges>(entity =>
            {
                entity.HasKey(e => e.PatientdeliverychargeId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_patient_delivery_charges");

                entity.Property(e => e.PatientdeliverychargeId)
                    .HasColumnName("patientdeliverychargeID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ChargeAmount)
                    .HasColumnName("charge_amount")
                    .HasColumnType("decimal(5,2)");

                entity.Property(e => e.ChargeEndDate)
                    .HasColumnName("charge_end_date")
                    .HasColumnType("date");

                entity.Property(e => e.ChargeNote)
                    .HasColumnName("charge_note")
                    .HasColumnType("text")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.ChargeStartDate)
                    .HasColumnName("charge_start_date")
                    .HasColumnType("date");

                entity.Property(e => e.DoNotBill)
                    .HasColumnName("do_not_bill")
                    .HasColumnType("int(1)");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OrderType)
                    .HasColumnName("order_type")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblPatientMoves>(entity =>
            {
                entity.HasKey(e => e.MoveId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_patient_moves");

                entity.HasIndex(e => e.LocationId)
                    .HasName("locationID");

                entity.HasIndex(e => e.MoveId)
                    .HasName("moveID");

                entity.HasIndex(e => e.OrderIdPickup)
                    .HasName("orderID_pickup");

                entity.Property(e => e.MoveId)
                    .HasColumnName("moveID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasColumnName("address_1")
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Address2)
                    .IsRequired()
                    .HasColumnName("address_2")
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.FacilityId)
                    .HasColumnName("facilityID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationType)
                    .HasColumnName("location_type")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.MoveDate)
                    .HasColumnName("move_date")
                    .HasColumnType("timestamp");

                entity.Property(e => e.OrderIdDelivery)
                    .HasColumnName("orderID_delivery")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OrderIdPickup)
                    .HasColumnName("orderID_pickup")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Phone1)
                    .HasColumnName("phone_1")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Phone2)
                    .HasColumnName("phone_2")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Zip)
                    .HasColumnName("zip")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<TblPatientRespite>(entity =>
            {
                entity.HasKey(e => e.RespiteId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_patient_respite");

                entity.HasIndex(e => e.OrderIdDelivery)
                    .HasName("orderID_delivery");

                entity.HasIndex(e => e.OrderIdPickup)
                    .HasName("orderID_pickup");

                entity.Property(e => e.RespiteId)
                    .HasColumnName("respiteID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasColumnName("address_1")
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Address2)
                    .IsRequired()
                    .HasColumnName("address_2")
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.ContactPerson)
                    .HasColumnName("contact_person")
                    .HasColumnType("text")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.MoveDate)
                    .HasColumnName("move_date")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.OrderIdDelivery)
                    .HasColumnName("orderID_delivery")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OrderIdPickup)
                    .HasColumnName("orderID_pickup")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Phone1)
                    .HasColumnName("phone_1")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Zip)
                    .HasColumnName("zip")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<TblPatientUpdates>(entity =>
            {
                entity.HasKey(e => e.UpdateId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_patient_updates");

                entity.Property(e => e.UpdateId)
                    .HasColumnName("updateID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblPatients>(entity =>
            {
                entity.HasKey(e => e.PatientId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_patients");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedTimestamp)
                    .HasColumnName("created_timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.HospiceBillingId)
                    .HasColumnName("hospice_billing_ID")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PatientAddress)
                    .HasColumnName("patient_address")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PatientCity)
                    .HasColumnName("patient_city")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PatientFirst)
                    .HasColumnName("patient_first")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PatientLast)
                    .HasColumnName("patient_last")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PatientState)
                    .HasColumnName("patient_state")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PatientZip)
                    .HasColumnName("patient_zip")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UpdateTimestamp)
                    .HasColumnName("update_timestamp")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<TblPhysicalInventory>(entity =>
            {
                entity.HasKey(e => e.PhysicalinvId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_physical_inventory");

                entity.Property(e => e.PhysicalinvId)
                    .HasColumnName("physicalinvID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTags)
                    .HasColumnName("asset_tags")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvCount)
                    .HasColumnName("inv_count")
                    .HasColumnType("int(5)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Processed)
                    .HasColumnName("processed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.SerialNumbers)
                    .HasColumnName("serial_numbers")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Submitted)
                    .HasColumnName("submitted")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblPhysicalInventory71015>(entity =>
            {
                entity.HasKey(e => e.PhysicalinvId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_physical_inventory_71015");

                entity.Property(e => e.PhysicalinvId)
                    .HasColumnName("physicalinvID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTags)
                    .HasColumnName("asset_tags")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvCount)
                    .HasColumnName("inv_count")
                    .HasColumnType("int(5)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SerialNumbers)
                    .HasColumnName("serial_numbers")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Submitted)
                    .HasColumnName("submitted")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblPhysicalInventoryCompleted>(entity =>
            {
                entity.HasKey(e => e.ComppiId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_physical_inventory_completed");

                entity.Property(e => e.ComppiId)
                    .HasColumnName("comppiID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblPhysicalInventoryCompletedCopy>(entity =>
            {
                entity.HasKey(e => e.ComppiId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_physical_inventory_completed_copy");

                entity.Property(e => e.ComppiId)
                    .HasColumnName("comppiID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblPhysicalInventoryCompletedCopy1>(entity =>
            {
                entity.HasKey(e => e.ComppiId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_physical_inventory_completed_copy1");

                entity.Property(e => e.ComppiId)
                    .HasColumnName("comppiID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblPhysicalInventoryCompletedCopy2>(entity =>
            {
                entity.HasKey(e => e.ComppiId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_physical_inventory_completed_copy2");

                entity.Property(e => e.ComppiId)
                    .HasColumnName("comppiID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblPhysicalInventoryCompletedCopy3>(entity =>
            {
                entity.HasKey(e => e.ComppiId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_physical_inventory_completed_copy3");

                entity.Property(e => e.ComppiId)
                    .HasColumnName("comppiID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblPhysicalInventoryCopy>(entity =>
            {
                entity.HasKey(e => e.PhysicalinvId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_physical_inventory_copy");

                entity.Property(e => e.PhysicalinvId)
                    .HasColumnName("physicalinvID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTags)
                    .HasColumnName("asset_tags")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvCount)
                    .HasColumnName("inv_count")
                    .HasColumnType("int(5)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SerialNumbers)
                    .HasColumnName("serial_numbers")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Submitted)
                    .HasColumnName("submitted")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblPhysicalInventoryCopy1>(entity =>
            {
                entity.HasKey(e => e.PhysicalinvId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_physical_inventory_copy1");

                entity.Property(e => e.PhysicalinvId)
                    .HasColumnName("physicalinvID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTags)
                    .HasColumnName("asset_tags")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvCount)
                    .HasColumnName("inv_count")
                    .HasColumnType("int(5)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SerialNumbers)
                    .HasColumnName("serial_numbers")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Submitted)
                    .HasColumnName("submitted")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblPhysicalInventoryCopy2>(entity =>
            {
                entity.HasKey(e => e.PhysicalinvId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_physical_inventory_copy2");

                entity.Property(e => e.PhysicalinvId)
                    .HasColumnName("physicalinvID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTags)
                    .HasColumnName("asset_tags")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvCount)
                    .HasColumnName("inv_count")
                    .HasColumnType("int(5)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Processed)
                    .HasColumnName("processed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.SerialNumbers)
                    .HasColumnName("serial_numbers")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Submitted)
                    .HasColumnName("submitted")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblPhysicalInventoryCopy3>(entity =>
            {
                entity.HasKey(e => e.PhysicalinvId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_physical_inventory_copy3");

                entity.Property(e => e.PhysicalinvId)
                    .HasColumnName("physicalinvID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTags)
                    .HasColumnName("asset_tags")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvCount)
                    .HasColumnName("inv_count")
                    .HasColumnType("int(5)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Processed)
                    .HasColumnName("processed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.SerialNumbers)
                    .HasColumnName("serial_numbers")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Submitted)
                    .HasColumnName("submitted")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblPhysicalInventoryVern20160922>(entity =>
            {
                entity.HasKey(e => e.PhysicalinvId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_physical_inventory_vern_2016_09_22");

                entity.Property(e => e.PhysicalinvId)
                    .HasColumnName("physicalinvID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTags)
                    .HasColumnName("asset_tags")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvCount)
                    .HasColumnName("inv_count")
                    .HasColumnType("int(5)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Processed)
                    .HasColumnName("processed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.SerialNumbers)
                    .HasColumnName("serial_numbers")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Submitted)
                    .HasColumnName("submitted")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblPiIssues>(entity =>
            {
                entity.HasKey(e => e.PiissueId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_pi_issues");

                entity.Property(e => e.PiissueId)
                    .HasColumnName("piissueID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IssueDescription)
                    .HasColumnName("issue_description")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.IssueLocationId)
                    .HasColumnName("issue_locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblPoDeleteItems>(entity =>
            {
                entity.HasKey(e => e.PodelId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_po_delete_items");

                entity.Property(e => e.PodelId)
                    .HasColumnName("podelID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PoitemId)
                    .HasColumnName("poitemID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblProducts>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_products");

                entity.Property(e => e.ProductId)
                    .HasColumnName("productID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("categoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProductName)
                    .HasColumnName("product_name")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<TblPurchaseOrderDocuments>(entity =>
            {
                entity.HasKey(e => e.PodocId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_purchase_order_documents");

                entity.Property(e => e.PodocId)
                    .HasColumnName("podocID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Filename)
                    .HasColumnName("filename")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PoId)
                    .HasColumnName("poID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblPurchaseOrderItems>(entity =>
            {
                entity.HasKey(e => e.PoitemId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_purchase_order_items");

                entity.Property(e => e.PoitemId)
                    .HasColumnName("poitemID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ActualPrice)
                    .HasColumnName("actual_price")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.BariCustomer)
                    .HasColumnName("bari_customer")
                    .HasColumnType("int(5)");

                entity.Property(e => e.BariNotes)
                    .HasColumnName("bari_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.GrowthCensus)
                    .HasColumnName("growth_census")
                    .HasColumnType("int(5)");

                entity.Property(e => e.GrowthCustomer)
                    .HasColumnName("growth_customer")
                    .HasColumnType("int(5)");

                entity.Property(e => e.GrowthNotes)
                    .HasColumnName("growth_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.GrowthReason)
                    .HasColumnName("growth_reason")
                    .HasColumnType("int(1)");

                entity.Property(e => e.MainDeliveries)
                    .HasColumnName("main_deliveries")
                    .HasColumnType("int(5)");

                entity.Property(e => e.MainNotes)
                    .HasColumnName("main_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.MainPickups)
                    .HasColumnName("main_pickups")
                    .HasColumnType("int(5)");

                entity.Property(e => e.MainReason)
                    .HasColumnName("main_reason")
                    .HasColumnType("int(1)");

                entity.Property(e => e.MainSpend)
                    .HasColumnName("main_spend")
                    .HasColumnType("int(5)");

                entity.Property(e => e.MainWoNums)
                    .HasColumnName("main_wo_nums")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PoId)
                    .HasColumnName("poID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PoitemNotes)
                    .HasColumnName("poitem_notes")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PricingId)
                    .HasColumnName("pricingID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Processed)
                    .HasColumnName("processed")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PurchaseReason)
                    .HasColumnName("purchase_reason")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(11)");

                entity.Property(e => e.QuantityReceived)
                    .HasColumnName("quantity_received")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SpecRequestBari)
                    .HasColumnName("spec_request_bari")
                    .HasColumnType("int(1)");

                entity.Property(e => e.SpecRequestCustomer)
                    .HasColumnName("spec_request_customer")
                    .HasColumnType("int(5)");

                entity.Property(e => e.SpecRequestNotes)
                    .HasColumnName("spec_request_notes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.SpecRequestOff)
                    .HasColumnName("spec_request_off")
                    .HasColumnType("int(1)");
            });

            modelBuilder.Entity<TblPurchaseOrderItemsReceived>(entity =>
            {
                entity.HasKey(e => e.PoitemrecId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_purchase_order_items_received");

                entity.Property(e => e.PoitemrecId)
                    .HasColumnName("poitemrecID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PoId)
                    .HasColumnName("poID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PoitemId)
                    .HasColumnName("poitemID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.QuantityReceived)
                    .HasColumnName("quantity_received")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblPurchaseOrderItemsReceivedCopy>(entity =>
            {
                entity.HasKey(e => e.PoitemrecId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_purchase_order_items_received_copy");

                entity.Property(e => e.PoitemrecId)
                    .HasColumnName("poitemrecID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PoId)
                    .HasColumnName("poID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PoitemId)
                    .HasColumnName("poitemID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.QuantityReceived)
                    .HasColumnName("quantity_received")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblPurchaseOrderNotes>(entity =>
            {
                entity.HasKey(e => e.PonoteId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_purchase_order_notes");

                entity.Property(e => e.PonoteId)
                    .HasColumnName("ponoteID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PoId)
                    .HasColumnName("poID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblPurchaseOrders>(entity =>
            {
                entity.HasKey(e => e.PoId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_purchase_orders");

                entity.Property(e => e.PoId)
                    .HasColumnName("poID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("timestamp");

                entity.Property(e => e.InvoiceNumber)
                    .HasColumnName("invoice_number")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PoClassBariatric)
                    .HasColumnName("po_class_bariatric")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PoClassComments)
                    .HasColumnName("po_class_comments")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PoClassGrowth)
                    .HasColumnName("po_class_growth")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PoClassMaintenance)
                    .HasColumnName("po_class_maintenance")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PoClassSpecial)
                    .HasColumnName("po_class_special")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PoClassification)
                    .HasColumnName("po_classification")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PoPartner)
                    .HasColumnName("po_partner")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PoStatus)
                    .HasColumnName("po_status")
                    .HasColumnType("int(1)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.VendorId)
                    .HasColumnName("vendorID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblQaCalls>(entity =>
            {
                entity.HasKey(e => e.QacallId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_qa_calls");

                entity.Property(e => e.QacallId)
                    .HasColumnName("qacallID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AgentRating)
                    .HasColumnName("agent_rating")
                    .HasColumnType("int(1)");

                entity.Property(e => e.CommRating)
                    .HasColumnName("comm_rating")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.ContactInfo)
                    .HasColumnName("contact_info")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.EquipAgent)
                    .HasColumnName("equip_agent")
                    .HasColumnType("int(1)");

                entity.Property(e => e.EquipClean)
                    .HasColumnName("equip_clean")
                    .HasColumnType("int(1)");

                entity.Property(e => e.EquipFunction)
                    .HasColumnName("equip_function")
                    .HasColumnType("int(1)");

                entity.Property(e => e.EquipTimely)
                    .HasColumnName("equip_timely")
                    .HasColumnType("int(1)");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.HsRating)
                    .HasColumnName("hs_rating")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InstRating)
                    .HasColumnName("inst_rating")
                    .HasColumnType("int(1)");

                entity.Property(e => e.NotCompleted)
                    .HasColumnName("not_completed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TechRating)
                    .HasColumnName("tech_rating")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblResponses>(entity =>
            {
                entity.HasKey(e => e.ResponseId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_responses");

                entity.Property(e => e.ResponseId)
                    .HasColumnName("responseID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Response)
                    .HasColumnName("response")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.ResponseClosed)
                    .HasColumnName("response_closed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ResponseDate)
                    .HasColumnName("response_date")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.TicketId)
                    .HasColumnName("ticketID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblResponsesOld>(entity =>
            {
                entity.HasKey(e => e.ResponseId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_responses_old");

                entity.Property(e => e.ResponseId)
                    .HasColumnName("responseID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Response)
                    .HasColumnName("response")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.ResponseClosed)
                    .HasColumnName("response_closed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ResponseDate)
                    .HasColumnName("response_date")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.TicketId)
                    .HasColumnName("ticketID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblSerialNumbers>(entity =>
            {
                entity.HasKey(e => e.SerialId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_serial_numbers");

                entity.HasIndex(e => e.AssetTag)
                    .HasName("atag");

                entity.Property(e => e.SerialId)
                    .HasColumnName("serialID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.BookValue)
                    .HasColumnName("book_value")
                    .HasColumnType("decimal(11,2)");

                entity.Property(e => e.ImportDescription)
                    .HasColumnName("import_description")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.ImportProcessed)
                    .HasColumnName("import_processed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvaddId)
                    .HasColumnName("invaddID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvtransId)
                    .HasColumnName("invtransID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvwoId)
                    .HasColumnName("invwoID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationInvId)
                    .HasColumnName("location_invID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PhysicalinvId)
                    .HasColumnName("physicalinvID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PoId)
                    .HasColumnName("poID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SerialNumber)
                    .HasColumnName("serial_number")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Transferred)
                    .HasColumnName("transferred")
                    .HasColumnType("int(1)");

                entity.Property(e => e.WoCandidate)
                    .HasColumnName("wo_candidate")
                    .HasColumnType("int(1)");
            });

            modelBuilder.Entity<TblSerialNumbers61015>(entity =>
            {
                entity.HasKey(e => e.SerialId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_serial_numbers_61015");

                entity.Property(e => e.SerialId)
                    .HasColumnName("serialID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.BookValue)
                    .HasColumnName("book_value")
                    .HasColumnType("decimal(11,2)");

                entity.Property(e => e.ImportDescription)
                    .HasColumnName("import_description")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.ImportProcessed)
                    .HasColumnName("import_processed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvaddId)
                    .HasColumnName("invaddID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvtransId)
                    .HasColumnName("invtransID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvwoId)
                    .HasColumnName("invwoID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationInvId)
                    .HasColumnName("location_invID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PhysicalinvId)
                    .HasColumnName("physicalinvID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PoId)
                    .HasColumnName("poID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SerialNumber)
                    .HasColumnName("serial_number")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Transferred)
                    .HasColumnName("transferred")
                    .HasColumnType("int(1)");

                entity.Property(e => e.WoCandidate)
                    .HasColumnName("wo_candidate")
                    .HasColumnType("int(1)");
            });

            modelBuilder.Entity<TblSerialNumbers6112015>(entity =>
            {
                entity.HasKey(e => e.SerialId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_serial_numbers_6112015");

                entity.Property(e => e.SerialId)
                    .HasColumnName("serialID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.BookValue)
                    .HasColumnName("book_value")
                    .HasColumnType("decimal(11,2)");

                entity.Property(e => e.ImportDescription)
                    .HasColumnName("import_description")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.ImportProcessed)
                    .HasColumnName("import_processed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvaddId)
                    .HasColumnName("invaddID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvtransId)
                    .HasColumnName("invtransID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvwoId)
                    .HasColumnName("invwoID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationInvId)
                    .HasColumnName("location_invID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PhysicalinvId)
                    .HasColumnName("physicalinvID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PoId)
                    .HasColumnName("poID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SerialNumber)
                    .HasColumnName("serial_number")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Transferred)
                    .HasColumnName("transferred")
                    .HasColumnType("int(1)");

                entity.Property(e => e.WoCandidate)
                    .HasColumnName("wo_candidate")
                    .HasColumnType("int(1)");
            });

            modelBuilder.Entity<TblSerialNumbers61215>(entity =>
            {
                entity.HasKey(e => e.SerialId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_serial_numbers_61215");

                entity.Property(e => e.SerialId)
                    .HasColumnName("serialID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.BookValue)
                    .HasColumnName("book_value")
                    .HasColumnType("decimal(11,2)");

                entity.Property(e => e.ImportDescription)
                    .HasColumnName("import_description")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.ImportProcessed)
                    .HasColumnName("import_processed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvaddId)
                    .HasColumnName("invaddID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvtransId)
                    .HasColumnName("invtransID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvwoId)
                    .HasColumnName("invwoID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationInvId)
                    .HasColumnName("location_invID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PhysicalinvId)
                    .HasColumnName("physicalinvID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PoId)
                    .HasColumnName("poID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SerialNumber)
                    .HasColumnName("serial_number")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Transferred)
                    .HasColumnName("transferred")
                    .HasColumnType("int(1)");

                entity.Property(e => e.WoCandidate)
                    .HasColumnName("wo_candidate")
                    .HasColumnType("int(1)");
            });

            modelBuilder.Entity<TblSerialNumbers61315>(entity =>
            {
                entity.HasKey(e => e.SerialId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_serial_numbers_61315");

                entity.Property(e => e.SerialId)
                    .HasColumnName("serialID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("asset_tag")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.BookValue)
                    .HasColumnName("book_value")
                    .HasColumnType("decimal(11,2)");

                entity.Property(e => e.ImportDescription)
                    .HasColumnName("import_description")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.ImportProcessed)
                    .HasColumnName("import_processed")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvaddId)
                    .HasColumnName("invaddID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvtransId)
                    .HasColumnName("invtransID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvwoId)
                    .HasColumnName("invwoID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationInvId)
                    .HasColumnName("location_invID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PhysicalinvId)
                    .HasColumnName("physicalinvID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PoId)
                    .HasColumnName("poID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SerialNumber)
                    .HasColumnName("serial_number")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Transferred)
                    .HasColumnName("transferred")
                    .HasColumnType("int(1)");

                entity.Property(e => e.WoCandidate)
                    .HasColumnName("wo_candidate")
                    .HasColumnType("int(1)");
            });

            modelBuilder.Entity<TblSubCategories>(entity =>
            {
                entity.HasKey(e => e.SubCategoryId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_sub_categories");

                entity.Property(e => e.SubCategoryId)
                    .HasColumnName("sub_categoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("categoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProductLimit)
                    .HasColumnName("product_limit")
                    .HasColumnType("int(1)");

                entity.Property(e => e.RequiredSubCategoryId1)
                    .HasColumnName("required_sub_categoryID_1")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RequiredSubCategoryId2)
                    .HasColumnName("required_sub_categoryID_2")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RequiredSubCategoryId3)
                    .HasColumnName("required_sub_categoryID_3")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SubCatRank)
                    .HasColumnName("sub_cat_rank")
                    .HasColumnType("int(3)");

                entity.Property(e => e.SubCategoryName)
                    .HasColumnName("sub_category_name")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<TblTagExceptions>(entity =>
            {
                entity.HasKey(e => e.ExceptionId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_tag_exceptions");

                entity.Property(e => e.ExceptionId)
                    .HasColumnName("exceptionID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTagDelivered)
                    .HasColumnName("asset_tag_delivered")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AssetTagPickedup)
                    .HasColumnName("asset_tag_pickedup")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.ExceptionType)
                    .HasColumnName("exception_type")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvCode)
                    .HasColumnName("inv_code")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LinkId)
                    .HasColumnName("linkID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblTicketSettings>(entity =>
            {
                entity.HasKey(e => e.SettingsId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_ticket_settings");

                entity.Property(e => e.SettingsId)
                    .HasColumnName("settingsID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Allpccr)
                    .HasColumnName("allpccr")
                    .HasColumnType("text")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.FourtyEight)
                    .HasColumnName("fourty_eight")
                    .HasColumnType("text")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.NoLocation)
                    .HasColumnName("no_location")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TwentyFour)
                    .HasColumnName("twenty_four")
                    .HasColumnType("text")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<TblTickets>(entity =>
            {
                entity.HasKey(e => e.TicketId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_tickets");

                entity.Property(e => e.TicketId)
                    .HasColumnName("ticketID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Attachment)
                    .HasColumnName("attachment")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AttachmentClose)
                    .IsRequired()
                    .HasColumnName("attachment_close")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Csr)
                    .IsRequired()
                    .HasColumnName("csr")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Driver)
                    .IsRequired()
                    .HasColumnName("driver")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PendingClose)
                    .HasColumnName("pending_close")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ReviewOrder)
                    .IsRequired()
                    .HasColumnName("review_order")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Survey)
                    .HasColumnName("survey")
                    .HasColumnType("int(1)");

                entity.Property(e => e.TicketAssignment)
                    .HasColumnName("ticket_assignment")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TicketCreated)
                    .HasColumnName("ticket_created")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.TicketInfo)
                    .HasColumnName("ticket_info")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.TicketLocationId)
                    .HasColumnName("ticket_locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TicketModified)
                    .HasColumnName("ticket_modified")
                    .HasColumnType("timestamp");

                entity.Property(e => e.TicketStatus)
                    .HasColumnName("ticket_status")
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.TicketType)
                    .HasColumnName("ticket_type")
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.TicketUserId)
                    .HasColumnName("ticket_userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblTicketsCopy>(entity =>
            {
                entity.HasKey(e => e.TicketId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_tickets_copy");

                entity.Property(e => e.TicketId)
                    .HasColumnName("ticketID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Attachment)
                    .HasColumnName("attachment")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Csr)
                    .IsRequired()
                    .HasColumnName("csr")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Driver)
                    .IsRequired()
                    .HasColumnName("driver")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PendingClose)
                    .HasColumnName("pending_close")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ReviewOrder)
                    .IsRequired()
                    .HasColumnName("review_order")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Survey)
                    .HasColumnName("survey")
                    .HasColumnType("int(1)");

                entity.Property(e => e.TicketAssignment)
                    .HasColumnName("ticket_assignment")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TicketCreated)
                    .HasColumnName("ticket_created")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.TicketInfo)
                    .HasColumnName("ticket_info")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.TicketLocationId)
                    .HasColumnName("ticket_locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TicketModified)
                    .HasColumnName("ticket_modified")
                    .HasColumnType("timestamp");

                entity.Property(e => e.TicketStatus)
                    .HasColumnName("ticket_status")
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.TicketType)
                    .HasColumnName("ticket_type")
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.TicketUserId)
                    .HasColumnName("ticket_userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblTicketsCopy1>(entity =>
            {
                entity.HasKey(e => e.TicketId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_tickets_copy1");

                entity.Property(e => e.TicketId)
                    .HasColumnName("ticketID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Attachment)
                    .HasColumnName("attachment")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AttachmentClose)
                    .IsRequired()
                    .HasColumnName("attachment_close")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Csr)
                    .IsRequired()
                    .HasColumnName("csr")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Driver)
                    .IsRequired()
                    .HasColumnName("driver")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PendingClose)
                    .HasColumnName("pending_close")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ReviewOrder)
                    .IsRequired()
                    .HasColumnName("review_order")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Survey)
                    .HasColumnName("survey")
                    .HasColumnType("int(1)");

                entity.Property(e => e.TicketAssignment)
                    .HasColumnName("ticket_assignment")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TicketCreated)
                    .HasColumnName("ticket_created")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.TicketInfo)
                    .HasColumnName("ticket_info")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.TicketLocationId)
                    .HasColumnName("ticket_locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TicketModified)
                    .HasColumnName("ticket_modified")
                    .HasColumnType("timestamp");

                entity.Property(e => e.TicketStatus)
                    .HasColumnName("ticket_status")
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.TicketType)
                    .HasColumnName("ticket_type")
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.TicketUserId)
                    .HasColumnName("ticket_userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblTicketsCopy2>(entity =>
            {
                entity.HasKey(e => e.TicketId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_tickets_copy2");

                entity.Property(e => e.TicketId)
                    .HasColumnName("ticketID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Attachment)
                    .HasColumnName("attachment")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.AttachmentClose)
                    .IsRequired()
                    .HasColumnName("attachment_close")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Csr)
                    .IsRequired()
                    .HasColumnName("csr")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Driver)
                    .IsRequired()
                    .HasColumnName("driver")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PendingClose)
                    .HasColumnName("pending_close")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ReviewOrder)
                    .IsRequired()
                    .HasColumnName("review_order")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Survey)
                    .HasColumnName("survey")
                    .HasColumnType("int(1)");

                entity.Property(e => e.TicketAssignment)
                    .HasColumnName("ticket_assignment")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TicketCreated)
                    .HasColumnName("ticket_created")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.TicketInfo)
                    .HasColumnName("ticket_info")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.TicketLocationId)
                    .HasColumnName("ticket_locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TicketModified)
                    .HasColumnName("ticket_modified")
                    .HasColumnType("timestamp");

                entity.Property(e => e.TicketStatus)
                    .HasColumnName("ticket_status")
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.TicketType)
                    .HasColumnName("ticket_type")
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.TicketUserId)
                    .HasColumnName("ticket_userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblTicketsOld>(entity =>
            {
                entity.HasKey(e => e.TicketId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_tickets_old");

                entity.Property(e => e.TicketId)
                    .HasColumnName("ticketID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Attachment)
                    .HasColumnName("attachment")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.TicketAssignment)
                    .HasColumnName("ticket_assignment")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TicketCreated)
                    .HasColumnName("ticket_created")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.TicketInfo)
                    .HasColumnName("ticket_info")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.TicketLocationId)
                    .HasColumnName("ticket_locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TicketModified)
                    .HasColumnName("ticket_modified")
                    .HasColumnType("timestamp");

                entity.Property(e => e.TicketStatus)
                    .HasColumnName("ticket_status")
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.TicketType)
                    .HasColumnName("ticket_type")
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.TicketUserId)
                    .HasColumnName("ticket_userID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblUnadjusted>(entity =>
            {
                entity.HasKey(e => e.AdjId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_unadjusted");

                entity.Property(e => e.AdjId)
                    .HasColumnName("adjID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Filename)
                    .HasColumnName("filename")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Month)
                    .HasColumnName("month")
                    .HasColumnType("int(2)");

                entity.Property(e => e.NonPerDiem)
                    .HasColumnName("non_per_diem")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.PatientDays)
                    .HasColumnName("patient_days")
                    .HasColumnType("int(10)");

                entity.Property(e => e.PerDiem)
                    .HasColumnName("per_diem")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.Utilization)
                    .HasColumnName("utilization")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.Year)
                    .HasColumnName("year")
                    .HasColumnType("int(4)");
            });

            modelBuilder.Entity<TblUnadjustedForecast>(entity =>
            {
                entity.HasKey(e => e.AdjId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_unadjusted_forecast");

                entity.Property(e => e.AdjId)
                    .HasColumnName("adjID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Inserted)
                    .HasColumnName("inserted")
                    .HasColumnType("timestamp");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Month)
                    .HasColumnName("month")
                    .HasColumnType("int(2)");

                entity.Property(e => e.NoRevenue)
                    .HasColumnName("no_revenue")
                    .HasColumnType("int(1)");

                entity.Property(e => e.NonPerDiem)
                    .HasColumnName("non_per_diem")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.PatientDays)
                    .HasColumnName("patient_days")
                    .HasColumnType("int(10)");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PerDiem)
                    .HasColumnName("per_diem")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.Utilization)
                    .HasColumnName("utilization")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.Year)
                    .HasColumnName("year")
                    .HasColumnType("int(4)");
            });

            modelBuilder.Entity<TblUnadjustedForecastCopy>(entity =>
            {
                entity.HasKey(e => e.AdjId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_unadjusted_forecast_copy");

                entity.Property(e => e.AdjId)
                    .HasColumnName("adjID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AllCustomers)
                    .HasColumnName("all_customers")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Inserted)
                    .HasColumnName("inserted")
                    .HasColumnType("timestamp");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Month)
                    .HasColumnName("month")
                    .HasColumnType("int(2)");

                entity.Property(e => e.NonPerDiem)
                    .HasColumnName("non_per_diem")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.OdyExcluded)
                    .HasColumnName("ody_excluded")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OdyOnly)
                    .HasColumnName("ody_only")
                    .HasColumnType("int(1)");

                entity.Property(e => e.PatientDays)
                    .HasColumnName("patient_days")
                    .HasColumnType("int(10)");

                entity.Property(e => e.PerDiem)
                    .HasColumnName("per_diem")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.Utilization)
                    .HasColumnName("utilization")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.V2)
                    .HasColumnName("v2")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Year)
                    .HasColumnName("year")
                    .HasColumnType("int(4)");
            });

            modelBuilder.Entity<TblUsers>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_users");

                entity.HasIndex(e => e.HospiceId)
                    .HasName("hospiceID");

                entity.HasIndex(e => e.Lastname)
                    .HasName("lastname");

                entity.HasIndex(e => e.UserLevel)
                    .HasName("level");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AlwaysEmail)
                    .HasColumnName("always_email")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ApprovingManager)
                    .HasColumnName("approving_manager")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Carrier)
                    .HasColumnName("carrier")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DispatchAreas)
                    .HasColumnName("dispatch_areas")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DmeId)
                    .HasColumnName("dmeID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.FacilityManager)
                    .HasColumnName("facility_manager")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Firstname)
                    .HasColumnName("firstname")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.GmtOffset)
                    .HasColumnName("gmt_offset")
                    .HasColumnType("varchar(11)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Inactive)
                    .HasColumnName("inactive")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InventoryAccess)
                    .HasColumnName("inventory_access")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InventoryAreas)
                    .HasColumnName("inventory_areas")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvoicePreviewer)
                    .HasColumnName("invoice_previewer")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Lastname)
                    .HasColumnName("lastname")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LegacyMitsClass)
                    .HasColumnName("legacy_MITS_class")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LegacyMitsDivision)
                    .HasColumnName("legacy_MITS_division")
                    .HasColumnType("varchar(5)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.MgrMultiSite)
                    .HasColumnName("mgr_multi_site")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Mobile)
                    .HasColumnName("mobile")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.MultipleHospices)
                    .HasColumnName("multiple_hospices")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.NurseManager)
                    .HasColumnName("nurse_manager")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OffCapApproval)
                    .HasColumnName("off_cap_approval")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OffCapApprovalType)
                    .HasColumnName("off_cap_approval_type")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PccrPermission)
                    .HasColumnName("pccr_permission")
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Pwtoken)
                    .IsRequired()
                    .HasColumnName("pwtoken")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PwtokenExpired)
                    .HasColumnName("pwtoken_expired")
                    .HasColumnType("datetime");

                entity.Property(e => e.QuickFacilities)
                    .HasColumnName("quick_facilities")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.QuickState)
                    .HasColumnName("quick_state")
                    .HasColumnType("varchar(2)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.SuperLogin)
                    .HasColumnName("super_login")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Temppass)
                    .HasColumnName("temppass")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UserCreated)
                    .HasColumnName("user_created")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserLevel)
                    .HasColumnName("user_level")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UsersManager)
                    .HasColumnName("users_manager")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Version2)
                    .HasColumnName("version_2")
                    .HasColumnType("int(1)");
            });

            modelBuilder.Entity<TblUsers2>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tbl_users2");

                entity.Property(e => e.AlwaysEmail)
                    .HasColumnName("always_email")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ApprovingManager)
                    .HasColumnName("approving_manager")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DispatchAreas)
                    .HasColumnName("dispatch_areas")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DmeId)
                    .HasColumnName("dmeID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FacilityManager)
                    .HasColumnName("facility_manager")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Firstname)
                    .HasColumnName("firstname")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.GmtOffset)
                    .HasColumnName("gmt_offset")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Inactive)
                    .HasColumnName("inactive")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InventoryAccess)
                    .HasColumnName("inventory_access")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InventoryAreas)
                    .HasColumnName("inventory_areas")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.InvoicePreviewer)
                    .HasColumnName("invoice_previewer")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Lastname)
                    .HasColumnName("lastname")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.LegacyMitsClass)
                    .HasColumnName("legacy_MITS_class")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.LegacyMitsDivision)
                    .HasColumnName("legacy_MITS_division")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.MgrMultiSite)
                    .HasColumnName("mgr_multi_site")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Mobile)
                    .HasColumnName("mobile")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.MultipleHospices)
                    .HasColumnName("multiple_hospices")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.NurseManager)
                    .HasColumnName("nurse_manager")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OffCapApproval)
                    .HasColumnName("off_cap_approval")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OffCapApprovalType)
                    .HasColumnName("off_cap_approval_type")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.PccrPermission)
                    .HasColumnName("pccr_permission")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Pwtoken)
                    .HasColumnName("pwtoken")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.QuickFacilities)
                    .HasColumnName("quick_facilities")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.QuickState)
                    .HasColumnName("quick_state")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.SuperLogin)
                    .HasColumnName("super_login")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Temppass)
                    .HasColumnName("temppass")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.UserCreated)
                    .HasColumnName("user_created")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserLevel)
                    .HasColumnName("user_level")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.UsersManager)
                    .HasColumnName("users_manager")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Version2)
                    .HasColumnName("version_2")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TblUsersCopy>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_users_copy");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AlwaysEmail)
                    .HasColumnName("always_email")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ApprovingManager)
                    .HasColumnName("approving_manager")
                    .HasColumnType("int(1)");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DispatchAreas)
                    .HasColumnName("dispatch_areas")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DmeId)
                    .HasColumnName("dmeID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.FacilityManager)
                    .HasColumnName("facility_manager")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Firstname)
                    .HasColumnName("firstname")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.GmtOffset)
                    .HasColumnName("gmt_offset")
                    .HasColumnType("varchar(11)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Inactive)
                    .HasColumnName("inactive")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InventoryAccess)
                    .HasColumnName("inventory_access")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InventoryAreas)
                    .HasColumnName("inventory_areas")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvoicePreviewer)
                    .HasColumnName("invoice_previewer")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Lastname)
                    .HasColumnName("lastname")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LegacyMitsClass)
                    .HasColumnName("legacy_MITS_class")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LegacyMitsDivision)
                    .HasColumnName("legacy_MITS_division")
                    .HasColumnType("varchar(5)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.MgrMultiSite)
                    .HasColumnName("mgr_multi_site")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Mobile)
                    .HasColumnName("mobile")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.MultipleHospices)
                    .HasColumnName("multiple_hospices")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.NurseManager)
                    .HasColumnName("nurse_manager")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OffCapApproval)
                    .HasColumnName("off_cap_approval")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OffCapApprovalType)
                    .HasColumnName("off_cap_approval_type")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PccrPermission)
                    .HasColumnName("pccr_permission")
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Pwtoken)
                    .IsRequired()
                    .HasColumnName("pwtoken")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.QuickFacilities)
                    .HasColumnName("quick_facilities")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.QuickState)
                    .HasColumnName("quick_state")
                    .HasColumnType("varchar(2)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.SuperLogin)
                    .HasColumnName("super_login")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Temppass)
                    .HasColumnName("temppass")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UserLevel)
                    .HasColumnName("user_level")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UsersManager)
                    .HasColumnName("users_manager")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Version2)
                    .HasColumnName("version_2")
                    .HasColumnType("int(1)");
            });

            modelBuilder.Entity<TblUsersCopy1>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_users_copy1");

                entity.HasIndex(e => e.HospiceId)
                    .HasName("hospiceID");

                entity.HasIndex(e => e.Lastname)
                    .HasName("lastname");

                entity.HasIndex(e => e.UserLevel)
                    .HasName("level");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AlwaysEmail)
                    .HasColumnName("always_email")
                    .HasColumnType("int(1)");

                entity.Property(e => e.ApprovingManager)
                    .HasColumnName("approving_manager")
                    .HasColumnType("int(1)");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DispatchAreas)
                    .HasColumnName("dispatch_areas")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.DmeId)
                    .HasColumnName("dmeID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.FacilityManager)
                    .HasColumnName("facility_manager")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Firstname)
                    .HasColumnName("firstname")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.GmtOffset)
                    .HasColumnName("gmt_offset")
                    .HasColumnType("varchar(11)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.HospiceId)
                    .HasColumnName("hospiceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Inactive)
                    .HasColumnName("inactive")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InventoryAccess)
                    .HasColumnName("inventory_access")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InventoryAreas)
                    .HasColumnName("inventory_areas")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.InvoicePreviewer)
                    .HasColumnName("invoice_previewer")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Lastname)
                    .HasColumnName("lastname")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LegacyMitsClass)
                    .HasColumnName("legacy_MITS_class")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LegacyMitsDivision)
                    .HasColumnName("legacy_MITS_division")
                    .HasColumnType("varchar(5)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.MgrMultiSite)
                    .HasColumnName("mgr_multi_site")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Mobile)
                    .HasColumnName("mobile")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.MultipleHospices)
                    .HasColumnName("multiple_hospices")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.NurseManager)
                    .HasColumnName("nurse_manager")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OffCapApproval)
                    .HasColumnName("off_cap_approval")
                    .HasColumnType("int(1)");

                entity.Property(e => e.OffCapApprovalType)
                    .HasColumnName("off_cap_approval_type")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.PccrPermission)
                    .HasColumnName("pccr_permission")
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Pwtoken)
                    .IsRequired()
                    .HasColumnName("pwtoken")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.QuickFacilities)
                    .HasColumnName("quick_facilities")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.QuickState)
                    .HasColumnName("quick_state")
                    .HasColumnType("varchar(2)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.SuperLogin)
                    .HasColumnName("super_login")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Temppass)
                    .HasColumnName("temppass")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UserCreated)
                    .HasColumnName("user_created")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserLevel)
                    .HasColumnName("user_level")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UsersManager)
                    .HasColumnName("users_manager")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Version2)
                    .HasColumnName("version_2")
                    .HasColumnType("int(1)");
            });

            modelBuilder.Entity<TblVehicles>(entity =>
            {
                entity.HasKey(e => e.VehicleId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_vehicles");

                entity.Property(e => e.VehicleId)
                    .HasColumnName("vehicleID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ExpDate)
                    .HasColumnName("exp_date")
                    .HasColumnType("date");

                entity.Property(e => e.License)
                    .HasColumnName("license")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.LocationCode)
                    .HasColumnName("location_code")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MakeModel)
                    .HasColumnName("make_model")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.VehicleUnitId)
                    .HasColumnName("vehicle_unit_id")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.VinNumber)
                    .HasColumnName("vin_number")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Year)
                    .HasColumnName("year")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<TblVendors>(entity =>
            {
                entity.HasKey(e => e.VendorId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_vendors");

                entity.Property(e => e.VendorId)
                    .HasColumnName("vendorID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.VendorAcctnum)
                    .HasColumnName("vendor_acctnum")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.VendorAddress)
                    .HasColumnName("vendor_address")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.VendorCtystzip)
                    .HasColumnName("vendor_ctystzip")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.VendorMethod)
                    .HasColumnName("vendor_method")
                    .HasColumnType("int(1)");

                entity.Property(e => e.VendorName)
                    .HasColumnName("vendor_name")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.VendorNotes)
                    .HasColumnName("vendor_notes")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.VendorPhone)
                    .HasColumnName("vendor_phone")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.VendorRep)
                    .HasColumnName("vendor_rep")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.VendorRepemail)
                    .HasColumnName("vendor_repemail")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.VendorRepphone)
                    .HasColumnName("vendor_repphone")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.VendorTerms)
                    .HasColumnName("vendor_terms")
                    .HasColumnType("varchar(254)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<TblZipToLocation>(entity =>
            {
                entity.HasKey(e => e.ZipId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_zip_to_location");

                entity.Property(e => e.ZipId)
                    .HasColumnName("zipID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Zip)
                    .HasColumnName("zip")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<TblZipToLocationCopy>(entity =>
            {
                entity.HasKey(e => e.ZipId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_zip_to_location_copy");

                entity.Property(e => e.ZipId)
                    .HasColumnName("zipID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Zip)
                    .HasColumnName("zip")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<TblZipToLocationCopy1>(entity =>
            {
                entity.HasKey(e => e.ZipId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_zip_to_location_copy1");

                entity.Property(e => e.ZipId)
                    .HasColumnName("zipID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Zip)
                    .HasColumnName("zip")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<TblZipToLocationCopy2>(entity =>
            {
                entity.HasKey(e => e.ZipId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_zip_to_location_copy2");

                entity.Property(e => e.ZipId)
                    .HasColumnName("zipID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Zip)
                    .HasColumnName("zip")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<TblZipToLocationCopy3>(entity =>
            {
                entity.HasKey(e => e.ZipId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_zip_to_location_copy3");

                entity.Property(e => e.ZipId)
                    .HasColumnName("zipID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Zip)
                    .HasColumnName("zip")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Id)
                    .HasName("ID");

                entity.HasIndex(e => e.Keycode)
                    .HasName("keycode");

                entity.HasIndex(e => e.Username)
                    .HasName("username");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Fullname)
                    .HasColumnName("fullname")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Ilayout)
                    .HasColumnName("ilayout")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Keycode)
                    .HasColumnName("keycode")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Layout)
                    .HasColumnName("layout")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Mlayout)
                    .HasColumnName("mlayout")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Params)
                    .HasColumnName("params")
                    .HasColumnType("varchar(10000)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Passcode)
                    .HasColumnName("passcode")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Stampcode)
                    .HasColumnName("stampcode")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.World)
                    .HasColumnName("world")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
