using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWALKS.API.CustomActionFilters;
using NZWALKS.API.DTO;
using NZWALKS.API.Models.Domain;
using NZWALKS.API.Repositories;
using System.Net;

namespace NZWALKS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalkController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalkController( IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addwalkRequestDto)
        { 
            //Map Dto to domain model
            var walkDomainModel = mapper.Map<Walk>(addwalkRequestDto);
            await walkRepository.CreateAsync(walkDomainModel);

            //Map domainmodel to Dto
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string?filterOn, [FromQuery] string? filterquery,
        [FromQuery] string? SortBy, [FromQuery] bool? isascending , [FromQuery] int pageNumber = 1,[FromQuery] int pageSize =1000 )
        {
        var walksDomainModel = await walkRepository.GetAllAsync(filterOn, filterquery, SortBy, isascending ?? true, pageNumber, pageSize);

        throw new Exception("This is a new Exception");
         //Map domain model to DTO
        return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));

        }


        [HttpGet] 
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var walkdomainmodel = await walkRepository.GetByIdAsync(id);
            if (walkdomainmodel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(walkdomainmodel));
            
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        { 
            
         var walkdomainmodel = mapper.Map<Walk>(updateWalkRequestDto);

        walkdomainmodel = await walkRepository.UpdateAsync(id, walkdomainmodel);

        if (walkdomainmodel == null)
        {
        return NotFound();
        }
        return Ok(mapper.Map<WalkDto>(walkdomainmodel));
        }

        [HttpDelete]
        [Route("{id:guid}")]
        
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
         var deletedWalkdomainmodel = await walkRepository.DeleteAsync(id);
         if(deletedWalkdomainmodel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(deletedWalkdomainmodel));
        }

    }
}