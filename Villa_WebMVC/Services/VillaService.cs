using Villa_Utility;
using Villa_WebMVC.Models.Dto;
using Villa_WebMVC.Models;
using Villa_WebMVC.Services.IServices;

namespace Villa_WebMVC.Services
{
    public class VillaService : IVillaService
    {
        //implement ihttpclientfactory
        private readonly IHttpClientFactory _clientFactory;
        //implement api url
        private string villaUrl;
        //implement base service
        private readonly IBaseService _baseService;

        //remember to add path to the base class
        public VillaService(IHttpClientFactory clientFactory, IConfiguration configuration, IBaseService baseService) 
        {
            _clientFactory = clientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
            _baseService = baseService;
        }

        //CRUD with base service
        public async Task<T> CreateAsync<T>(VillaCreateDTO dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType=SD.ApiType.POST,
                Data = dto,
                Url=villaUrl+ $"/api/{SD.CurrentAPIVersion}/VillaAPI",
                
                ContentType=SD.ContentType.MultipartFormData
            });
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,               
                Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/VillaAPI/" + id,
                                
            });
        }

        public async Task<T> GetAllAsync<T>()
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,                
                Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/VillaAPI",
                              
            });
        }

        public async Task<T> GetAsync<T>(int id)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/VillaAPI/" + id,
                             
            });
        }

        public async Task<T> UpdateAsync<T>(VillaUpdateDTO dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/VillaAPI/" + dto.Id,                
                ContentType = SD.ContentType.MultipartFormData
            });
        }
    }
}
