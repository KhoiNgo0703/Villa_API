using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;
using Villa_Utility;
using Villa_WebMVC.Models;
using Villa_WebMVC.Models.Dto;
using Villa_WebMVC.Models.VM;
using Villa_WebMVC.Services;
using Villa_WebMVC.Services.IServices;

namespace Villa_WebMVC.Controllers
{
    public class VillaNumberController : Controller
    {
        //implement IVillaService +AutoMapper+IVillaNumberService
        private readonly IVillaNumberService _villaNumberService;
        private readonly IMapper _mapper;
        private readonly IVillaService _villaService;

        //dependency injection for implementation
        public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper, IVillaService villaService)
        {
            _villaNumberService = villaNumberService;
            _mapper = mapper;
            _villaService = villaService;
        }

        //action methods

        //VillaNumber View--------------------------------------------
        public async Task<IActionResult> IndexVillaNumber()
        {
            //declare
            List<VillaNumberDTO> list = new();

            var response = await _villaNumberService.GetAllAsync<APIResponse>();
            //validation to deserialize
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        //Create VillaNumber View------------------------------------
        [Authorize(Roles = "admin")]
        //GET View
        public async Task<IActionResult> CreateVillaNumber()
        {
            VillaNumberCreateVM VillaNumberVM = new();
            var response = await _villaService.GetAllAsync<APIResponse>();
            //validation to deserialize
            if (response != null && response.IsSuccess)
            {
                VillaNumberVM.VillaList=JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result)).Select(i=>new SelectListItem 
                {
                    Text=i.Name,
                    Value=i.Id.ToString(),
                });                
            }
            return View(VillaNumberVM);
        }

        [Authorize(Roles = "admin")]
        //POST DATA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM  model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.CreateAsync<APIResponse>(model.VillaNumber);
                if (response != null && response.IsSuccess)
                {
                    //sweetalert2 notification
                    TempData["success"] = "Villa number created";
					return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    //check the error to display it in asp-validation-summary
                    TempData["error"] = (response.ErrorMessage != null && response.ErrorMessage.Count > 0) ?
                        response.ErrorMessage[0] : "Error Encountered";
                }
            }

            //populate the drop down            
            var resp = await _villaService.GetAllAsync<APIResponse>();
            //validation to deserialize
            if (resp != null && resp.IsSuccess)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(resp.Result)).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                });
            }
			//sweetalert2 notification
			TempData["error"] = "Error Encountered!";
			return View(model); ;            
        }

        //Update VillaNumber View-----------------------------------------------------
        [Authorize(Roles = "admin")]
        //GET (see info)
        public async Task<IActionResult> UpdateVillaNumber(int villaNo)
        {
            //declare the model
            VillaNumberUpdateVM VillaNumberVM = new();
            //get Info of VillaNumber
            var response = await _villaNumberService.GetAsync<APIResponse>(villaNo);
            //validation to deserialize
            if (response != null && response.IsSuccess)
            {
                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                VillaNumberVM.VillaNumber=_mapper.Map<VillaNumberUpdateDTO>(model);
            }

            //get Info of VillaList
            response = await _villaService.GetAllAsync<APIResponse>();            
            //validation to deserialize
            if (response != null && response.IsSuccess)
            {
                
                VillaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result)).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                });
                return View(VillaNumberVM);
            }


            return NotFound();
        }

        [Authorize(Roles = "admin")]
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.UpdateAsync<APIResponse>(model.VillaNumber);
                if (response != null && response.IsSuccess)
                {
                    //sweetalert2 notification
                    TempData["success"] = "Villa number updated!";
					return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    //check the error to display it in asp-validation-summary
                    TempData["error"] = (response.ErrorMessage != null && response.ErrorMessage.Count > 0) ?
                        response.ErrorMessage[0] : "Error Encountered";
                }
            }

            //populate the drop down            
            var resp = await _villaService.GetAllAsync<APIResponse>();
            //validation to deserialize
            if (resp != null && resp.IsSuccess)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(resp.Result)).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                });
            }
			//sweetalert2 notification
			TempData["error"] = "Error Encountered!";
			return View(model); ;
        }

        //DELETE VillaNumber View-----------------------------------------------------
        [Authorize(Roles = "admin")]
        //GET (see info)
        public async Task<IActionResult> DeleteVillaNumber(int villaNo)
        {
            //declare the model
            VillaNumberDeleteVM VillaNumberVM = new();
            //get Info of VillaNumber
            var response = await _villaNumberService.GetAsync<APIResponse>(villaNo);
            //validation to deserialize
            if (response != null && response.IsSuccess)
            {
                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                VillaNumberVM.VillaNumber = model;
            }

            //get Info of VillaList
            response = await _villaService.GetAllAsync<APIResponse>();
            //validation to deserialize
            if (response != null && response.IsSuccess)
            {
                VillaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result)).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                });
                return View(VillaNumberVM);
            }


            return NotFound();
        }

        [Authorize(Roles = "admin")]
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM model)
        {
            var response = await _villaNumberService.DeleteAsync<APIResponse>(model.VillaNumber.VillaNo);
            //redirect back to Index view
            if (response != null && response.IsSuccess)
            {
                //sweetalert2 notification
                TempData["success"] = "Villa number deleted!";
				return RedirectToAction(nameof(IndexVillaNumber));
            }
            else
            {
                //check the error to display it in asp-validation-summary
                TempData["error"] = (response.ErrorMessage != null && response.ErrorMessage.Count > 0) ?
                    response.ErrorMessage[0] : "Error Encountered";
            }
            return View(model);
        }
    }   
}
