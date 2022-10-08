using System.ComponentModel.DataAnnotations;

namespace ArandaSoftLab.Core.Domain.Base
{
    public abstract class BaseEntity
    {

    }

    public abstract class Entity<T> : BaseEntity, IEntity<T>
    {
        [Key]
        public virtual T Id { get; set; }
    }
}
