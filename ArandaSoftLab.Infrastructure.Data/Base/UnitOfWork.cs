using ArandaSoftLab.Core.Domain.Contracts.Repositories.Productos;
using ArandaSoftLab.Infrastructure.Data.Repositories.Productos;
using Core.Entities.Contracts;
using Core.UseCase.Base;
using Core.UseCase.Util;
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
        private readonly ILogger _logger;
        private readonly ISistema _sistema;


        private IProductoRepository _productoRepository;
        public IProductoRepository ProductoRepository { get { return _productoRepository ?? (_productoRepository = new ProductoRepository(_dbContext)); } }

        /// <summary>
        /// Initializes a new instance of the UnitOfWork class.
        /// </summary>
        /// <param name="context">The object context</param>
        public UnitOfWork(IDbContext context, ISistema sistema, ILogger logger)
        {
            _dbContext = context;
            _logger = logger;
            _sistema = sistema;
        }

        /// <summary>
        /// Obtiene la Información del Sistema como el Usuario Actual y La Fecha Actual
        /// </summary>
        public ISistema Sistema
        {
            get
            {
                return _sistema;
            }
        }

      

        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        public int Commit(IInteractor interactor)
        {
            try
            {
                int NumeroFilas = _dbContext.SaveChanges(_sistema.HostName, _sistema.IpAddress, _sistema.UserName, _sistema.Now, interactor.Module, interactor.Name);
                _logger.Info("Se realizó Transacción");
                return NumeroFilas;
            }
            catch (DbEntityValidationException e)
            {
                string message = DynamicException.Formatted(e);
                _logger.Error(message);
                throw new DynamicFormattedException(message);
            }
            catch (DbUpdateException e)
            {
                string message = DynamicException.Formatted(e);
                _logger.Error(message);
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
