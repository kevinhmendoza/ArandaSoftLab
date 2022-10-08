using ArandaSoftLab.Core.Domain.Contracts.Repositories.Productos;
using ArandaSoftLab.Core.Domain.Productos;
using ArandaSoftLab.Infrastructure.Data.Base;

namespace ArandaSoftLab.Infrastructure.Data.Repositories.Productos
{
    public class ProductoRepository : GenericRepository<Producto>, IProductoRepository
    {
        public ProductoRepository(IDbContext context) : base(context)
        {
        }
    }
}
