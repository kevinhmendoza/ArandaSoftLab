using ArandaSoftLab.Core.Domain.Contracts.Repositories.Productos;
using ArandaSoftLab.Core.Domain.Productos;
using ArandaSoftLab.Infrastructure.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArandaSoftLab.Infrastructure.Data.Repositories.Productos
{
    public class CategoriaRepository : GenericRepository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(IDbContext context) : base(context)
        {

        }
    }
}
