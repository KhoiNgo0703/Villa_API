using Villa_WebMVC.Models;

namespace Villa_WebMVC.Services.IServices
{
    public interface IApiMessageRequestBuilder
    {
        HttpRequestMessage Build(APIRequest apiRequest);
    }
}
