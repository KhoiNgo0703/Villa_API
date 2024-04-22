using static Villa_Utility.SD;

namespace Villa_WebMVC.Models
{
    public class APIRequest
    {
        //things need to request 
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string Token { get; set; }

        public ContentType ContentType { get; set; } = ContentType.Json;

    }
}
