using ArandaSoftLab.Core.Domain.Contracts.Repositories.Productos;
using ArandaSoftLab.Infrastructure.Data.Repositories.Productos;
using Core.UseCase.Base;
using Core.UseCase.Util;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace ArandaSoftLab.Infrastructure.Data.Base
{
    /// <summary>
    /// The Entity Framework implementation of IUnitOfWork
    /// </summary>
    public sealed class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The DbContext Base que tiene implementado la gestión de Auditoria
        /// </summary>
        private IDbContext _dbContext;
     


        private IProductoRepository _productoRepository;
        public IProductoRepository ProductoRepository { get { return _productoRepository ?? (_productoRepository = new ProductoRepository(_dbContext)); } }

        private ICategoriaRepository _categoriaRepository;
        public ICategoriaRepository CategoriaRepository { get { return _categoriaRepository ?? (_categoriaRepository = new CategoriaRepository(_dbContext)); } }

        /// <summary>
        /// Initializes a new instance of the UnitOfWork class.
        /// </summary>
        /// <param name="context">The object context</param>
        public UnitOfWork(IDbContext context)
        {
            _dbContext = context;
          
        }

   

      

        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        public int Commit(IInteractor interactor)
        {
            try
            {
                int NumeroFilas = _dbContext.SaveChanges("LocalHost", "::", "Admin", DateTime.Now, interactor.Module, interactor.Name);
                return NumeroFilas;
            }
            catch (DbEntityValidationException e)
            {
                string message = DynamicException.Formatted(e);
                throw new DynamicFormattedException(message);
            }
            catch (DbUpdateException e)
            {
                string message = DynamicException.Formatted(e);
                throw new DynamicFormattedException(message);
            }

        }
        /// <summary>
        /// Disposes the current object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Disposes all external resources.
        /// </summary>
        /// <param name="disposing">The dispose indicator.</param>
        private void Dispose(bool disposing)
        {
            if (disposing && _dbContext != null)
            {
                ((DbContext)_dbContext).Dispose();
                _dbContext = null;
            }
        }

    }
}
