using ArandaSoftLab.Core.Domain.Contracts.Repositories.Productos;
using ArandaSoftLab.Core.Domain.Enumerations;
using ArandaSoftLab.Core.Domain.Productos;
using ArandaSoftLab.Infrastructure.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ArandaSoftLab.Infrastructure.Data.Repositories.Productos
{
    public class ProductoRepository : GenericRepository<Producto>, IProductoRepository
    {
        public ProductoRepository(IDbContext context) : base(context)
        {
        }

        public List<Producto> ConsultaParametrizada(ConsularProductosParameterRequest request)
        {
            var query = _dbset.AsQueryable().Where(t => t.State == EstadoEnumeration.Activo.Value);


            if (request.CategoriaId.HasValue)
            {
                query = query.Where(t => t.CategoriaId == request.CategoriaId.Value);
            }
            if (!string.IsNullOrEmpty(request.Descripcion))
            {
                query = query.Where(t => t.Descripcion == request.Descripcion);
            }
            if (!string.IsNullOrEmpty(request.Nombre))
            {
                query = query.Where(t => t.Nombre == request.Nombre);
            }

            if (request.OrdenNombreDesc)
            {
                query = query.OrderByDescending(t => t.Nombre);
            }

            else
            {
                query = query.OrderBy(t => t.Nombre);
            }

            if (request.OrdenDescripcionDesc)
            {
                query = query.AppendOrderByDescending(t => t.Descripcion);
            }
            else
            {
                query = query.AppendOrderBy(t => t.Descripcion);
            }

            var ItemsByPage = !request.ItemsByPage.HasValue || request.ItemsByPage.Value <= 0 ? 100 : request.ItemsByPage.Value;
            var Page = (!request.Page.HasValue || request.Page.Value <= 0 ? 0 : request.Page.Value);
            query = query
              .Skip(ItemsByPage * Page)
              .Take(ItemsByPage);

            return query.ToList();
        }
    }

    public static class QueryableExtensions
    {
        public static IOrderedQueryable<T> AppendOrderBy<T, TKey>(this IQueryable<T> query, Expression<Func<T, TKey>> keySelector)
            => query.Expression.Type == typeof(IOrderedQueryable<T>)
            ? ((IOrderedQueryable<T>)query).ThenBy(keySelector)
            : query.OrderBy(keySelector);

        public static IOrderedQueryable<T> AppendOrderByDescending<T, TKey>(this IQueryable<T> query, Expression<Func<T, TKey>> keySelector)
            => query.Expression.Type == typeof(IOrderedQueryable<T>)
                ? ((IOrderedQueryable<T>)query).ThenByDescending(keySelector)
                : query.OrderByDescending(keySelector);
    }
}
