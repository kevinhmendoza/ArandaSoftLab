using ArandaSoftLab.Core.Domain.Base;
using ArandaSoftLab.Core.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArandaSoftLab.Core.Domain.Productos
{
    public class Categoria : AuditableEntity<int>
    {
    
        public string Nombre { get; set; }
        public virtual ICollection<Producto> Productos { get; set; }

        public void Inactivar()
        {

            State = EstadoEnumeration.Inactivo.Value;
        }

    }
}
