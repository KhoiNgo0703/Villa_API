using AutoMapper;
using VillaAPIProject.Model;
using VillaAPIProject.Model.Dto;

namespace VillaAPIProject
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
            //---------Villa------
            //Source first, destination after
            CreateMap<Villa,VillaDTO>();
            CreateMap<VillaDTO, Villa>();

            //for mapping update and create and avoid writing multiple times
            CreateMap<Villa,VillaCreateDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();

            //---------Villa Number-----
            CreateMap<VillaNumber,VillaNumberDTO>();
            CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();
        }
    }
}
