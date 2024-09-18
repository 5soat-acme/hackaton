using HT.Core.Commons.DomainObjects;

namespace HT.Core.Commons.Repository;

public interface IRepository<TEntity> where TEntity : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}