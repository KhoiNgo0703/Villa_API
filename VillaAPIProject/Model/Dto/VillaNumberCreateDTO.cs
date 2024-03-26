﻿using System.ComponentModel.DataAnnotations;

namespace VillaAPIProject.Model.Dto
{
    public class VillaNumberCreateDTO
    {

        [Required]
        public int VillaNo { get; set; }
        public string SpecialDetails { get; set; }
        [Required]
        public int VillaID { get; set; }

    }
}
