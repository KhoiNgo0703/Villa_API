﻿using System.Net;

namespace Villa_WebMVC.Models
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
