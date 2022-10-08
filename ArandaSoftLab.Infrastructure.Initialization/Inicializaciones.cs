using ArandaSoftLab.Infrastructure.Data;
using Infrastructure.Initialization.Data;
using System.Linq;

namespace Infrastructure.Initialization
{
    public class Inicializaciones
    {
        private readonly ArandaSoftLabContext _arandaSoftLabContext;
        public Inicializaciones()
        {
            _arandaSoftLabContext = new ArandaSoftLabContext();
        }
        public void Seeders()
        {
            var existeCategorias = _arandaSoftLabContext.Categorias.Any();
            if (!existeCategorias)
            {
                InicializacionData();
            }
        }

        private void InicializacionData()
        {
            new InicializacionCategorias().Seed(_arandaSoftLabContext);
            _arandaSoftLabContext.SaveChanges();
        }
    }
}
