using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using Villa_Utility;
using Villa_WebMVC.Models;
using Villa_WebMVC.Models.Dto;
using Villa_WebMVC.Services.IServices;

namespace Villa_WebMVC.Controllers
{
    public class HomeController : Controller
    {
        ////implement IVillaService +AutoMapper
        //private readonly IVillaService _villaService;
        //private readonly IMapper _mapper;

        ////dependency injection for implementation
        //public HomeController(IVillaService villaService, IMapper mapper)
        //{
        //    _villaService = villaService;
        //    _mapper = mapper;
        //}

        ////action methods

        ////HOME PAGE
        ////GET
        //public async Task<IActionResult> Index()
        //{
        //    List<VillaDTO> list = new();
        //    var response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.AccessToken));
        //    if (response != null && response.IsSuccess) 
        //    {
        //        list=JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
        //    }
        //    return View(list);
        //}

        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        public HomeController(IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<VillaDTO> list = new();

            var response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
    }
}
