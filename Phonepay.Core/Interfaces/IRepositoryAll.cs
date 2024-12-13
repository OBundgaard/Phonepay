

namespace Phonepay.Core.Interfaces;

public interface IRepositoryAll<T> : IBaseRepository<T>
{
    Task<IEnumerable<T>> GetAll();
}
