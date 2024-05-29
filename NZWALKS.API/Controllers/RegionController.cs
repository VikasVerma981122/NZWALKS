using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWALKS.API.CustomActionFilters;
using NZWALKS.API.Data;
using NZWALKS.API.DTO;
using NZWALKS.API.Models.Domain;
using NZWALKS.API.Repositories;
using System.Collections.Generic;
using System.Text.Json;

namespace NZWALKS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RegionController : ControllerBase
    {
        private readonly NZWalkDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionController> logger;

        public RegionController(NZWalkDbContext dbContext, IRegionRepository regionRepository, IMapper mapper,
        ILogger<RegionController> logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }


        [HttpGet]
        //[Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAll()  // Asynchronous Programming...
        {
            var regionsdomain = await regionRepository.GetAllAsync();

            return Ok(mapper.Map<List<RegionDto>>(regionsdomain));
        }
            //try
            //{
            //    throw new Exception("This is a custom exception");
            //logger.LogInformation("GetAllRegions Action Method was invoked..");
            //logger.LogWarning("This is a warning log..");
            //logger.LogError("This is a error log..");

            // get data from datbase -- domain model


            // Map domain model to dtos
            //var regionsDto = new List<RegionDto>();
            //foreach (var regiondomain in regionsdomain)
            //{
            //    regionsDto.Add(new RegionDto()
            //    {
            //        Id = regiondomain.Id,
            //        Code = regiondomain.Code,
            //        Name = regiondomain.Name,   
            //        RegionImageUrl = regiondomain.RegionImageUrl
            //    });
            //}
            //var regionsDto = mapper.Map<List<RegionDto>>(regionsdomain);
            //logger.LogInformation($"Finished GetAllRegions Request with data:{JsonSerializer.Serialize(regionsdomain)}");

        //}
        ////catch (Exception ex)
        ////{
        ////    logger.LogError(ex,ex.Message);
        ////    throw;
        ////}

    

        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) 
        {
 
            //var regionDomain = dbContext.Regions.Find(id);
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null) 
            { 
             return NotFound();

            }
            // Converting/ Mapping Domain models to DTos.
            //var regionDto = new RegionDto
            //{
            //    Id = regionDomain.Id,
            //    Code = regionDomain.Code,
            //    Name = regionDomain.Name,
            //    RegionImageUrl = regionDomain.RegionImageUrl

            //};
            //Return DTO back to client..
            //return Ok(regionDto);
            // By using Automapper..We can shorting the line of code.
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        [HttpPost]
        [ValidateModel]
       //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddregionRequestDto addregionRequestDto) 
        {
            var regionDomainModel = mapper.Map<Region>(addregionRequestDto);
            //{
            //    Code = addregionRequestDto.Code,
            //    Name = addregionRequestDto.Name,
            //    RegionImageUrl = addregionRequestDto.RegionImageUrl
            //};
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);
            await dbContext.SaveChangesAsync();

            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};
            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id,[FromBody] UpdateRegionRequestDto updateregionRequestDto) 
        {  
            
        var RegionDomainModel = mapper.Map<Region>(updateregionRequestDto);
                //{
                //    Code = updateregionRequestDto.Code,
                //    Name = updateregionRequestDto.Name,
                //    RegionImageUrl = updateregionRequestDto.RegionImageUrl
                //};
        RegionDomainModel = await regionRepository.UpdateAsync(id, RegionDomainModel);

        if (RegionDomainModel == null)
        {
        return NotFound();
        }

        RegionDomainModel.Code = updateregionRequestDto.Code;
        RegionDomainModel.Name = updateregionRequestDto.Name;
        RegionDomainModel.RegionImageUrl = updateregionRequestDto.RegionImageUrl;

        await dbContext.SaveChangesAsync();
                // Convert DomainModel to Dto....
        var regionDto = mapper.Map<UpdateRegionRequestDto>(RegionDomainModel);
                //var regionDto = new RegionDto
                //{
                //    Id = RegionDomainModel.Id,
                //    Code = RegionDomainModel.Code,
                //    Name = RegionDomainModel.Name,
                //    RegionImageUrl = RegionDomainModel.RegionImageUrl
                //};
        return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer,Reader")]

        public async Task<IActionResult> Delete([FromRoute] Guid id) 
        {
            var Regiondomainmodel = await regionRepository.DeleteAsync(id);

            if(Regiondomainmodel == null)
            {
                return NotFound();
            }
            
            // return deleted region back

            //var regionDto = new RegionDto
            //{
            //    Id = Regiondomainmodel.Id,
            //    Code = Regiondomainmodel.Code,
            //    Name = Regiondomainmodel.Name,
            //    RegionImageUrl = Regiondomainmodel.RegionImageUrl

            //};
            return Ok(mapper.Map<RegionDto>(Regiondomainmodel));

        
        }
    }
}
