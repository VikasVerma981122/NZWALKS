using AutoMapper;
using NZWALKS.API.DTO;
using NZWALKS.API.Models.Domain;

namespace NZWALKS.API.Mappings
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region,RegionDto>().ReverseMap();
            CreateMap<AddregionRequestDto,Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto,Region>().ReverseMap();
            CreateMap<AddWalkRequestDto,Walk>().ReverseMap();
            CreateMap<Walk, WalkDto>().ReverseMap();
            CreateMap<Difficulty,DifficultyDto>().ReverseMap();
            CreateMap<UpdateWalkRequestDto,Walk>().ReverseMap();   
        }
    }
}
