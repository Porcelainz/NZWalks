using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.CustomActionFilters;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    //https://localhost:port/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        //DI
        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            _dbContext = dbContext;
            _regionRepository = regionRepository;
            _mapper = mapper;
        }
        //GET ALL REGIONS
        //GET: https://localhost:port/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get data from database - domain models
            var regionsDomain = await _regionRepository.GetAllAsync();

            //var regionsDto = _mapper.Map<List<RegionDto>>(regionsDomain);
            //return DTOs
            return Ok(_mapper.Map<List<RegionDto>>(regionsDomain));
        }

        //GET SINGLE REGION (Get region By ID)
        //GET:  https://localhost:port/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var region = _dbContext.Regions.Find(id);
            //get domain model from db
            var regionDomain = await _regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            //convert domain model to dto

            return Ok(_mapper.Map<RegionDto>(regionDomain));
        }

        //POST To create new region
        //POST:https://localhost:port/api/regions
        [HttpPost]
        [VaildateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
           
                //Map or convert dto to domain model
                var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);
                //Use domain model to create Region
                regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);

                //Map domain model back to DTO
                var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
                return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDto);                       

        }


        //Update region
        //PUT: https://localhost:port/api/regions/{id}
        [HttpPut("{id:Guid}")]
        [VaildateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            
             //Map dto to domain model
                var domaimRegion = _mapper.Map<Region>(updateRegionRequestDto);

                //check if region exists
                domaimRegion = await _regionRepository.UpdateAsync(id, domaimRegion);
                if (domaimRegion == null)
                {
                    return NotFound();
                }               
                //Convert domain model to dto
                //var regionDto = _mapper.Map<RegionDto>(domaimRegion);
                return Ok(_mapper.Map<RegionDto>(domaimRegion));          
            
        }

        //Delete Region
        //DELETE: https://localhost:port/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await _regionRepository.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }


            //return deleted region back
            //map Domain Model to DTO
            //var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
            return Ok(_mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}
