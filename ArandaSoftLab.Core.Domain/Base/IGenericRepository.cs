using ArandaSoftLab.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.UseCase.Base
{
    public interface IGenericRepository<T> where T : BaseEntity
    {

        IEnumerable<T> GetAll();

        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);

        T Find(object id);

        bool Any(Expression<Func<T, bool>> predicate);

        T Add(T entity);

        void Delete(T entity);

        void Edit(T entity);

        IEnumerable<T> AddRange(List<T> entities);

        void DeleteRange(List<T> entities);

        T FindFirstOrDefault(Expression<Func<T, bool>> predicate);

        IEnumerable<T> FindBy(Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = "");
    }
}
