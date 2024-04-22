using System.Linq.Expressions;
using Villa_WebMVC.Models.Dto;

namespace Villa_WebMVC.Services.IServices
{
    public interface IVillaService
    {
        //perform all CRUD operations
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(VillaCreateDTO dto);
        Task<T> UpdateAsync<T>(VillaUpdateDTO dto);
        Task<T> DeleteAsync<T>(int id);
    }
}
