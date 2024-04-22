using System.Linq.Expressions;
using Villa_WebMVC.Models.Dto;

namespace Villa_WebMVC.Services.IServices
{
    public interface IVillaNumberService
    {
        //perform all CRUD operations
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(VillaNumberCreateDTO dto);
        Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto);
        Task<T> DeleteAsync<T>(int id);
    }
}
