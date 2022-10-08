using ArandaSoftLab.Core.Domain.Productos;
using ArandaSoftLab.Infrastructure.Data.Base;
using ArandaSoftLab.Infrastructure.Data.Map;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ArandaSoftLab.Infrastructure.Data
{


    public class ArandaSoftLabContext : DbContextBase
    {

        public ArandaSoftLabContext()
            : base("Name=ArandaSoftLabContext")
        {



        }

        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            MaperEntityToDataBase.MapearEntityToDataBase(modelBuilder);
        }

    }
}
