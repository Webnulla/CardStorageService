using System.Collections.Generic;
using System.Threading.Tasks;

namespace CardStorageService.Services
{
    public interface IRepository<T, TId>
    {
        IList<T> GetAll();
        T GetById(TId id);
        TId Create(T data);
        Task Update(T data);
        Task Delete(TId id);
    }
}