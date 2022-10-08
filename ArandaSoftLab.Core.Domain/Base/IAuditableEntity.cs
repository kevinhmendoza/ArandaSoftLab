using System;

namespace ArandaSoftLab.Core.Domain.Base
{
    public interface IAuditableEntity
    {
        DateTime CreatedDate { get; set; }

        string CreatedBy { get; set; }

        DateTime UpdatedDate { get; set; }

        string UpdatedBy { get; set; }

        string IPAddress { get; set; }

        string State { get; set; }
    }
}
