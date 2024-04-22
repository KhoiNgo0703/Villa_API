﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Villa_WebMVC.Models.Dto;

namespace Villa_WebMVC.Models.VM
{
    public class VillaNumberDeleteVM
    {
        public VillaNumberDeleteVM() 
        {
            VillaNumber = new VillaNumberDTO();
        }

        public VillaNumberDTO VillaNumber { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> VillaList { get; set; }
    }
}