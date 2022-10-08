using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.UseCase.Base
{
    public interface IGenericRepositoryQuery<T> 
    {
        IEnumerable<T> GetAll();

        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);

        T Find(object id);
        
        T FindFirstOrDefault(Expression<Func<T, bool>> predicate);
    }
}
