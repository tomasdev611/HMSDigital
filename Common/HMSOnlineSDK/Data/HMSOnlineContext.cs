using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using HMSOnlineSDK.Data.Models;

namespace HMSOnlineSDK.Data
{
    public partial class HMSOnlineContext : DbContext
    {
        public HMSOnlineContext()
        {
        }

        public HMSOnlineContext(DbContextOptions<HMSOnlineContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Hms2OrderAuditLogs> Hms2OrderAuditLogs { get; set; }
        public virtual DbSet<Hms2PatientLinkAuditLogs> Hms2PatientLinkAuditLogs { get; set; }
        public virtual DbSet<ItemAuditLogs> ItemAuditLogs { get; set; }
        public virtual DbSet<Items> Items { get; set; }
        public virtual DbSet<Manufacturers> Manufacturers { get; set; }
        public virtual DbSet<OrderTracking> OrderTracking { get; set; }
        public virtual DbSet<PatientEquipmentTracking> PatientEquipmentTracking { get; set; }
        public virtual DbSet<ProductCategories> ProductCategories { get; set; }
        public virtual DbSet<ProductVariants> ProductVariants { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<ProductsOld> ProductsOld { get; set; }
        public virtual DbSet<QuantityTrackedItemAuditLogs> QuantityTrackedItemAuditLogs { get; set; }
        public virtual DbSet<QuantityTrackedItems> QuantityTrackedItems { get; set; }
        public virtual DbSet<QuantityTrackedItemsCopy1> QuantityTrackedItemsCopy1 { get; set; }
        public virtual DbSet<Sites> Sites { get; set; }
        public virtual DbSet<Temp> Temp { get; set; }
        public virtual DbSet<UserInformations> UserInformations { get; set; }
        public virtual DbSet<UserSites> UserSites { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hms2OrderAuditLogs>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Action)
                    .IsRequired()
                    .HasColumnName("action")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Hms2OrderId)
                    .HasColumnName("hms2OrderId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Parameters)
                    .IsRequired()
                    .HasColumnName("parameters")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.QueryString)
                    .IsRequired()
                    .HasColumnName("queryString")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Hms2PatientLinkAuditLogs>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Action)
                    .IsRequired()
                    .HasColumnName("action")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Hms2PatientLinkId)
                    .HasColumnName("hms2PatientLinkId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Parameters)
                    .IsRequired()
                    .HasColumnName("parameters")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.QueryString)
                    .IsRequired()
                    .HasColumnName("queryString")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<ItemAuditLogs>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Action)
                    .IsRequired()
                    .HasColumnName("action")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ItemId)
                    .HasColumnName("itemId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Parameters)
                    .IsRequired()
                    .HasColumnName("parameters")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.QueryString)
                    .IsRequired()
                    .HasColumnName("queryString")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Items>(entity =>
            {
                entity.HasIndex(e => e.AssetTag)
                    .HasName("assetTag")
                    .IsUnique();

                entity.HasIndex(e => e.DestinationSiteId)
                    .HasName("fk_transfer_site");

                entity.HasIndex(e => e.ProductVariantId)
                    .HasName("Items_ProductVariants_FK");

                entity.HasIndex(e => e.SiteId)
                    .HasName("Items_Sites_FK");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AcquiredDate)
                    .HasColumnName("acquiredDate")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.AssetTag)
                    .HasColumnName("assetTag")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CleanedDate)
                    .HasColumnName("cleanedDate")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DestinationSiteId)
                    .HasColumnName("destinationSiteId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InTransit)
                    .HasColumnName("inTransit")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.IsClean)
                    .HasColumnName("isClean")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.LotNumber)
                    .HasColumnName("lotNumber")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ManufacturedDate)
                    .HasColumnName("manufacturedDate")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ProductVariantId)
                    .HasColumnName("productVariantId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SerialNumber)
                    .HasColumnName("serialNumber")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.SiteId)
                    .HasColumnName("siteId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasColumnType("enum('Available','On Truck','Not Ready','On Patient','Disposed','Disposal Requested')")
                    .HasDefaultValueSql("'Available'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updatedBy")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.DestinationSite)
                    .WithMany(p => p.ItemsDestinationSite)
                    .HasForeignKey(d => d.DestinationSiteId)
                    .HasConstraintName("Items_ibfk_3");

                entity.HasOne(d => d.ProductVariant)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.ProductVariantId)
                    .HasConstraintName("Items_ibfk_1");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.ItemsSite)
                    .HasForeignKey(d => d.SiteId)
                    .HasConstraintName("Items_ibfk_2");
            });

            modelBuilder.Entity<Manufacturers>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updatedBy")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<OrderTracking>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ArrivalDate)
                    .HasColumnName("arrivalDate")
                    .HasColumnType("timestamp");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DeliveryEndDate)
                    .HasColumnName("deliveryEndDate")
                    .HasColumnType("timestamp");

                entity.Property(e => e.DeliveryStartDate)
                    .HasColumnName("deliveryStartDate")
                    .HasColumnType("timestamp");

                entity.Property(e => e.DepartureDate)
                    .HasColumnName("departureDate")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Hms2OrderId)
                    .HasColumnName("hms2OrderId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Hms2PatientId)
                    .HasColumnName("hms2PatientId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Loaded).HasColumnName("loaded");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updatedBy")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<PatientEquipmentTracking>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AssetTag)
                    .IsRequired()
                    .HasColumnName("assetTag")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DeliveryOrderTrackingId)
                    .HasColumnName("deliveryOrderTrackingId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Hms2PatientId)
                    .HasColumnName("hms2PatientId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InvCode)
                    .IsRequired()
                    .HasColumnName("invCode")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ItemId)
                    .HasColumnName("itemId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Pickedup)
                    .HasColumnName("pickedup")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PickupOrderTrackingId)
                    .HasColumnName("pickupOrderTrackingId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.QuantityTrackedItemId)
                    .HasColumnName("quantityTrackedItemId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updatedBy")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<ProductCategories>(entity =>
            {
                entity.HasIndex(e => e.ParentCategoryId)
                    .HasName("ProductCategories_ProductCategories_FK");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CatalogName)
                    .HasColumnName("catalogName")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ParentCategoryId)
                    .HasColumnName("parentCategoryId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updatedBy")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.WarehouseName)
                    .HasColumnName("warehouseName")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.ParentCategory)
                    .WithMany(p => p.InverseParentCategory)
                    .HasForeignKey(d => d.ParentCategoryId)
                    .HasConstraintName("ProductCategories_ProductCategories_FK");
            });

            modelBuilder.Entity<ProductVariants>(entity =>
            {
                entity.HasIndex(e => e.ManufacturerId)
                    .HasName("ProductVariants_Manufacturers_FK");

                entity.HasIndex(e => e.ProductId)
                    .HasName("ProductVariants_Products_FK");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ManufacturerId)
                    .HasColumnName("manufacturerId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ManufacturerModel)
                    .HasColumnName("manufacturerModel")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ProductId)
                    .HasColumnName("productId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updatedBy")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Manufacturer)
                    .WithMany(p => p.ProductVariants)
                    .HasForeignKey(d => d.ManufacturerId)
                    .HasConstraintName("ProductVariants_Manufacturers_FK");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductVariants)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("ProductVariants_Products_FK");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasIndex(e => e.CategoryId)
                    .HasName("Products_ProductCategories_FK");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.AssetTagRequired)
                    .HasColumnName("assetTagRequired")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CatalogName)
                    .HasColumnName("catalogName")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("categoryId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FieldCleanAllowed)
                    .HasColumnName("fieldCleanAllowed")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.InvCode)
                    .HasColumnName("invCode")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.IsSoftGood)
                    .HasColumnName("isSoftGood")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.LotNumRequired)
                    .HasColumnName("lotNumRequired")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.QuantityTracked)
                    .HasColumnName("quantityTracked")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.QuantityTrackedAssetTag)
                    .HasColumnName("quantityTrackedAssetTag")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.SerialNumRequired)
                    .HasColumnName("serialNumRequired")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasColumnType("enum('equipment','disposable')")
                    .HasDefaultValueSql("'equipment'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updatedBy")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.WarehouseName)
                    .HasColumnName("warehouseName")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("Products_ibfk_1");
            });

            modelBuilder.Entity<ProductsOld>(entity =>
            {
                entity.ToTable("Products_old");

                entity.HasIndex(e => e.CategoryId)
                    .HasName("Products_ProductCategories_FK");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.AssetTagRequired)
                    .HasColumnName("assetTagRequired")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CatalogName)
                    .HasColumnName("catalogName")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("categoryId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FieldCleanAllowed)
                    .HasColumnName("fieldCleanAllowed")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.InvCode)
                    .HasColumnName("invCode")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.IsSoftGood)
                    .HasColumnName("isSoftGood")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.LotNumRequired)
                    .HasColumnName("lotNumRequired")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.QuantityTracked)
                    .HasColumnName("quantityTracked")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.QuantityTrackedAssetTag)
                    .HasColumnName("quantityTrackedAssetTag")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.SerialNumRequired)
                    .HasColumnName("serialNumRequired")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasColumnType("enum('equipment','disposable')")
                    .HasDefaultValueSql("'equipment'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updatedBy")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.WarehouseName)
                    .HasColumnName("warehouseName")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<QuantityTrackedItemAuditLogs>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Action)
                    .IsRequired()
                    .HasColumnName("action")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Parameters)
                    .IsRequired()
                    .HasColumnName("parameters")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.QuantityTrackedItemId).HasColumnType("int(11)");

                entity.Property(e => e.QueryString)
                    .IsRequired()
                    .HasColumnName("queryString")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<QuantityTrackedItems>(entity =>
            {
                entity.HasIndex(e => e.ProductVariantId)
                    .HasName("QuantityTrackedItems_productVariantId_IDX");

                entity.HasIndex(e => e.SiteId)
                    .HasName("QuantityTrackedItems_Sites_FK");

                entity.HasIndex(e => new { e.ProductVariantId, e.SiteId })
                    .HasName("QuantityTrackedItems_UK")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Available)
                    .HasColumnName("available")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DisposalRequested)
                    .HasColumnName("disposalRequested")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Disposed)
                    .HasColumnName("disposed")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NotReady)
                    .HasColumnName("notReady")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OnPatient)
                    .HasColumnName("onPatient")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OnTruck)
                    .HasColumnName("onTruck")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProductVariantId)
                    .HasColumnName("productVariantId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SiteId)
                    .HasColumnName("siteId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updatedBy")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<QuantityTrackedItemsCopy1>(entity =>
            {
                entity.ToTable("QuantityTrackedItems_copy1");

                entity.HasIndex(e => e.ProductVariantId)
                    .HasName("QuantityTrackedItems_productVariantId_IDX");

                entity.HasIndex(e => e.SiteId)
                    .HasName("QuantityTrackedItems_Sites_FK");

                entity.HasIndex(e => new { e.ProductVariantId, e.SiteId })
                    .HasName("QuantityTrackedItems_UK")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Available)
                    .HasColumnName("available")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DisposalRequested)
                    .HasColumnName("disposalRequested")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Disposed)
                    .HasColumnName("disposed")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NotReady)
                    .HasColumnName("notReady")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OnPatient)
                    .HasColumnName("onPatient")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OnTruck)
                    .HasColumnName("onTruck")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProductVariantId)
                    .HasColumnName("productVariantId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SiteId)
                    .HasColumnName("siteId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updatedBy")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.ProductVariant)
                    .WithMany(p => p.QuantityTrackedItemsCopy1)
                    .HasForeignKey(d => d.ProductVariantId)
                    .HasConstraintName("QuantityTrackedItems_copy1_ibfk_1");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.QuantityTrackedItemsCopy1)
                    .HasForeignKey(d => d.SiteId)
                    .HasConstraintName("QuantityTrackedItems_copy1_ibfk_2");
            });

            modelBuilder.Entity<Sites>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.SiteCode)
                    .HasColumnName("siteCode")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedAt")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updatedBy")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Temp>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.AssetOrQuantity)
                    .IsRequired()
                    .HasColumnName("assetOrQuantity")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DescriptionProper)
                    .IsRequired()
                    .HasColumnName("descriptionProper")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Disposable)
                    .IsRequired()
                    .HasColumnName("disposable")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DisposableBool)
                    .HasColumnName("disposableBool")
                    .HasColumnType("int(1)");

                entity.Property(e => e.InvCode)
                    .IsRequired()
                    .HasColumnName("invCode")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.LotNumBool)
                    .HasColumnName("lotNumBool")
                    .HasColumnType("int(1)");

                entity.Property(e => e.LotNumRequired)
                    .IsRequired()
                    .HasColumnName("lotNumRequired")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.QuantityTracked)
                    .HasColumnName("quantityTracked")
                    .HasColumnType("int(1)");

                entity.Property(e => e.SerialNumBool)
                    .HasColumnName("serialNumBool")
                    .HasColumnType("int(1)");

                entity.Property(e => e.SerialNumRequired)
                    .IsRequired()
                    .HasColumnName("serialNumRequired")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<UserInformations>(entity =>
            {
                entity.HasIndex(e => e.DefaultSiteId)
                    .HasName("UserInformations_Sites_FK");

                entity.HasIndex(e => e.Username)
                    .HasName("idx_username")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DefaultSiteId)
                    .HasColumnName("defaultSiteId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DriverId)
                    .HasColumnName("driverId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserUuid)
                    .HasColumnName("userUUId")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.DefaultSite)
                    .WithMany(p => p.UserInformations)
                    .HasForeignKey(d => d.DefaultSiteId)
                    .HasConstraintName("UserInformations_Sites_FK");
            });

            modelBuilder.Entity<UserSites>(entity =>
            {
                entity.HasIndex(e => e.UserId)
                    .HasName("UserSites_UserINformations_FK");

                entity.HasIndex(e => new { e.SiteId, e.UserId })
                    .HasName("idx_site_user")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SiteId)
                    .HasColumnName("siteId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.UserSites)
                    .HasForeignKey(d => d.SiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserSites_Sites_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSites)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserSites_UserINformations_FK");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
