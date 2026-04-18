using FinTrack.Core.Entities;

namespace FinTrack.Core.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task Insert(T entity);
        void Update(T entity);
        Task Delete(int id);
    }
}
