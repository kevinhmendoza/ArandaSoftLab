using System;
using System.ComponentModel.DataAnnotations;

namespace ArandaSoftLab.Infrastructure.Audit
{
    public class AuditLog
    {
        [Key]
        public Guid AuditLogID { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserID { get; set; }

        [Required]
        public DateTime EventDateUTC { get; set; }

        [Required]
        public DateTime EventDateLocalTime { get; set; }

        [Required]
        [MaxLength(20)]
        public string EventType { get; set; }

        [Required]
        [MaxLength(100)]
        public string TableName { get; set; }

        [Required]
        [MaxLength(100)]
        public string RecordID { get; set; }

        [Required]
        [MaxLength(100)]
        public string ColumnName { get; set; }

        public string OriginalValue { get; set; }

        public string NewValue { get; set; }

        [Required]
        [MaxLength(100)]
        public string Module { get; set; }

        [Required]
        [MaxLength(100)]
        public string Interactor { get; set; }

        [Required]
        [MaxLength(100)]
        public string IpAddress { get; set; }
        public string HostName { get; set; }
    }
}
