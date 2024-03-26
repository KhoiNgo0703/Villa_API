using System.Linq.Expressions;
using VillaAPIProject.Model;

namespace VillaAPIProject.Repository.IRepository
{
    public interface IVillaNumberRepository: IRepository<VillaNumber>
    {
        //Use async here        
        Task<VillaNumber> UpdateAsync(VillaNumber entity);        
    }
}
