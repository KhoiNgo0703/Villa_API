using Villa_WebMVC.Models;


namespace Villa_WebMVC.Services.IServices
{
    public interface IBaseService
    {
        //API response
         APIResponse responseModel { get; set; }
        //method to send and call API
        Task<T> SendAsync<T>(APIRequest apiRequest, bool withBearer = true);

    }
}
