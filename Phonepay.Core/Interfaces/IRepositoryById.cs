

namespace Phonepay.Core.Interfaces;

public interface IRepositoryById<T> : IBaseRepository<T>
{
    Task<IEnumerable<T>> GetAll(int id);
}
