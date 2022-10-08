using ArandaSoftLab.Core.Domain.Productos;
using ArandaSoftLab.Infrastructure.Data;
using System;
using System.Collections.Generic;

namespace Infrastructure.Initialization.Data
{
    public class InicializacionCategorias
    {
        public void Seed(ArandaSoftLabContext ctx)
        {

            List<Categoria> _categorias = new List<Categoria>()
            {
                Categoria_1(),
                Categoria_2(),
                Categoria_3()
            };
            ctx.Categorias.AddRange(_categorias);
        }

        private Categoria Categoria_1()
        {
            return new Categoria()
            {
                Nombre = "Cosmeticos",
                State = "AC",
                CreatedBy = "admin",
                CreatedDate = DateTime.Now,
                UpdatedBy = "admin",
                UpdatedDate = DateTime.Now,
                IPAddress = "admin"
            };
        }
        private Categoria Categoria_2()
        {
            return new Categoria()
            {
                Nombre = "Alimentos",
                State = "AC",
                CreatedBy = "admin",
                CreatedDate = DateTime.Now,
                UpdatedBy = "admin",
                UpdatedDate = DateTime.Now,
                IPAddress = "admin"
            };
        }
        private Categoria Categoria_3()
        {
            return new Categoria()
            {
                Nombre = "Aseo",
                State = "AC",
                CreatedBy = "admin",
                CreatedDate = DateTime.Now,
                UpdatedBy = "admin",
                UpdatedDate = DateTime.Now,
                IPAddress = "admin"
            };
        }
    }
}
