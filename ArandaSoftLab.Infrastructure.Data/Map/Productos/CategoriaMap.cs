using ArandaSoftLab.Core.Domain.Productos;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArandaSoftLab.Infrastructure.Data.Map.Productos
{
    public class CategoriaMap : EntityTypeConfiguration<Categoria>
    {
        public CategoriaMap()
        {
            HasKey(t => t.Id);
            Property(t => t.Nombre).IsRequired();
        }

    }
}
