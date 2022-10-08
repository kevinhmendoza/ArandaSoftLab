using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArandaSoftLab.Infrastructure.Audit
{
    public class AuditoriaService
    {
        private readonly ArandaSoftLabAuditLogContext _ctx;
        public AuditoriaService()
        {
            _ctx = new ArandaSoftLabAuditLogContext();
        }
        public void GuardarAuditorias(List<AuditLog> Auditorias)
        {
            _ctx.AuditLogs.AddRange(Auditorias);
            _ctx.SaveChanges();
        }
    }
}
