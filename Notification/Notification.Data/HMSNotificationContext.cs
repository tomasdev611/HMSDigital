using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using HMSDigital.Notification.Data.Models;

#nullable disable

namespace HMSDigital.Notification.Data
{
    public partial class HMSNotificationContext : DbContext
    {
        public HMSNotificationContext()
        {
        }

        public HMSNotificationContext(DbContextOptions<HMSNotificationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PushNotificationMetadata> PushNotificationMetadata { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<PushNotificationMetadata>(entity =>
            {
                entity.ToTable("PushNotificationMetadata", "notification");

                entity.Property(e => e.DeviceId)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Platform)
                    .IsRequired()
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
