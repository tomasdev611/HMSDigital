using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using HMSDigital.Core.Data.Models;

#nullable disable

namespace HMSDigital.Core.Data
{
    public partial class HMSDigitalContext : DbContext
    {
        public HMSDigitalContext()
        {
        }

        public HMSDigitalContext(DbContextOptions<HMSDigitalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AddOnGroupProducts> AddOnGroupProducts { get; set; }
        public virtual DbSet<AddOnGroups> AddOnGroups { get; set; }
        public virtual DbSet<Addresses> Addresses { get; set; }
        public virtual DbSet<ContractRecords> ContractRecords { get; set; }
        public virtual DbSet<CreditHoldHistory> CreditHoldHistory { get; set; }
        public virtual DbSet<CsvMappings> CsvMappings { get; set; }
        public virtual DbSet<CustomerTypes> CustomerTypes { get; set; }
        public virtual DbSet<DispatchAuditLog> DispatchAuditLog { get; set; }
        public virtual DbSet<DispatchInstructions> DispatchInstructions { get; set; }
        public virtual DbSet<Drivers> Drivers { get; set; }
        public virtual DbSet<Emails> Emails { get; set; }
        public virtual DbSet<EquipmentSettingTypes> EquipmentSettingTypes { get; set; }
        public virtual DbSet<EquipmentSettingsConfig> EquipmentSettingsConfig { get; set; }
        public virtual DbSet<Facilities> Facilities { get; set; }
        public virtual DbSet<FacilityPatient> FacilityPatient { get; set; }
        public virtual DbSet<FacilityPatientHistory> FacilityPatientHistory { get; set; }
        public virtual DbSet<FacilityPhoneNumber> FacilityPhoneNumber { get; set; }
        public virtual DbSet<Features> Features { get; set; }
        public virtual DbSet<FilesMetadata> FilesMetadata { get; set; }
        public virtual DbSet<Hms2ContractItems> Hms2ContractItems { get; set; }
        public virtual DbSet<Hms2Contracts> Hms2Contracts { get; set; }
        public virtual DbSet<Hms2HmsDigitalHospiceMappings> Hms2HmsDigitalHospiceMappings { get; set; }
        public virtual DbSet<HospiceLocationMembers> HospiceLocationMembers { get; set; }
        public virtual DbSet<HospiceLocations> HospiceLocations { get; set; }
        public virtual DbSet<HospiceMember> HospiceMember { get; set; }
        public virtual DbSet<Hospices> Hospices { get; set; }
        public virtual DbSet<Inventory> Inventory { get; set; }
        public virtual DbSet<InventoryStatusTypes> InventoryStatusTypes { get; set; }
        public virtual DbSet<ItemCategories> ItemCategories { get; set; }
        public virtual DbSet<ItemCategoryMapping> ItemCategoryMapping { get; set; }
        public virtual DbSet<ItemImageFiles> ItemImageFiles { get; set; }
        public virtual DbSet<ItemImages> ItemImages { get; set; }
        public virtual DbSet<ItemSubCategories> ItemSubCategories { get; set; }
        public virtual DbSet<ItemSubCategoryMapping> ItemSubCategoryMapping { get; set; }
        public virtual DbSet<ItemTransferRequests> ItemTransferRequests { get; set; }
        public virtual DbSet<Items> Items { get; set; }
        public virtual DbSet<OrderFulfillmentLineItems> OrderFulfillmentLineItems { get; set; }
        public virtual DbSet<OrderHeaderStatusTypes> OrderHeaderStatusTypes { get; set; }
        public virtual DbSet<OrderHeaders> OrderHeaders { get; set; }
        public virtual DbSet<OrderLineItemStatusTypes> OrderLineItemStatusTypes { get; set; }
        public virtual DbSet<OrderLineItems> OrderLineItems { get; set; }
        public virtual DbSet<OrderNotes> OrderNotes { get; set; }
        public virtual DbSet<OrderTypes> OrderTypes { get; set; }
        public virtual DbSet<PatientInventory> PatientInventory { get; set; }
        public virtual DbSet<PermissionNouns> PermissionNouns { get; set; }
        public virtual DbSet<PermissionVerbs> PermissionVerbs { get; set; }
        public virtual DbSet<PhoneNumberTypes> PhoneNumberTypes { get; set; }
        public virtual DbSet<PhoneNumbers> PhoneNumbers { get; set; }
        public virtual DbSet<PrePostDeploymentScriptRuns> PrePostDeploymentScriptRuns { get; set; }
        public virtual DbSet<RolePermissions> RolePermissions { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<SiteMembers> SiteMembers { get; set; }
        public virtual DbSet<SitePhoneNumber> SitePhoneNumber { get; set; }
        public virtual DbSet<SiteServiceAreas> SiteServiceAreas { get; set; }
        public virtual DbSet<Sites> Sites { get; set; }
        public virtual DbSet<StorageTypes> StorageTypes { get; set; }
        public virtual DbSet<SubscriptionItems> SubscriptionItems { get; set; }
        public virtual DbSet<Subscriptions> Subscriptions { get; set; }
        public virtual DbSet<TransferRequestStatusTypes> TransferRequestStatusTypes { get; set; }
        public virtual DbSet<UserProfilePicture> UserProfilePicture { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }
        public virtual DbSet<UserVerify> UserVerify { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AddOnGroupProducts>(entity =>
            {
                entity.ToTable("AddOnGroupProducts", "core");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.AddOnGroupProducts)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_AddOnGroupProducts_AddOnGroups");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.AddOnGroupProducts)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_AddOnGroupProducts_Items");
            });

            modelBuilder.Entity<AddOnGroups>(entity =>
            {
                entity.ToTable("AddOnGroups", "core");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.AddOnGroups)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_AddOnGroups_Items");
            });

            modelBuilder.Entity<Addresses>(entity =>
            {
                entity.ToTable("Addresses", "core");

                entity.Property(e => e.AddressUuid).HasColumnName("AddressUUID");

                entity.Property(e => e.Latitude)
                    .HasColumnType("decimal(11, 8)")
                    .HasDefaultValueSql("((0.00000000))");

                entity.Property(e => e.Longitude)
                    .HasColumnType("decimal(11, 8)")
                    .HasDefaultValueSql("((0.00000000))");

                entity.Property(e => e.NetSuiteAddressId).HasColumnName("NetSuiteAddressId ");

                entity.Property(e => e.SkentityId).HasColumnName("SKEntityID");

                entity.Property(e => e.SkentityType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SKEntityType");

                entity.Property(e => e.VerifiedBy).IsUnicode(false);
            });

            modelBuilder.Entity<ContractRecords>(entity =>
            {
                entity.ToTable("ContractRecords", "core");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.ContractRecordsCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContractRecords_ToUsers_Created");

                entity.HasOne(d => d.Hospice)
                    .WithMany(p => p.ContractRecords)
                    .HasForeignKey(d => d.HospiceId)
                    .HasConstraintName("FK_ContractRecords_ToHospices");

                entity.HasOne(d => d.HospiceLocation)
                    .WithMany(p => p.ContractRecords)
                    .HasForeignKey(d => d.HospiceLocationId)
                    .HasConstraintName("FK_ContractRecords_ToHospiceLocations");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ContractRecords)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_ContractRecords_ToItems");

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany(p => p.ContractRecordsUpdatedByUser)
                    .HasForeignKey(d => d.UpdatedByUserId)
                    .HasConstraintName("FK_ContractRecords_ToUsers_Updated");
            });

            modelBuilder.Entity<CreditHoldHistory>(entity =>
            {
                entity.ToTable("CreditHoldHistory", "core");

                entity.Property(e => e.CreditHoldNote).IsUnicode(false);

                entity.HasOne(d => d.CreditHoldByUser)
                    .WithMany(p => p.CreditHoldHistory)
                    .HasForeignKey(d => d.CreditHoldByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Credit_Hold_By_User");

                entity.HasOne(d => d.Hospice)
                    .WithMany(p => p.CreditHoldHistory)
                    .HasForeignKey(d => d.HospiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Credit_Hold_History_Hospice");
            });

            modelBuilder.Entity<CsvMappings>(entity =>
            {
                entity.ToTable("CsvMappings", "core");

                entity.Property(e => e.MappedTable)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MappingType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.CsvMappingsCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CsvMappings_CreatedUsers");

                entity.HasOne(d => d.Hospice)
                    .WithMany(p => p.CsvMappings)
                    .HasForeignKey(d => d.HospiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CsvMappings_Hospice");

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany(p => p.CsvMappingsUpdatedByUser)
                    .HasForeignKey(d => d.UpdatedByUserId)
                    .HasConstraintName("FK_CsvMappings_UpdatedUsers");
            });

            modelBuilder.Entity<CustomerTypes>(entity =>
            {
                entity.ToTable("CustomerTypes", "core");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<DispatchAuditLog>(entity =>
            {
                entity.HasKey(e => e.AuditUuid);

                entity.ToTable("DispatchAuditLog", "core");

                entity.Property(e => e.AuditUuid).HasDefaultValueSql("(newid())");

                entity.Property(e => e.AuditAction)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ClientIpaddress)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ClientIPAddress");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DispatchAuditLog)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_DispatchAuditLog_User");
            });

            modelBuilder.Entity<DispatchInstructions>(entity =>
            {
                entity.ToTable("DispatchInstructions", "core");

                entity.HasIndex(e => e.OrderHeaderId, "AK_OrderHeader")
                    .IsUnique();

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.DispatchInstructionsCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DispatchInstructions_Users_Created");

                entity.HasOne(d => d.OrderHeader)
                    .WithOne(p => p.DispatchInstructions)
                    .HasForeignKey<DispatchInstructions>(d => d.OrderHeaderId)
                    .HasConstraintName("FK_DispatchInstructions_OrderHeaders");

                entity.HasOne(d => d.TransferRequest)
                    .WithMany(p => p.DispatchInstructions)
                    .HasForeignKey(d => d.TransferRequestId)
                    .HasConstraintName("FK_DispatchInstructions_TransferReqeusts");

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany(p => p.DispatchInstructionsUpdatedByUser)
                    .HasForeignKey(d => d.UpdatedByUserId)
                    .HasConstraintName("FK_DispatchInstructions_Users_Updated");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.DispatchInstructions)
                    .HasForeignKey(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DispatchInstructions_To_Vehicles");
            });

            modelBuilder.Entity<Drivers>(entity =>
            {
                entity.ToTable("Drivers", "core");

                entity.HasIndex(e => e.CurrentVehicleId, "AK_Drivers_Vehicle")
                    .IsUnique()
                    .HasFilter("([CurrentVehicleId] IS NOT NULL AND [IsDeleted]=(0))");

                entity.Property(e => e.LastKnownLatitude).HasColumnType("decimal(11, 8)");

                entity.Property(e => e.LastKnownLongitude).HasColumnType("decimal(11, 8)");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.DriversCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Drivers_Users_Created");

                entity.HasOne(d => d.CurrentSite)
                    .WithMany(p => p.DriversCurrentSite)
                    .HasForeignKey(d => d.CurrentSiteId)
                    .HasConstraintName("FK_Drivers_Sites");

                entity.HasOne(d => d.CurrentVehicle)
                    .WithOne(p => p.DriversCurrentVehicle)
                    .HasForeignKey<Drivers>(d => d.CurrentVehicleId)
                    .HasConstraintName("FK_Drivers_Vehicle");

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany(p => p.DriversUpdatedByUser)
                    .HasForeignKey(d => d.UpdatedByUserId)
                    .HasConstraintName("FK_Drivers_Users_Updated");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DriversUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Drivers_Users");
            });

            modelBuilder.Entity<Emails>(entity =>
            {
                entity.ToTable("Emails", "core");

                entity.Property(e => e.EmailAddress).IsRequired();

                entity.Property(e => e.EmailType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EquipmentSettingTypes>(entity =>
            {
                entity.ToTable("EquipmentSettingTypes", "core");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EquipmentSettingsConfig>(entity =>
            {
                entity.ToTable("EquipmentSettingsConfig", "core");

                entity.HasOne(d => d.EquipmentSettingType)
                    .WithMany(p => p.EquipmentSettingsConfig)
                    .HasForeignKey(d => d.EquipmentSettingTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EquipmentSettingsConfig_ToEquipmentSettingTypes");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.EquipmentSettingsConfig)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_EquipmentSettingsConfig_ToItems");
            });

            modelBuilder.Entity<Facilities>(entity =>
            {
                entity.ToTable("Facilities", "core");

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Facilities)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_Facilities_Address");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.FacilitiesCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .HasConstraintName("FK_Facilities_ToUsers_Created");

                entity.HasOne(d => d.Hospice)
                    .WithMany(p => p.Facilities)
                    .HasForeignKey(d => d.HospiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Facilities_ToHospices");

                entity.HasOne(d => d.HospiceLocation)
                    .WithMany(p => p.Facilities)
                    .HasForeignKey(d => d.HospiceLocationId)
                    .HasConstraintName("FK_Facilities_ToHospiceLocations");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.Facilities)
                    .HasForeignKey(d => d.SiteId)
                    .HasConstraintName("FK_Facilities_Sites");

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany(p => p.FacilitiesUpdatedByUser)
                    .HasForeignKey(d => d.UpdatedByUserId)
                    .HasConstraintName("FK_Facilities_ToUsers_Updated");
            });

            modelBuilder.Entity<FacilityPatient>(entity =>
            {
                entity.ToTable("FacilityPatient", "core");

                entity.Property(e => e.PatientRoomNumber).IsUnicode(false);

                entity.Property(e => e.PatientUuid).HasColumnName("PatientUUID");

                entity.HasOne(d => d.Facility)
                    .WithMany(p => p.FacilityPatient)
                    .HasForeignKey(d => d.FacilityId)
                    .HasConstraintName("FK_FacilityPatient_Facilities");
            });

            modelBuilder.Entity<FacilityPatientHistory>(entity =>
            {
                entity.ToTable("FacilityPatientHistory", "core");

                entity.Property(e => e.PatientUuid).HasColumnName("PatientUUID");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.FacilityPatientHistoryCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FacilityPatientHistory_Users_Created");

                entity.HasOne(d => d.Facility)
                    .WithMany(p => p.FacilityPatientHistory)
                    .HasForeignKey(d => d.FacilityId)
                    .HasConstraintName("FK_FacilityPatientHistory_Facilities");

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany(p => p.FacilityPatientHistoryUpdatedByUser)
                    .HasForeignKey(d => d.UpdatedByUserId)
                    .HasConstraintName("FK_FacilityPatientHistory_Users_Updated");
            });

            modelBuilder.Entity<FacilityPhoneNumber>(entity =>
            {
                entity.ToTable("FacilityPhoneNumber", "core");

                entity.HasOne(d => d.Facility)
                    .WithMany(p => p.FacilityPhoneNumber)
                    .HasForeignKey(d => d.FacilityId)
                    .HasConstraintName("FK_FacilityPhoneNumber_Facility");

                entity.HasOne(d => d.PhoneNumber)
                    .WithMany(p => p.FacilityPhoneNumber)
                    .HasForeignKey(d => d.PhoneNumberId)
                    .HasConstraintName("FK_FacilityPhoneNumber_PhoneNumber");
            });

            modelBuilder.Entity<Features>(entity =>
            {
                entity.ToTable("Features", "core");

                entity.HasIndex(e => e.Name, "AK_Features_Name")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FilesMetadata>(entity =>
            {
                entity.ToTable("FilesMetadata", "core");

                entity.Property(e => e.ContentType)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.StorageFilePath)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.StorageRoot)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.StorageType)
                    .WithMany(p => p.FilesMetadata)
                    .HasForeignKey(d => d.StorageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FilesMetadata_ToStorageTypes");
            });

            modelBuilder.Entity<Hms2ContractItems>(entity =>
            {
                entity.ToTable("Hms2ContractItems", "core");

                entity.Property(e => e.RentalPrice).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.SalePrice).HasColumnType("decimal(8, 2)");

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.Hms2ContractItems)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Hms2ContractItems_Hms2Contracts");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Hms2ContractItems)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_Hms2ContractItems_Items");
            });

            modelBuilder.Entity<Hms2Contracts>(entity =>
            {
                entity.ToTable("Hms2Contracts", "core");

                entity.Property(e => e.ContractName).IsUnicode(false);

                entity.Property(e => e.ContractNumber).IsUnicode(false);

                entity.Property(e => e.PerDiemRate).HasColumnType("decimal(8, 2)");

                entity.HasOne(d => d.Hospice)
                    .WithMany(p => p.Hms2Contracts)
                    .HasForeignKey(d => d.HospiceId)
                    .HasConstraintName("FK_Hms2Contracts_Hospices");

                entity.HasOne(d => d.HospiceLocation)
                    .WithMany(p => p.Hms2Contracts)
                    .HasForeignKey(d => d.HospiceLocationId)
                    .HasConstraintName("FK_Hms2Contracts_HospiceLocations");
            });

            modelBuilder.Entity<Hms2HmsDigitalHospiceMappings>(entity =>
            {
                entity.HasKey(e => e.Hms2id);

                entity.ToTable("Hms2HmsDigitalHospiceMappings", "core");

                entity.Property(e => e.Hms2id)
                    .ValueGeneratedNever()
                    .HasColumnName("HMS2Id");

                entity.Property(e => e.DigitalType).IsUnicode(false);

                entity.Property(e => e.Hms2name)
                    .IsUnicode(false)
                    .HasColumnName("HMS2Name");

                entity.Property(e => e.HospiceLocationName).IsUnicode(false);

                entity.Property(e => e.HospiceName).IsUnicode(false);

                entity.Property(e => e.NetSuiteName).IsUnicode(false);

                entity.HasOne(d => d.Hospice)
                    .WithMany(p => p.Hms2HmsDigitalHospiceMappings)
                    .HasForeignKey(d => d.HospiceId)
                    .HasConstraintName("FK_Hms2HmsDigitalHospiceMappings_Hospices");

                entity.HasOne(d => d.HospiceLocation)
                    .WithMany(p => p.Hms2HmsDigitalHospiceMappings)
                    .HasForeignKey(d => d.HospiceLocationId)
                    .HasConstraintName("FK_Hms2HmsDigitalHospiceMappings_HospiceLocations");
            });

            modelBuilder.Entity<HospiceLocationMembers>(entity =>
            {
                entity.ToTable("HospiceLocationMembers", "core");

                entity.Property(e => e.CanApproveOrder).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.HospiceLocationMembersCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .HasConstraintName("FK_HospiceLocationMember_Users_Created");

                entity.HasOne(d => d.HospiceLocation)
                    .WithMany(p => p.HospiceLocationMembers)
                    .HasForeignKey(d => d.HospiceLocationId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_HospiceLocationMember_HospiceLocation");

                entity.HasOne(d => d.HospiceMember)
                    .WithMany(p => p.HospiceLocationMembers)
                    .HasForeignKey(d => d.HospiceMemberId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_HospiceLocationMember_HospiceMember");

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany(p => p.HospiceLocationMembersUpdatedByUser)
                    .HasForeignKey(d => d.UpdatedByUserId)
                    .HasConstraintName("FK_HospiceLocationMember_Users_Updated");
            });

            modelBuilder.Entity<HospiceLocations>(entity =>
            {
                entity.ToTable("HospiceLocations", "core");

                entity.Property(e => e.DeletedDateTime).HasDefaultValueSql("(datefromparts((1),(1),(1)))");

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.HospiceLocations)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_HospiceLocation_Address");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.HospiceLocationsCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .HasConstraintName("FK_HospiceLocations_ToUsers_Created");

                entity.HasOne(d => d.CustomerType)
                    .WithMany(p => p.HospiceLocations)
                    .HasForeignKey(d => d.CustomerTypeId)
                    .HasConstraintName("FK_HospiceLocation_CustomerTypes");

                entity.HasOne(d => d.DeletedByUser)
                    .WithMany(p => p.HospiceLocationsDeletedByUser)
                    .HasForeignKey(d => d.DeletedByUserId)
                    .HasConstraintName("FK_HospiceLocations_ToUsers_Deleted");

                entity.HasOne(d => d.Hospice)
                    .WithMany(p => p.HospiceLocations)
                    .HasForeignKey(d => d.HospiceId)
                    .HasConstraintName("FK_HospiceLocations_ToHospices");

                entity.HasOne(d => d.PhoneNumber)
                    .WithMany(p => p.HospiceLocations)
                    .HasForeignKey(d => d.PhoneNumberId)
                    .HasConstraintName("FK_HospiceLocation_PhoneNumbers");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.HospiceLocations)
                    .HasForeignKey(d => d.SiteId)
                    .HasConstraintName("FK_HospiceLocations_Sites");

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany(p => p.HospiceLocationsUpdatedByUser)
                    .HasForeignKey(d => d.UpdatedByUserId)
                    .HasConstraintName("FK_HospiceLocations_ToUsers_Updated");
            });

            modelBuilder.Entity<HospiceMember>(entity =>
            {
                entity.ToTable("HospiceMember", "core");

                entity.Property(e => e.AdditionalField1).HasDefaultValueSql("((0))");

                entity.Property(e => e.CanAccessWebStore).HasDefaultValueSql("((0))");

                entity.Property(e => e.CanApproveOrder).HasDefaultValueSql("((0))");

                entity.Property(e => e.Designation).IsUnicode(false);

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.HospiceMemberCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .HasConstraintName("FK_HospiceMember_ToUsers_Created");

                entity.HasOne(d => d.Hospice)
                    .WithMany(p => p.HospiceMember)
                    .HasForeignKey(d => d.HospiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HospiceUser_Hospice");

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany(p => p.HospiceMemberUpdatedByUser)
                    .HasForeignKey(d => d.UpdatedByUserId)
                    .HasConstraintName("FK_HospiceMember_ToUsers_Updated");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HospiceMemberUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HospiceUser_Users");
            });

            modelBuilder.Entity<Hospices>(entity =>
            {
                entity.ToTable("Hospices", "core");

                entity.Property(e => e.CreditHoldNote).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Hospices)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_Hospices_Address");

                entity.HasOne(d => d.AssignedSite)
                    .WithMany(p => p.Hospices)
                    .HasForeignKey(d => d.AssignedSiteId)
                    .HasConstraintName("FK_Hospices_ToSite");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.HospicesCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .HasConstraintName("FK_Hospices_Users_Created");

                entity.HasOne(d => d.CreditHoldByUser)
                    .WithMany(p => p.HospicesCreditHoldByUser)
                    .HasForeignKey(d => d.CreditHoldByUserId)
                    .HasConstraintName("FK_Hospices_Credit_Hold_By_User");

                entity.HasOne(d => d.CustomerType)
                    .WithMany(p => p.Hospices)
                    .HasForeignKey(d => d.CustomerTypeId)
                    .HasConstraintName("FK_Hospices_ToCustomerTypes");

                entity.HasOne(d => d.DeletedByUser)
                    .WithMany(p => p.HospicesDeletedByUser)
                    .HasForeignKey(d => d.DeletedByUserId)
                    .HasConstraintName("FK_Hospices_Users_Deleted");

                entity.HasOne(d => d.PhoneNumber)
                    .WithMany(p => p.Hospices)
                    .HasForeignKey(d => d.PhoneNumberId)
                    .HasConstraintName("FK_Hospices_PhoneNumbers");

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany(p => p.HospicesUpdatedByUser)
                    .HasForeignKey(d => d.UpdatedByUserId)
                    .HasConstraintName("FK_Hospices_Users_Updated");
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.ToTable("Inventory", "core");

                entity.HasIndex(e => e.NetSuiteInventoryId, "IX_NetsuiteInventoryId");

                entity.Property(e => e.AdditionalField1).IsUnicode(false);

                entity.Property(e => e.AssetTagNumber).IsUnicode(false);

                entity.Property(e => e.LotNumber).IsUnicode(false);

                entity.Property(e => e.SerialNumber).IsUnicode(false);

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.InventoryCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .HasConstraintName("FK_Inventory_Users_Created");

                entity.HasOne(d => d.DeletedByUser)
                    .WithMany(p => p.InventoryDeletedByUser)
                    .HasForeignKey(d => d.DeletedByUserId)
                    .HasConstraintName("FK_Inventory_Users_Deleted");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Inventory)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_Inventory_ToItems");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Inventory)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inventory_ToInventoryStatusTypes");

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany(p => p.InventoryUpdatedByUser)
                    .HasForeignKey(d => d.UpdatedByUserId)
                    .HasConstraintName("FK_Inventory_Users_Updated");
            });

            modelBuilder.Entity<InventoryStatusTypes>(entity =>
            {
                entity.ToTable("InventoryStatusTypes", "core");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ItemCategories>(entity =>
            {
                entity.ToTable("ItemCategories", "core");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.ItemCategoriesCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .HasConstraintName("FK_ItemCategories_Users_Created");

                entity.HasOne(d => d.DeletedByUser)
                    .WithMany(p => p.ItemCategoriesDeletedByUser)
                    .HasForeignKey(d => d.DeletedByUserId)
                    .HasConstraintName("FK_ItemCategories_Users_Deleted");

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany(p => p.ItemCategoriesUpdatedByUser)
                    .HasForeignKey(d => d.UpdatedByUserId)
                    .HasConstraintName("FK_ItemCategories_Users_Updated");
            });

            modelBuilder.Entity<ItemCategoryMapping>(entity =>
            {
                entity.ToTable("ItemCategoryMapping", "core");

                entity.HasOne(d => d.ItemCategory)
                    .WithMany(p => p.ItemCategoryMapping)
                    .HasForeignKey(d => d.ItemCategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ItemCategoryMapping_ItemCategories");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemCategoryMapping)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ItemCategoryMapping_Items");
            });

            modelBuilder.Entity<ItemImageFiles>(entity =>
            {
                entity.ToTable("ItemImageFiles", "core");

                entity.HasIndex(e => e.FileMetadataId, "AK_IMAGE_FILEMETADATA")
                    .IsUnique();

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.ItemImageFilesCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemImageFiles_ToUsers_Created");

                entity.HasOne(d => d.FileMetadata)
                    .WithOne(p => p.ItemImageFiles)
                    .HasForeignKey<ItemImageFiles>(d => d.FileMetadataId)
                    .HasConstraintName("FK_ItemImageFiles_ToFilesMetadata");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemImageFiles)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemImageFiles_ToItems");

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany(p => p.ItemImageFilesUpdatedByUser)
                    .HasForeignKey(d => d.UpdatedByUserId)
                    .HasConstraintName("FK_ItemImageFiles_ToUsers_Updated");
            });

            modelBuilder.Entity<ItemImages>(entity =>
            {
                entity.ToTable("ItemImages", "core");

                entity.Property(e => e.Url).IsRequired();

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.ItemImagesCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .HasConstraintName("FK_ItemImages_Users_Created");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemImages)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_ItemImages_ToItems");

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany(p => p.ItemImagesUpdatedByUser)
                    .HasForeignKey(d => d.UpdatedByUserId)
                    .HasConstraintName("FK_ItemImages_Users_Updated");
            });

            modelBuilder.Entity<ItemSubCategories>(entity =>
            {
                entity.ToTable("ItemSubCategories", "core");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.ItemSubCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemSubCategories_ToItemCategories");
            });

            modelBuilder.Entity<ItemSubCategoryMapping>(entity =>
            {
                entity.ToTable("ItemSubCategoryMapping", "core");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemSubCategoryMapping)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ItemSubCategoryMapping_Items");

                entity.HasOne(d => d.ItemSubCategory)
                    .WithMany(p => p.ItemSubCategoryMapping)
                    .HasForeignKey(d => d.ItemSubCategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ItemSubCategoryMapping_ItemCategories");
            });

            modelBuilder.Entity<ItemTransferRequests>(entity =>
            {
                entity.ToTable("ItemTransferRequests", "core");

                entity.Property(e => e.ItemCount).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.ItemTransferRequestsCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransferReqeust_Users_Created");

                entity.HasOne(d => d.DestinationSiteMember)
                    .WithMany(p => p.ItemTransferRequests)
                    .HasForeignKey(d => d.DestinationSiteMemberId)
                    .HasConstraintName("FK_TransferRequests_ToSiteMembers");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemTransferRequests)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransferRequest_ToItems");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.ItemTransferRequests)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransferRequest_ToTransferRequestStatusTypes");

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany(p => p.ItemTransferRequestsUpdatedByUser)
                    .HasForeignKey(d => d.UpdatedByUserId)
                    .HasConstraintName("FK_TransferReqeust_Users_Updated");
            });

            modelBuilder.Entity<Items>(entity =>
            {
                entity.ToTable("Items", "core");

                entity.HasIndex(e => e.NetSuiteItemId, "IX_NetsuiteItemId");

                entity.Property(e => e.AverageCost).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.AvgDeliveryProcessingTime).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.AvgPickUpProcessingTime).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.CogsAccountName).IsUnicode(false);

                entity.Property(e => e.Depreciation).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.IsDme).HasColumnName("IsDME");

                entity.Property(e => e.ItemNumber)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.ItemsCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .HasConstraintName("FK_Items_Users_Created");

                entity.HasOne(d => d.DeletedByUser)
                    .WithMany(p => p.ItemsDeletedByUser)
                    .HasForeignKey(d => d.DeletedByUserId)
                    .HasConstraintName("FK_Items_Users_Deleted");

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany(p => p.ItemsUpdatedByUser)
                    .HasForeignKey(d => d.UpdatedByUserId)
                    .HasConstraintName("FK_Items_Users_Updated");
            });

            modelBuilder.Entity<OrderFulfillmentLineItems>(entity =>
            {
                entity.ToTable("OrderFulfillmentLineItems", "core");

                entity.Property(e => e.AssetTag).IsUnicode(false);

                entity.Property(e => e.DeliveredStatus).IsUnicode(false);

                entity.Property(e => e.FulfillmentEndAtLatitude).HasColumnType("decimal(11, 8)");

                entity.Property(e => e.FulfillmentEndAtLongitude).HasColumnType("decimal(11, 8)");

                entity.Property(e => e.FulfillmentStartAtLatitude).HasColumnType("decimal(11, 8)");

                entity.Property(e => e.FulfillmentStartAtLongitude).HasColumnType("decimal(11, 8)");

                entity.Property(e => e.LotNumber).IsUnicode(false);

                entity.Property(e => e.OrderType).IsUnicode(false);

                entity.Property(e => e.PatientUuid).HasColumnName("PatientUUID");

                entity.Property(e => e.SerialNumber).IsUnicode(false);

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.OrderFulfillmentLineItemsCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .HasConstraintName("FK_OrderLineItemFulfilment_Users_Created");

                entity.HasOne(d => d.FulfilledByDriver)
                    .WithMany(p => p.OrderFulfillmentLineItems)
                    .HasForeignKey(d => d.FulfilledByDriverId)
                    .HasConstraintName("FK_OrderLineItemFulfilment_Drivers");

                entity.HasOne(d => d.FulfilledByVehicle)
                    .WithMany(p => p.OrderFulfillmentLineItems)
                    .HasForeignKey(d => d.FulfilledByVehicleId)
                    .HasConstraintName("FK_OrderLineItemFulfilment_Vehicles");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OrderFulfillmentLineItems)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderLineItemFulfilment_Items");

                entity.HasOne(d => d.OrderHeader)
                    .WithMany(p => p.OrderFulfillmentLineItems)
                    .HasForeignKey(d => d.OrderHeaderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderLineItemFulfilment_ToOrderHeaders");

                entity.HasOne(d => d.OrderLineItem)
                    .WithMany(p => p.OrderFulfillmentLineItems)
                    .HasForeignKey(d => d.OrderLineItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderLineItemFulfilment_ToOrderLineItems");

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany(p => p.OrderFulfillmentLineItemsUpdatedByUser)
                    .HasForeignKey(d => d.UpdatedByUserId)
                    .HasConstraintName("FK_OrderLineItemFulfilment_Users_Updated");
            });

            modelBuilder.Entity<OrderHeaderStatusTypes>(entity =>
            {
                entity.ToTable("OrderHeaderStatusTypes", "core");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OrderHeaders>(entity =>
            {
                entity.ToTable("OrderHeaders", "core");

                entity.Property(e => e.ConfirmationNumber).IsUnicode(false);

                entity.Property(e => e.ExternalOrderNumber).IsUnicode(false);

                entity.Property(e => e.FulfillmentEndAtLatitude).HasColumnType("decimal(11, 8)");

                entity.Property(e => e.FulfillmentEndAtLongitude).HasColumnType("decimal(11, 8)");

                entity.Property(e => e.FulfillmentStartAtLatitude).HasColumnType("decimal(11, 8)");

                entity.Property(e => e.FulfillmentStartAtLongitude).HasColumnType("decimal(11, 8)");

                entity.Property(e => e.OrderNumber)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(NEXT VALUE FOR [core].[OrderNumberSequence])");

                entity.Property(e => e.OrderTypeId).HasDefaultValueSql("((1))");

                entity.Property(e => e.PartialFulfillmentReason).IsUnicode(false);

                entity.Property(e => e.PatientUuid).HasColumnName("PatientUUID");

                entity.Property(e => e.PickupReason).IsUnicode(false);

                entity.Property(e => e.StatOrder).HasDefaultValueSql("((0))");

                entity.Property(e => e.StatusId).HasDefaultValueSql("((10))");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.OrderHeadersCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .HasConstraintName("FK_OrderHeaders_Users_Created");

                entity.HasOne(d => d.DeliveryAddress)
                    .WithMany(p => p.OrderHeadersDeliveryAddress)
                    .HasForeignKey(d => d.DeliveryAddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderHeaders_ToAddress_Delivery");

                entity.HasOne(d => d.DispatchStatus)
                    .WithMany(p => p.OrderHeadersDispatchStatus)
                    .HasForeignKey(d => d.DispatchStatusId)
                    .HasConstraintName("FK_OrderHeaders_ToDispatchStatusTypes");

                entity.HasOne(d => d.Hospice)
                    .WithMany(p => p.OrderHeaders)
                    .HasForeignKey(d => d.HospiceId)
                    .HasConstraintName("FK_OrderHeaders_ToHospices");

                entity.HasOne(d => d.HospiceLocation)
                    .WithMany(p => p.OrderHeaders)
                    .HasForeignKey(d => d.HospiceLocationId)
                    .HasConstraintName("FK_OrderHeaders_ToHospiceLocations");

                entity.HasOne(d => d.HospiceMember)
                    .WithMany(p => p.OrderHeaders)
                    .HasForeignKey(d => d.HospiceMemberId)
                    .HasConstraintName("FK_OrderHeaders_HospiceMember");

                entity.HasOne(d => d.OrderType)
                    .WithMany(p => p.OrderHeaders)
                    .HasForeignKey(d => d.OrderTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderHeaders_ToOrderTypes");

                entity.HasOne(d => d.PickupAddress)
                    .WithMany(p => p.OrderHeadersPickupAddress)
                    .HasForeignKey(d => d.PickupAddressId)
                    .HasConstraintName("FK_OrderHeaders_ToAddress_Pickup");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.OrderHeaders)
                    .HasForeignKey(d => d.SiteId)
                    .HasConstraintName("FK_OrderHeaders_ToSites");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.OrderHeadersStatus)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderHeaders_ToOrderStatusTypes");

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany(p => p.OrderHeadersUpdatedByUser)
                    .HasForeignKey(d => d.UpdatedByUserId)
                    .HasConstraintName("FK_OrderHeaders_Users_Updated");
            });

            modelBuilder.Entity<OrderLineItemStatusTypes>(entity =>
            {
                entity.ToTable("OrderLineItemStatusTypes", "core");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OrderLineItems>(entity =>
            {
                entity.ToTable("OrderLineItems", "core");

                entity.Property(e => e.AdditionalField2).HasColumnType("decimal(11, 8)");

                entity.Property(e => e.AdditionalField3).HasColumnType("decimal(11, 8)");

                entity.Property(e => e.AdditionalField5).HasColumnType("decimal(11, 8)");

                entity.Property(e => e.AdditionalField6).HasColumnType("decimal(11, 8)");

                entity.Property(e => e.AssetTagNumber).IsUnicode(false);

                entity.Property(e => e.ItemCount).HasDefaultValueSql("((1))");

                entity.Property(e => e.LotNumber).IsUnicode(false);

                entity.Property(e => e.SerialNumber).IsUnicode(false);

                entity.HasOne(d => d.Action)
                    .WithMany(p => p.OrderLineItems)
                    .HasForeignKey(d => d.ActionId)
                    .HasConstraintName("FK_OrderLineItems_ToOrderTypes");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.OrderLineItemsCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .HasConstraintName("FK_OrderLineItems_Users_Created");

                entity.HasOne(d => d.DeliveryAddress)
                    .WithMany(p => p.OrderLineItemsDeliveryAddress)
                    .HasForeignKey(d => d.DeliveryAddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderLineItems_ToAddresses_Delivery");

                entity.HasOne(d => d.DispatchStatus)
                    .WithMany(p => p.OrderLineItemsDispatchStatus)
                    .HasForeignKey(d => d.DispatchStatusId)
                    .HasConstraintName("FK_OrderLineItems_ToOrderLineItemDispatchStatusTypes");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OrderLineItems)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_OrderLineItems_ToItems");

                entity.HasOne(d => d.OrderHeader)
                    .WithMany(p => p.OrderLineItems)
                    .HasForeignKey(d => d.OrderHeaderId)
                    .HasConstraintName("FK_OrderLineItems_ToOrderHeaders");

                entity.HasOne(d => d.PickupAddress)
                    .WithMany(p => p.OrderLineItemsPickupAddress)
                    .HasForeignKey(d => d.PickupAddressId)
                    .HasConstraintName("FK_OrderLineItems_ToAddresses_Pickup");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.OrderLineItems)
                    .HasForeignKey(d => d.SiteId)
                    .HasConstraintName("FK_OrderLineItems_ToSites");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.OrderLineItemsStatus)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_OrderLineItems_ToOrderLineItemStatusTypes");

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany(p => p.OrderLineItemsUpdatedByUser)
                    .HasForeignKey(d => d.UpdatedByUserId)
                    .HasConstraintName("FK_OrderLineItems_Users_Updated");
            });

            modelBuilder.Entity<OrderNotes>(entity =>
            {
                entity.ToTable("OrderNotes", "core");

                entity.Property(e => e.Note).IsRequired();

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.OrderNotesCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderNotes_Users_Created");

                entity.HasOne(d => d.HospiceMember)
                    .WithMany(p => p.OrderNotes)
                    .HasForeignKey(d => d.HospiceMemberId)
                    .HasConstraintName("FK_OrderNotes_HospiceMember");

                entity.HasOne(d => d.OrderHeader)
                    .WithMany(p => p.OrderNotes)
                    .HasForeignKey(d => d.OrderHeaderId)
                    .HasConstraintName("FK_OrderNotes_OrderHeaders");

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany(p => p.OrderNotesUpdatedByUser)
                    .HasForeignKey(d => d.UpdatedByUserId)
                    .HasConstraintName("FK_OrderNotes_Users_Updated");
            });

            modelBuilder.Entity<OrderTypes>(entity =>
            {
                entity.ToTable("OrderTypes", "core");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PatientInventory>(entity =>
            {
                entity.ToTable("PatientInventory", "core");

                entity.Property(e => e.AdditionalField1).IsUnicode(false);

                entity.Property(e => e.DataBridgeRunUuid).HasColumnName("DataBridgeRunUUID");

                entity.Property(e => e.DeliveryAddressUuid).HasColumnName("DeliveryAddressUUID");

                entity.Property(e => e.ItemCount).HasDefaultValueSql("((1))");

                entity.Property(e => e.PatientUuid).HasColumnName("PatientUUID");

                entity.HasOne(d => d.Inventory)
                    .WithMany(p => p.PatientInventory)
                    .HasForeignKey(d => d.InventoryId)
                    .HasConstraintName("FK_PatientInventory_Inventory");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.PatientInventory)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PatientInventory_ToItems");

                entity.HasOne(d => d.OrderHeader)
                    .WithMany(p => p.PatientInventory)
                    .HasForeignKey(d => d.OrderHeaderId)
                    .HasConstraintName("FK_PatientInventory_OrderHeader");

                entity.HasOne(d => d.OrderLineItem)
                    .WithMany(p => p.PatientInventory)
                    .HasForeignKey(d => d.OrderLineItemId)
                    .HasConstraintName("FK_PatientInventory_OrderLineItems");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.PatientInventory)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PatientInventory_ToInventoryStatusTypes");
            });

            modelBuilder.Entity<PermissionNouns>(entity =>
            {
                entity.ToTable("PermissionNouns", "core");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<PermissionVerbs>(entity =>
            {
                entity.ToTable("PermissionVerbs", "core");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PhoneNumberTypes>(entity =>
            {
                entity.ToTable("PhoneNumberTypes", "core");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PhoneNumbers>(entity =>
            {
                entity.ToTable("PhoneNumbers", "core");

                entity.Property(e => e.SkentityId).HasColumnName("SKEntityID");

                entity.Property(e => e.SkentityType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SKEntityType");

                entity.HasOne(d => d.NumberType)
                    .WithMany(p => p.PhoneNumbers)
                    .HasForeignKey(d => d.NumberTypeId)
                    .HasConstraintName("FK_PhoneNumbers_PhoneNumberTypes");
            });

            modelBuilder.Entity<PrePostDeploymentScriptRuns>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("PrePostDeploymentScriptRuns", "core");

                entity.Property(e => e.ScriptName)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RolePermissions>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.PermissionNounId, e.PermissionVerbId });

                entity.ToTable("RolePermissions", "core");

                entity.HasOne(d => d.PermissionNoun)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(d => d.PermissionNounId)
                    .HasConstraintName("FK_RolePermission_ToPermissionNouns");

                entity.HasOne(d => d.PermissionVerb)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(d => d.PermissionVerbId)
                    .HasConstraintName("FK_RolePermissions_ToPermissionVerbs");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_RolePermission_ToRoles");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.ToTable("Roles", "core");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RoleType)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Internal')");
            });

            modelBuilder.Entity<SiteMembers>(entity =>
            {
                entity.ToTable("SiteMembers", "core");

                entity.Property(e => e.Designation).IsUnicode(false);

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.SiteMembersCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SiteMembers_ToUsers_Created");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.SiteMembers)
                    .HasForeignKey(d => d.SiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SiteMembers_Sites");

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany(p => p.SiteMembersUpdatedByUser)
                    .HasForeignKey(d => d.UpdatedByUserId)
                    .HasConstraintName("FK_SiteMembers_ToUsers_Updated");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SiteMembersUser)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_SiteMembers_Users");
            });

            modelBuilder.Entity<SitePhoneNumber>(entity =>
            {
                entity.ToTable("SitePhoneNumber", "core");

                entity.HasOne(d => d.PhoneNumber)
                    .WithMany(p => p.SitePhoneNumber)
                    .HasForeignKey(d => d.PhoneNumberId)
                    .HasConstraintName("FK_SitePhoneNumber_PhoneNumbers");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.SitePhoneNumber)
                    .HasForeignKey(d => d.SiteId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_SitePhoneNumber_Sites");
            });

            modelBuilder.Entity<SiteServiceAreas>(entity =>
            {
                entity.ToTable("SiteServiceAreas", "core");

                entity.Property(e => e.AdditionalField1).HasColumnType("decimal(11, 8)");

                entity.Property(e => e.AdditionalField2).HasColumnType("decimal(11, 8)");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.SiteServiceAreasCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .HasConstraintName("FK_SiteServiceAreas_Created_User");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.SiteServiceAreas)
                    .HasForeignKey(d => d.SiteId)
                    .HasConstraintName("FK_SiteServiceAreas_ToSites");

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany(p => p.SiteServiceAreasUpdatedByUser)
                    .HasForeignKey(d => d.UpdatedByUserId)
                    .HasConstraintName("FK_SiteServiceAreas_Updated_User");
            });

            modelBuilder.Entity<Sites>(entity =>
            {
                entity.ToTable("Sites", "core");

                entity.Property(e => e.Capacity).HasColumnType("decimal(15, 4)");

                entity.Property(e => e.Cvn)
                    .IsUnicode(false)
                    .HasColumnName("CVN");

                entity.Property(e => e.Length).HasColumnType("decimal(15, 4)");

                entity.Property(e => e.LicensePlate).IsUnicode(false);

                entity.Property(e => e.LocationType).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Vin)
                    .IsUnicode(false)
                    .HasColumnName("VIN");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Sites)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_Sites_Address");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.SitesCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .HasConstraintName("FK_Sites_Users_Created");

                entity.HasOne(d => d.DeletedByUser)
                    .WithMany(p => p.SitesDeletedByUser)
                    .HasForeignKey(d => d.DeletedByUserId)
                    .HasConstraintName("FK_Sites_Users_Deleted");

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany(p => p.SitesUpdatedByUser)
                    .HasForeignKey(d => d.UpdatedByUserId)
                    .HasConstraintName("FK_Sites_Users_Updated");
            });

            modelBuilder.Entity<StorageTypes>(entity =>
            {
                entity.ToTable("StorageTypes", "core");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SubscriptionItems>(entity =>
            {
                entity.ToTable("SubscriptionItems", "core");

                entity.Property(e => e.ChargeScheduleUsed).IsUnicode(false);

                entity.Property(e => e.Currency).IsUnicode(false);

                entity.Property(e => e.ExcludeChargesFromBillingWhen).IsUnicode(false);

                entity.Property(e => e.InheritChargeScheduleFrom).IsUnicode(false);

                entity.Property(e => e.ItemDescription).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.ProRationType).IsUnicode(false);

                entity.Property(e => e.RatePlan).IsUnicode(false);

                entity.Property(e => e.RateType).IsUnicode(false);

                entity.Property(e => e.RenewalItem).IsUnicode(false);

                entity.HasOne(d => d.Hospice)
                    .WithMany(p => p.SubscriptionItems)
                    .HasForeignKey(d => d.HospiceId)
                    .HasConstraintName("FK_SubscriptionItems_ToHospices");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.SubscriptionItems)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_SubscriptionItems_ToItems");

                entity.HasOne(d => d.Subscription)
                    .WithMany(p => p.SubscriptionItems)
                    .HasForeignKey(d => d.SubscriptionId)
                    .HasConstraintName("FK_SubscriptionItems_ToSubscriptions");
            });

            modelBuilder.Entity<Subscriptions>(entity =>
            {
                entity.ToTable("Subscriptions", "core");

                entity.Property(e => e.BillToCustomer).IsUnicode(false);

                entity.Property(e => e.BillToEntity).IsUnicode(false);

                entity.Property(e => e.BillingProfile).IsUnicode(false);

                entity.Property(e => e.ChargeSchedule).IsUnicode(false);

                entity.Property(e => e.Currency).IsUnicode(false);

                entity.Property(e => e.EnableLineItemShipping).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RenewalTemplate).IsUnicode(false);

                entity.HasOne(d => d.Hospice)
                    .WithMany(p => p.Subscriptions)
                    .HasForeignKey(d => d.HospiceId)
                    .HasConstraintName("FK_Subscriptions_ToHospices");
            });

            modelBuilder.Entity<TransferRequestStatusTypes>(entity =>
            {
                entity.ToTable("TransferRequestStatusTypes", "core");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserProfilePicture>(entity =>
            {
                entity.ToTable("UserProfilePicture", "core");

                entity.HasIndex(e => e.FileMetadataId, "AK_USERPROFILEPICTURE_FILEMETADATA")
                    .IsUnique();

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.UserProfilePictureCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserProfilePicture_ToUsers_Created");

                entity.HasOne(d => d.FileMetadata)
                    .WithOne(p => p.UserProfilePicture)
                    .HasForeignKey<UserProfilePicture>(d => d.FileMetadataId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserProfilePicture_FiltesMetadata");

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany(p => p.UserProfilePictureUpdatedByUser)
                    .HasForeignKey(d => d.UpdatedByUserId)
                    .HasConstraintName("FK_UserProfilePicture_ToUsers_Updated");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserProfilePictureUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserProfilePicture_User");
            });

            modelBuilder.Entity<UserRoles>(entity =>
            {
                entity.ToTable("UserRoles", "core");

                entity.Property(e => e.ResourceId)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.ResourceType)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_UserRoles_ToRole");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserRoles_ToUsers");
            });

            modelBuilder.Entity<UserVerify>(entity =>
            {
                entity.ToTable("UserVerify", "core");

                entity.Property(e => e.Nonce).IsRequired();
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("Users", "core");

                entity.Property(e => e.CognitoUserId).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.SklocationId).HasColumnName("SKLocationID");

                entity.Property(e => e.SkroleId).HasColumnName("SKRoleID");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.InverseCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .HasConstraintName("FK_Created_User");

                entity.HasOne(d => d.DisabledByUser)
                    .WithMany(p => p.InverseDisabledByUser)
                    .HasForeignKey(d => d.DisabledByUserId)
                    .HasConstraintName("FK_Disabled_User");
            });

            modelBuilder.HasSequence("OrderNumberSequence", "core").StartsAt(1452);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
