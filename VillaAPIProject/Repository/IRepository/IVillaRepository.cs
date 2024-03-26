using System.Linq.Expressions;
using VillaAPIProject.Model;

namespace VillaAPIProject.Repository.IRepository
{
    public interface IVillaRepository: IRepository<Villa>
    {
        //Use async here        
        Task<Villa> UpdateAsync(Villa entity);        
    }
}
