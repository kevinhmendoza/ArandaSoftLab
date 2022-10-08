using ArandaSoftLab.Infrastructure.Data.Map.Productos;
using System.Data.Entity;

namespace ArandaSoftLab.Infrastructure.Data.Map
{
    class MaperEntityToDataBase
    {
        protected MaperEntityToDataBase()
        {

        }

        internal static void MapearEntityToDataBase(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ProductoMap());
            modelBuilder.Configurations.Add(new CategoriaMap());

        }
    }
}
