using ArandaSoftLab.Core.Domain.Contracts.Repositories.Productos;
using Core.UseCase.Util;
using System;

namespace Core.UseCase.Base
{
    public interface IUnitOfWork : IDisposable
    {
     

        IProductoRepository ProductoRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }
        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        int Commit(IInteractor interactor);
    }
}
