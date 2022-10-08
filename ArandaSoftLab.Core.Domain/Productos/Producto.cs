using ArandaSoftLab.Core.Domain.Base;
using ArandaSoftLab.Core.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArandaSoftLab.Core.Domain.Productos
{
    public class Producto : AuditableEntity<long>
    {
        public string Nombre { get;  set; }
        public string Descripcion { get;  set; }
        public int CategoriaId { get;  set; }
        public string Imagen { get;  set; }
        public virtual Categoria Categoria { get; set; }

        public void Inactivar() {

            State = EstadoEnumeration.Inactivo.Value;
        }

       
    }

    




}
