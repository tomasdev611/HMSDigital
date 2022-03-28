using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using HMSDigital.Patient.Data.Models;

#nullable disable

namespace HMSDigital.Patient.Data
{
    public partial class HMSPatientContext : DbContext
    {
        public HMSPatientContext()
        {
        }

        public HMSPatientContext(DbContextOptions<HMSPatientContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AddressType> AddressType { get; set; }
        public virtual DbSet<Addresses> Addresses { get; set; }
        public virtual DbSet<PatientAddress> PatientAddress { get; set; }
        public virtual DbSet<PatientDetails> PatientDetails { get; set; }
        public virtual DbSet<PatientMergeHistory> PatientMergeHistory { get; set; }
        public virtual DbSet<PatientNotes> PatientNotes { get; set; }
        public virtual DbSet<PatientStatusTypes> PatientStatusTypes { get; set; }
        public virtual DbSet<PhoneNumberTypes> PhoneNumberTypes { get; set; }
        public virtual DbSet<PhoneNumbers> PhoneNumbers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AddressType>(entity =>
            {
                entity.ToTable("AddressType", "patient");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Addresses>(entity =>
            {
                entity.ToTable("Addresses", "patient");

                entity.Property(e => e.AddressUuid).HasColumnName("AddressUUID");

                entity.Property(e => e.Latitude)
                    .HasColumnType("decimal(11, 8)")
                    .HasDefaultValueSql("((0.00000000))");

                entity.Property(e => e.Longitude)
                    .HasColumnType("decimal(11, 8)")
                    .HasDefaultValueSql("((0.00000000))");

                entity.Property(e => e.VerifiedBy).IsUnicode(false);
            });

            modelBuilder.Entity<PatientAddress>(entity =>
            {
                entity.ToTable("PatientAddress", "patient");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.PatientAddress)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_PatientAddressMapping_To_Address");

                entity.HasOne(d => d.AddressType)
                    .WithMany(p => p.PatientAddress)
                    .HasForeignKey(d => d.AddressTypeId)
                    .HasConstraintName("FK_Address_ToTable");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientAddress)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PatientAddressMapping_To_Patient");
            });

            modelBuilder.Entity<PatientDetails>(entity =>
            {
                entity.ToTable("PatientDetails", "patient");

                entity.Property(e => e.AdditionalField4).IsUnicode(false);

                entity.Property(e => e.AdditionalField5).HasColumnType("decimal(15, 4)");

                entity.Property(e => e.DataBridgeRunUuid).HasColumnName("DataBridgeRunUUID");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.LastOrderNumber).IsUnicode(false);

                entity.Property(e => e.PatientHeight).HasDefaultValueSql("((0.0000))");

                entity.Property(e => e.StatusChangedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.PatientDetailsStatus)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_PatientDetails_statusTypes");

                entity.HasOne(d => d.StatusReason)
                    .WithMany(p => p.PatientDetailsStatusReason)
                    .HasForeignKey(d => d.StatusReasonId)
                    .HasConstraintName("FK_PatientDetails_statusReasonTypes");
            });

            modelBuilder.Entity<PatientMergeHistory>(entity =>
            {
                entity.ToTable("PatientMergeHistory", "patient");
            });

            modelBuilder.Entity<PatientNotes>(entity =>
            {
                entity.ToTable("PatientNotes", "patient");

                entity.Property(e => e.Note).IsUnicode(false);

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientNotes)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PatientNotes_patientDetails");
            });

            modelBuilder.Entity<PatientStatusTypes>(entity =>
            {
                entity.ToTable("PatientStatusTypes", "patient");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Color)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<PhoneNumberTypes>(entity =>
            {
                entity.ToTable("PhoneNumberTypes", "patient");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PhoneNumbers>(entity =>
            {
                entity.ToTable("PhoneNumbers", "patient");

                entity.Property(e => e.ContactName).IsUnicode(false);

                entity.Property(e => e.IsPrimary).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.NumberType)
                    .WithMany(p => p.PhoneNumbers)
                    .HasForeignKey(d => d.NumberTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PhoneNumbers_PhoneNumberType");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PhoneNumbers)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_PhoneNumbers_Patients");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
