using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Villa_Utility;
using Villa_WebMVC.Models;
using Villa_WebMVC.Models.Dto;
using Villa_WebMVC.Services.IServices;

namespace Villa_WebMVC.Controllers
{
    public class VillaController: Controller
    {
        //implement IVillaService +AutoMapper
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        //dependency injection for implementation
        public VillaController(IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
        }

        //action methods
        
        //Villa View--------------------------------------------
        public async Task<IActionResult> IndexVilla()
        {
            //declare
            List<VillaDTO> list = new();

            var response = await _villaService.GetAllAsync<APIResponse>();
            //validation to deserialize
            if (response !=null && response.IsSuccess) 
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        //Create Villa View------------------------------------
        //Authorize
        [Authorize(Roles ="admin")]
        //GET
        public async Task<IActionResult> CreateVilla()
        {            
            return View();
        }

        [Authorize(Roles = "admin")]
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO dto)
        {
            if (ModelState.IsValid)
            {
                //pass dto to create async 
                var response = await _villaService.CreateAsync<APIResponse>(dto);
                //redirect back to Index view
                if (response != null && response.IsSuccess) 
                {
                    //sweetalert2 notification
                    TempData["success"] = "Villa created!";
                    return RedirectToAction(nameof(IndexVilla));
                }

            }
            //return dto with error if it is invalid
            //sweetalert2 notification
            TempData["error"] = "Error Encountered!";
            return View(dto);
        }

        //Update Villa View-----------------------------------------------------
        [Authorize(Roles = "admin")]
        //GET (see info)
        public async Task<IActionResult> UpdateVilla(int villaID)
        {
            //get Info
            var response = await _villaService.GetAsync<APIResponse>(villaID);
            //validation to deserialize
            if (response != null && response.IsSuccess)
            {
                VillaDTO model =JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
                return View(_mapper.Map<VillaUpdateDTO>(model));
            }
            return NotFound();
        }


        [Authorize(Roles = "admin")]
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO dto)
        {
            if (ModelState.IsValid)
            {
                //pass dto to create async 
                var response = await _villaService.UpdateAsync<APIResponse>(dto);
                TempData["success"] = "Villa updated!";
                //redirect back to Index view
                if (response != null && response.IsSuccess)
                {
                    
                    return RedirectToAction(nameof(IndexVilla));
                }

            }
            //return dto with error if it is invalid
            //sweetalert2 notification
            TempData["error"] = "Error Encountered!";
            return View(dto);
        }

        //DELETE Villa View-----------------------------------------------------
        [Authorize(Roles = "admin")]
        //GET (see info)
        public async Task<IActionResult> DeleteVilla(int villaID)
        {
            //get Info
            var response = await _villaService.GetAsync<APIResponse>(villaID);
            //validation to deserialize
            if (response != null && response.IsSuccess)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVilla(VillaDTO dto)
        {
            var response = await _villaService.DeleteAsync<APIResponse>(dto.Id);
            //redirect back to Index view
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa deleted!";
                return RedirectToAction(nameof(IndexVilla));
            }
            //return dto with error if it is invalid
            //sweetalert2 notification
            TempData["error"] = "Error Encountered!";
            return View(dto);
        }
    }
}
