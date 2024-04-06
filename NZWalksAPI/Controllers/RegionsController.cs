using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Controllers
{
    //https://localhost:port/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;
        public RegionsController(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        //GET ALL REGIONS
        //GET: https://localhost:port/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get data from database - domain models
            var regionsDomain = await _dbContext.Regions.ToListAsync();

            //Map domain models to DTOs
            var regionDto = new List<RegionDto>();
            foreach (var region in regionsDomain)
            {
                regionDto.Add(new RegionDto()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl
                });
            }

            //return DTOs

            return Ok(regionDto);
        }

        //GET SINGLE REGION (Get region By ID)
        //GET:  https://localhost:port/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var region = _dbContext.Regions.Find(id);
            //get domain model from db
            var region = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (region == null)
            {
                return NotFound();
            }
            //convert domain model to dto
            var regionDto = new RegionDto()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };
            return Ok(regionDto);
        }

        //POST To create new region
        //POST:https://localhost:port/api/regions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map or convert dto to domain model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };
            //Use domain model to create Region
            await _dbContext.Regions.AddAsync(regionDomainModel);
            await _dbContext.SaveChangesAsync();
            //Map domain model back to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDto);
        }


        //Update region
        //PUT: https://localhost:port/api/regions/{id}
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto) 
        {
            //check if region exists
            var regionDomainModel = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            //Mapt dto to domain model
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;
            await _dbContext.SaveChangesAsync();
            //Convert domain model to dto
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return Ok(regionDto);
        }

        //Delete Region
        //DELETE: https://localhost:port/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            _dbContext.Regions.Remove(regionDomainModel);
            await _dbContext.SaveChangesAsync();

            //return deleted region back
            //map Domain Model to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return Ok(regionDto);
        }
    }
}
