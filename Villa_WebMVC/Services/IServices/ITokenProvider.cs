using Villa_WebMVC.Models.Dto;

namespace Villa_WebMVC.Services.IServices
{
    public interface ITokenProvider
    {
        void SetToken(TokenDTO tokenDTO);
        TokenDTO? GetToken();
        void ClearToken();
    }
}
