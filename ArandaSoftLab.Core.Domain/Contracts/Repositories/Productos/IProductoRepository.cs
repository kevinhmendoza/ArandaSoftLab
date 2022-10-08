using ArandaSoftLab.Core.Domain.Productos;
using Core.UseCase.Base;
using System.Collections.Generic;

namespace ArandaSoftLab.Core.Domain.Contracts.Repositories.Productos
{
    public interface IProductoRepository : IGenericRepository<Producto>
    {
        List<Producto> ConsultaParametrizada(ConsularProductosParameterRequest request);
    }
    public class ConsularProductosParameterRequest
    {
        public int? CategoriaId { get; set; }
        public string Nombre { get; set; }
        public bool OrdenNombreDesc { get; set; }
        public string Descripcion { get; set; }
        public bool OrdenDescripcionDesc { get; set; }

        public int? Page { get; set; }
        public int? ItemsByPage { get; set; }
    }
}
