using AutoMapper;
using Villa_WebMVC.Models;
using Villa_WebMVC.Models.Dto;

namespace Villa_WebMVC
{
    public class MappingConfig: Profile// Profile here to inherit from AutoMapper
    {
        //to reduce these lines of code
        //ex:
        //Name = villaDTO.Name,
        //Details = villaDTO.Details,
        //ImageUrl = villaDTO.ImageUrl,
        //Occupancy = villaDTO.Occupancy,
        //Rate = villaDTO.Rate,
        //Sqft = villaDTO.Sqft,
        //Amenity = villaDTO.Amenity

        public MappingConfig() 
        {
            //---------VillaDTO------
            //Source first, destination after          

            //for mapping update and create and avoid writing multiple times
            CreateMap<VillaDTO,VillaCreateDTO>().ReverseMap();
            CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();

            //---------Villa NumberDTO-----            
            CreateMap<VillaNumberDTO, VillaNumberUpdateDTO>().ReverseMap();
            CreateMap<VillaNumberDTO, VillaNumberCreateDTO>().ReverseMap();
        }
    }
}
