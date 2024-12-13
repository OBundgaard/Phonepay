

namespace Phonepay.Core.Interfaces;

public interface IRepository<T>
{
    Task<T> Post(T entry);
    Task<T?> Get(int id);
    Task<T> Put(T entry);
    Task Delete(int id);
}
