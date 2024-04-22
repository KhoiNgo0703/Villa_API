﻿using System.ComponentModel.DataAnnotations;

namespace Villa_WebMVC.Models.Dto
{
    public class VillaCreateDTO
    {
       
        //put validation before the var
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public string Details { get; set; }
        [Required]
        public double Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image {  get; set; }
        public string Amenity { get; set; }

    }
}
