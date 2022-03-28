using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Hms2BillingSDK.Models;

#nullable disable

namespace Hms2BillingSDK
{
    public partial class Hms2BillingContext : DbContext
    {
        public Hms2BillingContext()
        {
        }

        public Hms2BillingContext(DbContextOptions<Hms2BillingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Inventory> Inventory { get; set; }
        public virtual DbSet<LinkPatientProducts> LinkPatientProducts { get; set; }
        public virtual DbSet<Logging> Logging { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<PatientHms> PatientHms { get; set; }
        public virtual DbSet<Patientlineitems> Patientlineitems { get; set; }
        public virtual DbSet<TblBillingAdjReview> TblBillingAdjReview { get; set; }
        public virtual DbSet<TblBillingInvoiceCombine> TblBillingInvoiceCombine { get; set; }
        public virtual DbSet<TblBillingProcess> TblBillingProcess { get; set; }
        public virtual DbSet<TblContractInventory> TblContractInventory { get; set; }
        public virtual DbSet<TblContracts> TblContracts { get; set; }
        public virtual DbSet<TblCustomers> TblCustomers { get; set; }
        public virtual DbSet<TblHospices> TblHospices { get; set; }
        public virtual DbSet<TblInventory> TblInventory { get; set; }
        public virtual DbSet<TblInvoices> TblInvoices { get; set; }
        public virtual DbSet<TblLocations> TblLocations { get; set; }
        public virtual DbSet<TblMessages> TblMessages { get; set; }
        public virtual DbSet<TblOrders> TblOrders { get; set; }
        public virtual DbSet<TblPatientBillingGaps> TblPatientBillingGaps { get; set; }
        public virtual DbSet<TblUnadjustedForecast> TblUnadjustedForecast { get; set; }
        public virtual DbSet<TblUsers> TblUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.ToTable("inventory");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.Disposable, "disposable");

                entity.HasIndex(e => e.Inactive, "inactive");

                entity.HasIndex(e => e.InvSerial, "inv_serial");

                entity.HasIndex(e => e.Supply, "supply");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.Concentrator)
                    .HasColumnType("int(11)")
                    .HasColumnName("concentrator");

                entity.Property(e => e.DefaultRentPrice)
                    .HasPrecision(20, 2)
                    .HasColumnName("default_rent_price");

                entity.Property(e => e.DefaultSalePrice)
                    .HasPrecision(20, 2)
                    .HasColumnName("default_sale_price");

                entity.Property(e => e.Disposable)
                    .HasColumnType("int(11)")
                    .HasColumnName("disposable");

                entity.Property(e => e.Enteral)
                    .HasColumnType("int(11)")
                    .HasColumnName("enteral");

                entity.Property(e => e.Hidden)
                    .HasColumnType("int(11)")
                    .HasColumnName("hidden");

                entity.Property(e => e.Inactive)
                    .HasColumnType("int(11)")
                    .HasColumnName("inactive");

                entity.Property(e => e.InvCode)
                    .HasMaxLength(20)
                    .HasColumnName("inv_code");

                entity.Property(e => e.InvDesc1)
                    .HasMaxLength(255)
                    .HasColumnName("inv_desc_1");

                entity.Property(e => e.InvDesc2)
                    .HasMaxLength(255)
                    .HasColumnName("inv_desc_2");

                entity.Property(e => e.InvSerial)
                    .HasColumnType("int(11)")
                    .HasColumnName("inv_serial");

                entity.Property(e => e.InvTank)
                    .HasColumnType("int(11)")
                    .HasColumnName("inv_tank");

                entity.Property(e => e.InvTax)
                    .HasColumnType("int(11)")
                    .HasColumnName("inv_tax");

                entity.Property(e => e.MedCode)
                    .HasMaxLength(20)
                    .HasColumnName("med_code");

                entity.Property(e => e.Supply)
                    .HasColumnType("int(11)")
                    .HasColumnName("supply");

                entity.Property(e => e.VQuantity)
                    .HasColumnType("int(11)")
                    .HasColumnName("v_quantity");
            });

            modelBuilder.Entity<LinkPatientProducts>(entity =>
            {
                entity.HasKey(e => e.LinkId)
                    .HasName("PRIMARY");

                entity.ToTable("link_patient_products");

                entity.HasIndex(e => e.AssetTag, "assettags")
                    .HasAnnotation("MySql:FullTextIndex", true);

                entity.HasIndex(e => e.Delivered, "delivered");

                entity.HasIndex(e => e.OrderId, "orderID");

                entity.HasIndex(e => e.PatientId, "patientID");

                entity.HasIndex(e => e.ProductId, "productID");

                entity.Property(e => e.LinkId)
                    .HasColumnType("int(11)")
                    .HasColumnName("linkID");

                entity.Property(e => e.AssetStatus)
                    .HasColumnType("int(1)")
                    .HasColumnName("asset_status");

                entity.Property(e => e.AssetTag)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("asset_tag")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.AssetTagExchange)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("asset_tag_exchange");

                entity.Property(e => e.AssetTagPickup)
                    .HasMaxLength(100)
                    .HasColumnName("asset_tag_pickup");

                entity.Property(e => e.Delivered)
                    .HasColumnType("int(11)")
                    .HasColumnName("delivered");

                entity.Property(e => e.DeliveredTimestamp)
                    .HasColumnType("timestamp")
                    .HasColumnName("delivered_timestamp");

                entity.Property(e => e.DmeServiced)
                    .HasColumnType("int(1)")
                    .HasColumnName("dme_serviced");

                entity.Property(e => e.EmailCheckProcessed)
                    .HasColumnType("int(1)")
                    .HasColumnName("email_check_processed");

                entity.Property(e => e.ExchangeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("exchangeID");

                entity.Property(e => e.InventoryProcessed)
                    .HasColumnType("int(1)")
                    .HasColumnName("inventory_processed");

                entity.Property(e => e.IssueNotification)
                    .HasColumnType("int(1)")
                    .HasColumnName("issue_notification");

                entity.Property(e => e.LotNum)
                    .HasMaxLength(100)
                    .HasColumnName("lot_num");

                entity.Property(e => e.OffCapCost)
                    .HasPrecision(8, 2)
                    .HasColumnName("off_cap_cost");

                entity.Property(e => e.OffCapItem)
                    .HasColumnType("int(1)")
                    .HasColumnName("off_cap_item");

                entity.Property(e => e.OffCapUser)
                    .HasColumnType("int(11)")
                    .HasColumnName("off_cap_user");

                entity.Property(e => e.OrderId)
                    .HasColumnType("int(11)")
                    .HasColumnName("orderID");

                entity.Property(e => e.PatientId)
                    .HasColumnType("int(11)")
                    .HasColumnName("patientID");

                entity.Property(e => e.Pickedup)
                    .HasColumnType("int(1)")
                    .HasColumnName("pickedup");

                entity.Property(e => e.PickupTimestamp)
                    .HasColumnType("timestamp")
                    .HasColumnName("pickup_timestamp");

                entity.Property(e => e.ProductId)
                    .HasColumnType("int(11)")
                    .HasColumnName("productID");

                entity.Property(e => e.Quantity)
                    .HasColumnType("int(11)")
                    .HasColumnName("quantity");

                entity.Property(e => e.RespiteId)
                    .HasColumnType("int(11)")
                    .HasColumnName("respiteID");
            });

            modelBuilder.Entity<Logging>(entity =>
            {
                entity.ToTable("logging");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Created)
                    .HasColumnType("timestamp")
                    .HasColumnName("created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Instance)
                    .HasColumnType("int(11)")
                    .HasColumnName("instance");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("message");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("source");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("patient");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.AccountNumber, "accountNumberIndex");

                entity.HasIndex(e => e.AcctNumber, "acct_numberIndex");

                entity.HasIndex(e => e.Class, "classIndex");

                entity.HasIndex(e => e.Dc, "dc");

                entity.HasIndex(e => e.Division, "divisionIndex");

                entity.HasIndex(e => e.Firstname, "firstname");

                entity.HasIndex(e => e.HospiceId, "hospiceID");

                entity.HasIndex(e => e.Lastname, "lastnameIndex");

                entity.HasIndex(e => e.LocationId, "locationID");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.AccountNumber)
                    .HasMaxLength(40)
                    .HasColumnName("accountNumber");

                entity.Property(e => e.AcctNumber)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasColumnName("acct_number")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("address_1")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Address2)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("address_2")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.CTaxCode)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("c_tax_code")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .HasColumnName("city");

                entity.Property(e => e.Class)
                    .HasMaxLength(5)
                    .HasColumnName("class")
                    .IsFixedLength(true);

                entity.Property(e => e.CoTaxCode)
                    .HasMaxLength(40)
                    .HasColumnName("co_tax_code");

                entity.Property(e => e.Comment1)
                    .HasMaxLength(1000)
                    .HasColumnName("comment_1");

                entity.Property(e => e.Comment2)
                    .HasMaxLength(1000)
                    .HasColumnName("comment_2");

                entity.Property(e => e.Comment3)
                    .HasMaxLength(1000)
                    .HasColumnName("comment_3");

                entity.Property(e => e.Comment4)
                    .HasMaxLength(1000)
                    .HasColumnName("comment_4");

                entity.Property(e => e.Cpap)
                    .HasMaxLength(100)
                    .HasColumnName("cpap");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date")
                    .HasColumnName("create_date");

                entity.Property(e => e.CustomerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("customerID");

                entity.Property(e => e.Dc)
                    .HasColumnType("int(1)")
                    .HasColumnName("dc");

                entity.Property(e => e.DcDate)
                    .HasColumnType("date")
                    .HasColumnName("dc_date");

                entity.Property(e => e.DefaultDme)
                    .HasColumnType("int(1)")
                    .HasColumnName("default_dme");

                entity.Property(e => e.Diagnosis)
                    .HasMaxLength(100)
                    .HasColumnName("diagnosis");

                entity.Property(e => e.Division)
                    .HasMaxLength(5)
                    .HasColumnName("division")
                    .IsFixedLength(true);

                entity.Property(e => e.DmeServiced)
                    .HasColumnType("int(1)")
                    .HasColumnName("dme_serviced");

                entity.Property(e => e.DoNotCall)
                    .HasColumnType("int(11)")
                    .HasColumnName("do_not_call");

                entity.Property(e => e.DobDay)
                    .HasMaxLength(10)
                    .HasColumnName("dob_day");

                entity.Property(e => e.DobMonth)
                    .HasMaxLength(10)
                    .HasColumnName("dob_month");

                entity.Property(e => e.DobYear)
                    .HasMaxLength(10)
                    .HasColumnName("dob_year");

                entity.Property(e => e.Epap)
                    .HasMaxLength(100)
                    .HasColumnName("epap");

                entity.Property(e => e.FacilityId)
                    .HasColumnType("int(10)")
                    .HasColumnName("facilityID");

                entity.Property(e => e.FacilityName)
                    .HasMaxLength(254)
                    .HasColumnName("facility_name");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(40)
                    .HasColumnName("firstname");

                entity.Property(e => e.HeightFeet)
                    .HasColumnType("int(3)")
                    .HasColumnName("height_feet");

                entity.Property(e => e.HeightInches)
                    .HasColumnType("int(3)")
                    .HasColumnName("height_inches");

                entity.Property(e => e.HospiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("hospiceID");

                entity.Property(e => e.Inactive)
                    .HasColumnType("int(11)")
                    .HasColumnName("inactive");

                entity.Property(e => e.Indigent)
                    .HasColumnType("int(1)")
                    .HasColumnName("indigent");

                entity.Property(e => e.Ipap)
                    .HasMaxLength(100)
                    .HasColumnName("ipap");

                entity.Property(e => e.IsPediatric).HasColumnName("is_pediatric");

                entity.Property(e => e.LastChanged)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("last_changed")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(40)
                    .HasColumnName("lastname");

                entity.Property(e => e.LocationId)
                    .HasColumnType("int(11)")
                    .HasColumnName("locationID");

                entity.Property(e => e.LocationType)
                    .HasMaxLength(50)
                    .HasColumnName("location_type");

                entity.Property(e => e.MInitial)
                    .HasMaxLength(1)
                    .HasColumnName("m_initial")
                    .IsFixedLength(true);

                entity.Property(e => e.MapId)
                    .HasMaxLength(40)
                    .HasColumnName("map_id");

                entity.Property(e => e.Nurse)
                    .HasMaxLength(80)
                    .HasColumnName("nurse");

                entity.Property(e => e.NursingHome)
                    .HasMaxLength(100)
                    .HasColumnName("nursing_home");

                entity.Property(e => e.O2Order)
                    .HasMaxLength(40)
                    .HasColumnName("o2_order");

                entity.Property(e => e.OtherContact)
                    .HasMaxLength(250)
                    .HasColumnName("other_contact");

                entity.Property(e => e.PatientHeight)
                    .HasColumnType("int(5)")
                    .HasColumnName("patient_height");

                entity.Property(e => e.PatientWeight)
                    .HasColumnType("int(5)")
                    .HasColumnName("patient_weight");

                entity.Property(e => e.Phone1)
                    .HasMaxLength(12)
                    .HasColumnName("phone_1");

                entity.Property(e => e.Phone2)
                    .HasMaxLength(12)
                    .HasColumnName("phone_2");

                entity.Property(e => e.PreviousDcDate)
                    .HasColumnType("date")
                    .HasColumnName("previous_dc_date");

                entity.Property(e => e.STaxCode)
                    .HasMaxLength(40)
                    .HasColumnName("s_tax_code");

                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .HasColumnName("state");

                entity.Property(e => e.Taxable)
                    .HasColumnType("int(11)")
                    .HasColumnName("taxable");

                entity.Property(e => e.Team)
                    .HasMaxLength(80)
                    .HasColumnName("team");

                entity.Property(e => e.Zip)
                    .HasMaxLength(10)
                    .HasColumnName("zip");
            });

            modelBuilder.Entity<PatientHms>(entity =>
            {
                entity.ToTable("patient_hms");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.AccountNumber, "accountNumberIndex");

                entity.HasIndex(e => e.AcctNumber, "acct_numberIndex");

                entity.HasIndex(e => e.Class, "classIndex");

                entity.HasIndex(e => e.CustomerId, "customerID");

                entity.HasIndex(e => e.Dc, "dc");

                entity.HasIndex(e => e.DcDate, "dc_date");

                entity.HasIndex(e => e.Division, "divisionIndex");

                entity.HasIndex(e => e.DefaultDme, "dme");

                entity.HasIndex(e => e.DmeServiced, "dme_serviced");

                entity.HasIndex(e => e.Firstname, "firstname")
                    .HasAnnotation("MySql:FullTextIndex", true);

                entity.HasIndex(e => e.Lastname, "ft_lastname")
                    .HasAnnotation("MySql:FullTextIndex", true);

                entity.HasIndex(e => e.HospiceId, "hospiceID");

                entity.HasIndex(e => e.LocationId, "locationID");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.AccountNumber)
                    .HasMaxLength(40)
                    .HasColumnName("accountNumber");

                entity.Property(e => e.AcctNumber)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasColumnName("acct_number")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("address_1")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Address2)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("address_2")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.BipapRate)
                    .HasColumnType("int(11)")
                    .HasColumnName("bipap_rate");

                entity.Property(e => e.CTaxCode)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("c_tax_code")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .HasColumnName("city");

                entity.Property(e => e.Class)
                    .HasMaxLength(5)
                    .HasColumnName("class")
                    .IsFixedLength(true);

                entity.Property(e => e.CoTaxCode)
                    .HasMaxLength(40)
                    .HasColumnName("co_tax_code");

                entity.Property(e => e.Comment1)
                    .HasMaxLength(1000)
                    .HasColumnName("comment_1");

                entity.Property(e => e.Comment2)
                    .HasMaxLength(1000)
                    .HasColumnName("comment_2");

                entity.Property(e => e.Comment3)
                    .HasMaxLength(1000)
                    .HasColumnName("comment_3");

                entity.Property(e => e.Comment4)
                    .HasMaxLength(1000)
                    .HasColumnName("comment_4");

                entity.Property(e => e.Covid19)
                    .HasColumnName("covid_19")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CovidDate)
                    .HasMaxLength(10)
                    .HasColumnName("covid_date");

                entity.Property(e => e.CovidSession)
                    .HasMaxLength(20)
                    .HasColumnName("covid_session");

                entity.Property(e => e.CovidUser)
                    .HasColumnType("int(11)")
                    .HasColumnName("covid_user");

                entity.Property(e => e.Cpap)
                    .HasMaxLength(100)
                    .HasColumnName("cpap");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date")
                    .HasColumnName("create_date");

                entity.Property(e => e.CustomerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("customerID");

                entity.Property(e => e.Dc)
                    .HasColumnType("int(1)")
                    .HasColumnName("dc");

                entity.Property(e => e.DcDate)
                    .HasColumnType("date")
                    .HasColumnName("dc_date");

                entity.Property(e => e.DefaultDme)
                    .HasColumnType("int(11)")
                    .HasColumnName("default_dme");

                entity.Property(e => e.Devnotes)
                    .HasColumnType("text")
                    .HasColumnName("devnotes");

                entity.Property(e => e.Diagnosis)
                    .HasMaxLength(100)
                    .HasColumnName("diagnosis");

                entity.Property(e => e.Division)
                    .HasMaxLength(5)
                    .HasColumnName("division")
                    .IsFixedLength(true);

                entity.Property(e => e.DmeServiced)
                    .HasColumnType("int(1)")
                    .HasColumnName("dme_serviced");

                entity.Property(e => e.DoNotCall)
                    .HasColumnType("int(1)")
                    .HasColumnName("do_not_call");

                entity.Property(e => e.DobDay)
                    .HasMaxLength(10)
                    .HasColumnName("dob_day");

                entity.Property(e => e.DobMonth)
                    .HasMaxLength(10)
                    .HasColumnName("dob_month");

                entity.Property(e => e.DobYear)
                    .HasMaxLength(10)
                    .HasColumnName("dob_year");

                entity.Property(e => e.EmergencyOptout)
                    .HasColumnType("int(1)")
                    .HasColumnName("emergency_optout");

                entity.Property(e => e.Epap)
                    .HasMaxLength(100)
                    .HasColumnName("epap");

                entity.Property(e => e.FacilityId)
                    .HasColumnType("int(10)")
                    .HasColumnName("facilityID");

                entity.Property(e => e.FacilityName)
                    .HasMaxLength(254)
                    .HasColumnName("facility_name");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(40)
                    .HasColumnName("firstname");

                entity.Property(e => e.GeoCoded)
                    .HasColumnType("int(1)")
                    .HasColumnName("geo_coded");

                entity.Property(e => e.HeightFeet)
                    .HasColumnType("int(3)")
                    .HasColumnName("height_feet");

                entity.Property(e => e.HeightInches)
                    .HasColumnType("int(3)")
                    .HasColumnName("height_inches");

                entity.Property(e => e.HospiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("hospiceID");

                entity.Property(e => e.Inactive)
                    .HasColumnType("int(11)")
                    .HasColumnName("inactive");

                entity.Property(e => e.Indigent)
                    .HasColumnType("int(1)")
                    .HasColumnName("indigent");

                entity.Property(e => e.Ipap)
                    .HasMaxLength(100)
                    .HasColumnName("ipap");

                entity.Property(e => e.IsPediatric).HasColumnName("is_pediatric");

                entity.Property(e => e.LastChanged)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("last_changed")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(40)
                    .HasColumnName("lastname");

                entity.Property(e => e.LocationId)
                    .HasColumnType("int(11)")
                    .HasColumnName("locationID");

                entity.Property(e => e.LocationType)
                    .HasMaxLength(50)
                    .HasColumnName("location_type");

                entity.Property(e => e.MInitial)
                    .HasMaxLength(1)
                    .HasColumnName("m_initial")
                    .IsFixedLength(true);

                entity.Property(e => e.MapId)
                    .HasMaxLength(40)
                    .HasColumnName("map_id");

                entity.Property(e => e.Nurse)
                    .HasMaxLength(80)
                    .HasColumnName("nurse");

                entity.Property(e => e.NursingHome)
                    .HasMaxLength(100)
                    .HasColumnName("nursing_home");

                entity.Property(e => e.O2Order)
                    .HasMaxLength(40)
                    .HasColumnName("o2_order");

                entity.Property(e => e.OtherContact)
                    .HasMaxLength(250)
                    .HasColumnName("other_contact");

                entity.Property(e => e.PassagesPatientId)
                    .HasColumnType("int(8)")
                    .HasColumnName("passages_patientID");

                entity.Property(e => e.PatientHeight)
                    .HasColumnType("int(5)")
                    .HasColumnName("patient_height");

                entity.Property(e => e.PatientWeight)
                    .HasColumnType("int(5)")
                    .HasColumnName("patient_weight");

                entity.Property(e => e.Phone1)
                    .HasMaxLength(25)
                    .HasColumnName("phone_1");

                entity.Property(e => e.Phone2)
                    .HasMaxLength(25)
                    .HasColumnName("phone_2");

                entity.Property(e => e.PreviousDcDate)
                    .HasColumnType("date")
                    .HasColumnName("previous_dc_date");

                entity.Property(e => e.STaxCode)
                    .HasMaxLength(40)
                    .HasColumnName("s_tax_code");

                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .HasColumnName("state");

                entity.Property(e => e.Taxable)
                    .HasColumnType("int(11)")
                    .HasColumnName("taxable");

                entity.Property(e => e.Team)
                    .HasMaxLength(80)
                    .HasColumnName("team");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("userID");

                entity.Property(e => e.Zip)
                    .HasMaxLength(10)
                    .HasColumnName("zip");
            });

            modelBuilder.Entity<Patientlineitems>(entity =>
            {
                entity.ToTable("patientlineitems");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.CreateDate, "create_dateIndex");

                entity.HasIndex(e => e.DeliveryTs, "delivery_tsIndex");

                entity.HasIndex(e => e.InvId, "inv_IDIndex");

                entity.HasIndex(e => e.PatientId, "patientIDIndex");

                entity.HasIndex(e => e.PickupReqTs, "pickup_req_ts");

                entity.HasIndex(e => e.PickupTs, "pickup_tsIndex");

                entity.HasIndex(e => e.SnId, "sn_ID");

                entity.HasIndex(e => e.Status, "status");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date")
                    .HasColumnName("create_date");

                entity.Property(e => e.CreateTs)
                    .HasColumnType("datetime")
                    .HasColumnName("create_ts");

                entity.Property(e => e.DeliveryTs)
                    .HasColumnType("datetime")
                    .HasColumnName("delivery_ts");

                entity.Property(e => e.HistoryArchive)
                    .HasMaxLength(1)
                    .HasColumnName("historyArchive")
                    .IsFixedLength(true);

                entity.Property(e => e.Instructions)
                    .HasMaxLength(10000)
                    .HasColumnName("instructions");

                entity.Property(e => e.InvId)
                    .HasColumnType("int(11)")
                    .HasColumnName("inv_ID");

                entity.Property(e => e.LastChanged)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("last_changed")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.LotNumber)
                    .HasMaxLength(255)
                    .HasColumnName("lot_number");

                entity.Property(e => e.Lpm)
                    .HasColumnType("int(11)")
                    .HasColumnName("LPM");

                entity.Property(e => e.OrderedBy)
                    .HasColumnType("int(11)")
                    .HasColumnName("ordered_by");

                entity.Property(e => e.Other)
                    .HasMaxLength(255)
                    .HasColumnName("other");

                entity.Property(e => e.PatientId)
                    .HasColumnType("int(11)")
                    .HasColumnName("patientID");

                entity.Property(e => e.PatientOrdersId)
                    .HasColumnType("int(11)")
                    .HasColumnName("patientOrdersID");

                entity.Property(e => e.PickupCode)
                    .HasMaxLength(1)
                    .HasColumnName("pickup_code")
                    .IsFixedLength(true);

                entity.Property(e => e.PickupOrderedBy)
                    .HasColumnType("int(11)")
                    .HasColumnName("pickup_ordered_by");

                entity.Property(e => e.PickupReqTs)
                    .HasColumnType("datetime")
                    .HasColumnName("pickup_req_ts");

                entity.Property(e => e.PickupTs)
                    .HasColumnType("datetime")
                    .HasColumnName("pickup_ts");

                entity.Property(e => e.Quantity)
                    .HasPrecision(20, 2)
                    .HasColumnName("quantity");

                entity.Property(e => e.SnId)
                    .HasColumnType("int(11)")
                    .HasColumnName("sn_ID");

                entity.Property(e => e.Status)
                    .HasColumnType("int(11)")
                    .HasColumnName("status");
            });

            modelBuilder.Entity<TblBillingAdjReview>(entity =>
            {
                entity.HasKey(e => e.ReviewId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_billing_adj_review");

                entity.UseCollation("utf8_unicode_ci");

                entity.HasIndex(e => e.CustomerId, "customer_index");

                entity.HasIndex(e => e.HospiceId, "hospice_index");

                entity.HasIndex(e => e.ReviewId, "review_index");

                entity.HasIndex(e => e.UserId, "user_index");

                entity.Property(e => e.ReviewId)
                    .HasColumnType("int(11)")
                    .HasColumnName("reviewID");

                entity.Property(e => e.CustomerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("customerID");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("end_date");

                entity.Property(e => e.HospiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("hospiceID");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("start_date");

                entity.Property(e => e.Timestamp)
                    .HasColumnType("timestamp")
                    .HasColumnName("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(1)")
                    .HasColumnName("userID");
            });

            modelBuilder.Entity<TblBillingInvoiceCombine>(entity =>
            {
                entity.HasKey(e => e.CombineId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_billing_invoice_combine");

                entity.UseCollation("utf8_unicode_ci");

                entity.HasIndex(e => e.CombineId, "combine_index");

                entity.HasIndex(e => e.CustomerId, "customer_index");

                entity.HasIndex(e => e.HospiceId, "hospice_index");

                entity.HasIndex(e => e.UserId, "user_index");

                entity.Property(e => e.CombineId)
                    .HasColumnType("int(11)")
                    .HasColumnName("combineID");

                entity.Property(e => e.CustomerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("customerID");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("end_date");

                entity.Property(e => e.HospiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("hospiceID");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("start_date");

                entity.Property(e => e.Timestamp)
                    .HasColumnType("timestamp")
                    .HasColumnName("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(1)")
                    .HasColumnName("userID");
            });

            modelBuilder.Entity<TblBillingProcess>(entity =>
            {
                entity.HasKey(e => e.ProcessId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_billing_process");

                entity.UseCollation("utf8_unicode_ci");

                entity.HasIndex(e => e.CustomerId, "customer_index");

                entity.HasIndex(e => e.LimitHospiceId, "limit_hospice_index");

                entity.HasIndex(e => e.ProcessComplete, "process_complete_index");

                entity.HasIndex(e => e.ProcessId, "process_index");

                entity.HasIndex(e => e.UserId, "user_index");

                entity.Property(e => e.ProcessId)
                    .HasColumnType("int(11)")
                    .HasColumnName("processID");

                entity.Property(e => e.CustomerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("customerID");

                entity.Property(e => e.EmailSent)
                    .HasColumnType("timestamp")
                    .HasColumnName("email_sent");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("end_date");

                entity.Property(e => e.LimitHospiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("limit_hospiceID");

                entity.Property(e => e.NoBilling)
                    .HasColumnType("int(1)")
                    .HasColumnName("no_billing");

                entity.Property(e => e.ProcessComplete)
                    .HasColumnType("int(1)")
                    .HasColumnName("process_complete");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("start_date");

                entity.Property(e => e.Timestamp)
                    .HasColumnType("timestamp")
                    .HasColumnName("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(1)")
                    .HasColumnName("userID");
            });

            modelBuilder.Entity<TblContractInventory>(entity =>
            {
                entity.HasKey(e => e.InvctrId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_contract_inventory");

                entity.UseCollation("utf8_unicode_ci");

                entity.Property(e => e.InvctrId)
                    .HasColumnType("int(11)")
                    .HasColumnName("invctrID");

                entity.Property(e => e.ContractId)
                    .HasColumnType("int(11)")
                    .HasColumnName("contractID");

                entity.Property(e => e.HzOnly)
                    .HasColumnType("int(1)")
                    .HasColumnName("hz_only");

                entity.Property(e => e.InventoryId)
                    .HasColumnType("int(11)")
                    .HasColumnName("inventoryID");

                entity.Property(e => e.NoApprovalRequired)
                    .HasColumnType("int(1)")
                    .HasColumnName("no_approval_required");

                entity.Property(e => e.NumIncluded)
                    .HasColumnType("int(1)")
                    .HasColumnName("num_included");

                entity.Property(e => e.OrderScreen)
                    .HasColumnType("int(1)")
                    .HasColumnName("order_screen");

                entity.Property(e => e.Perdiem)
                    .HasColumnType("int(1)")
                    .HasColumnName("perdiem");

                entity.Property(e => e.RentalPrice)
                    .HasPrecision(8, 2)
                    .HasColumnName("rental_price");

                entity.Property(e => e.SalePrice)
                    .HasPrecision(8, 2)
                    .HasColumnName("sale_price");

                entity.Property(e => e.UpdateTimestamp)
                    .HasColumnType("timestamp")
                    .HasColumnName("update_timestamp");

                entity.Property(e => e.UpdateUserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("update_userID");
            });

            modelBuilder.Entity<TblContracts>(entity =>
            {
                entity.HasKey(e => e.ContractId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_contracts");

                entity.UseCollation("utf8_unicode_ci");

                entity.Property(e => e.ContractId)
                    .HasColumnType("int(11)")
                    .HasColumnName("contractID");

                entity.Property(e => e.Bill2ndConc)
                    .HasColumnType("int(1)")
                    .HasColumnName("bill_2nd_conc");

                entity.Property(e => e.CombineTanks)
                    .HasColumnType("int(1)")
                    .HasColumnName("combine_tanks");

                entity.Property(e => e.Comments)
                    .HasColumnType("text")
                    .HasColumnName("comments");

                entity.Property(e => e.ContractName)
                    .HasMaxLength(254)
                    .HasColumnName("contract_name");

                entity.Property(e => e.ContractNumber)
                    .HasMaxLength(254)
                    .HasColumnName("contract_number");

                entity.Property(e => e.CustomerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("customerID");

                entity.Property(e => e.DeliveryCharge)
                    .HasColumnType("int(1)")
                    .HasColumnName("delivery_charge");

                entity.Property(e => e.DetailBilling)
                    .HasColumnType("int(1)")
                    .HasColumnName("detail_billing");

                entity.Property(e => e.DmeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("dmeID");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("end_date");

                entity.Property(e => e.Filename)
                    .HasMaxLength(254)
                    .HasColumnName("filename");

                entity.Property(e => e.HospiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("hospiceID");

                entity.Property(e => e.MaxMileage)
                    .HasColumnType("int(5)")
                    .HasColumnName("max_mileage");

                entity.Property(e => e.MileageItem)
                    .HasMaxLength(50)
                    .HasColumnName("mileage_item");

                entity.Property(e => e.OverageFee1)
                    .HasPrecision(8, 2)
                    .HasColumnName("overage_fee_1");

                entity.Property(e => e.OverageFee2)
                    .HasPrecision(8, 2)
                    .HasColumnName("overage_fee_2");

                entity.Property(e => e.OverageFee3)
                    .HasPrecision(8, 2)
                    .HasColumnName("overage_fee_3");

                entity.Property(e => e.OverageFee4)
                    .HasPrecision(8, 2)
                    .HasColumnName("overage_fee_4");

                entity.Property(e => e.OverageFee5)
                    .HasPrecision(8, 2)
                    .HasColumnName("overage_fee_5");

                entity.Property(e => e.OverageFee6)
                    .HasPrecision(8, 2)
                    .HasColumnName("overage_fee_6");

                entity.Property(e => e.OverageFee7)
                    .HasPrecision(8, 2)
                    .HasColumnName("overage_fee_7");

                entity.Property(e => e.OverageFee8)
                    .HasPrecision(8, 2)
                    .HasColumnName("overage_fee_8");

                entity.Property(e => e.OverageFee9)
                    .HasPrecision(8, 2)
                    .HasColumnName("overage_fee_9");

                entity.Property(e => e.PerDiemRate)
                    .HasPrecision(8, 2)
                    .HasColumnName("per_diem_rate");

                entity.Property(e => e.SalesTaxRate)
                    .HasPrecision(11, 5)
                    .HasColumnName("sales_tax_rate");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("start_date");

                entity.Property(e => e.UpdateTimestamp)
                    .HasColumnType("timestamp")
                    .HasColumnName("update_timestamp");

                entity.Property(e => e.UpdateUserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("update_userID");

                entity.Property(e => e.UtilizationItem1)
                    .HasMaxLength(150)
                    .HasColumnName("utilization_item_1");

                entity.Property(e => e.UtilizationItem2)
                    .HasMaxLength(150)
                    .HasColumnName("utilization_item_2");

                entity.Property(e => e.UtilizationItem3)
                    .HasMaxLength(150)
                    .HasColumnName("utilization_item_3");

                entity.Property(e => e.UtilizationItem4)
                    .HasMaxLength(150)
                    .HasColumnName("utilization_item_4");

                entity.Property(e => e.UtilizationItem5)
                    .HasMaxLength(150)
                    .HasColumnName("utilization_item_5");

                entity.Property(e => e.UtilizationItem6)
                    .HasMaxLength(150)
                    .HasColumnName("utilization_item_6");

                entity.Property(e => e.UtilizationItem7)
                    .HasMaxLength(150)
                    .HasColumnName("utilization_item_7");

                entity.Property(e => e.UtilizationItem8)
                    .HasMaxLength(150)
                    .HasColumnName("utilization_item_8");

                entity.Property(e => e.UtilizationItem9)
                    .HasMaxLength(150)
                    .HasColumnName("utilization_item_9");

                entity.Property(e => e.UtilizationPctg1)
                    .HasPrecision(5, 3)
                    .HasColumnName("utilization_pctg_1");

                entity.Property(e => e.UtilizationPctg2)
                    .HasPrecision(5, 3)
                    .HasColumnName("utilization_pctg_2");

                entity.Property(e => e.UtilizationPctg3)
                    .HasPrecision(5, 3)
                    .HasColumnName("utilization_pctg_3");

                entity.Property(e => e.UtilizationPctg4)
                    .HasPrecision(5, 3)
                    .HasColumnName("utilization_pctg_4");

                entity.Property(e => e.UtilizationPctg5)
                    .HasPrecision(5, 3)
                    .HasColumnName("utilization_pctg_5");

                entity.Property(e => e.UtilizationPctg6)
                    .HasPrecision(5, 3)
                    .HasColumnName("utilization_pctg_6");

                entity.Property(e => e.UtilizationPctg7)
                    .HasPrecision(5, 3)
                    .HasColumnName("utilization_pctg_7");

                entity.Property(e => e.UtilizationPctg8)
                    .HasPrecision(5, 3)
                    .HasColumnName("utilization_pctg_8");

                entity.Property(e => e.UtilizationPctg9)
                    .HasPrecision(5, 3)
                    .HasColumnName("utilization_pctg_9");

                entity.Property(e => e.UtilizationQty1)
                    .HasColumnType("int(5)")
                    .HasColumnName("utilization_qty_1");

                entity.Property(e => e.UtilizationQty2)
                    .HasColumnType("int(5)")
                    .HasColumnName("utilization_qty_2");

                entity.Property(e => e.UtilizationQty3)
                    .HasColumnType("int(5)")
                    .HasColumnName("utilization_qty_3");

                entity.Property(e => e.UtilizationQty4)
                    .HasColumnType("int(5)")
                    .HasColumnName("utilization_qty_4");

                entity.Property(e => e.UtilizationQty5)
                    .HasColumnType("int(5)")
                    .HasColumnName("utilization_qty_5");

                entity.Property(e => e.UtilizationQty6)
                    .HasColumnType("int(5)")
                    .HasColumnName("utilization_qty_6");

                entity.Property(e => e.UtilizationQty7)
                    .HasColumnType("int(5)")
                    .HasColumnName("utilization_qty_7");

                entity.Property(e => e.UtilizationQty8)
                    .HasColumnType("int(5)")
                    .HasColumnName("utilization_qty_8");

                entity.Property(e => e.UtilizationQty9)
                    .HasColumnType("int(5)")
                    .HasColumnName("utilization_qty_9");
            });

            modelBuilder.Entity<TblCustomers>(entity =>
            {
                entity.HasKey(e => e.CustomerId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_customers");

                entity.UseCollation("utf8_unicode_ci");

                entity.Property(e => e.CustomerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("customerID");

                entity.Property(e => e.CompanyAddress)
                    .HasMaxLength(254)
                    .HasColumnName("company_address");

                entity.Property(e => e.CompanyCity)
                    .HasMaxLength(254)
                    .HasColumnName("company_city");

                entity.Property(e => e.CompanyInactive)
                    .HasColumnType("int(1)")
                    .HasColumnName("company_inactive");

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(254)
                    .HasColumnName("company_name");

                entity.Property(e => e.CompanyReportingNational)
                    .HasColumnType("int(1)")
                    .HasColumnName("company_reporting_national");

                entity.Property(e => e.CompanyReportingQuality)
                    .HasColumnType("int(1)")
                    .HasColumnName("company_reporting_quality");

                entity.Property(e => e.CompanyReportingRegions)
                    .HasColumnType("int(1)")
                    .HasColumnName("company_reporting_regions");

                entity.Property(e => e.CompanyState)
                    .HasMaxLength(254)
                    .HasColumnName("company_state");

                entity.Property(e => e.CompanyZip)
                    .HasMaxLength(254)
                    .HasColumnName("company_zip");

                entity.Property(e => e.UpdateTimestamp)
                    .HasColumnType("timestamp")
                    .HasColumnName("update_timestamp");

                entity.Property(e => e.UpdateUserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("update_userID");
            });

            modelBuilder.Entity<TblHospices>(entity =>
            {
                entity.HasKey(e => e.HospiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_hospices");

                entity.UseCollation("utf8_unicode_ci");

                entity.HasIndex(e => e.CustomerId, "tbl_hospices_customerid_idx");

                entity.Property(e => e.HospiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("hospiceID");

                entity.Property(e => e.AutoApproval)
                    .HasColumnType("int(3)")
                    .HasColumnName("auto_approval");

                entity.Property(e => e.BillingContact)
                    .HasMaxLength(254)
                    .HasColumnName("billing_contact");

                entity.Property(e => e.BillingContactPhone)
                    .HasMaxLength(254)
                    .HasColumnName("billing_contact_phone");

                entity.Property(e => e.CallInDays)
                    .HasColumnType("int(1)")
                    .HasColumnName("call_in_days");

                entity.Property(e => e.CostCenter)
                    .HasMaxLength(254)
                    .HasColumnName("cost_center");

                entity.Property(e => e.CpdBudget)
                    .HasPrecision(5, 2)
                    .HasColumnName("cpd_budget");

                entity.Property(e => e.CreditHold)
                    .HasColumnType("int(1)")
                    .HasColumnName("credit_hold");

                entity.Property(e => e.CustomerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("customerID");

                entity.Property(e => e.DetailBilling)
                    .HasColumnType("int(1)")
                    .HasColumnName("detail_billing");

                entity.Property(e => e.DmeId)
                    .HasMaxLength(100)
                    .HasColumnName("dmeID");

                entity.Property(e => e.EmailType)
                    .HasColumnType("int(1)")
                    .HasColumnName("email_type");

                entity.Property(e => e.HospiceAddress)
                    .HasMaxLength(254)
                    .HasColumnName("hospice_address");

                entity.Property(e => e.HospiceBillingId)
                    .HasMaxLength(254)
                    .HasColumnName("hospice_billing_ID");

                entity.Property(e => e.HospiceCity)
                    .HasMaxLength(254)
                    .HasColumnName("hospice_city");

                entity.Property(e => e.HospiceInactive)
                    .HasColumnType("int(1)")
                    .HasColumnName("hospice_inactive");

                entity.Property(e => e.HospiceName)
                    .HasMaxLength(254)
                    .HasColumnName("hospice_name");

                entity.Property(e => e.HospiceNotifications)
                    .HasColumnType("text")
                    .HasColumnName("hospice_notifications");

                entity.Property(e => e.HospicePhone)
                    .HasMaxLength(254)
                    .HasColumnName("hospice_phone");

                entity.Property(e => e.HospiceShortName)
                    .HasMaxLength(20)
                    .HasColumnName("hospice_short_name");

                entity.Property(e => e.HospiceState)
                    .HasMaxLength(2)
                    .HasColumnName("hospice_state");

                entity.Property(e => e.HospiceZip)
                    .HasMaxLength(15)
                    .HasColumnName("hospice_zip");

                entity.Property(e => e.IncludeCoverSheet)
                    .HasColumnType("int(1)")
                    .HasColumnName("include_cover_sheet");

                entity.Property(e => e.InvoiceEmail)
                    .HasMaxLength(254)
                    .HasColumnName("invoice_email");

                entity.Property(e => e.LegacyMitsClass)
                    .HasMaxLength(10)
                    .HasColumnName("legacy_MITS_class");

                entity.Property(e => e.LegacyMitsDivision)
                    .HasMaxLength(5)
                    .HasColumnName("legacy_MITS_division");

                entity.Property(e => e.LocationCode)
                    .HasMaxLength(20)
                    .HasColumnName("location_code");

                entity.Property(e => e.NoPerdiemLines)
                    .HasColumnType("int(1)")
                    .HasColumnName("no_perdiem_lines");

                entity.Property(e => e.OffCapApproval)
                    .HasColumnType("int(1)")
                    .HasColumnName("off_cap_approval");

                entity.Property(e => e.PaymentPlan)
                    .HasColumnType("int(1)")
                    .HasColumnName("payment_plan");

                entity.Property(e => e.PaymentTerms)
                    .HasMaxLength(25)
                    .HasColumnName("payment_terms");

                entity.Property(e => e.PccrEmailCopy)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("pccr_email_copy");

                entity.Property(e => e.PdfInvoice)
                    .HasColumnType("int(1)")
                    .HasColumnName("pdf_invoice");

                entity.Property(e => e.PerDiemOverride)
                    .HasPrecision(6, 2)
                    .HasColumnName("per_diem_override");

                entity.Property(e => e.RegionTag)
                    .HasMaxLength(254)
                    .HasColumnName("region_tag");

                entity.Property(e => e.RevenueOnly)
                    .HasColumnType("int(1)")
                    .HasColumnName("revenue_only");

                entity.Property(e => e.SendExcel)
                    .HasColumnType("int(1)")
                    .HasColumnName("send_excel");

                entity.Property(e => e.TestHospice)
                    .HasColumnType("int(1)")
                    .HasColumnName("test_hospice");

                entity.Property(e => e.UpdateTimestamp)
                    .HasColumnType("timestamp")
                    .HasColumnName("update_timestamp");

                entity.Property(e => e.UpdateUserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("update_userID");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("userID");

                entity.Property(e => e.Version2)
                    .HasColumnType("int(1)")
                    .HasColumnName("version_2");
            });

            modelBuilder.Entity<TblInventory>(entity =>
            {
                entity.HasKey(e => e.InventoryId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_inventory");

                entity.UseCollation("utf8_unicode_ci");

                entity.Property(e => e.InventoryId)
                    .HasColumnType("int(11)")
                    .HasColumnName("inventoryID");

                entity.Property(e => e.Active)
                    .HasColumnType("int(1)")
                    .HasColumnName("active");

                entity.Property(e => e.AssetTagRequired)
                    .HasColumnType("int(1)")
                    .HasColumnName("asset_tag_required");

                entity.Property(e => e.BariItem)
                    .HasColumnType("int(1)")
                    .HasColumnName("bari_item");

                entity.Property(e => e.BipapSetting)
                    .HasColumnType("int(1)")
                    .HasColumnName("bipap_setting");

                entity.Property(e => e.CapexItem)
                    .HasColumnType("int(1)")
                    .HasColumnName("capex_item");

                entity.Property(e => e.CategoryId)
                    .HasColumnType("int(4)")
                    .HasColumnName("categoryID");

                entity.Property(e => e.ChooseQuantity)
                    .HasColumnType("int(1)")
                    .HasColumnName("choose_quantity");

                entity.Property(e => e.Concentrator)
                    .HasColumnType("int(1)")
                    .HasColumnName("concentrator");

                entity.Property(e => e.Cost)
                    .HasPrecision(8, 2)
                    .HasColumnName("cost");

                entity.Property(e => e.CpapSetting)
                    .HasColumnType("int(1)")
                    .HasColumnName("cpap_setting");

                entity.Property(e => e.DbmCode)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("dbm_code");

                entity.Property(e => e.DefaultRentPrice)
                    .HasPrecision(8, 2)
                    .HasColumnName("default_rent_price");

                entity.Property(e => e.DefaultSalePrice)
                    .HasPrecision(8, 2)
                    .HasColumnName("default_sale_price");

                entity.Property(e => e.Depreciation)
                    .HasColumnType("int(5)")
                    .HasColumnName("depreciation");

                entity.Property(e => e.Description)
                    .HasMaxLength(254)
                    .HasColumnName("description");

                entity.Property(e => e.Disposable)
                    .HasColumnType("int(1)")
                    .HasColumnName("disposable");

                entity.Property(e => e.Donated)
                    .HasColumnType("int(1)")
                    .HasColumnName("donated");

                entity.Property(e => e.GlId)
                    .HasColumnType("int(11)")
                    .HasColumnName("glID");

                entity.Property(e => e.IncludedInvCodes)
                    .HasColumnType("text")
                    .HasColumnName("included_inv_codes");

                entity.Property(e => e.InvCode)
                    .HasMaxLength(254)
                    .HasColumnName("inv_code");

                entity.Property(e => e.Inventoried)
                    .HasColumnType("int(1)")
                    .HasColumnName("inventoried");

                entity.Property(e => e.LastUpdate)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("last_update");

                entity.Property(e => e.LotNumRequired)
                    .HasColumnType("int(1)")
                    .HasColumnName("lot_num_required");

                entity.Property(e => e.MaintInterval)
                    .HasColumnType("int(11)")
                    .HasColumnName("maint_interval")
                    .HasComment("maximum maintenance interval for equipment that requires regular maintenance");

                entity.Property(e => e.OxygenSetting)
                    .HasColumnType("int(1)")
                    .HasColumnName("oxygen_setting");

                entity.Property(e => e.ProductDescription)
                    .HasColumnType("text")
                    .HasColumnName("product_description");

                entity.Property(e => e.ProductImage)
                    .HasMaxLength(255)
                    .HasColumnName("product_image");

                entity.Property(e => e.QuantityLimit)
                    .HasColumnType("int(2)")
                    .HasColumnName("quantity_limit");

                entity.Property(e => e.RegMaintRequired)
                    .HasColumnName("reg_maint_required")
                    .HasComment("flag for equipment that requires regular maintenance");

                entity.Property(e => e.SerialNumRequired)
                    .HasColumnType("int(1)")
                    .HasColumnName("serial_num_required");

                entity.Property(e => e.SubCategoryId)
                    .HasColumnType("int(11)")
                    .HasColumnName("sub_categoryID");

                entity.Property(e => e.TimeDelivery)
                    .HasColumnType("int(4)")
                    .HasColumnName("time_delivery");

                entity.Property(e => e.TimePickup)
                    .HasColumnType("int(4)")
                    .HasColumnName("time_pickup");
            });

            modelBuilder.Entity<TblInvoices>(entity =>
            {
                entity.HasKey(e => e.InvoiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_invoices");

                entity.UseCollation("utf8_unicode_ci");

                entity.Property(e => e.InvoiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("invoiceID");

                entity.Property(e => e.CombinedInv)
                    .HasMaxLength(250)
                    .HasColumnName("combined_inv");

                entity.Property(e => e.CustomerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("customerID");

                entity.Property(e => e.DatemonthPrefix)
                    .HasMaxLength(6)
                    .HasColumnName("datemonth_prefix");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("end_date");

                entity.Property(e => e.Filename)
                    .HasMaxLength(254)
                    .HasColumnName("filename");

                entity.Property(e => e.HospiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("hospiceID");

                entity.Property(e => e.NonPerDiem)
                    .HasPrecision(10, 2)
                    .HasColumnName("non_per_diem");

                entity.Property(e => e.PatientDays)
                    .HasColumnType("int(10)")
                    .HasColumnName("patient_days");

                entity.Property(e => e.PerDiem)
                    .HasPrecision(10, 2)
                    .HasColumnName("per_diem");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("start_date");

                entity.Property(e => e.Timestamp)
                    .HasColumnType("timestamp")
                    .HasColumnName("timestamp");

                entity.Property(e => e.Utilization)
                    .HasPrecision(10, 2)
                    .HasColumnName("utilization");
            });

            modelBuilder.Entity<TblLocations>(entity =>
            {
                entity.HasKey(e => e.LocationId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_locations");

                entity.UseCollation("utf8_unicode_ci");

                entity.Property(e => e.LocationId)
                    .HasColumnType("int(11)")
                    .HasColumnName("locationID");

                entity.Property(e => e.ExcludeReports)
                    .HasColumnType("int(1)")
                    .HasColumnName("exclude_reports");

                entity.Property(e => e.H2hRev)
                    .HasColumnType("int(1)")
                    .HasColumnName("H2H_rev");

                entity.Property(e => e.Inactive)
                    .HasColumnType("int(1)")
                    .HasColumnName("inactive");

                entity.Property(e => e.Invemail)
                    .HasColumnType("text")
                    .HasColumnName("invemail");

                entity.Property(e => e.LegacyMitsDivision)
                    .HasMaxLength(5)
                    .HasColumnName("legacy_MITS_division");

                entity.Property(e => e.LocationAddress)
                    .HasMaxLength(254)
                    .HasColumnName("location_address");

                entity.Property(e => e.LocationCity)
                    .HasMaxLength(100)
                    .HasColumnName("location_city");

                entity.Property(e => e.LocationCode)
                    .HasMaxLength(25)
                    .HasColumnName("location_code");

                entity.Property(e => e.LocationName)
                    .HasMaxLength(100)
                    .HasColumnName("location_name");

                entity.Property(e => e.LocationPhone)
                    .HasMaxLength(15)
                    .HasColumnName("location_phone");

                entity.Property(e => e.LocationState)
                    .HasMaxLength(2)
                    .HasColumnName("location_state");

                entity.Property(e => e.LocationZip)
                    .HasMaxLength(10)
                    .HasColumnName("location_zip");

                entity.Property(e => e.NoPi)
                    .HasColumnType("int(1)")
                    .HasColumnName("no_pi");

                entity.Property(e => e.NoShowRevReport)
                    .HasColumnType("int(1)")
                    .HasColumnName("no_show_rev_report");

                entity.Property(e => e.PccrAssignment)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("pccr_assignment");

                entity.Property(e => e.PccrEmailNotifications)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("pccr_email_notifications");

                entity.Property(e => e.Timezone)
                    .HasColumnType("int(11)")
                    .HasColumnName("timezone")
                    .HasDefaultValueSql("'-6'");

                entity.Property(e => e.Vehicle)
                    .HasColumnType("int(1)")
                    .HasColumnName("vehicle");

                entity.Property(e => e.VehicleLocation)
                    .HasColumnType("int(11)")
                    .HasColumnName("vehicle_location");
            });

            modelBuilder.Entity<TblMessages>(entity =>
            {
                entity.HasKey(e => e.MessageId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_messages");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.MessageId)
                    .HasColumnType("int(11)")
                    .HasColumnName("messageID");

                entity.Property(e => e.MessageBody)
                    .HasColumnType("text")
                    .HasColumnName("message_body");

                entity.Property(e => e.MessageEnd)
                    .HasColumnType("date")
                    .HasColumnName("message_end");

                entity.Property(e => e.MessageStatus)
                    .HasColumnType("int(1)")
                    .HasColumnName("message_status");

                entity.Property(e => e.MessageTitle)
                    .HasMaxLength(254)
                    .HasColumnName("message_title");
            });

            modelBuilder.Entity<TblOrders>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_orders");

                entity.HasIndex(e => e.AckEmailSent, "ack_email_sent");

                entity.HasIndex(e => e.AutoAckTimestamp, "auto_ack_timestamp");

                entity.HasIndex(e => e.AutoApprovalTimestamp, "auto_approval_timestamp");

                entity.HasIndex(e => e.CancelledTimestamp, "cancelled_timestamp");

                entity.HasIndex(e => e.DeliveredTimestamp, "delivered_timestamp");

                entity.HasIndex(e => e.DispatchTimestamp, "dispatch_timestamp");

                entity.HasIndex(e => e.DmeId, "dmeID");

                entity.HasIndex(e => e.DmeAcknowledgement, "dme_acknowledgement");

                entity.HasIndex(e => e.DriverId, "driverID");

                entity.HasIndex(e => e.HospiceId, "hospiceID");

                entity.HasIndex(e => e.MigrationOrder, "migration_order");

                entity.HasIndex(e => e.MoveId, "moveID");

                entity.HasIndex(e => e.OrderDate, "order_date");

                entity.HasIndex(e => e.PatientId, "patientID");

                entity.HasIndex(e => e.PickedupTimestamp, "pickedup_timestamp");

                entity.HasIndex(e => e.Status, "status");

                entity.HasIndex(e => e.Timestamp, "timestamp");

                entity.Property(e => e.OrderId)
                    .HasColumnType("int(11)")
                    .HasColumnName("orderID");

                entity.Property(e => e.AckComments)
                    .HasColumnType("text")
                    .HasColumnName("ack_comments");

                entity.Property(e => e.AckEmailSent)
                    .HasColumnType("int(1)")
                    .HasColumnName("ack_email_sent");

                entity.Property(e => e.AppComments)
                    .HasColumnType("text")
                    .HasColumnName("app_comments");

                entity.Property(e => e.ApprCustomer)
                    .HasColumnType("int(11)")
                    .HasColumnName("appr_customer");

                entity.Property(e => e.AppuserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("appuserID");

                entity.Property(e => e.AutoAckTimestamp)
                    .HasColumnType("timestamp")
                    .HasColumnName("auto_ack_timestamp");

                entity.Property(e => e.AutoApprovalTimestamp)
                    .HasColumnType("timestamp")
                    .HasColumnName("auto_approval_timestamp");

                entity.Property(e => e.BopOrder)
                    .HasColumnType("int(1)")
                    .HasColumnName("bop_order");

                entity.Property(e => e.BypassApproval)
                    .HasColumnType("int(1)")
                    .HasColumnName("bypass_approval");

                entity.Property(e => e.BypassApprovalNotes)
                    .HasColumnType("text")
                    .HasColumnName("bypass_approval_notes");

                entity.Property(e => e.CancelledTimestamp)
                    .HasColumnType("timestamp")
                    .HasColumnName("cancelled_timestamp");

                entity.Property(e => e.DelAddress)
                    .HasColumnType("int(1)")
                    .HasColumnName("del_address");

                entity.Property(e => e.DeliveredTimestamp)
                    .HasColumnType("timestamp")
                    .HasColumnName("delivered_timestamp");

                entity.Property(e => e.DeliveryNotes)
                    .HasColumnType("text")
                    .HasColumnName("delivery_notes");

                entity.Property(e => e.DeliveryTiming)
                    .HasColumnType("int(1)")
                    .HasColumnName("delivery_timing");

                entity.Property(e => e.DeliveryType)
                    .HasMaxLength(254)
                    .HasColumnName("delivery_type");

                entity.Property(e => e.DispatchTimestamp)
                    .HasColumnType("timestamp")
                    .HasColumnName("dispatch_timestamp");

                entity.Property(e => e.DmeAcknowledgement)
                    .HasColumnType("timestamp")
                    .HasColumnName("dme_acknowledgement");

                entity.Property(e => e.DmeId)
                    .HasColumnType("int(1)")
                    .HasColumnName("dmeID");

                entity.Property(e => e.DmeuserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("dmeuserID");

                entity.Property(e => e.DriverId)
                    .HasColumnType("int(11)")
                    .HasColumnName("driverID");

                entity.Property(e => e.Exchange)
                    .HasColumnType("int(1)")
                    .HasColumnName("exchange");

                entity.Property(e => e.ExchangeOrderId)
                    .HasColumnType("int(11)")
                    .HasColumnName("exchange_orderID");

                entity.Property(e => e.ExchangeReason)
                    .HasColumnType("text")
                    .HasColumnName("exchange_reason");

                entity.Property(e => e.FacilityName)
                    .HasMaxLength(254)
                    .HasColumnName("facility_name");

                entity.Property(e => e.FutureDate)
                    .HasColumnType("date")
                    .HasColumnName("future_date");

                entity.Property(e => e.FutureTimeRange)
                    .HasMaxLength(254)
                    .HasColumnName("future_time_range");

                entity.Property(e => e.HospDischarge)
                    .HasColumnType("int(1)")
                    .HasColumnName("hosp_discharge");

                entity.Property(e => e.HospDischargeTime)
                    .HasMaxLength(25)
                    .HasColumnName("hosp_discharge_time");

                entity.Property(e => e.HospiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("hospiceID");

                entity.Property(e => e.HsApproval1)
                    .HasColumnType("timestamp")
                    .HasColumnName("hs_approval");

                entity.Property(e => e.HsAutoApproval)
                    .HasColumnType("timestamp")
                    .HasColumnName("hs_auto_approval");

                entity.Property(e => e.Hsapproval)
                    .HasColumnType("int(1)")
                    .HasColumnName("hsapproval");

                entity.Property(e => e.Instructions)
                    .HasColumnType("text")
                    .HasColumnName("instructions");

                entity.Property(e => e.InvProcessed)
                    .HasColumnType("int(1)")
                    .HasColumnName("inv_processed");

                entity.Property(e => e.MailCustomer)
                    .HasColumnType("int(11)")
                    .HasColumnName("mail_customer");

                entity.Property(e => e.MigrationOrder)
                    .HasColumnType("int(1)")
                    .HasColumnName("migration_order");

                entity.Property(e => e.ModAutoApproval)
                    .HasColumnType("int(1)")
                    .HasColumnName("mod_auto_approval");

                entity.Property(e => e.MoveId)
                    .HasColumnType("int(11)")
                    .HasColumnName("moveID");

                entity.Property(e => e.MoveReason)
                    .HasColumnType("text")
                    .HasColumnName("move_reason");

                entity.Property(e => e.OffCapApproval)
                    .HasColumnType("int(1)")
                    .HasColumnName("off_cap_approval");

                entity.Property(e => e.OffCapApprovalOrgUserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("off_cap_approval_org_userID");

                entity.Property(e => e.OffCapApprovalUserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("off_cap_approval_userID");

                entity.Property(e => e.OffCapArray)
                    .HasColumnType("text")
                    .HasColumnName("off_cap_array");

                entity.Property(e => e.OffCapAutoApproval)
                    .HasColumnType("int(1)")
                    .HasColumnName("off_cap_auto_approval");

                entity.Property(e => e.OffCapOrder)
                    .HasColumnType("int(1)")
                    .HasColumnName("off_cap_order");

                entity.Property(e => e.OrderArray)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("order_array");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("date")
                    .HasColumnName("order_date");

                entity.Property(e => e.OrderSource)
                    .HasColumnType("int(1)")
                    .HasColumnName("order_source");

                entity.Property(e => e.OrderingNurse)
                    .HasColumnType("text")
                    .HasColumnName("ordering_nurse");

                entity.Property(e => e.OrderingNursePhone)
                    .HasMaxLength(25)
                    .HasColumnName("ordering_nurse_phone");

                entity.Property(e => e.OrgOrderArray)
                    .HasColumnType("text")
                    .HasColumnName("org_order_array");

                entity.Property(e => e.OrgOrderDate)
                    .HasColumnType("date")
                    .HasColumnName("org_order_date");

                entity.Property(e => e.PatientId)
                    .HasColumnType("int(11)")
                    .HasColumnName("patientID");

                entity.Property(e => e.PickedupTimestamp)
                    .HasColumnType("timestamp")
                    .HasColumnName("pickedup_timestamp");

                entity.Property(e => e.Pickup)
                    .HasColumnType("int(1)")
                    .HasColumnName("pickup");

                entity.Property(e => e.PickupReason)
                    .HasMaxLength(254)
                    .HasColumnName("pickup_reason");

                entity.Property(e => e.PickupType)
                    .HasColumnType("int(1)")
                    .HasColumnName("pickup_type");

                entity.Property(e => e.RekeyOrder)
                    .HasColumnType("int(1)")
                    .HasColumnName("rekey_order");

                entity.Property(e => e.Respite)
                    .HasColumnType("int(11)")
                    .HasColumnName("respite");

                entity.Property(e => e.Status)
                    .HasColumnType("int(1)")
                    .HasColumnName("status");

                entity.Property(e => e.Timestamp)
                    .HasColumnType("timestamp")
                    .HasColumnName("timestamp")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.TimingOverride)
                    .HasMaxLength(25)
                    .HasColumnName("timing_override");

                entity.Property(e => e.TimingOverrideNotes)
                    .HasColumnType("text")
                    .HasColumnName("timing_override_notes");

                entity.Property(e => e.TransitionOrder)
                    .HasColumnType("int(1)")
                    .HasColumnName("transition_order");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("userID");
            });

            modelBuilder.Entity<TblPatientBillingGaps>(entity =>
            {
                entity.HasKey(e => e.PatientbillinggapId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_patient_billing_gaps");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.PatientbillinggapId)
                    .HasColumnType("int(11)")
                    .HasColumnName("patientbillinggapID");

                entity.Property(e => e.BillingGapEndDate)
                    .HasColumnType("date")
                    .HasColumnName("billing_gap_end_date");

                entity.Property(e => e.BillingGapStartDate)
                    .HasColumnType("date")
                    .HasColumnName("billing_gap_start_date");

                entity.Property(e => e.BillingNote)
                    .HasColumnType("text")
                    .HasColumnName("billing_note");

                entity.Property(e => e.BillingSubmitted)
                    .HasColumnType("timestamp")
                    .HasColumnName("billing_submitted");

                entity.Property(e => e.HospiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("hospiceID");

                entity.Property(e => e.PatientId)
                    .HasColumnType("int(11)")
                    .HasColumnName("patientID");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("userID");
            });

            modelBuilder.Entity<TblUnadjustedForecast>(entity =>
            {
                entity.HasKey(e => e.AdjId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_unadjusted_forecast");

                entity.UseCollation("utf8_unicode_ci");

                entity.Property(e => e.AdjId)
                    .HasColumnType("int(11)")
                    .HasColumnName("adjID");

                entity.Property(e => e.CustomerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("customerID");

                entity.Property(e => e.HospiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("hospiceID");

                entity.Property(e => e.Inserted)
                    .HasColumnType("timestamp")
                    .HasColumnName("inserted");

                entity.Property(e => e.LocationId)
                    .HasColumnType("int(11)")
                    .HasColumnName("locationID");

                entity.Property(e => e.Month)
                    .HasColumnType("int(2)")
                    .HasColumnName("month");

                entity.Property(e => e.NoRevenue)
                    .HasColumnType("int(1)")
                    .HasColumnName("no_revenue");

                entity.Property(e => e.NonPerDiem)
                    .HasPrecision(10, 2)
                    .HasColumnName("non_per_diem");

                entity.Property(e => e.PatientDays)
                    .HasColumnType("int(10)")
                    .HasColumnName("patient_days");

                entity.Property(e => e.PatientId)
                    .HasColumnType("int(11)")
                    .HasColumnName("patientID");

                entity.Property(e => e.PerDiem)
                    .HasPrecision(10, 2)
                    .HasColumnName("per_diem");

                entity.Property(e => e.Utilization)
                    .HasPrecision(10, 2)
                    .HasColumnName("utilization");

                entity.Property(e => e.Year)
                    .HasColumnType("int(4)")
                    .HasColumnName("year");
            });

            modelBuilder.Entity<TblUsers>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_users");

                entity.UseCollation("utf8_unicode_ci");

                entity.HasIndex(e => e.HospiceId, "hospiceID");

                entity.HasIndex(e => e.Lastname, "lastname");

                entity.HasIndex(e => e.UserLevel, "level");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("userID");

                entity.Property(e => e.AlwaysEmail)
                    .HasColumnType("int(1)")
                    .HasColumnName("always_email");

                entity.Property(e => e.ApprovingManager)
                    .HasColumnType("int(1)")
                    .HasColumnName("approving_manager");

                entity.Property(e => e.Carrier)
                    .HasMaxLength(255)
                    .HasColumnName("carrier");

                entity.Property(e => e.CustomerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("customerID");

                entity.Property(e => e.DispatchAreas)
                    .HasMaxLength(50)
                    .HasColumnName("dispatch_areas");

                entity.Property(e => e.DmeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("dmeID");

                entity.Property(e => e.Email)
                    .HasMaxLength(254)
                    .HasColumnName("email");

                entity.Property(e => e.FacilityManager)
                    .HasColumnType("int(1)")
                    .HasColumnName("facility_manager");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(254)
                    .HasColumnName("firstname");

                entity.Property(e => e.GmtOffset)
                    .HasMaxLength(11)
                    .HasColumnName("gmt_offset");

                entity.Property(e => e.HospiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("hospiceID");

                entity.Property(e => e.Inactive)
                    .HasColumnType("int(1)")
                    .HasColumnName("inactive");

                entity.Property(e => e.InventoryAccess)
                    .HasColumnType("int(1)")
                    .HasColumnName("inventory_access");

                entity.Property(e => e.InventoryAreas)
                    .HasMaxLength(200)
                    .HasColumnName("inventory_areas");

                entity.Property(e => e.InvoicePreviewer)
                    .HasColumnType("int(1)")
                    .HasColumnName("invoice_previewer");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(254)
                    .HasColumnName("lastname");

                entity.Property(e => e.LegacyMitsClass)
                    .HasMaxLength(10)
                    .HasColumnName("legacy_MITS_class");

                entity.Property(e => e.LegacyMitsDivision)
                    .HasMaxLength(5)
                    .HasColumnName("legacy_MITS_division");

                entity.Property(e => e.LocationId)
                    .HasMaxLength(254)
                    .HasColumnName("locationID");

                entity.Property(e => e.MgrMultiSite)
                    .HasColumnType("int(1)")
                    .HasColumnName("mgr_multi_site");

                entity.Property(e => e.Mobile)
                    .HasMaxLength(254)
                    .HasColumnName("mobile");

                entity.Property(e => e.MultipleHospices)
                    .HasMaxLength(200)
                    .HasColumnName("multiple_hospices");

                entity.Property(e => e.NurseManager)
                    .HasColumnType("int(1)")
                    .HasColumnName("nurse_manager");

                entity.Property(e => e.OffCapApproval)
                    .HasColumnType("int(1)")
                    .HasColumnName("off_cap_approval");

                entity.Property(e => e.OffCapApprovalType)
                    .HasColumnType("int(1)")
                    .HasColumnName("off_cap_approval_type");

                entity.Property(e => e.Password)
                    .HasMaxLength(254)
                    .HasColumnName("password");

                entity.Property(e => e.PccrPermission)
                    .HasColumnType("int(1)")
                    .HasColumnName("pccr_permission")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Phone)
                    .HasMaxLength(254)
                    .HasColumnName("phone");

                entity.Property(e => e.Pwtoken)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("pwtoken");

                entity.Property(e => e.PwtokenExpired)
                    .HasColumnType("datetime")
                    .HasColumnName("pwtoken_expired");

                entity.Property(e => e.QuickFacilities)
                    .HasColumnType("text")
                    .HasColumnName("quick_facilities");

                entity.Property(e => e.QuickState)
                    .HasMaxLength(2)
                    .HasColumnName("quick_state");

                entity.Property(e => e.SuperLogin)
                    .HasColumnType("int(1)")
                    .HasColumnName("super_login");

                entity.Property(e => e.Temppass)
                    .HasMaxLength(25)
                    .HasColumnName("temppass");

                entity.Property(e => e.UserCreated)
                    .HasColumnType("timestamp")
                    .HasColumnName("user_created");

                entity.Property(e => e.UserLevel)
                    .HasColumnType("int(1)")
                    .HasColumnName("user_level");

                entity.Property(e => e.Username)
                    .HasMaxLength(254)
                    .HasColumnName("username");

                entity.Property(e => e.UsersManager)
                    .HasColumnType("int(1)")
                    .HasColumnName("users_manager");

                entity.Property(e => e.Version2)
                    .HasColumnType("int(1)")
                    .HasColumnName("version_2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
