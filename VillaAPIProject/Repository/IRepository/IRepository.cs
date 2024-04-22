using System.Linq.Expressions;
using VillaAPIProject.Model;

namespace VillaAPIProject.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //Use async here
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null,
            int pageSize = 0, int pageNumber = 1);
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        Task CreateAsync(T entity);
        Task RemoveAsync(T entity);        
        Task SaveAsync();
    }
}
