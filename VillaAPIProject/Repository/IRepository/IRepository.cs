using System.Linq.Expressions;
using VillaAPIProject.Model;

namespace VillaAPIProject.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //Use async here
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true);
        Task CreateAsync(T entity);
        Task RemoveAsync(T entity);        
        Task SaveAsync();
    }
}
