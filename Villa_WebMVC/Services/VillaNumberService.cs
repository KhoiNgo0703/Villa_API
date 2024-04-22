using Villa_Utility;
using Villa_WebMVC.Models.Dto;
using Villa_WebMVC.Models;
using Villa_WebMVC.Services.IServices;

namespace Villa_WebMVC.Services
{
    public class VillaNumberService : IVillaNumberService
    {
        //implement ihttpclientfactory
        private readonly IHttpClientFactory _clientFactory;
        //implement api url
        private string villaUrl;
        //implement base service
        private readonly IBaseService _baseService;

        //remember to add path to the base class
        public VillaNumberService(IHttpClientFactory clientFactory, IConfiguration configuration, IBaseService baseService) 
        {
            _clientFactory = clientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
            _baseService = baseService;
        }

        //CRUD with base service
        public async Task<T> CreateAsync<T>(VillaNumberCreateDTO dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType=SD.ApiType.POST,
                Data = dto,
                Url=villaUrl+ $"/api/{SD.CurrentAPIVersion}/VillaNumberAPI" ,                
            });
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/VillaNumberAPI/" + id,
            });
        }

        public async Task<T> GetAllAsync<T>()
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,                
                Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/VillaNumberAPI",
               
            });
        }

        public async Task<T> GetAsync<T>(int id)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/VillaNumberAPI/" + id,
                
            });
        }

        public async Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/VillaNumberAPI/" + dto.VillaNo,
                
            });
        }
    }
}
