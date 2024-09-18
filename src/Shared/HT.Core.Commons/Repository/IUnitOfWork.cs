namespace HT.Core.Commons.Repository;

public interface IUnitOfWork
{
    Task<bool> Commit();
}