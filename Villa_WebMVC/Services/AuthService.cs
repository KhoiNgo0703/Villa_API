using Villa_Utility;
using Villa_WebMVC.Models;
using Villa_WebMVC.Models.Dto;
using Villa_WebMVC.Services.IServices;

namespace Villa_WebMVC.Services
{
	public class AuthService: IAuthService
	{
		//implement ihttpclientfactory
		private readonly IHttpClientFactory _clientFactory;
		//implement api url
		private string villaUrl;
        //implement base service
        private readonly IBaseService _baseService;

        //remember to add path to the base class
        public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration, IBaseService baseService) 
		{
			_clientFactory = clientFactory;
			_baseService = baseService;
			villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
		}

		public async Task<T> LoginAsync<T>(LoginRequestDTO obj)
		{
			return await _baseService.SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.POST,
				Data = obj,
				Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/UsersAuth/login"
			},withBearer:false);
		}

		public async Task<T> RegisterAsync<T>(RegisterationRequestDTO obj)
		{
			return await _baseService.SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.POST,
				Data = obj,
				Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/UsersAuth/register"
			}, withBearer: false);
		}

        public async Task<T> LogoutAsync<T>(TokenDTO obj)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/UsersAuth/revoke"
            });
        }
    }
}
