using ArandaSoftLab.Infrastructure.Audit.Base;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ArandaSoftLab.Infrastructure.Audit
{
    public class ArandaSoftLabAuditLogContext : DbContextBaseAudit
    {

        public ArandaSoftLabAuditLogContext()
            : base("Name=ArandaSoftLabAuditLogContext")
        {
            
        }

        public virtual DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
