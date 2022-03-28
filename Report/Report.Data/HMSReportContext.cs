using HMSDigital.Report.Data.Models;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HMSDigital.Report.Data
{
    public partial class HMSReportContext : DbContext
    {
        private const string DB_SCHEMA = "report";

        public HMSReportContext()
        {
        }

        public HMSReportContext(DbContextOptions<HMSReportContext> options)
            : base(options)
        {
        }

        public virtual DbSet<OrdersMetric> PendingOrders { get; set; }

        public virtual DbSet<PatientsMetric> PatientsMetric { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DB_SCHEMA);

            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<OrdersMetric>().ToFunction("fnc_Orders").HasNoKey();

            modelBuilder.Entity<PatientsMetric>().HasNoKey();

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
