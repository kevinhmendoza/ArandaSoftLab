namespace ArandaSoftLab.Core.Domain.Base
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
