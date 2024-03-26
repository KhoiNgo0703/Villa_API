using System.Net;

namespace VillaAPIProject.Model
{
    public class APIResponse
    {
        //For props of API response
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessage { get; set; }
        public object Result { get; set; }
    }
}
