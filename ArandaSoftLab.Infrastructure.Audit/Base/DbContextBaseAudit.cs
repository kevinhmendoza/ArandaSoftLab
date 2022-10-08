using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ArandaSoftLab.Infrastructure.Audit.Base
{
    public interface IDbContextAudit 
    {
        DbSet<T> Set<T>() where T : class;
        DbEntityEntry Entry(object entity);
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
    public class DbContextBaseAudit : DbContext, IDbContextAudit
    {
        public DbContextBaseAudit(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

    }
}
