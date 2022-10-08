using System;
using System.ComponentModel.DataAnnotations;

namespace ArandaSoftLab.Core.Domain.Base
{
    public abstract class AuditableEntity<T> : Entity<T>, IAuditableEntity
    {
        [ScaffoldColumn(false)]
        public DateTime CreatedDate { get; set; }

        [MaxLength(256)]
        [ScaffoldColumn(false)]
        public string CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime UpdatedDate { get; set; }

        [MaxLength(256)]
        [ScaffoldColumn(false)]
        public string UpdatedBy { get; set; }

        [MaxLength(20)]
        [ScaffoldColumn(false)]
        public string IPAddress { get; set; }

        [MaxLength(20)]
        public string State { get; set; }

    }
}
