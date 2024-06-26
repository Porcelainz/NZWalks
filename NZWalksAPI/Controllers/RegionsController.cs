﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.CustomActionFilters;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;
using System.Text.Json;

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
        private readonly ILogger<RegionsController> _logger;

        //DI     
        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger)
        {
            _dbContext = dbContext;
            _regionRepository = regionRepository;
            _mapper = mapper;
            _logger = logger;
        }
        //GET ALL REGIONS
        //GET: https://localhost:port/api/regions
        [HttpGet]
        //[Authorize(Roles = "Reader, Writer")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll Action Method was invoked");
            //Get data from database - domain models
            var regionsDomain = await _regionRepository.GetAllAsync();

            //var regionsDto = _mapper.Map<List<RegionDto>>(regionsDomain);
            //return DTOs
            _logger.LogInformation($"Finish GetAllRegions request with data: {JsonSerializer.Serialize(regionsDomain)}");
            return Ok(_mapper.Map<List<RegionDto>>(regionsDomain));
        }

        //GET SINGLE REGION (Get region By ID)
        //GET:  https://localhost:port/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader")]
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
        //[Authorize(Roles = "Writer")]
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
        //[Authorize(Roles = "Writer")]
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
        //[Authorize(Roles = "Writer")]
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
