using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArandaSoftLab.Core.UseCase.Dtos
{
    public class ProductoDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int CategoriaId { get; set; }
        public string Imagen { get; set; }
    }
}
